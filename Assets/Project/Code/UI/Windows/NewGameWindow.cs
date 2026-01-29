using Code.Hero.Data;
using Code.Infrastructure.AssetManagement;
using Code.UI.MainMenuElements;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI.Windows
{
    public class NewGameWindow : WindowBase
    {
        //Основной класс фасад окна - инстанируем окно через WindowService с прокинутыми зависимостями
        private readonly CompositeDisposable _disposables = new CompositeDisposable();

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
            heroAttributesView.Construct(_heroData, _disposables);
            heroParamentresUI.Construct(_heroData);
        }

        private void OnDestroy() => _disposables.Dispose();

    }
}
