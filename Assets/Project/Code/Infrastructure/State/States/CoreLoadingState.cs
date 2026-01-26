using Code.Infrastructure.Utils;
using System;

namespace Code.Infrastructure.State.States
{
    public class CoreLoadingState : IState
    {
        private readonly IGameStateMachine _gameStateMachine;

        public CoreLoadingState(IGameStateMachine gameStateMachine)
        {
            _gameStateMachine = gameStateMachine;

            // DI сервисов загрузок данных
        }

        public void Enter()
        {
            _gameStateMachine.Enter<MainMenuState>();
        }

        public void Exit() { }
    }
}
