using System;
using System.Collections.Generic;

namespace Code.Hero.Data
{
    [Serializable]
    public class HeroClassDTO
    {
        public int id;
        public string name;
        public string color;
        public string description;
        public string imagePath;   // путь к Sprite
        public List<string> abilities;
    }
}

