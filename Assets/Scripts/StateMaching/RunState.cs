using System.Runtime.CompilerServices;
using UnityEngine;

class RunState: IState{
    private float timer;
    private float randomTime;
    public void OnExecute(Enemy enemy){
        if(timer < randomTime){
            enemy.Run();
        }
        else{
            enemy.ChangeCurrentState(new IdleState());
        }
        timer += Time.deltaTime;
    }
    public void OnEnter(Enemy enemy){
        timer = 0;
        randomTime = Random.Range(2f, 4f);
    }
    public void OnExit(Enemy enemy){
    }
}