using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Avocado
{
	class Projectile : Entity
	{
		public Projectile(Texture2D texture, Vector2 position, float speed) :
			base(texture, position, speed)
		{
		}
	}
}
