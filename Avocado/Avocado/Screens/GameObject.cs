using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Avocado
{
	interface GameObject
	{
		void Update(GameTime deltaTime);
		void Draw(SpriteBatch spriteBatch);
	}
}
