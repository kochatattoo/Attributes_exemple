using Code.Hero.Data;
using Code.Infrastructure.Factory;
using Code.Infrastructure.Utils;
using Code.UI.Services.Factory;
using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

namespace Code.Infrastructure.State.States
{
    public class MainMenuState : IState
    {
        private const string MainMenu = "MainMenu";
        private readonly SceneLoader _sceneLoader;
        private readonly LoadingCurtain _loadingCurtain;
        private readonly IMenuFactory _menuFactory;
        private readonly IUIFactory _uiFactory;

        private HeroDataFabric _heroData;
        private CancellationTokenSource _cancellationTokenSource;

        public MainMenuState(
            SceneLoader sceneLoader,
            IMenuFactory menuFactory,
            LoadingCurtain loadingCurtain,
            IUIFactory uiFactory)
        {
            _sceneLoader = sceneLoader;
            _menuFactory = menuFactory;
            _loadingCurtain = loadingCurtain;
            _uiFactory = uiFactory;
        }

        public void Enter()
        {
            _cancellationTokenSource = new CancellationTokenSource();

            _sceneLoader.Load(
                MainMenu,
                () => OnLoadedAsync(_cancellationTokenSource.Token).Forget(),
                _cancellationTokenSource.Token
            );
        }

        public void Exit()
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource?.Dispose();
        }

        private async UniTaskVoid OnLoadedAsync(CancellationToken token)
        {
            try
            {
                await UniTask.WhenAll(_menuFactory.CreateMenu(token),
                                      _uiFactory.CreateUIRoot(token));
            }
            catch (OperationCanceledException)
            {
                Debug.Log("MainMenuState: загрузка меню отменена");
            }
            catch (Exception ex)
            {
                Debug.LogError($"MainMenuState: ошибка при создании меню: {ex}");
            }

            _loadingCurtain.Hide();
        }
    }
}
