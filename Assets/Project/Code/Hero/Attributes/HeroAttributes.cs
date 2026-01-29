using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Code.Hero.Attributes
{
    public class HeroAttributes: IDisposable
    {
        private readonly Dictionary<string, Attribute> _attributes = new();
        private readonly Dictionary<string, ReactiveProperty<int>> _reactiveValues = new();

        private readonly CompositeDisposable _disposables = new();

        public IReadOnlyDictionary<string, Attribute> AllAttributes => _attributes;
        public IReadOnlyReactiveProperty<int> this[string name]
        {
            get
            {
                if (_reactiveValues.TryGetValue(name, out var prop))
                    return prop;
                Debug.LogError($"Attribute '{name}' not found");
                return null;
            }
        }

        public HeroAttributes(Dictionary<string, int> baseValues) 
        {
            foreach (KeyValuePair<string, int> kvp in baseValues)
            {
                Attribute attribute = new Attribute(kvp.Value);
                _attributes.Add(kvp.Key, attribute);

                ReactiveProperty<int> reactProperty = new ReactiveProperty<int>(attribute.FinalValue.Value);
                _reactiveValues.Add(kvp.Key, reactProperty);

                // Подписываемся на изменение FinalValue
                attribute.FinalValue
                    .Subscribe(newValue => reactProperty.Value = newValue)
                    .AddTo(_disposables);
            }
        }

        public void Dispose()
        {
            _disposables.Dispose();

            foreach (var prop in _reactiveValues.Values)
            {
                prop.Dispose();
            }

            foreach (var attr in _attributes.Values)
            {
                attr.Dispose();
            }

            _attributes.Clear();
            _reactiveValues.Clear();
        }

        public void AddModifier(string attributeName, AttributeModifier modifier)
        {
            if (_attributes.TryGetValue(attributeName, out var attr))
                attr.AddModifier(modifier); 
        }

        public void RemoveModifier(string attributeName, AttributeModifier modifier)
        {
            if (_attributes.TryGetValue(attributeName, out var attr))
                attr.RemoveModifier(modifier);
        }

        public int GetValue(string name) => _attributes[name].FinalValue.Value;
  
    }
}
