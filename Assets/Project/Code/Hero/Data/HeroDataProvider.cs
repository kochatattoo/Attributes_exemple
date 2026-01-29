using Code.Hero.Attributes;
using Code.Infrastructure.AssetManagement;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Code.Hero.Data
{
    public class HeroDataProvider : IHeroDataProvider, IDisposable
    {
        private readonly IAsset _asset;
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();
        private readonly Dictionary<int, HeroClass> _heroClasses = new Dictionary<int, HeroClass>();
        
        private HeroClassesRoot _heroClassesRoot;
        private TextAsset _heroClassesTextAsset;
        private bool _disposed;

        public IReadOnlyDictionary<int, HeroClass> HeroClassesReadonly => _heroClasses;

        public HeroDataProvider(IAsset asset)
        {
            _asset = asset;
        }

        ~HeroDataProvider()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;
            _disposed = true;

            if (disposing)
            {
                _cts.Cancel();
                _cts.Dispose();

                if (_heroClassesTextAsset != null)
                {
                    _asset.Release(_heroClassesTextAsset);
                    _heroClassesTextAsset = null;
                }

                _heroClasses.Clear();
                _heroClassesRoot = null;
            }
        }

        public async UniTask LoadAsync(CancellationToken token)
        {
            try
            {
                TextAsset classesJson = await _asset.LoadAsync<TextAsset>(HeroAssetAddress.Classes, token);
                _heroClassesTextAsset = classesJson;
                _heroClassesRoot = JsonUtility.FromJson<HeroClassesRoot>(_heroClassesTextAsset.text);

                if (TryHeroClassesRoot())
                {
                    Debug.LogWarning("HeroClassRepository: JSON contains no hero classes.");
                    return;
                }

                CompleteHeroClass();
            }
            catch (OperationCanceledException) { }
        }

        private bool TryHeroClassesRoot() => 
            _heroClassesRoot?.heroClasses == null ||
                _heroClassesRoot.heroClasses.Count == 0;

        private void CompleteHeroClass()
        {
            _heroClasses.Clear();
            foreach (var dto in _heroClassesRoot.heroClasses)
            {
                var hero = new HeroClass(dto);
                _heroClasses.Add(hero.Id, hero);
               // Debug.Log(hero.Name);
                Debug.Log(hero.ImagePath);
                Debug.Log(hero.Attributes[AttributeConstants.STM]);
            }
        }
    }
}

