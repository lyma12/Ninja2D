using UnityEngine;

class ThrowState: IState{
    private float timer;
    private float randomTime;
    public void OnExecute(Enemy enemy){
        if(timer > randomTime){
            enemy.StopAttack();
            enemy.ChangeCurrentState(new RunState());
        }
        timer += Time.deltaTime;
    }
    public void OnEnter(Enemy enemy){
        timer = 0;
        randomTime = Random.Range(1f, 2f);
        enemy.Throw();
    }
    public void OnExit(Enemy enemy){
    }
}