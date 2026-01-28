using System.Collections.Generic;
using System.Threading;

namespace Code.Hero.Data
{
    public interface IHeroDataService
    {
        IReadOnlyDictionary<int, HeroClass> HeroClasses { get; }

        void LoadAsync(CancellationToken token);
    }
}