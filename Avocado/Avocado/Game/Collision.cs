using System;
using Microsoft.Xna.Framework;

namespace Avocado
{
	class Collision
	{
		public static void resolve(Player player1, Player player2)
		{

            Vector2 distance = player1.Position - player2.Position;
            float overlap = player1.Radius + player2.Radius - distance.Length();
            double angle = Math.Atan2(distance.Y, distance.X);

            player1.Position.X += (float) (overlap * Math.Cos(angle));
            player1.Position.Y += (float) (overlap * Math.Sin(angle));
            player2.Position.X -= (float) (overlap * Math.Cos(angle));
            player2.Position.Y -= (float) (overlap * Math.Sin(angle));
		}

		public static void resolve(Player player, Enemy enemy)
		{
		}

		public static void resolve(Player player, Item item)
		{
		}

		public static void resolve(Projectile projectile, Enemy enemy)
		{
		}
	}
}
