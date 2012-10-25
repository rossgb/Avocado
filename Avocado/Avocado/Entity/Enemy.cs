using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;


namespace Avocado
{
	public class Enemy : Entity
	{
		int health;

		public Enemy(Texture2D texture, Vector2 position, int health, float speed) : 
			base(texture, position, speed)
		{
			this.health = health;
		}
	}
}
