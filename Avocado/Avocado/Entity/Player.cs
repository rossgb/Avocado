using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;


namespace Avocado
{

    public class Player : Entity
    {
        int health;

        public Player(Texture2D texture, int velocity, int health)
            : base(texture, velocity)
        {
            this.health = health;
        }

        public override void Update(GameTime gameTime)
        {

          
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {


            base.Draw(spriteBatch);
        }

        public void HandleInput(InputState input)
        {
			this.position.X += input.CurrentGamePadStates[0].ThumbSticks.Left.X * this.velocity;
			this.position.Y += input.CurrentGamePadStates[0].ThumbSticks.Left.Y * this.velocity;


        }
    }
}