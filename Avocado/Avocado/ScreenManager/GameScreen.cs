using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;

namespace Avocado
{

	public enum ScreenState
	{
		TransitionOn,
		Active,
		TransitionOff,
		Hidden,
	}

	public abstract class GameScreen
	{
		#region Fields

		PlayerIndex? controllingPlayer;
			
		bool isExiting = false;
		bool isPopup = false;
		bool otherScreenHasFocus;

		ScreenManager screenManager;
		ScreenState screenState = ScreenState.TransitionOn;

		float transitionPosition = 1.0f;
		TimeSpan transitionOffTime = TimeSpan.Zero;
		TimeSpan transitionOnTime = TimeSpan.Zero;

		#endregion

		#region Properties

		public PlayerIndex? ControllingPlayer
		{
			get { return controllingPlayer; }
			internal set { controllingPlayer = value; }
		}

		public bool IsActive
		{
			get
			{
				return !otherScreenHasFocus &&
					(screenState == ScreenState.TransitionOn || screenState == ScreenState.Active);
			}
		}

		public bool IsExiting
		{
			get { return isExiting; }
			protected internal set { isExiting = value; }
		}

		public bool IsPopup
		{
			get { return isPopup; }
			protected set { isPopup = value; }
		}

		public ScreenManager ScreenManager
		{
			get { return screenManager; }
			internal set { screenManager = value; }
		}

		public ScreenState ScreenState
		{
			get { return screenState; }
			protected set { screenState = value; }
		}

		public float TransitionAlpha
		{
			get { return 1.0f - TransitionPosition; }
		}

		public TimeSpan TransitionOffTime
		{
			get { return transitionOffTime; }
			protected set { transitionOffTime = value; }
		}

		public TimeSpan TransitionOnTime
		{
			get { return transitionOnTime; }
			protected set { transitionOnTime = value; }
		}

		public float TransitionPosition
		{
			get { return transitionPosition; }
			protected set { transitionPosition = value; }
		}

		#endregion

		public virtual void LoadContent() { }
		public virtual void UnloadContent() { }

		public virtual void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
		{
			this.otherScreenHasFocus = otherScreenHasFocus;

			if (this.isExiting)
			{
				this.screenState = ScreenState.TransitionOff;

				if (!this.UpdateTransition(gameTime, this.transitionOffTime, 1))
				{
					this.screenManager.RemoveScreen(this);
				}
			}
			else if (coveredByOtherScreen)
			{
				if (this.UpdateTransition(gameTime, this.transitionOffTime, 1))
				{
					this.screenState = ScreenState.TransitionOff;
				}
				else
				{
					this.screenState = ScreenState.Hidden;
				}
			}
			else
			{
				if (this.UpdateTransition(gameTime, this.transitionOnTime, -1))
				{
					this.screenState = ScreenState.TransitionOn;
				}
				else
				{
					this.screenState = ScreenState.Active;
				}
			}
		}

		bool UpdateTransition(GameTime gameTime, TimeSpan time, int direction)
		{
			float transitionDelta;

			if (time == TimeSpan.Zero)
			{
				transitionDelta = 1;
			}
			else
			{
				transitionDelta = (float) (gameTime.ElapsedGameTime.TotalMilliseconds / time.TotalMilliseconds);
			}

			this.transitionPosition += transitionDelta * direction;

			if ((direction < 0 && transitionPosition <= 0) ||
				(direction > 0 && transitionPosition >= 1))
			{
				this.transitionPosition = MathHelper.Clamp(this.transitionPosition, 0, 1);
				return false;
			}

			// Otherwise we are still busy transitioning.
			return true;
		}

		public virtual void HandleInput(InputState input) { }
		public virtual void Draw(GameTime gameTime) { }

		public void ExitScreen()
		{
			if (TransitionOffTime == TimeSpan.Zero)
			{
				// If the screen has a zero transition time, remove it immediately.
				this.screenManager.RemoveScreen(this);
			}
			else
			{
				// Otherwise flag that it should transition off and then exit.
				this.isExiting = true;
			}
		}
	}

}
