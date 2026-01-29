using System.Collections.Generic;
using System;
using UnityEngine;
using UniRx;

namespace Code.Hero.Attributes
{
    [Serializable]
    public class Attribute: IDisposable
    {
        [field: SerializeField] private int _baseValue;
        [SerializeField] private List<AttributeModifier> _modifiers = new();

        private readonly IntReactiveProperty _finalValue = new();

        public IReadOnlyReactiveProperty<int> FinalValue => _finalValue;
        public int BaseValue => _baseValue;

        public Attribute(int baseValue)
        {
            _baseValue = baseValue;
            _finalValue.Value = RecalculateFinalValue(_baseValue, _modifiers);
        }

        public void Dispose()
        {
            _finalValue.Dispose();
        }

        public void AddModifier(AttributeModifier modifier)
        {
            if (_modifiers.Contains(modifier)) return;
            _modifiers.Add(modifier);

            Recalculate();
        }

        public void RemoveModifier(AttributeModifier modifier)
        {
            if (!_modifiers.Contains(modifier)) return;
            _modifiers.Remove(modifier);

            Recalculate();
        }

        private void Recalculate()
        {
            _finalValue.Value = RecalculateFinalValue(_baseValue, _modifiers);
        }

        private int RecalculateFinalValue(int baseValue, List<AttributeModifier> modifiers)
        {
            float calculatedValue = baseValue;
            float finalPercentage = 1f;

            foreach (AttributeModifier modifier in modifiers) //Первым делом складываем все плоские значения 
            {
                if (modifier.Type == ModifierType.Flat)
                {
                    calculatedValue += modifier.Amount;
                }
            }

            foreach (AttributeModifier modifier in modifiers) // Составляем общий процент умножения характеристики
            {
                if (modifier.Type == ModifierType.Percantage)
                {
                    finalPercentage += (modifier.Amount / 100f);
                }
            }

            calculatedValue = calculatedValue * finalPercentage; // Расчет final = (базовая + сумма плоских)*(сумма процентов) 


            return (int)calculatedValue; // Кастим до целочисленного значения (можно сделать метод, что бы задать логику округления характеристики)
        }
    }
}
