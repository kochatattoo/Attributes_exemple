using Code.Infrastructure.Utils;
using Code.UI.Services.Factory;
using System;

namespace Code.Infrastructure.State.States
{
    public class GameLoopState : IState
    {
        private const string GameScene = "DemoScene";
        private readonly SceneLoader _sceneLoader;
        private readonly IUIFactory _uiFactory;

        public GameLoopState( SceneLoader sceneLoader, IUIFactory uiFactory)
        {
            _sceneLoader = sceneLoader;
            _uiFactory = uiFactory;
        }

        public void Enter()
        {
            _sceneLoader.Load(GameScene, OnLoaded);
        }

        private void OnLoaded()
        {
            _uiFactory.CreateDemoMenu();
        }

        public void Exit() { }
    }
}
