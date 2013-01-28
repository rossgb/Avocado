namespace Avocado
{
    class MultiAttackEnchantment : Enchantment
    {
        public MultiAttackEnchantment(Player target, int duration = 10000)
            : base(target, duration)
        {
        }

        public override void apply()
        {
            if (this.target.spellType == SpellType.SINGLE)
            {
                this.target.spellType = SpellType.MULTI;
            }
        }
    }
}