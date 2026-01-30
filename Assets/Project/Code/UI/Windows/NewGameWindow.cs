using Code.Hero.Data;
using Code.Infrastructure.AssetManagement;
using Code.Infrastructure.State;
using Code.Infrastructure.State.States;
using Code.UI.MainMenuElements;
using System.Collections.Generic;
using TMPro;
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
        [SerializeField] private TextMeshProUGUI _classNameField;

        private IGameStateMachine _gameStateMachine;
        private IHeroDataService _heroDataService;
        private HeroDataFabric _heroDataFabric; // Объявялем HeroDataFabric

        public void Construct(IHeroDataProvider heroDataProvider, 
                              IAsset asset, 
                              IHeroDataService heroDataService, 
                              IGameStateMachine gameStateMachine)
        {
            _heroDataService = heroDataService;
            _gameStateMachine = gameStateMachine;

            _heroDataFabric = new(heroDataProvider);
            _heroDataFabric.Initialize();

            _heroDataFabric.BonusPoints
                .Subscribe(pointsLeft =>
                {
                    _acceptButton.interactable = (pointsLeft == 0 && _heroDataFabric.SelectedClass.Value != null);
                })
                .AddTo(_disposables);

            _heroDataFabric.SelectedClass
                .Subscribe(hero =>
                {
                    if (hero != null)
                    {
                        _classNameField.text = hero.Name;
                        ColorUtility.TryParseHtmlString(hero.Color, out var color);
                        _classNameField.color = color;
                    }
                    // Проверяем состояние кнопки при смене класса
                    _acceptButton.interactable = (_heroDataFabric.BonusPoints.Value == 0 && hero != null);
                })
                .AddTo(_disposables);

            heroClassContainerUI.Construct(_heroDataFabric, asset);
            heroClassDescriptionUI.Construct(_heroDataFabric);
            heroAttributesView.Construct(heroParamentresUI, _heroDataFabric, _disposables);

            _acceptButton.onClick.AsObservable()
                .Subscribe(_ => ConfirmAndClose())
                .AddTo(_disposables);
        }

        private void OnDestroy()
        {
            // Если окно закрывается (и это не подтверждение), откатываем изменения
            if (_heroDataFabric != null && _heroDataFabric.SelectedClass.Value != null)
            {
                // Вызываем сброс текущего героя и возвращаем очки
                heroAttributesView.ResetHeroProgress(_heroDataFabric.SelectedClass.Value, _heroDataFabric);
            }
            _disposables.Dispose(); 
        }

        private void ConfirmAndClose()
        {
            // Вместо модификаторов, мы делаем финальный "слепок" атрибутов.
            var currentHero = _heroDataFabric.SelectedClass.Value;
            if (currentHero == null) return;

            // --- Финализация атрибутов ---
            var finalAttributesDict = new Dictionary<string, int>();
            foreach (var kvp in currentHero.Attributes.AllAttributes)
            {
                // Берем финальное значение и делаем его базовым для нового HeroData
                finalAttributesDict[kvp.Key] = kvp.Value.FinalValue.Value;
            }

            // Создаем новый HeroClass с уже "запеченными" статами
            var finalizedHero = new HeroClass(currentHero.Id, 
                                              currentHero.Name, 
                                              finalAttributesDict, 
                                              currentHero.Color,
                                              currentHero.Description,
                                              currentHero.ImagePath,
                                              currentHero.Abilities);

            // Создаем финальный HeroData с "чистым" классом
            HeroData finalData = new HeroData(finalizedHero);
            _heroDataService.SetHero(finalData);

            _heroDataFabric = null;

            Debug.Log("Character Created! Progress Saved.");
            _gameStateMachine.Enter<GameLoopState>();

            Destroy(gameObject);
        }

        #region OldConfirm 
        /*
        private void ConfirmAndCloseObstacle()
        {
            if (_heroDataFabric.SelectedClass.Value == null) return;

            HeroData finalHero = _heroDataFabric.CreateHeroData();
            _heroDataService.SetHero(finalHero);
            _heroDataFabric = null; // Зануляем фабрику, чтобы OnDestroy не сбросил статы

            Debug.Log("Character Created! Progress Saved.");
            _gameStateMachine.Enter<GameLoopState>();

            Destroy(gameObject);
        }
        */
        #endregion
    }
}
