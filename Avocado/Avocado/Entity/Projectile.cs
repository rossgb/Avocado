using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Avocado
{
	class Projectile : Entity
	{
        public int damage;

		public Projectile(Texture2D texture, Vector2 position, float speed, int damage) :
			base(texture, position, speed)
		{
            this.damage = damage;
		}
	}
}
