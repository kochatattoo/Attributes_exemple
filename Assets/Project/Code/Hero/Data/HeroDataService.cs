using UniRx;
using UnityEngine;

namespace Code.Hero.Data
{
    public class HeroDataService : IHeroDataService
    {
        private readonly ReactiveProperty<HeroData> _currentHero = new();
        public IReadOnlyReactiveProperty<HeroData> CurrentHero => _currentHero;

        public void SetHero(HeroData data)
        {
            _currentHero.Value = data;
            Debug.Log($"Hero {data.Hero.Name} saved to Service!");
        }
    }
}

