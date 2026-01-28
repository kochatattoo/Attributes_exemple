using UniRx;

namespace Code.Hero.Data
{
    public class HeroData
    {
        private readonly IHeroDataService _heroDataService;

        public ReactiveProperty<HeroClass> HeroClass { get; } = new ReactiveProperty<HeroClass>();
        public ReactiveProperty<int> HeroId { get; } = new ReactiveProperty<int>();

        public HeroData(IHeroDataService heroDataService)
        {
            _heroDataService = heroDataService;
        }
    }
}

