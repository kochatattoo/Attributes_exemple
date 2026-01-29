using Code.Hero.Attributes;
using Code.Hero.Data;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI.MainMenuElements
{
    public class HeroAttributesView : MonoBehaviour
    {
        //Класс хранит в себе ссылки на объекты отображающие атрибуты
        // При обновлении выбранного класса - обновляем значение текстовых полей, что бы обновить значение атрибутов
        // Пусть отслеживает через поток
        [SerializeField] private DistributePoints[] _statRows;
        [SerializeField] private TextMeshProUGUI _globalPointsText;

        private HeroParamentresUI _paramsUI;
        private HeroClass _lastSelectedHero; // Запоминаем, кто был выбран до этого
        private int _initialBonusPoints = 10; // Сколько очков было изначально

        public void Construct(HeroParamentresUI paramsUI, HeroData heroData, CompositeDisposable windowLifetime)
        {
            _paramsUI = paramsUI;
            _initialBonusPoints = heroData.BonusPoints.Value;

            _paramsUI.Construct(heroData, windowLifetime);

            // Следим за очками
            heroData.BonusPoints
                .Subscribe(p => _globalPointsText.text = $"POINTS: {p}")
                .AddTo(windowLifetime);

            heroData.SelectedClass
           .Subscribe(newHero =>
           {
               // ЛОГИКА СБРОСА ПРИ СМЕНЕ КЛАССА
               if (_lastSelectedHero != null && _lastSelectedHero != newHero)
               {
                   ResetHeroProgress(_lastSelectedHero, heroData);
               }

               if (newHero == null) return;

               foreach (var row in _statRows)
                   row.Bind(newHero, heroData, windowLifetime);

               _lastSelectedHero = newHero;
           })
           .AddTo(windowLifetime);
        }

        // Метод для возврата очков и очистки статов
        private void ResetHeroProgress(HeroClass hero, HeroData data)
        {
            hero.ResetAllAttributes(); // Убираем модификаторы
            data.BonusPoints.Value = _initialBonusPoints; // Возвращаем очки в пул
        }

        // Вызывается при закрытии окна (через кнопку "Назад" или Close)
        public void OnCloseWindow(HeroData data)
        {
            if (_lastSelectedHero != null)
            {
                ResetHeroProgress(_lastSelectedHero, data);
            }
        }
    }
}
