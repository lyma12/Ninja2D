using UnityEngine;

public class AttackArea : MonoBehaviour {
    [SerializeField]
    private Enemy enemy;
    private void OnTriggerStay2D(Collider2D other) {
        if(other.gameObject.tag == "Player"){
            if(!enemy.IsAttack){
                other.gameObject.GetComponent<Character>().BeAttack(enemy.Damage);
                enemy.ChangeCurrentState(new AttackState());
            }
        }
    }
}