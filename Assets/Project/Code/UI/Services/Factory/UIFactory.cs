using Code.Infrastructure.AssetManagement;
using Code.Infrastructure.Data.StaticData;
using Code.Infrastructure.Data.StaticData.Window;
using Code.UI.Windows;
using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

namespace Code.UI.Services.Factory
{
    public class UIFactory : IUIFactory
    {
        private readonly IAsset _asset;
        private readonly IStaticDataService _staticData;
        private Transform _uiRoot;

        public UIFactory(IAsset asset, IStaticDataService staticData)
        {
            _asset = asset;
            _staticData = staticData;
        }

        public void CreateExit()
        {
            
        }

        public void CreateNewGame()
        {
            
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
