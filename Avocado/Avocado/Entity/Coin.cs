using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Avocado
{
	class Coin : Item
	{
        public int value;

		public Coin(Texture2D texture, Vector2 position, int radius = 17, int value = 1, float speed = 0.9f) :
			base(texture, position, speed, radius)
		{
            this.value = value;
		}

        public override void Update(GameTime gameTime)
        {
            if (this.Speed != 0)
            {
                base.Speed /=1.1f;
            }
            base.Update(gameTime);
        }
	}
}
