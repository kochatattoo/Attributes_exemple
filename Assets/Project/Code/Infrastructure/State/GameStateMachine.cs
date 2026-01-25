using Code.Infrastructure.State.States;
using System;
using System.Collections.Generic;

namespace Code.Infrastructure.State
{
    public class GameStateMachine : IGameStateMachine
    {
        private readonly IStateFactory _stateFactory;

        private Dictionary<Type, IState> _states;
        private IState _activeState;

        public GameStateMachine(IStateFactory stateFactory)
        {
            _stateFactory = stateFactory;
        }

        public void CreateGameStates()
        {
            _states = new Dictionary<Type, IState>
            {
                [typeof(BootstrapState)] = _stateFactory
                .CreateState<BootstrapState>(),
                [typeof(CoreLoadingState)] = _stateFactory 
                .CreateState<CoreLoadingState>(),
                [typeof(MainMenuState)] = _stateFactory 
                .CreateState<MainMenuState>(),
            };
        }

        public void Enter<TState>() where TState : class, IState
        {
            IState state = ChangeState<TState>();
            state.Enter(); // Запускаем его
        }
        private TState ChangeState<TState>() where TState : class, IState
        {
            _activeState?.Exit();

            TState state = GetState<TState>();
            _activeState = state;

            return state;
        }

        private TState GetState<TState>() where TState : class, IState =>
            _states[typeof(TState)] as TState;
    }
}
