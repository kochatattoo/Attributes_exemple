using Code.Hero.Attributes;
using Code.Hero.Data;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Code.Hero.Items
{
    public class HeroBuffService
    {
        // Список активных баффов для отображения в UI (иконки с таймером)
        public ReactiveCollection<ActiveBuff> ActiveBuffs { get; } = new();

        public void ApplyBuff(HeroClass hero, ConsumableItem item)
        {
            // 1. Создаем модификаторы
            var runtimeModifiers = new List<AttributeModifier>();
            foreach (var modData in item.Modifiers)
            {
                var modifier = new AttributeModifier(modData.Type, modData.Amount);
                hero.Attributes.AddModifier(modData.GetKey(), modifier);
                runtimeModifiers.Add(modifier);
            }

            var buff = new ActiveBuff(item, runtimeModifiers);
            ActiveBuffs.Add(buff);

            // 2. Таймер удаления через UniRx
            Observable.Timer(TimeSpan.FromSeconds(item.Duration))
                .Subscribe(_ => RemoveBuff(hero, buff))
                .AddTo(buff.Disposables);
        }

        private void RemoveBuff(HeroClass hero, ActiveBuff buff)
        {
            // Удаляем модификаторы из атрибутов
            for (int i = 0; i < buff.SourceItem.Modifiers.Count; i++)
            {
                hero.Attributes.RemoveModifier(buff.SourceItem.Modifiers[i].GetKey(), buff.Modifiers[i]);
            }

            ActiveBuffs.Remove(buff);
            buff.Disposables.Dispose();
            Debug.Log($"Эффект от {buff.SourceItem.ItemName} закончился");
        }
    }
}
