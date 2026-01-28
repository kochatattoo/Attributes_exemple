using Code.Hero.Data;
using System;
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
            Container.BindInterfacesTo<HeroDataService>()
                .AsSingle()
                .NonLazy();
    }
}
