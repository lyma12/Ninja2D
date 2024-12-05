using UnityEngine;

public class AttackAreaPlayer : MonoBehaviour {
    [SerializeField]
    private Player player;
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Enemy"){
            other.GetComponent<Enemy>().BeAttack(player.Damage);
        }
    }
}