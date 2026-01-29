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
        [SerializeField] private Button _confirmButton;

        public void Construct(HeroData heroData, CompositeDisposable windowLifetime)
        {
            // Следим за тем, какой герой выбран
            heroData.SelectedClass
                .Where(hero => hero != null)
                .Subscribe(hero => {
                    foreach (var row in _statRows)
                        row.Bind(hero, heroData.BonusPoints, windowLifetime);
                }).AddTo(windowLifetime);

            // Обновляем общий текст свободных очков
            heroData.BonusPoints
                .Subscribe(p => _globalPointsText.text = $"POINTS LEFT: {p}")
                .AddTo(windowLifetime);

            // Кнопка "Окей"
            _confirmButton.onClick.AsObservable()
                .Subscribe(_ => ConfirmDistribution(heroData))
                .AddTo(windowLifetime);
        }

        private void ConfirmDistribution(HeroData data)
        {
            var hero = data.SelectedClass.Value;
            foreach (var row in _statRows)
            {
                if (row.AddedPoints > 0)
                {
                    // Создаем модификатор и применяем его навсегда
                    var mod = new AttributeModifier( ModifierType.Flat, row.AddedPoints);
                    hero.Attributes.AddModifier(row.AttrName, mod);
                }
            }
            Debug.Log("Points Distributed!");
        }
    }
}
