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

        private CompositeDisposable _classSubscribers = new();

        public void Construct(HeroData heroData)
        {
            heroData.SelectedClass
                .Subscribe(hc =>
                {
                    _classSubscribers.Clear(); // Отписываемся от статов старого героя
                    if (hc == null) return;

                    // Подписываемся на каждое изменение нужных атрибутов
                    // Теперь при нажатии "+" в распределении очков, этот код сработает мгновенно

                    BindParam(hc, AttributeConstants.STM, val => _health.text = $"Здоровье: {val * 10}");
                    BindParam(hc, AttributeConstants.INT, val => _mana.text = $"Мана: {val * 5}");
                    BindParam(hc, AttributeConstants.STR, val => _attack.text = $"Атака: {val * 2}");
                    BindParam(hc, AttributeConstants.AGI, val => _defence.text = $"Защита: {val * 2}");
                    BindParam(hc, AttributeConstants.WIS, val => _evesion.text = $"Уклонение: {val * 2}");
                    BindParam(hc, AttributeConstants.STM, val => _resistance.text = $"Сопротивление: {val * 2}");
                    // STM используется дважды, это нормально — обе подписки будут работать
                })
                .AddTo(this);
        }

        private void BindParam(HeroClass hc, string attrName, System.Action<int> onUpdate)
        {
            var attr = hc.Attributes[attrName];
            if (attr != null)
            {
                attr.Subscribe(onUpdate).AddTo(_classSubscribers);
            }
        }

        private void OnDestroy() => _classSubscribers.Dispose();
    }
}
