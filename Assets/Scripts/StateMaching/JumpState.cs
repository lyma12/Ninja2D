class JumpState: IState{
    public void OnExecute(Enemy enemy){
        enemy.Jump();
    }
    public void OnEnter(Enemy enemy){}
    public void OnExit(Enemy enemy){}
}