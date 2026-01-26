using Code.Infrastructure.AssetManagement;
using Code.Infrastructure.Data.StaticData;
using Code.Infrastructure.Data.StaticData.Window;
using Code.Infrastructure.Services;
using Code.UI.Windows;
using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEditor.VersionControl;
using UnityEngine;

namespace Code.UI.Services.Factory
{
    public interface IUIFactory: IService
    {
        UniTask CreateUIRoot(CancellationToken ct = default);
        void CreateNewGame();
        void CreateExit();
    }

    public class UIFactory : IUIFactory
    {
        private readonly IAsset _asset;
        private readonly IStaticDataService _staticData;
        private Transform _uiRoot;

        public void CreateExit()
        {
            throw new System.NotImplementedException();
        }

        public void CreateNewGame()
        {
            throw new System.NotImplementedException();
        }

        public async UniTask CreateUIRoot(CancellationToken ct = default)
        {
            GameObject pref = await _asset.Load<GameObject>(AssetAddress.UIRoot, ct);
            _uiRoot = Object.Instantiate(pref).transform;

            GameObject.DontDestroyOnLoad(_uiRoot);
        }
        private T CreateWindow<T>(WindowID ind) where T : WindowBase
        {
            WindowConfig config = _staticData.ForWindow(ind);
            T window = Object.Instantiate(config.prefab, _uiRoot) as T;
            return window;
        }
    }
}
