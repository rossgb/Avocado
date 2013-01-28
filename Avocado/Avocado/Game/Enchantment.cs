using Microsoft.Xna.Framework;

namespace Avocado
{
    abstract class Enchantment
    {
        public Player target;
        public int duration;

        public Enchantment(Player target, int duration)
        {
            this.target = target;
            this.duration = duration;
        }

        public abstract void apply();

        public virtual void Update(GameTime time)
        {
            this.duration -= time.ElapsedGameTime.Milliseconds;
        }
    }
}
