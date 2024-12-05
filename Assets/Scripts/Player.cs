using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Player : Character
{
    private bool isAttack = false;
    private bool isJumping = false;
    private bool isGrounded = true;
    private float horizontal = 0;
    private bool directionByControl = false;
    private Rigidbody2D rb;
    private float jumpForce = 300f;
    public bool facingRight = true;
    private bool isClimb = false;
    [SerializeField]
    private Transform startKunai;
    [SerializeField]
    private Transform renderSprite;
    private ScoreManager scoreManager;
    [SerializeField]
    private LayerMask groundLayer;
    [SerializeField]
    private GameObject kunai;
    private float damage = 100f;
    public float Damage{
        get{ return damage;}
    }
    private Vector3 savePoint;
    [SerializeField]
    private GameObject areaAttack;
    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        scoreManager = FindObjectOfType<ScoreManager>();
        scoreManager.Score = 0;
        speed = 3;
        OnInit();
    }
    protected override void OnInit()
    {
        SetDeActiveAttack();
        savePoint = transform.position;
        setMaxHealth(100);
        base.OnInit();
    }
    private bool CheckGrounded(){
        Debug.DrawLine(transform.position, transform.position + Vector3.down * 1.2f, Color.red);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1.2f, groundLayer);
        if(hit.collider != null){
            return true;
        }
        else{
            return false;
        }
    }
    private void CheckClimb(){
        Debug.DrawLine(transform.position, transform.position + Vector3.up * 1.2f, Color.green);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up, 1.2f, groundLayer);
        if(hit.collider != null && hit.collider.tag == "rope"){
            StartClimb();
        }
    }
    private void StartClimb(){
        isClimb = true;
        isAttack = false;
        rb.gravityScale = 0;
        ChangeAnim(AnimationEnum.CLIMB);
        rb.velocity =  new Vector2(0, speed);
    }
    private void ResetClimb(){
        isClimb = false;
        isAttack = false;
        rb.gravityScale = 1;
    }

    public void Run(float direction){
        if(!isJumping) ChangeAnim(AnimationEnum.RUN);
        rb.velocity = new Vector2(direction * speed, rb.velocity.y);
        renderSprite.rotation = Quaternion.Euler(new Vector3(0, direction > 0 ? 0 : 180, 0));
    }
    private void Update() {
        if(!directionByControl) horizontal = Input.GetAxis("Horizontal");
        if(isGrounded){
            if(Input.GetKeyDown(KeyCode.W)){
                Throw();
                return;
            }
            if(Input.GetKeyDown(KeyCode.Space)){
                Jump();
                return;
            }
        }
        if(Input.GetKeyDown(KeyCode.Return)){
                Attack();
                return;
            }
    }
    public void Move(int horizontal){
        this.horizontal = horizontal;
        if(horizontal == 0){
            directionByControl = false;
        }
        else{
            directionByControl = true;
        }
    }
    public void ControlAttack(){
        Attack();
    }
    public void ControlThrow(){
        if(isGrounded){
            Throw();
        }
    }
    public void ControlJump(){
        if(isGrounded){
            Jump();
        }
    }

    protected override void FixedUpdate(){
        base.FixedUpdate();
        if(isDead) return;
        CheckClimb();
        if(isClimb) return;
        isGrounded = CheckGrounded();
        if(Mathf.Abs(horizontal) > 0.1f){
            Run(horizontal);
        }
        else if(isGrounded && !isJumping && !isAttack){
            ChangeAnim(AnimationEnum.IDLE);
            rb.velocity = Vector2.zero;
        }
    }
    private void Throw(){
        if(!isAttack){
            isAttack = true;
            ChangeAnim(AnimationEnum.THROW);
            var playerRotation = renderSprite.transform.localRotation.y;
            var rotation = Quaternion.Euler(new Vector3(0, playerRotation , playerRotation == 0 ? -90 : 90));
            var kunaiClone = ObjectPoolManager.SpawnObject(kunai, startKunai.position, rotation);
            kunaiClone.GetComponent<Kunai>().targetEnemy = "Enemy";
            Invoke(nameof(ResetAttack), 0.02f);
        }
    }
    private void Attack(){
        if(!isAttack){
            isAttack = true;
            if(isJumping){
                ChangeAnim(AnimationEnum.JUMP_ATT);
            }
            else{
                ChangeAnim(AnimationEnum.ATTACK);
            }
            SetActiveAttack();
            Invoke(nameof(ResetAttack), 1.2f);
        }
    }
    private void ResetAttack(){
        isAttack = false;
        SetDeActiveAttack();
        ChangeAnim(AnimationEnum.IDLE);
    }
    public void SavePoint(){
        savePoint = transform.position;
    }
    private void Jump(){
        isJumping = true;
        ChangeAnim(AnimationEnum.JUMP);
        rb.AddForce(jumpForce * Vector2.up);
    }
    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.layer == 3){// is ground
            isJumping = false;
        }
        if(other.gameObject.tag == "DeadZone"){
            scoreManager.EndGame();
        }
    }
    private async void CollectionCoin(GameObject coin){
        coin.GetComponent<ParticleSystem>().Play();
        await Task.Delay(300);
        DestroyImmediate(coin);
    }
    private void OnTriggerStay2D(Collider2D other) {
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if(!isDead){
            if(other.gameObject.tag == "coin"){
            CollectionCoin(other.gameObject);
            scoreManager.addScore(1);
            }
            if(other.gameObject.layer == 4){
                OnDeath();
            }
            if(other.gameObject.tag == "Finish"){
                scoreManager.EndGame();
            }
        }

    }
    private void OnCollisionExit2D(Collision2D other) {
        if(other.gameObject.tag == "EndRope"){
            ResetClimb();
        }
    }
    private void SetActiveAttack(){
        if(areaAttack != null){
            areaAttack.SetActive(true);
        }
    }
    private void SetDeActiveAttack(){
        if(areaAttack != null){
            areaAttack.SetActive(false);
        }
    }
    protected override void OnDeath(){
        base.OnDeath();
        if(scoreManager.Live - 1  <= 0){
            scoreManager.Live = 0;
            scoreManager.EndGame();
            return;
        }
        else{
            scoreManager.Live -= 1;
        }
        if(savePoint != null){
            Invoke(nameof(StartActive), 0.5f);
        }
    }
    protected void StartActive(){
        transform.position = savePoint;
        ChangeAnim(AnimationEnum.IDLE);
        OnInit();
    }
}
