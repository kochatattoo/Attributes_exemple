using System.Collections.Generic;

namespace Code.Hero.Data
{
    public class HeroClass
    {
        public int Id { get; }
        public string Name { get; }
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
        }
    }
}

