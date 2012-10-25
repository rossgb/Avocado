using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Avocado
{
	abstract class Item : Entity
	{
		public Item(Texture2D texture, Vector2 position, float speed) :
			base(texture, position, speed)
		{
		}
	}
}
