using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Avocado
{
	
	class Enchantment : Item
	{
		public Enchantment(Texture2D texture, Vector2 position, float speed, int radius) :
			base(texture, position, speed, radius)
		{
		}
	}
}
