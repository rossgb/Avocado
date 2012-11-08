using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Avocado
{
	class AvocadoGame : Game
	{
		#region Fields

		GraphicsDeviceManager graphics;
		ScreenManager screenManager;

		#endregion

		public AvocadoGame()
		{
			this.Content.RootDirectory = "Content";

			this.graphics = new GraphicsDeviceManager(this);

			this.graphics.IsFullScreen = true;
			this.graphics.PreferredBackBufferHeight = 768;
			this.graphics.PreferredBackBufferWidth = 1024;

			this.screenManager = new ScreenManager(this);
			this.Components.Add(screenManager);
			
			// begin game at main menu
			this.screenManager.AddScreen(new MainMenuScreen(), null);
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
