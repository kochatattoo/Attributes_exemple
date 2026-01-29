using Code.Infrastructure.State;
using Code.Infrastructure.State.States;
using Zenject;

namespace Code.Infrastructure
{
    public class Game : IInitializable
    {
        private readonly IGameStateMachine _gameStateMachine;

        public Game(IGameStateMachine gameStateMachine)
        {
            _gameStateMachine = gameStateMachine;
        }

        public void Initialize()
        {
            CreateGame();
        }

        private void CreateGame()
        {
            _gameStateMachine.CreateGameStates();
            _gameStateMachine.Enter<BootstrapState>();
        }
    }
}
