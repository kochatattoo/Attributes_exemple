using Code.Infrastructure.State.States;
using System;
using System.Collections.Generic;

namespace Code.Infrastructure.State
{
    public class GameStateMachine : IGameStateMachine
    {
        private readonly IStateFactory _stateFactory;

        private Dictionary<Type, IExitableState> _states;
        private IExitableState _activeState;

        public GameStateMachine(IStateFactory stateFactory)
        {
            _stateFactory = stateFactory;
        }

        public void CreateGameStates()
        {
            _states = new Dictionary<Type, IExitableState>
            {
                [typeof(BootstrapState)] = _stateFactory
                .CreateState<BootstrapState>(),
                [typeof(CoreLoadingState)] = _stateFactory 
                .CreateState<CoreLoadingState>(),
                [typeof(MainMenuState)] = _stateFactory 
                .CreateState<MainMenuState>(),
                [typeof(GameLoopState)] = _stateFactory
                .CreateState<GameLoopState>(),
            };
        }

        public void Enter<TState>() where TState : class, IState
        {
            IState state = ChangeState<TState>();
            state.Enter();
        }

        public void Enter<TState, TPayload>(TPayload payload) where TState : class, IPayloadedState<TPayload>
        {
            TState state = ChangeState<TState>();
            state.Enter(payload);
        }

        private TState ChangeState<TState>() where TState : class, IExitableState
        {
            _activeState?.Exit();

            TState state = GetState<TState>();
            _activeState = state;

            return state;
        }

        private TState GetState<TState>() where TState : class, IExitableState =>
            _states[typeof(TState)] as TState;
    }
}
