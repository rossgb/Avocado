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
		float speed;

		public Vector2 Direction;
		public Vector2 Position;
		public int Radius;

		// TEMPORARY FOR COLLISION TESTING
		public Rectangle Body;
		public Color Color;

		public Entity(Texture2D texture, Vector2 position, float speed)
		{
			this.Position = position;
			this.Radius = 25;

			this.drawOffset = new Vector2(-this.Radius, -this.Radius);
			this.speed = speed;
			this.texture = texture;

			this.Color = Color.Red;
			this.Body = new Rectangle(0, 0, this.Radius * 2, this.Radius * 2);
		}

		public virtual void Update(GameTime gameTime)
		{
			this.Position += this.Direction * this.speed * gameTime.ElapsedGameTime.Milliseconds;
		}

		public virtual void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(this.texture, this.Position + this.drawOffset, this.Body, this.Color);
		}
	}
}
