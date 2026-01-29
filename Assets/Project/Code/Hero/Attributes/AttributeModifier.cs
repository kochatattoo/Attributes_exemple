namespace Code.Hero.Attributes
{
    public class AttributeModifier
    {
        public ModifierType Type;
        private readonly int _amount;

        public int Amount => _amount;

        public AttributeModifier(ModifierType type, int amount)
        {
            Type = type;
            _amount = amount;
        }
    }
}
