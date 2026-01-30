using Code.Hero.Attributes;
using Code.Hero.Data;
using TMPro;
using UniRx;
using UnityEngine;

namespace Code.UI.MainMenuElements
{
    public class HeroParamentresUI : MonoBehaviour
    {
        //В данном классе логика для расчета значений от полученных характеристик
        //При смене класса происходит перерасчет значений

        [SerializeField] private TextMeshProUGUI _health;
        [SerializeField] private TextMeshProUGUI _mana;
        [SerializeField] private TextMeshProUGUI _attack;
        [SerializeField] private TextMeshProUGUI _defence;
        [SerializeField] private TextMeshProUGUI _evesion;
        [SerializeField] private TextMeshProUGUI _resistance;

        private readonly CompositeDisposable _subscriptions = new();

        /// <summary>
        /// Вызываем для окна создания героя
        /// </summary>
        public void Construct(HeroDataFabric fabric, CompositeDisposable windowLifetime)
        {
            fabric.SelectedClass
                .Subscribe(hc =>
                {
                    if (hc != null) SetTargetHero(hc);
                })
                .AddTo(windowLifetime);

            _subscriptions.AddTo(windowLifetime);
        }

        /// <summary>
        /// Вызываем когда параметры уже созданы
        /// </summary>
        public void Construct(HeroData heroData, CompositeDisposable windowLifetime)
        {
            if (heroData?.Hero != null)
            {
                SetTargetHero(heroData.Hero);
            }
            _subscriptions.AddTo(windowLifetime);
        }


        private void SetTargetHero(HeroClass heroClass)
        {
            _subscriptions.Clear();

            Bind(heroClass, AttributeConstants.STM, val => _health.text = $"Здоровье: {val * 10}");
            Bind(heroClass, AttributeConstants.INT, val => _mana.text = $"Мана: {val * 5}");
            Bind(heroClass, AttributeConstants.STR, val => _attack.text = $"Атака: {val * 2}");
            Bind(heroClass, AttributeConstants.AGI, val => _defence.text = $"Защита: {val * 2}");
            Bind(heroClass, AttributeConstants.WIS, val => _evesion.text = $"Уклонение: {val * 2}");
            Bind(heroClass, AttributeConstants.STM, val => _resistance.text = $"Сопротивление: {val * 2}");
        }

        private void Bind(HeroClass heroClass, string attrName, System.Action<int> action) =>
            heroClass.Attributes[attrName]?.Subscribe(action).AddTo(_subscriptions); // Используем FinalValue, чтобы учитывать и статы, и возможный шмот в будущем

        private void OnDestroy() => _subscriptions.Dispose();
    }
}
