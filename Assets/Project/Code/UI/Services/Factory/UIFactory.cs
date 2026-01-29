using Code.Hero.Data;
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
        private readonly IHeroDataProvider _heroDataProvider;
        private readonly IStaticDataService _staticData;
        private Transform _uiRoot;

        public UIFactory(IAsset asset, IStaticDataService staticData, IHeroDataProvider heroDataProvider)
        {
            _asset = asset;
            _staticData = staticData;
            _heroDataProvider = heroDataProvider;
        }

        public void CreateExit()
        {
            
        }

        public void CreateNewGame()
        {
            NewGameWindow newGameWindow = CreateWindow<NewGameWindow>(WindowID.NewGame);
            newGameWindow.Construct(_heroDataProvider, _asset);
        }

        public async UniTask<GameObject> CreateUIRoot(CancellationToken ct = default)
        {
            GameObject pref = await _asset.LoadAsync<GameObject>(AssetAddress.UIRoot, ct);
            _uiRoot = Object.Instantiate(pref).transform;

            GameObject.DontDestroyOnLoad(_uiRoot);
            return pref;
        }

        private T CreateWindow<T>(WindowID ind) where T : WindowBase
        {
            WindowConfig config = _staticData.ForWindow(ind);
            T window = Object.Instantiate(config.prefab, _uiRoot) as T;
            return window;
        }
    }
}
