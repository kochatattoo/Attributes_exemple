using Code.Infrastructure.Utils;

namespace Code.Infrastructure.State.States
{
    public class BootstrapState : IState
    {
        private const string Bootstrap = "Bootstrap";
        private readonly IGameStateMachine _gameStateMachine;
        private readonly SceneLoader _sceneLoader;

        public BootstrapState(IGameStateMachine gameStateMachine, SceneLoader sceneLoader)
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
        }

        public void Enter()
        {
            _sceneLoader.Load(Bootstrap, OnLoaded);
        }

        private void OnLoaded()
        {
            // Запуск методов очистки кеша 
            _gameStateMachine.Enter<CoreLoadingState>();
        }

        public void Exit() {}
    }
}
