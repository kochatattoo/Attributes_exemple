using Code.Infrastructure.Services;
using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEditor.VersionControl;

namespace Code.UI.Services.Factory
{
    public interface IUIFactory: IService
    {
        UniTask CreateUIRoot(CancellationToken ct = default);
        void CreateNewGame();
        void CreateExit();
    }
}
