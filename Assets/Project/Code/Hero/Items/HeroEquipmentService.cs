using Code.Hero.Attributes;
using Code.Hero.Data;
using System.Collections.Generic;
using UniRx;

namespace Code.Hero.Items
{
    public class HeroEquipmentService
    {
        // Ключ - Слот, Значение - Текущий предмет в нем
        private readonly ReactiveDictionary<EquipmentSlot, EquipmentItem> _slots = new();
        // Для хранения модификаторов, чтобы их можно было удалить при снятии
        private readonly Dictionary<EquipmentItem, List<AttributeModifier>> _activeModifiers = new();

        public IReadOnlyReactiveDictionary<EquipmentSlot, EquipmentItem> Slots => _slots;

        public void Equip(HeroClass hero, EquipmentItem item)
        {
            // 1. Если в этом слоте уже что-то есть — сначала снимаем
            if (_slots.TryGetValue(item.Slot, out var oldItem))
            {
                Unequip(hero, oldItem);
            }

            // 2. Накладываем новые модификаторы
            var runtimeModifiers = new List<AttributeModifier>();
            foreach (var modData in item.Modifiers)
            {
                var modifier = new AttributeModifier( modData.Type, modData.Amount);
                hero.Attributes.AddModifier(modData.GetKey(), modifier);
                runtimeModifiers.Add(modifier);
            }

            // 3. Записываем в систему
            _activeModifiers.Add(item, runtimeModifiers);
            _slots.Add(item.Slot, item);
        }

        public void Unequip(HeroClass hero, EquipmentItem item)
        {
            if (!_activeModifiers.TryGetValue(item, out var modifiers)) return;

            // Удаляем модификаторы из атрибутов
            for (int i = 0; i < item.Modifiers.Count; i++)
            {
                hero.Attributes.RemoveModifier(item.Modifiers[i].GetKey(), modifiers[i]);
            }

            _activeModifiers.Remove(item);
            _slots.Remove(item.Slot);
        }

        public bool IsEquipped(EquipmentItem item)
        {
            return _slots.TryGetValue(item.Slot, out var equipped) && equipped == item;
        }
    }
}
