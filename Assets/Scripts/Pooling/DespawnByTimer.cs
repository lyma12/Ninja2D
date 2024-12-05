using UnityEngine;

public class DespawnByTimer : MonoBehaviour {
    private float timer =    2f;
    private float currentTimer = 0f;
    public float Timer{
        get{ return timer;}
    }
    public float CurrentTimer{
        get { return currentTimer;}
    }
    private void OnEnable(){
        currentTimer = 0;
    }
    private void LateUpdate() {
        currentTimer += Time.deltaTime;
        if(currentTimer > timer){
            Despawn();
        }
    }
    public void Despawn(){
        ObjectPoolManager.ReturnObjectToPool(gameObject);
    }
}