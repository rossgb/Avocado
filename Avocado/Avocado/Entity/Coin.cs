using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Avocado
{
	class Coin : Item
	{
        public int value;

		public Coin(Texture2D texture, Vector2 position, float speed, int value) :
			base(texture, position, speed)
		{
            this.value = value;
		}
	}
}
