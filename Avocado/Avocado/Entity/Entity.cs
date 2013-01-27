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

		public Vector2 Direction;
		public Vector2 Position;
		public int Radius;
		public float Speed;
		public bool IsMoving;
 
		public Rectangle Body;
		public Color Color;

		public Entity(Texture2D texture, Vector2 position, float speed, int radius)
		{
			this.Position = position;
			this.Radius = radius;
            this.Direction = new Vector2(1.0f, 0.0f);

			this.drawOffset = new Vector2(-this.Radius, -this.Radius);
			this.Speed = speed;
			this.texture = texture;

			this.Color = Color.White;
			this.Body = new Rectangle(0, 0, this.Radius * 2, this.Radius * 2);
			this.IsMoving = true;
		}

		#region Update and Draw

		public virtual void Update(GameTime gameTime)
		{
			if (this.IsMoving)
				this.Position += this.Direction * this.Speed * gameTime.ElapsedGameTime.Milliseconds;
		}

		public virtual void Draw(SpriteBatch spriteBatch)
		{
			//spriteBatch.Draw(this.texture, this.Position + this.drawOffset, this.Body, this.Color);
            spriteBatch.Draw(this.texture, this.Position, this.Body, this.Color, (float)Math.Atan2(Direction.Y, Direction.X), new Vector2(this.Radius, this.Radius), 1.0f, SpriteEffects.None, 0f);
		}

		#endregion
	}
}
