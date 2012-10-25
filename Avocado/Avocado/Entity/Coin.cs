using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Avocado
{
	class Coin : Item
	{
		public Coin(Texture2D texture, Vector2 position, float speed) :
			base(texture, position, speed)
		{
		}
	}
}
