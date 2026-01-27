using Code.Infrastructure.Factory;
using Code.Infrastructure.Utils;
using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

namespace Code.Infrastructure.State.States
{
    public class MainMenuState : IState
    {
        private const string MainMenu = "MainMenu";
        private readonly IGameStateMachine _gameStateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly IMenuFactory _menuFactory;

        private CancellationTokenSource _cancellationTokenSource;

        public MainMenuState(IGameStateMachine gameStateMachine, SceneLoader sceneLoader, IMenuFactory menuFactory)
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _menuFactory = menuFactory;
        }

        public void Enter()
        {
            _cancellationTokenSource = new CancellationTokenSource();

            _sceneLoader.Load(
                MainMenu,
                () => OnLoadedAsync(_cancellationTokenSource.Token).Forget(),
                _cancellationTokenSource.Token
            );

            //_sceneLoader.Load(MainMenu, OnLoadedAsync);
        }

        public void Exit()
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource?.Dispose();
        }

        private async void OnLoadedAsync()
        {
            await _menuFactory.CreateMenu();
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
        }
    }
}
