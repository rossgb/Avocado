using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Avocado
{
	class MenuEntry
	{
		#region Fields

		Vector2 position;
		float selectionFade;
		string text;

		#endregion

		#region Properties

		public Vector2 Position
		{
			get { return position; }
			set { position = value; }
		}

		public string Text
		{
			get { return text; }
			set { text = value; }
		}

		#endregion

		#region Events

		public event EventHandler<PlayerIndexEventArgs> Selected;

		protected internal virtual void OnSelect(PlayerIndex playerIndex)
		{
			if (this.Selected != null)
			{
				this.Selected(this, new PlayerIndexEventArgs(playerIndex));
			}
		}

		#endregion

		#region Initialization

		public MenuEntry(string text)
		{
			this.text = text;
		}

		#endregion

		#region Update and Draw

		public virtual void Update(MenuScreen screen, bool isSelected, GameTime gameTime)
		{
			float fadeSpeed = (float)gameTime.ElapsedGameTime.TotalSeconds * 4;

			if (isSelected)
			{
				this.selectionFade = Math.Min(this.selectionFade + fadeSpeed, 1);
			}
			else
			{
				this.selectionFade = Math.Max(this.selectionFade - fadeSpeed, 0);
			}
		}

		public virtual void Draw(MenuScreen screen, bool isSelected, GameTime gameTime)
		{
			Color color = isSelected ? Color.Yellow : Color.White;
			color *= screen.TransitionAlpha;
			
			float pulsate = (float) Math.Sin(gameTime.TotalGameTime.TotalSeconds * 6) + 1;
			float scale = 1 + pulsate * 0.05f * this.selectionFade;

			SpriteFont font = screen.ScreenManager.Font;
			Vector2 origin = new Vector2(0, font.LineSpacing / 2);

			screen.ScreenManager.SpriteBatch.DrawString(font, this.text, position, color, 
				0, origin, scale, SpriteEffects.None, 0);
		}

		#endregion

		public virtual int GetHeight(MenuScreen screen)
		{
			return screen.ScreenManager.Font.LineSpacing;
		}

		public virtual int GetWidth(MenuScreen screen)
		{
			return (int)screen.ScreenManager.Font.MeasureString(this.text).X;
		}
	}
}
