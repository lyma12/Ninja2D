using UnityEngine;

public class DespawnByDistance : MonoBehaviour {
    private float distance =    10f;
    private float currentDis = 0f;
    public Vector3 startPosition = Vector3.zero;
    public float Distance{
        get{ return distance;}
    }
    public float CurrentDistance{
        get { return currentDis;}
    }
    private void OnEnable(){
        startPosition = transform.position;
        currentDis = 0;
    }
    private void LateUpdate() {
        currentDis = Vector3.Distance(startPosition, transform.position);
        if(currentDis > distance){
            Despawn();
        }
    }
    public void Despawn(){
        ObjectPoolManager.ReturnObjectToPool(gameObject);
    }
}