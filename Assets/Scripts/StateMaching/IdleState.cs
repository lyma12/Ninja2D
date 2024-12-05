using UnityEngine;

class IdleState: IState{
    private float timer;
    private float randomTime;
    public void OnExecute(Enemy enemy){
        if(timer > randomTime){
            enemy.ChangeCurrentState(new RunState());
        }
        timer += Time.deltaTime;
    }
    public void OnEnter(Enemy enemy){
        enemy.StopMoving();
        timer = 0f;
        randomTime = Random.Range(3f, 4f);
    }
    public void OnExit(Enemy enemy){}
}