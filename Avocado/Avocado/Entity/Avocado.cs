using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Avocado
{
    public enum AvocadoType
    {
        MULTIATTACK,
        RINGATTACK,
        SPEEDATTACK,
        GHOSTY
    }

	class Avocado : Item
	{
        public AvocadoType type;

		public Avocado(Texture2D texture, Vector2 position, float speed, int radius) :
			base(texture, position, speed, radius)
		{
            switch (new Random().Next(4))
            {
                case 0:
                    type = AvocadoType.MULTIATTACK;
                    break;
                case 1:
                    type = AvocadoType.RINGATTACK;
                    break;
                case 2:
                    type = AvocadoType.SPEEDATTACK;
                    break;
                case 3:
                    type = AvocadoType.GHOSTY;
                    break;
            }
		}    
	}
}
