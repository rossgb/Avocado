using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Avocado
{
	class Coin : Item
	{
        public int value;

		public Coin(Texture2D texture, Vector2 position, float speed = 0.9f, int value = 1,int radius = 10) :
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
