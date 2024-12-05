
using UnityEngine;

public class MovingPlaform : MonoBehaviour
{
    [SerializeField]
    private float speed;
    [SerializeField]
    private Transform aPoint;
    [SerializeField]
    private Transform bPoint;
    private Rigidbody2D rb;
    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        transform.position = aPoint.position;
    }
    private void FixedUpdate() {
        if(transform.position.y <= aPoint.position.y){
            rb.velocity = Vector2.up * speed;
        }
        else if(transform.position.y >= bPoint.position.y){
            rb.velocity = Vector2.down * speed;
        }
    }
}
