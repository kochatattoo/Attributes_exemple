using Code.Hero.Data;
using Zenject;

namespace Code.Infrastructure
{
    public class MainMenuInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindHeroDataService();
        }

        private void BindHeroDataService() =>
            Container.BindInterfacesTo<HeroDataProvider>()
                .AsSingle()
                .NonLazy();
    }
}
