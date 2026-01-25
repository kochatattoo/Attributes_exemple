using Zenject;

namespace Code.Infrastructure.State.States
{
    public class StateFactory : IStateFactory
    {
        private readonly DiContainer _container;

        public StateFactory(DiContainer container) =>
            _container = container;

        public T CreateState<T>() where T : IState =>
            _container.Resolve<T>();
    }
}
