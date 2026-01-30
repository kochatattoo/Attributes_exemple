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

        private readonly CompositeDisposable _heroSubscribers = new();

        public void Construct(HeroDataFabric heroData, CompositeDisposable windowLifetime)
        {
            heroData.SelectedClass
                .Subscribe(hc =>
                {
                    _heroSubscribers.Clear();
                    if (hc == null) return;

                    // Подписываемся на "живые" изменения статов героя
                    Bind(hc, AttributeConstants.STM, val => _health.text = $"Здоровье: {val * 10}");
                    Bind(hc, AttributeConstants.INT, val => _mana.text = $"Мана: {val * 5}");
                    Bind(hc, AttributeConstants.STR, val => _attack.text = $"Атака: {val * 2}");
                    Bind(hc, AttributeConstants.AGI, val => _defence.text = $"Защита: {val * 2}");
                    Bind(hc, AttributeConstants.WIS, val => _evesion.text = $"Уклонение: {val * 2}");
                    Bind(hc, AttributeConstants.STM, val => _resistance.text = $"Сопротивление: {val * 2}");
                })
                .AddTo(windowLifetime);
        }

        private void Bind(HeroClass hc, string attr, System.Action<int> action)
        {
            hc.Attributes[attr].Subscribe(action).AddTo(_heroSubscribers);
        }
    }
}
