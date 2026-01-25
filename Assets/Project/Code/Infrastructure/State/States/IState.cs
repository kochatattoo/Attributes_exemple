namespace Code.Infrastructure.State.States
{
    public interface IState
    {
        void Enter();
        void Exit();
    }
}
