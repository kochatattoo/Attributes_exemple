using Code.Infrastructure.AssetManagement;
using Code.UI.Elements;
using Code.UI.Services;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Code.Infrastructure.Factory
{
    public class MenuFactory : IMenuFactory
    {
        private readonly IAsset _asset;
        private readonly IWindowService _windowService;

        public MenuFactory(IAsset asset, IWindowService windowService)
        {
            _asset = asset;
            _windowService = windowService;
        }

        public async UniTask<GameObject> CreateMenu(CancellationToken ct = default)
        {
            GameObject menu = await _asset.InstantiateAsync(AssetAddress.Menu, ct);

            foreach (OpenWindowButton openWindowButton in menu.GetComponentsInChildren<OpenWindowButton>())
            {
                openWindowButton.Construct(_windowService);
            }

            return menu;
        }
    }
}
