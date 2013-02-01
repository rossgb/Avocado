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

		public Avocado(Texture2D texture, Vector2 position, int typeKey, float speed = 0.0f, int radius = 35) :
			base(texture, position, speed, radius)
		{
            switch (typeKey)
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
                default:
                    type = AvocadoType.GHOSTY;
                    break;
            }
		}    
	}
}
