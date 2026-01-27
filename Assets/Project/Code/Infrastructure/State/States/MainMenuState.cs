using Code.Infrastructure.Factory;
using Code.Infrastructure.Utils;
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
        private readonly IGameStateMachine _gameStateMachine;
        private readonly IMenuFactory _menuFactory;

        private CancellationTokenSource _cancellationTokenSource;

        public MainMenuState(IGameStateMachine gameStateMachine, 
            SceneLoader sceneLoader, 
            IMenuFactory menuFactory, 
            LoadingCurtain loadingCurtain)
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _menuFactory = menuFactory;
            _loadingCurtain = loadingCurtain;
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
                GameObject menu = await _menuFactory.CreateMenu(token);
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
