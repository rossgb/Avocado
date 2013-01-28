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
            this.target.spellType = SpellType.MULTI;
        }
    }
}