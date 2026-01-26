using UnityEngine.AddressableAssets;
using UnityEngine;
using Code.Infrastructure.Services;
using Cysharp.Threading.Tasks;
using System.Threading;

namespace Code.Infrastructure.AssetManagement
{
    public interface IAsset : IService
    {
        UniTask<T> Load<T>(AssetReference assetReference, CancellationToken ct = default) where T : class;
        UniTask<T> Load<T>(string address, CancellationToken ct = default) where T : class;
        UniTask<GameObject> InstantiateAsync(string address, CancellationToken ct = default);
        UniTask<GameObject> InstantiateAsync(string address, Vector3 at, CancellationToken ct = default);
        UniTask<GameObject> InstantiateAsync(string address, Transform parent, CancellationToken ct = default);
        void CleanUp();
        void Initialize();
    }
}
