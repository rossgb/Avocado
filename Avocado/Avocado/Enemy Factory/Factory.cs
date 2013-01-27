using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace Avocado
{
	public class Factory
	{
		public Enemy grabEnemy(String text,Texture2D texture)
		{
			//string = x y health speed
			string[] tokens = text.Split(' ');
			int x = Convert.ToInt32(tokens[0]);
			int y = Convert.ToInt32(tokens[1]);
			int health = Convert.ToInt32(tokens[2]);
			int speed = Convert.ToInt32(tokens[3]);
			Enemy nextEnemy = new Enemy(texture,new Vector2(x,y),health,speed,50);
			nextEnemy.Radius = 50;
			nextEnemy.Color = Color.White;
            Debug.WriteLine("yay");
			return nextEnemy;
		}
	}
}
