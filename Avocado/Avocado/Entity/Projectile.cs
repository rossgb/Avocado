using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Avocado
{
	class Projectile : Entity
	{
        public int damage;

		public Projectile(Texture2D texture, Vector2 position, float speed, int damage, Vector2 direction, int radius = 25) :
			base(texture, position, speed, radius)
		{
            this.damage = damage;
            this.Direction = direction;
            this.IsMoving = true;
		}

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
	}
}
