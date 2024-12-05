using UnityEngine;

class AttackState: IState{
    private float timer = 0f;
    private float maxTimer = 0.45f;
    public void OnExecute(Enemy enemy){
        if(timer > maxTimer){
            enemy.ChangeCurrentState(new IdleState());
            enemy.StopAttack();
        }
        timer += Time.deltaTime;
    }
    public void OnEnter(Enemy enemy){
        timer = 0f;
        enemy.Attack();
    }
    public void OnExit(Enemy enemy){
    }
}