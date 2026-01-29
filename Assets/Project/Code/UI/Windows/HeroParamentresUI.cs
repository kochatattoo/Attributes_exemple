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

        public void Construct(HeroData heroData)
        {
            heroData.SelectedClass
                .Subscribe(hc =>
                {
                    if (hc == null) return;

                    _health.text = $"Здоровье :{hc.Attributes[AttributeConstants.STM].Value * 10}";
                    _mana.text = $"Мана :{hc.Attributes[AttributeConstants.INT].Value * 5}";
                    _attack.text = $"Атака :{hc.Attributes[AttributeConstants.STR].Value * 2}";
                    _defence.text = $"Защита :{hc.Attributes[AttributeConstants.AGI].Value * 2}";
                    _evesion.text = $"Уклонение :{hc.Attributes[AttributeConstants.WIS].Value * 2}";
                    _resistance.text = $"Сопротивление :{hc.Attributes[AttributeConstants.STM].Value * 2}";
                })
                .AddTo(this);
        }
    }
}
