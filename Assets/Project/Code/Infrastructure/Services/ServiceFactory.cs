using Zenject;

namespace Code.Infrastructure.Services
{
    public class ServiceFactory : IServiceFactory
    {
        private readonly DiContainer _container;

        public ServiceFactory(DiContainer container) =>
            _container = container;

        public T CreateService<T>() where T : IService =>
            _container.Resolve<T>();
    }
}
