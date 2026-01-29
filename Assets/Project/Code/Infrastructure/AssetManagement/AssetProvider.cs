using UnityEngine.AddressableAssets;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.ResourceManagement.AsyncOperations;
using Zenject;
using Cysharp.Threading.Tasks;
using System.Threading;
using System;

namespace Code.Infrastructure.AssetManagement
{
    public class AssetProvider : IAsset, IInitializable, IDisposable
    {
        private readonly Dictionary<string, AsyncOperationHandle> _completedCache = new Dictionary<string, AsyncOperationHandle>();
        private readonly Dictionary<string, List<AsyncOperationHandle>> _handles = new Dictionary<string, List<AsyncOperationHandle>>();

        private readonly CancellationTokenSource _providerCancellationToken = new CancellationTokenSource();

        public void Dispose()
        {
            CleanUp();
            _providerCancellationToken.Dispose();
        }

        public void Initialize()
        {
            Addressables.InitializeAsync();
        }

        public void CancelAll()
        {
            _providerCancellationToken.Cancel();
        }

        public async UniTask<T> LoadAsync<T>(AssetReference assetReference, CancellationToken ct = default)
            where T : class => 
            await LoadInternal(Addressables.LoadAssetAsync<T>(assetReference), assetReference.AssetGUID,ct);

        public async UniTask<T> LoadAsync<T>(string address, CancellationToken ct = default)
            where T : class => 
            await LoadInternal(Addressables.LoadAssetAsync<T>(address), address, ct);

        public UniTask<GameObject> InstantiateAsync(string address, CancellationToken ct = default) =>
            InstInternal(Addressables.InstantiateAsync(address), address, ct);

        public UniTask<GameObject> InstantiateAsync(string address, Vector3 at, CancellationToken ct = default) =>
            InstInternal(Addressables.InstantiateAsync(address, at, Quaternion.identity), address, ct);

        public UniTask<GameObject> InstantiateAsync(string address, Transform parent, CancellationToken ct = default) =>
            InstInternal( Addressables.InstantiateAsync(address, parent), address, ct);

        public void CleanUp()
        {
            foreach (List<AsyncOperationHandle> resourceHandles in _handles.Values)
                foreach (AsyncOperationHandle handle in resourceHandles)
                    Addressables.Release(handle);

            _completedCache.Clear();
            _handles.Clear();
            _providerCancellationToken.Cancel();
        }

        public void Release(UnityEngine.Object obj)
        {
            Addressables.Release(obj);
        }

        private async UniTask<T> LoadInternal<T>(AsyncOperationHandle<T> handle, string key, CancellationToken ct) 
            where T : class
        {
            // если уже загружено — вернём из кеша
            if (_completedCache.TryGetValue(key, out var cached))
                return cached.Result as T;

            // привяжем внешний токен + токен провайдера
            using CancellationTokenSource linkedCts = CancellationTokenSource.CreateLinkedTokenSource(ct, _providerCancellationToken.Token);
            CancellationToken token = linkedCts.Token;

            AddHandle(key, handle);
            try
            {
                // ждём завершения или отмены
                var result = await handle
                    .ToUniTask(cancellationToken: token);

                // запомним в кеш
                _completedCache[key] = handle;
                return result;
            }
            catch (OperationCanceledException)
            {
                // при отмене надо отпустить ручку
                Addressables.Release(handle);
                throw;
            }
            finally
            {
                RemoveHandle(key, handle);
            }
        }

        private async UniTask<GameObject> InstInternal(AsyncOperationHandle<GameObject> handle, string key, CancellationToken ct)
        {
            using CancellationTokenSource linkedCts = CancellationTokenSource
                .CreateLinkedTokenSource(ct, _providerCancellationToken.Token);
            CancellationToken token = linkedCts.Token;

            AddHandle(key, handle);
            try
            {
                GameObject go = await handle.ToUniTask(cancellationToken: token);
                return go;
            }
            catch (OperationCanceledException)
            {
                Addressables.Release(handle);
                throw;
            }
            finally
            {
                RemoveHandle(key, handle);
            }
        }

        private void AddHandle<T>(string key, AsyncOperationHandle<T> handle) where T : class
        {
            if (!_handles.TryGetValue(key, out List<AsyncOperationHandle> resourceHandles))
            {
                resourceHandles = new List<AsyncOperationHandle>();
                _handles[key] = resourceHandles;
            }

            resourceHandles.Add(handle);
        }

        private void RemoveHandle(string key, AsyncOperationHandle handle)
        {
            if (_handles.TryGetValue(key,out List<AsyncOperationHandle> resourceHandles))
            {
                resourceHandles.Remove(handle);
                if (resourceHandles.Count == 0)
                    _handles.Remove(key);
            }
        }
    }
}
