using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;


namespace Avocado
{
	public abstract class Entity
	{
		Texture2D texture;
		Vector2 drawOffset;
        float rotation;

		public Vector2 Direction;
		public Vector2 Position;
		public int Radius;
		public float Speed;
 
		public Rectangle Body;
		public Color Color;

		public Entity(Texture2D texture, Vector2 position, float speed, int radius)
		{
			this.Position = position;
			this.Radius = radius;

            rotation = (float)Math.Atan2(Direction.Y , Direction.X);

			this.drawOffset = new Vector2(-this.Radius, -this.Radius);
			this.Speed = speed;
			this.texture = texture;

			this.Color = Color.White;
			this.Body = new Rectangle(0, 0, this.Radius * 2, this.Radius * 2);
		}

		#region Update and Draw

		public virtual void Update(GameTime gameTime)
		{
            rotation = (float)Math.Atan2(Direction.Y, Direction.X);
			this.Position += this.Direction * this.Speed * gameTime.ElapsedGameTime.Milliseconds;
		}

		public virtual void Draw(SpriteBatch spriteBatch)
		{
			//spriteBatch.Draw(this.texture, this.Position + this.drawOffset, this.Body, this.Color);
            spriteBatch.Draw(this.texture, this.Position, this.Body, this.Color, rotation,new Vector2(this.Radius,this.Radius), 1.0f, SpriteEffects.None, 0f);
		}

		#endregion
	}
}
