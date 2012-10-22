using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;


namespace Avocado
{

	public class Enemy : Entity
	{
		int health;

		public Enemy(Texture2D texture, int velocity, int health) : 
			base(texture, velocity)
		{
			this.health = health;
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
		}
	}
}
