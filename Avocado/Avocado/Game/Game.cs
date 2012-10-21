using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Avocado
{
	class AvocadoGame : Game
	{
		GraphicsDeviceManager graphics;
		ScreenManager screenManager;

		public AvocadoGame()
		{
			this.Content.RootDirectory = "Content";
			this.graphics = new GraphicsDeviceManager(this);
			//this.graphics.IsFullScreen = true;
			
			this.screenManager = new ScreenManager(this);
			this.Components.Add(screenManager);
			this.screenManager.AddScreen(new GameplayScreen(this.Content), null);
		}

		protected override void Draw(GameTime gameTime)
		{
			this.graphics.GraphicsDevice.Clear(Color.Black);
			base.Draw(gameTime);
		}
	}

	static class Program
	{
		static void Main()
		{
			using (AvocadoGame game = new AvocadoGame())
			{
				game.Run();
			}
		}
	}
}
