using Code.Hero.Attributes;
using Code.Hero.Data;
using TMPro;
using UniRx;
using UnityEngine;

namespace Code.UI.HeroWindow
{
    public class HeroAttributesUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _stranghtFieldPoint;
        [SerializeField] private TextMeshProUGUI _agileFieldPoint;
        [SerializeField] private TextMeshProUGUI _intelligenceFieldPoint;
        [SerializeField] private TextMeshProUGUI _wisdomFieldPoint;
        [SerializeField] private TextMeshProUGUI _staminaFieldPoint;

        private bool _isAddedToLifetime = false;

        private readonly CompositeDisposable _disposables = new CompositeDisposable();

        public void Construct(HeroData heroData, CompositeDisposable windowLifetime)
        {
            _disposables.Clear();

            if (heroData?.Hero == null) return;

            var attributes = heroData.Hero.Attributes;

            // Связываем каждое поле с его атрибутом
            BindAttribute(attributes, AttributeConstants.STR, _stranghtFieldPoint);
            BindAttribute(attributes, AttributeConstants.AGI, _agileFieldPoint);
            BindAttribute(attributes, AttributeConstants.INT, _intelligenceFieldPoint);
            BindAttribute(attributes, AttributeConstants.WIS, _wisdomFieldPoint);
            BindAttribute(attributes, AttributeConstants.STM, _staminaFieldPoint);

            if (!_isAddedToLifetime)
            {
                _disposables.AddTo(windowLifetime);
                _isAddedToLifetime = true;
            }
        }

        private void BindAttribute(HeroAttributes attributes, string attrName, TextMeshProUGUI field)
        {
            var prop = attributes[attrName];
            if (prop != null)
            {
                prop.Subscribe(value => field.text = value.ToString())
                    .AddTo(_disposables);
            }
            else
            {
                Debug.LogWarning($"[HeroAttributesUI] Attribute {attrName} not found!");
            }
        }

        private void OnDestroy()
        {
            _disposables.Dispose();
        }
    }
}

