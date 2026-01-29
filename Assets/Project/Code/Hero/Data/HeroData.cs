using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UniRx;

namespace Code.Hero.Data
{
    public class HeroData
    {
        private readonly IHeroDataProvider _provider;
        public ReactiveDictionary<int, HeroClass> Classes { get; } = new ReactiveDictionary<int, HeroClass>();
        public ReactiveProperty<int> SelectedId { get; } = new ReactiveProperty<int>(-1);
        public IReadOnlyReactiveProperty<HeroClass> SelectedClass { get; }

        public HeroData(IHeroDataProvider heroDataProvider)
        {
            _provider = heroDataProvider;

            SelectedClass = SelectedId
                .Select(id => Classes.TryGetValue(id, out var hc) ? hc : null)
                .ToReadOnlyReactiveProperty();
        }

        public void Initialize()
        {
            InitializeClasses(_provider.HeroClassesReadonly);
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

