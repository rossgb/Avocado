namespace Avocado
{
    class GhostyEnchantment : Enchantment
    {
        public GhostyEnchantment(Player target, int duration = 10000)
            : base(target, duration)
        {
        }

        public override void apply()
        {
            this.target.ghosty = true;
            this.target.Speed *= 1.3f;
        }
    }
}
