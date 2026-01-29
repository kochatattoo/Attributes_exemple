using Code.Infrastructure.State.States;

namespace Code.Infrastructure.State
{
    public interface IGameStateMachine
    {
        void CreateGameStates();
        void Enter<TState>() where TState : class, IState;
    }
}