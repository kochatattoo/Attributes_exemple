using Code.Hero.Data;
using Code.Infrastructure.AssetManagement;
using Code.UI.MainMenuElements;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI.Windows
{
    public class NewGameWindow : WindowBase
    {
        //Основной класс фасад окна - инстанируем окно через WindowService с прокинутыми зависимостями

        [SerializeField] private HeroClassContainerUI heroClassContainerUI;
        [SerializeField] private HeroClassDescriptionUI heroClassDescriptionUI;
        [SerializeField] private HeroAttributesView heroAttributesView;
        [SerializeField] private HeroParamentresUI heroParamentresUI;
        [SerializeField] private Button _acceptButton;

        private HeroData _heroData; // Объявялем HeroData

        public void Construct(IHeroDataProvider heroDataProvider, IAsset asset)
        {
            _heroData = new(heroDataProvider);
            _heroData.Initialize();

            heroClassContainerUI.Construct(_heroData, asset);
            heroClassDescriptionUI.Construct(_heroData);
            heroParamentresUI.Construct(_heroData);
        }
    }
}
