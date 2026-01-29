namespace Code.Infrastructure.State.States
{
    public interface IStateFactory
    {
        T CreateState<T>() where T : IState;
    }
}