using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Avocado
{
	class Projectile : Entity
	{

        public int damage;

		public Projectile(Texture2D texture, Vector2 position, float speed, int damage, int radius = 25) :
			base(texture, position, speed, radius)
		{
            this.damage = damage;
		}

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
	}
}
