using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Avocado
{
	abstract class MenuScreen : GameScreen
	{
		#region Fields

		List<MenuEntry> menuEntries = new List<MenuEntry>();
		string menuTitle;
		int selectedEntry = 0;

		#endregion

		#region Properties

		protected IList<MenuEntry> MenuEntries
		{
			get { return menuEntries; }
		}

		#endregion

		public MenuScreen(string menuTitle)
		{
			this.menuTitle = menuTitle;
			this.TransitionOffTime = TimeSpan.FromSeconds(0.5);
			this.TransitionOnTime = TimeSpan.FromSeconds(0.5);
		}

		public override void HandleInput(InputState input)
		{
			if (input.IsMenuUp(this.ControllingPlayer))
			{
				this.selectedEntry = this.selectedEntry == 0 ? 
					this.menuEntries.Count - 1 : this.selectedEntry - 1;
			}

			if (input.IsMenuDown(this.ControllingPlayer))
			{
				this.selectedEntry = this.selectedEntry == this.menuEntries.Count - 1 ?
					0 : this.selectedEntry + 1;
			}

			PlayerIndex playerIndex;

			if (input.IsMenuSelect(this.ControllingPlayer, out playerIndex))
			{
				this.OnSelect(this.selectedEntry, playerIndex);
			}
			else if (input.IsMenuCancel(this.ControllingPlayer, out playerIndex))
			{
				this.OnCancel(playerIndex);
			}
		}

		protected virtual void OnSelect(int entryIndex, PlayerIndex playerIndex)
		{
			this.menuEntries[entryIndex].OnSelect(playerIndex);
		}

		protected virtual void OnCancel(PlayerIndex playerIndex)
		{
			this.ExitScreen();
		}

		protected void OnCancel(object sender, PlayerIndexEventArgs e)
		{
			this.OnCancel(e.PlayerIndex);
		}

		protected virtual void UpdateMenuEntryLocations()
		{
			float transitionOffset = (float)Math.Pow(this.TransitionPosition, 2);
			Vector2 position = new Vector2(0.0f, this.ScreenManager.GraphicsDevice.Viewport.Height / 2);

			foreach (MenuEntry menuEntry in this.menuEntries)
			{
				position.X = this.ScreenManager.GraphicsDevice.Viewport.Width / 4 - menuEntry.GetWidth(this) / 2;

				if (this.ScreenState == ScreenState.TransitionOn)
				{
					position.X -= transitionOffset * 256;
				}
				else
				{
					position.X += transitionOffset * 512;
				}

				menuEntry.Position = position;
				position.Y += menuEntry.GetHeight(this);
			}
		}

		public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
		{
			base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

			for (int i = 0; i < this.menuEntries.Count; i++)
			{
				bool isSelected = this.IsActive && (i == selectedEntry);
				this.menuEntries[i].Update(this, isSelected, gameTime);
			}
		}

		public override void Draw(GameTime gameTime)
		{
			this.UpdateMenuEntryLocations();

			for (int i = 0; i < this.menuEntries.Count; i++)
			{
				bool isSelected = this.IsActive && (i == this.selectedEntry);
				this.menuEntries[i].Draw(this, isSelected, gameTime);
			}

			float transitionOffset = (float) Math.Pow(this.TransitionPosition, 2);
			Vector2 titlePosition = new Vector2(this.ScreenManager.GraphicsDevice.Viewport.Width / 4,
				this.ScreenManager.GraphicsDevice.Viewport.Height / 3 - transitionOffset * 100);
			Vector2 titleOrigin = this.ScreenManager.Font.MeasureString(menuTitle) / 2;
			Color titleColor = Color.White * this.TransitionAlpha;

			this.ScreenManager.SpriteBatch.DrawString(this.ScreenManager.Font, this.menuTitle, 
				titlePosition, titleColor, 0, titleOrigin, 2.0f, SpriteEffects.None, 0);
		}
	}
}
