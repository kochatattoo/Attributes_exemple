using Code.Hero.Attributes;
using System;
using System.Collections.Generic;
using UniRx;
using Attribute = Code.Hero.Attributes.Attribute;

namespace Code.Hero.Data
{
    public class HeroClass: IDisposable
    {
        private readonly CompositeDisposable _disposables = new();

        public int Id { get; }
        public string Name { get; }
        public HeroAttributes Attributes { get; }
        public string Color { get; }
        public string Description { get; }
        public string ImagePath { get; }

        public List<string> Abilities { get; }

        public HeroClass(HeroClassDTO dto)
        {
            Id = dto.id;
            Name = dto.name;
            Color = dto.color;
            Description = dto.description;
            ImagePath = dto.imagePath;
            Abilities = new List<string>(dto.abilities);

            var dict = new Dictionary<string, int>();
            foreach (var entry in dto.attributes)
            {
                dict[entry.key] = entry.value;
            }
            Attributes = new HeroAttributes(dict);
        }

        public HeroClass(int id, 
            string name, 
            Dictionary<string, int> finalizedStats, 
            string color, 
            string description, 
            string imagePath, 
            List<string> abilities)
        {
            Id = id;
            Name = name;
            Color = color;
            Description = description;
            ImagePath = imagePath;
            Abilities = new List<string>(abilities);

            // Создаем новые атрибуты, где базой будут уже увеличенные числа
            Attributes = new HeroAttributes(finalizedStats);
        }

        public void Dispose()
        {
            Attributes?.Dispose();
            _disposables.Dispose();
        }

        public void ResetAllAttributes()
        {
            foreach (var attr in Attributes.AllAttributes) // AllAttributes — список всех Attribute в классе
            {
                Attribute attribute = Attributes.AllAttributes[attr.Key];
                attribute.ResetModifiers();
            }
        }
    }
}

