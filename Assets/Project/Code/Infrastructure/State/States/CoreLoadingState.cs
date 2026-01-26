using Code.Infrastructure.Data.StaticData;
using Code.Infrastructure.Utils;
using System;

namespace Code.Infrastructure.State.States
{
    public class CoreLoadingState : IState
    {
        private readonly IGameStateMachine _gameStateMachine;
        private readonly IStaticDataService _staticDataService;

        public CoreLoadingState(IGameStateMachine gameStateMachine, IStaticDataService staticDataService)
        {
            _gameStateMachine = gameStateMachine;
            _staticDataService = staticDataService;

        }

        public void Enter()
        {
            _staticDataService.Load();
            // Загрузка различных конфигов

            _gameStateMachine.Enter<MainMenuState>();
        }

        public void Exit() { }
    }
}
