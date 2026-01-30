using Code.Infrastructure.Services;
using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

namespace Code.UI.Services.Factory
{
    public interface IUIFactory: IService
    {
        UniTask<GameObject> CreateUIRoot(CancellationToken ct = default);
        void CreateNewGame();
        void CreateExit();
        void CreateDemoMenu();
    }
}
