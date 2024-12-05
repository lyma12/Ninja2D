using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.TextCore.Text;
using UnityEngine.UIElements;

public class Enemy : Character
{
    private Rigidbody2D rb;
    private float jumpForce = 300f;
    private float damage = 10f;
    [SerializeField]
    private Transform startKunai;
    [SerializeField]
    private Transform renderSprite;
    [SerializeField]
    private LayerMask groundLayer;
    [SerializeField]
    private GameObject kunai;
    private IState currentState;
    [SerializeField]
    private Vector2 direction = Vector2.right;
    private bool isAttack = false;
    private float kunaiRotation = 90;
    public bool IsAttack{
        get { return isAttack;}
    }
    public float Damage{
        get { return damage;}
    }
    private void Awake(){
        rb = GetComponent<Rigidbody2D>();
    }
    protected override void OnInit(){
        base.OnInit();
        ChangeCurrentState(new IdleState());
        isAttack = false;
    }
    private void OnEnable(){
        setMaxHealth(1000);
        speed = 2.5f;
        OnInit();
    }
    public void Jump(){
        ChangeAnim(AnimationEnum.JUMP);
        rb.AddForce(Vector2.up * speed);
    }
    public void Attack(){
        isAttack = true;
        ChangeAnim(AnimationEnum.ATTACK);
    }
    public void Throw(){
        ChangeAnim(AnimationEnum.THROW);
        rb.velocity = Vector2.zero;
        var playerRotation = renderSprite.transform.rotation.y;
        var rotation = Quaternion.Euler(new Vector3(0, playerRotation , kunaiRotation));
        var kunaiClone = ObjectPoolManager.SpawnObject(kunai, startKunai.position, rotation);
        kunaiClone.GetComponent<Kunai>().targetEnemy = "Player";
    }
    public void Run(){
        ChangeAnim(AnimationEnum.RUN);
        rb.velocity = direction * speed;
    }
    public void StopMoving(){
        ChangeAnim(AnimationEnum.IDLE);
        rb.velocity = Vector2.zero;
    }
    public void StopAttack(){
        isAttack = false;
    }
    private float CalDistance(Transform other){
        return Vector2.Distance(transform.position, other.position);
    }
    override protected void FixedUpdate() {
        base.FixedUpdate();
        currentState.OnExecute(this);
        Debug.DrawLine(transform.position + new Vector3(direction.x, direction.y, 0) * 1.5f, transform.position + new Vector3(direction.x, direction.y, 0) * 8f, Color.red);
        RaycastHit2D hit = Physics2D.Raycast(transform.position + new Vector3(direction.x, direction.y, 0) * 1.5f, direction, 8f, groundLayer);

        if(hit.collider != null && hit.collider.gameObject.tag == "Player"){
            if(!isAttack){
                if(CalDistance(hit.collider.transform) > 2f){
                    isAttack = true;
                    kunaiRotation = (transform.position.x < hit.collider.transform.position.x) ? -90  : 90;
                    ChangeCurrentState(new ThrowState());
                }
            }
        }
    }
    public void ChangeCurrentState(IState newState){
        if(currentState != null){
            currentState.OnExit(this);
        }
        currentState = newState;
        if(currentState != null){
            currentState.OnEnter(this);
        }
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "wallEnemy"){
            renderSprite.rotation = Quaternion.Euler(new Vector3(0, direction.x < 0 ? 0: 180, 0));
            direction = new Vector2(-direction.x, direction.y);
        }
    }
}
