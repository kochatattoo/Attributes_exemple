using Code.Infrastructure.AssetManagement;
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
            BindDiFactory();
            BindGameStateMachine();
            BindCoroutineRunner();
            BindSceneLoader();

            BindServices();

            BindStates();
            BindGame();
        }

        private void BindDiFactory() => 
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
        }

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