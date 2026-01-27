using Code.Infrastructure.Data.StaticData;
using Code.Infrastructure.Utils;

namespace Code.Infrastructure.State.States
{
    public class CoreLoadingState : IState
    {
        private readonly IGameStateMachine _gameStateMachine;
        private readonly IStaticDataService _staticDataService;
        private readonly LoadingCurtain _loadingCurtain;

        public CoreLoadingState(IGameStateMachine gameStateMachine, 
            IStaticDataService staticDataService, 
            LoadingCurtain loadingCurtain)
        {
            _gameStateMachine = gameStateMachine;
            _staticDataService = staticDataService;
            _loadingCurtain = loadingCurtain;
        }

        public void Enter()
        {
            _loadingCurtain.Show();

            _staticDataService.Load();
            // Загрузка различных конфигов

            _gameStateMachine.Enter<MainMenuState>();
        }

        public void Exit() { }
    }
}
