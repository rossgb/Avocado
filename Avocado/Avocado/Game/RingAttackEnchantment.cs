namespace Avocado
{
    class RingAttackEnchantment : Enchantment
    {
        public RingAttackEnchantment(Player target, int duration = 10000)
            : base(target, duration)
        {
        }

        public override void apply()
        {
            this.target.spellType = SpellType.RING;
        }
    }
}
