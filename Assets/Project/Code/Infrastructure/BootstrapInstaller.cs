using Code.Hero.Data;
using Code.Infrastructure.AssetManagement;
using Code.Infrastructure.Data.StaticData;
using Code.Infrastructure.Factory;
using Code.Infrastructure.Services;
using Code.Infrastructure.State;
using Code.Infrastructure.State.States;
using Code.Infrastructure.Utils;
using Code.UI.Services;
using Code.UI.Services.Factory;
using UnityEngine;
using Zenject;

namespace Code.Infrastructure
{
    public class BootstrapInstaller : MonoInstaller
    {
        [SerializeField] private CoroutineRunner _coroutineRunner;
        [SerializeField] private LoadingCurtain _loadingCurtain;

        public override void InstallBindings()
        {
            BindServiceFactory();
            BindStateFactory();
            BindGameStateMachine();

            BindCoroutineRunner();
            BindSceneLoader();
            BindLoadingCurtain();

            BindServices();

            BindStates();
            BindGame();
        }

        private void BindServiceFactory() =>
            Container.Bind<IServiceFactory>()
                .To<ServiceFactory>()
                .AsSingle()
                .NonLazy();

        private void BindStateFactory() => 
            Container.Bind<IStateFactory>()
                .To<StateFactory>()
                .AsSingle()
                .NonLazy();

        private void BindGameStateMachine() => 
            Container.Bind<IGameStateMachine>()
                .To<GameStateMachine>()
                .AsSingle()
                .NonLazy();

        private void BindCoroutineRunner() => 
            Container.Bind<ICoroutineRunner>()
                .To<CoroutineRunner>()
                .FromComponentInNewPrefab(_coroutineRunner)
                .AsSingle()
                .NonLazy();

        private void BindSceneLoader() => 
            Container.Bind<SceneLoader>()
                .AsSingle()
                .NonLazy();

        private void BindLoadingCurtain() => 
            Container
                .Bind<LoadingCurtain>()
                .FromComponentInNewPrefab(_loadingCurtain)
                .AsSingle()
                .NonLazy();

        private void BindServices()
        {
            BindAssetProvider();
            BindStaticDataService();
            BindHeroDataProvider();
            BindHeroDataService();
            BindWindowService();
            BindUIFactory();
            BindMenuFactory();
        }

        private void BindAssetProvider() =>
            Container.BindInterfacesTo<AssetProvider>()
                .AsSingle()
                .NonLazy();

        private void BindStaticDataService() =>
            Container.Bind<IStaticDataService>()
                .To<StaticDataService>()
                .AsSingle()
                .NonLazy();

        private void BindHeroDataProvider() =>
            Container.Bind<IHeroDataProvider>()
                .To<HeroDataProvider>()
                .AsSingle()
                .NonLazy();

        private void BindHeroDataService() =>
             Container.Bind<IHeroDataService>()
                .To<HeroDataService>()
                .AsSingle()
                .NonLazy();

        private void BindWindowService() =>
            Container.BindInterfacesTo<WindowService>()
               .AsSingle()
               .NonLazy();

        private void BindUIFactory()=>
           Container.Bind<IUIFactory>()
                     .To<UIFactory>()
                     .AsSingle()
                     .NonLazy();

        private void BindMenuFactory() =>
           Container.Bind<IMenuFactory>()
               .To<MenuFactory>()
               .AsSingle()
               .NonLazy();

        private void BindStates()
        {
            Container.Bind<BootstrapState>().AsSingle().NonLazy();
            Container.Bind<CoreLoadingState>().AsSingle().NonLazy();
            Container.Bind<MainMenuState>().AsSingle().NonLazy();
        }

        private void BindGame() => 
            Container.BindInterfacesTo<Game>()
                .AsSingle()
                .NonLazy();
    }
}