using System;
using UnityEngine;

public class Kunai : MonoBehaviour {
    [SerializeField]
    private LayerMask playerMark;
    [SerializeField]
    private float damage = 0f;
    [SerializeField]
    private float speed = 2f;
    private DespawnByDistance despawnByDistance;
    private Rigidbody2D rb;
    private bool isPlay = false;
    [SerializeField]
    private ParticleSystem collisionEnd;
    private Vector2 direction;
    public String targetEnemy = "Enemy";
    public bool isAttack = false;
    private void OnEnable() {
        if(despawnByDistance == null){
            despawnByDistance = GetComponent<DespawnByDistance>();
        }
        if(rb == null){
            rb = GetComponent<Rigidbody2D>();
        }
        isPlay = false;
        direction = transform.rotation.z < 0 ? Vector2.right : Vector2.left;
        isAttack = false;
    }
    private void FixedUpdate() {
        Debug.DrawLine(transform.position, transform.position + new Vector3(direction.x, direction.y, 0) * 0.5f, Color.green);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 0.5f, playerMark);
        if(hit.collider != null){
            if(!isPlay){
                collisionEnd.Play();
                isPlay = true;
            }
            if(hit.collider.tag == targetEnemy){
                Character character = hit.collider.gameObject.GetComponent<Character>();
                if(!isAttack){
                    character.BeAttack(damage);
                    isAttack = true;
                }
                rb.velocity = Vector2.zero;
            }
            if(despawnByDistance != null && collisionEnd.isStopped){
                despawnByDistance.Despawn();
            }
        }
        if(rb != null){
            rb.AddForce(direction * speed * 20);
        }
    }
}