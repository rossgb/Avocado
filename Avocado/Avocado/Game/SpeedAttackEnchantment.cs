namespace Avocado
{
    class SpeedAttackEnchantment : Enchantment
    {
        public SpeedAttackEnchantment(Player target, int duration = 10000)
            : base(target, duration)
        {
        }

        public override void apply()
        {
            this.target.reloadTime /= 2;
        }
    }
}
