using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Code.Hero.Data
{
    public class HeroDataFabric : IDisposable
    {
        private readonly IHeroDataProvider _provider;
        private readonly ReadOnlyReactiveProperty<HeroClass> _selectedClass;

        public ReactiveDictionary<int, HeroClass> Classes { get; } = new ReactiveDictionary<int, HeroClass>();
        public ReactiveProperty<int> SelectedId { get; } = new ReactiveProperty<int>(-1);
        public IReadOnlyReactiveProperty<HeroClass> SelectedClass => _selectedClass;

        public IntReactiveProperty BonusPoints { get; } = new IntReactiveProperty(10);

        public HeroDataFabric(IHeroDataProvider heroDataProvider)
        {
            _provider = heroDataProvider;

            _selectedClass = SelectedId
                .DistinctUntilChanged()
                .Select(id => Classes.TryGetValue(id, out var hc) ? hc : null)
                .ToReadOnlyReactiveProperty();
        }

        public void Initialize()
        {
            InitializeClasses(_provider.HeroClassesReadonly);
        }

        public void SelectClass(int id)
        {
            if (SelectedId.Value == id)
            {
                Debug.Log($"[Fabric] Class {id} already selected. Skipping.");
                return;
            }

            SelectedId.Value = id;
        }

        public HeroData CreateHeroData()
        {
            return new HeroData(SelectedClass.Value);
        }

        public void Dispose()
        {
            // 1. Уничтожаем ReadOnly свойство (оно отпишется от SelectedId)
            _selectedClass.Dispose();

            // 2. Очищаем словари
            Classes.Clear();

            // 3. Сбрасываем значения
            SelectedId.Value = -1;
        }

        private void InitializeClasses(IReadOnlyDictionary<int, HeroClass> source)
        {
            foreach (var kvp in source)
            {
                Classes[kvp.Key] = kvp.Value;
            }
        }
    }
}

