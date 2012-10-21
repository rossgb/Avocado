using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Avocado
{
	public class ScreenManager : DrawableGameComponent
	{
		InputState input = new InputState();

		List<GameScreen> screens = new List<GameScreen>();
		List<GameScreen> screensToUpdate = new List<GameScreen>();

		Texture2D blankTexture;
		SpriteFont font;
		SpriteBatch spriteBatch;

		bool isInitialized;
		bool traceEnabled;

		public SpriteFont Font
		{
			get { return font; }
		}

		public SpriteBatch SpriteBatch
		{
			get { return spriteBatch; }
		}

		public bool TraceEnabled
		{
			get { return traceEnabled; }
			set { traceEnabled = value; }
		}

		public ScreenManager(Game game)
			: base(game)
		{
		}

		public override void Initialize()
		{
			base.Initialize();

			this.isInitialized = true;
		}

		protected override void LoadContent()
		{
			ContentManager content = this.Game.Content;

			this.blankTexture = content.Load<Texture2D>("blank");
			this.font = content.Load<SpriteFont>("menufont");
			this.spriteBatch = new SpriteBatch(GraphicsDevice);

			foreach (GameScreen screen in screens)
			{
				screen.LoadContent();
			}
		}

		protected override void UnloadContent()
		{
			foreach (GameScreen screen in screens)
			{
				screen.UnloadContent();
			}
		}

		public override void Update(GameTime gameTime)
		{
			input.Update();
			screensToUpdate.Clear();

			foreach (GameScreen screen in this.screens)
			{
				this.screensToUpdate.Add(screen);
			}

			bool coveredByOtherScreen = false;
			bool otherScreenHasFocus = !Game.IsActive;

			while (this.screensToUpdate.Count > 0)
			{
				int i = this.screensToUpdate.Count - 1;
				GameScreen screen = this.screensToUpdate[i];
				this.screensToUpdate.RemoveAt(i);

				screen.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

				if (screen.ScreenState == ScreenState.TransitionOn ||
					screen.ScreenState == ScreenState.Active)
				{
					if (!otherScreenHasFocus)
					{
						screen.HandleInput(input);
						otherScreenHasFocus = true;
					}

					if (!screen.IsPopup)
					{
						coveredByOtherScreen = true;
					}
				}
			}

			if (this.traceEnabled)
			{
				TraceScreens();
			}
		}

		void TraceScreens()
        {
            List<string> screenNames = new List<string>();

            foreach (GameScreen screen in screens)
                screenNames.Add(screen.GetType().Name);

            Debug.WriteLine(string.Join(", ", screenNames.ToArray()));
        }

        public override void Draw(GameTime gameTime)
        {
            foreach (GameScreen screen in screens)
            {
                if (screen.ScreenState == ScreenState.Hidden)
                    continue;

                screen.Draw(gameTime);
            }
        }

        public void AddScreen(GameScreen screen, PlayerIndex? controllingPlayer)
        {
            screen.ControllingPlayer = controllingPlayer;
			screen.IsExiting = false;
			screen.ScreenManager = this;

            if (this.isInitialized)
            {
                screen.LoadContent();
            }

            this.screens.Add(screen);
        }

        public void RemoveScreen(GameScreen screen)
        {
            if (this.isInitialized)
            {
                screen.UnloadContent();
            }

            this.screens.Remove(screen);
            this.screensToUpdate.Remove(screen);
        }

        public GameScreen[] GetScreens()
        {
            return this.screens.ToArray();
        }

        public void FadeBackBufferToBlack(float alpha)
        {
            Viewport viewport = GraphicsDevice.Viewport;
			Rectangle destination = new Rectangle(0, 0, viewport.Width, viewport.Height);
            
			this.spriteBatch.Begin();
            this.spriteBatch.Draw(blankTexture, destination, Color.Black * alpha);
            this.spriteBatch.End();
        }
	}
}
