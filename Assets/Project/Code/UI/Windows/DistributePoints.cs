using Code.Hero.Attributes;
using Code.Hero.Data;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI.MainMenuElements
{
    public class DistributePoints : MonoBehaviour
    {
        [SerializeField] private string _attrName;

        [Header("UI Elements")]
        [SerializeField] private Button _increaseBtn;
        [SerializeField] private Button _decreaseBtn;
        [SerializeField] private TextMeshProUGUI _attributePointsText;
        // [SerializeField] private TextMeshProUGUI _attributeNameLabel;

        private CompositeDisposable _bindDisposables = new();

        public void Bind(HeroClass hero, HeroData data, CompositeDisposable windowLifetime)
        {
            _bindDisposables.Clear();

            var attribute = hero.Attributes[_attrName];
            if (attribute == null) return;

            // 1. Базовое значение из конфига (чтобы знать, когда подсвечивать желтым)
            int initialBase = attribute.Value;

            // 2. Подписка на изменение значения (отклик на кнопки)
            attribute.Subscribe(currentValue =>
            {
                _attributePointsText.text = currentValue.ToString();
                // Подсветка: если текущее значение больше того, что было в конфиге
                _attributePointsText.color = currentValue > initialBase ? Color.yellow : Color.white;
            }).AddTo(_bindDisposables);

            // 3. Кнопка ПЛЮС
            _increaseBtn.onClick.AsObservable()
                .Where(_ => data.BonusPoints.Value > 0)
                .Subscribe(_ =>
                {
                    data.BonusPoints.Value--;
                    hero.Attributes.AddModifier(_attrName, new AttributeModifier(ModifierType.Flat, 1));
                }).AddTo(_bindDisposables);

            // 4. Кнопка МИНУС
            _decreaseBtn.onClick.AsObservable()
                .Where(_ => attribute.Value > initialBase)
                .Subscribe(_ =>
                {
                    // Нам нужно удалить один плоский модификатор на +1
                    // Для простоты реализации "отката" в рамках этого примера:
                    hero.Attributes.AddModifier(_attrName, new AttributeModifier(ModifierType.Flat, -1));
                    data.BonusPoints.Value++;
                }).AddTo(_bindDisposables);

            _bindDisposables.AddTo(windowLifetime);
        }
    }
}
