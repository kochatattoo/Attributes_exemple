using Code.Infrastructure.AssetManagement;
using Code.Infrastructure.Data.StaticData;
using Code.Infrastructure.Services;
using Code.Infrastructure.State;
using Code.Infrastructure.State.States;
using Code.Infrastructure.Utils;
using System;
using UnityEngine;
using Zenject;

namespace Code.Infrastructure
{
    public class BootstrapInstaller : MonoInstaller
    {
        [SerializeField] private CoroutineRunner _coroutineRunner;
        public override void InstallBindings()
        {
            BindServiceFactory();
            BindStateFactory();
            BindGameStateMachine();
            BindCoroutineRunner();
            BindSceneLoader();

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

        private void BindServices()
        {
            BindAssetProvider();
            BindStaticDataService();
        }

        private void BindStaticDataService() =>
            Container.Bind<IStaticDataService>()
                .To<StaticDataService>()
                .AsSingle()
                .NonLazy();

        private void BindAssetProvider() =>
            Container.BindInterfacesTo<AssetProvider>()
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