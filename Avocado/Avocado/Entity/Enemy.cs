﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;


namespace Avocado
{
	public class Enemy : Entity
	{
		int health;

		public Enemy(Texture2D texture, Vector2 position, int health, float speed, int radius) : 
			base(texture, position, speed, radius)
		{
			this.health = health;
		}
	}
}
