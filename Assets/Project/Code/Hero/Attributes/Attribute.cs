using System.Collections.Generic;
using System;
using UnityEngine;
using UniRx;

namespace Code.Hero.Attributes
{
    [Serializable]
    public class Attribute: IDisposable
    {
        [SerializeField] private int _baseValue;
        [SerializeField] private List<AttributeModifier> _modifiers = new();

        private readonly IntReactiveProperty _finalValue = new();

        public IReadOnlyReactiveProperty<int> FinalValue => _finalValue;
        public int BaseValue => _baseValue;

        public Attribute(int baseValue)
        {
            _baseValue = baseValue;
            Recalculate();
        }

        public void Dispose() => 
            _finalValue.Dispose();

        /// <summary>
        /// Метод для повышения базового значения атрибута (при повышении уровня/глобальное изменение)
        /// </summary>
        /// <param name="amount">значение</param>
        public void AddBaseValue(int amount)
        {
            if (amount == 0) return;
            _baseValue += amount;
            Recalculate();
        }

        /// <summary>
        /// Добавить модификатор характеристики в коллекцию
        /// </summary>
        /// <param name="modifier">модификатор</param>
        public void AddModifier(AttributeModifier modifier)
        {
            if (_modifiers.Contains(modifier)) return;
            _modifiers.Add(modifier);

            Recalculate();
        }

        /// <summary>
        /// Убрать модификатор из коллекции
        /// </summary>
        /// <param name="modifier">модификатор</param>
        public void RemoveModifier(AttributeModifier modifier)
        {
            if (_modifiers.Remove(modifier))
               Recalculate();
        }

        /// <summary>
        /// Сбросить все модификаторы, вернутся к базовому значению
        /// </summary>
        public void ResetModifiers()
        {
            _modifiers.Clear(); // Удаляем все бонусы
            Recalculate();
        }

        private void Recalculate()
        {
            _finalValue.Value = RecalculateFinalValue(_baseValue, _modifiers);
        }

        private int RecalculateFinalValue(int baseValue, List<AttributeModifier> modifiers)
        {
            float calculatedValue = baseValue;
            float additiveSum = 0f;      // Для положительных (баффы)
            float multiplicativeProduct = 1f; // Для отрицательных (дебаффы)

            foreach (AttributeModifier modifier in modifiers) 
            {
                if (modifier.Type == ModifierType.Flat)
                {
                    calculatedValue += modifier.Amount; //Первым делом складываем все плоские значения 
                }
                else if (modifier.Type == ModifierType.Percantage)
                {
                    if (modifier.Amount >=0)
                    {
                        additiveSum += (modifier.Amount / 100f); 
                    }
                    else
                    {
                        multiplicativeProduct *= (1f + (modifier.Amount / 100f)); 
                    }
                }
            }

            calculatedValue = (calculatedValue * (1f + additiveSum)) * multiplicativeProduct; // Расчет final = (базовая + сумма плоских)*(сумма процентов) 
            int result = Mathf.RoundToInt(calculatedValue); // Округляем до ближайшего int значения

            return result; 
        }
    }
}
