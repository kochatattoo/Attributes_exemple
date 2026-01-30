using Code.Hero.Items;

namespace Code.Hero.Data
{
    public class HeroData
    {
        public HeroClass Hero {  get; }
        public HeroEquipmentService Equipment { get; }
        public HeroBuffService Buffs { get; }

        public HeroData (HeroClass heroClass)
        {
            Hero = heroClass;
            Equipment = new HeroEquipmentService ();
            Buffs = new HeroBuffService();
        }
    }
}

