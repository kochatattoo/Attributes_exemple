using Code.Infrastructure.Utils;

namespace Code.Infrastructure.State.States
{
    public class MainMenuState : IState
    {
        private const string MainMenu = "MainMenu";
        private readonly IGameStateMachine _gameStateMachine;
        private readonly SceneLoader _sceneLoader;

        public MainMenuState(IGameStateMachine gameStateMachine, SceneLoader sceneLoader)
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
        }

        public void Enter()
        {
            _sceneLoader.Load(MainMenu, OnLoaded);
        }

        public void Exit() {}

        private void OnLoaded()
        {
            // Инициализация главного меню
        }
    }
}
