using Code.Hero.Data;
using Code.Infrastructure.Data.StaticData;
using Code.Infrastructure.Utils;
using System.Threading;

namespace Code.Infrastructure.State.States
{
    public class CoreLoadingState : IState
    {
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();

        private readonly IGameStateMachine _gameStateMachine;
        private readonly IStaticDataService _staticDataService;
        private readonly IHeroDataProvider _heroDataProvider;
        private readonly LoadingCurtain _loadingCurtain;

        public CoreLoadingState(IGameStateMachine gameStateMachine,
            IStaticDataService staticDataService,
            LoadingCurtain loadingCurtain,
            IHeroDataProvider heroDataProvider)
        {
            _gameStateMachine = gameStateMachine;
            _staticDataService = staticDataService;
            _loadingCurtain = loadingCurtain;
            _heroDataProvider = heroDataProvider;
        }

        public void Enter()
        {
            _loadingCurtain.Show();

            _staticDataService.Load();
            _heroDataProvider.LoadAsync(_cts.Token);

            // Загрузка различных конфигов

            _gameStateMachine.Enter<MainMenuState>();
        }

        public void Exit() { }
    }
}
