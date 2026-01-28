using Code.Infrastructure.Services;
using Code.UI.Services.Factory;
using Code.UI.Windows;
using Zenject;

namespace Code.UI.Services
{
    internal class WindowService : IWindowService, IInitializable
    {
        private readonly IServiceFactory _serviceFactory;
        private IUIFactory _uiFactory;

        public WindowService(IServiceFactory serviceFactory)
        {
            _serviceFactory = serviceFactory;
        }

        public void Initialize()
        {
            _uiFactory = _serviceFactory.CreateService<IUIFactory>();
        }

        public void Open(WindowID windowId)
        {
            switch (windowId)
            {
                case WindowID.Unknow:
                    break;
                case WindowID.NewGame:
                    _uiFactory.CreateNewGame();
                    break;
                case WindowID.Exit:
                    _uiFactory.CreateExit();
                    break;
                case WindowID.Shop:
                    break;
                case WindowID.Option:
                    break;
            }
        }
    }
}
