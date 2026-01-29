using Code.Hero.Attributes;
using System;
using System.Collections.Generic;
using UniRx;

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
        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}

