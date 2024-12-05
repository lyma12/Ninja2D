using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    private float maxHP;
    private float maxHealth;
    protected float health;
    protected float hp;
    protected float speed;
    private string currentAnim;
    [SerializeField]
    private Animator animator;
    protected bool isDead => health <= 0;
    [SerializeField] Slider healthBar;
    [SerializeField]
    private GameObject combatHealth;
    protected virtual void OnInit(){
        health = maxHealth;
        hp = maxHP;
        currentAnim = AnimationEnum.IDLE;
    }

    protected void setMaxHealth(float hp){
        if(hp > 0){
            maxHealth = hp;
        }
    }

    public virtual void OnDespawn(){}
    public void OnHit(float damage){
        if(health >= damage){
            health -= damage;
            if(health <= 0){
                OnDeath();
            }
        }
    }

    protected virtual void OnDeath(){
        health = 0;
        ChangeAnim(AnimationEnum.DEAD);
    }
    protected void ChangeAnim(string anim){
        if(!currentAnim.Equals(anim)){
            animator.ResetTrigger(anim);
            currentAnim = anim;
            animator.SetTrigger(anim);
        }
    }

    protected virtual void FixedUpdate() {
        if(!isDead && maxHealth != 0){
            healthBar.value = health/maxHealth;
        }
        if(isDead){
            healthBar.value = 0;
        }
    }
    public void BeAttack(float damge){
        var combatHealthClone = ObjectPoolManager.SpawnObject(combatHealth, transform.position, transform.rotation);
        combatHealthClone.GetComponentInChildren<TextMeshProUGUI>().text = "-" + damge;
        health -= damge;
        if(isDead) OnDeath();
    }
}
