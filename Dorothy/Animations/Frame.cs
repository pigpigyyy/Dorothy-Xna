using Dorothy.Core;
using Dorothy.Defs;
using Microsoft.Xna.Framework.Graphics;

namespace Dorothy.Animations
{
	/// <summary>
	/// Frame animation is one that sequentially changes the sprite`s draw rectangle.
	/// Each rectangle is an area location on a specific texture.
	/// </summary>
	public class Frame : Component, IAnimation
	{
		#region Field
		private bool _pause;
		private bool _reversing;
		private int _currentFrame;
		private float _current;
		private float _currentKeep;
		private float _currentCount;
		private float _count;
		private float _add = 1.0f;
		#endregion

		/// <summary>
		/// Gets or sets the current progress time in milliseconds.
		/// Animation`s frame updates and presents immediately after the value is set, regardless of the play state.
		/// </summary>
		/// <value>
		/// The current progress time.
		/// </value>
		public float Current
		{
			set
			{
				_current = value / oGame.TargetFrameInterval;
				if (_current < 0.0f)
				{
					_current = 0.0f;
				}
				else if (_current > _count)
				{
					_current = _count;
				}
				float pos = 0.0f;
				for (int i = 0; i < this.FrameDef.Data.Length; i++)
				{
					float cur = pos + this.FrameDef.Data[i].KeepCount;
					if (cur >= _current)
					{
						_currentCount = _current - pos;
						_currentFrame = i;
						_currentKeep = this.FrameDef.Data[i].KeepCount;
						this.Target.DrawRectangle = this.FrameDef.Data[i].DrawRectangle;
						break;
					}
					else { pos = cur; }
				}
			}
			get
			{
				if (_current < 0.0f)
				{
					return 0.0f;
				}
				else if (_current > _count)
				{
					return _count * oGame.TargetFrameInterval;
				}
				else return _current * oGame.TargetFrameInterval;
			}
		}
		/// <summary>
		/// Gets the duration in milliseconds.
		/// </summary>
		public float Duration
		{
			get { return _count * oGame.TargetFrameInterval; }
		}
		/// <summary>
		/// Gets or sets the speed.
		/// 1.0f is normal speed, 0.5f to slow down by half of normal speed, 2.0f to double the speed.
		/// </summary>
		/// <value>
		/// The speed.
		/// </value>
		public float Speed
		{
			set { _add = (value < 0.0f ? 0.0f : value); }
			get { return _add; }
		}
		/// <summary>
		/// Gets or sets a value indicating whether this animation is in loop mode.
		/// When in loop mode, the animation OnEnd event won`t happen.
		/// </summary>
		/// <value>
		///   <c>true</c> if loop; otherwise, <c>false</c>.
		/// </value>
		public bool Loop
		{
			set;
			get;
		}
		/// <summary>
		/// Gets or sets a value indicating whether this animation is in reverse mode.
		/// When in reverse mode, the animation reversely plays right after normal playing.
		/// The OnEnd event happens when reverse play completes.
		/// </summary>
		/// <value>
		///   <c>true</c> if reverse; otherwise, <c>false</c>.
		/// </value>
		public bool Reverse
		{
			set;
			get;
		}
		/// <summary>
		/// Gets a value indicating whether this animation is playing.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this aniamtion is playing; otherwise, <c>false</c>.
		/// </value>
		public bool IsPlaying
		{
			get { return base.Enable; }
		}
		/// <summary>
		/// Gets a value indicating whether this animation is paused.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this animation is paused; otherwise, <c>false</c>.
		/// </value>
		public bool IsPaused
		{
			get { return _pause; }
		}
		/// <summary>
		/// Gets or sets the target.
		/// </summary>
		/// <value>
		/// A sprite instance.
		/// </value>
		public Sprite Target
		{
			set;
			get;
		}
		/// <summary>
		/// Gets the frame animation data.
		/// </summary>
		public FrameDef FrameDef
		{
			private set;
			get;
		}
		/// <summary>
		/// Gets the animation texture.
		/// </summary>
		public Texture2D Texture
		{
			private set;
			get;
		}
		/// <summary>
		/// Occurs when animation plays over.
		/// </summary>
		public event AnimationHandler OnEnd;

		/// <summary>
		/// Initializes a new instance of the <see cref="Frame"/> class.
		/// Its update controller would be initially set to current scene`s controller.
		/// </summary>
		/// <param name="def">The frame animation data, will taken as a reference.</param>
		/// <param name="tex">The frame animation texture.</param>
		public Frame(FrameDef def, Texture2D tex)
			: base(oSceneManager.CurrentScene.Controller)
		{
			this.FrameDef = def;
			this.Texture = tex;
			_count = 0.0f;
			foreach (var d in def.Data)
			{
				_count += d.KeepCount;
			}
		}
		/// <summary>
		/// Plays this animation.
		/// </summary>
		public void Play()
		{
			this.Reset();
			base.Enable = true;
		}
		/// <summary>
		/// Stops the animation.
		/// </summary>
		/// <param name="end">if set to <c>true</c> the animation immediateley jumps to its last frame.</param>
		public void Stop(bool end = false)
		{
			_pause = false;
			_reversing = false;
			if (end)
			{
				if (this.Reverse)
				{
					_currentFrame = 0;
					_currentCount = 0.0f;
					_current = 0.0f;
					this.Target.DrawRectangle = this.FrameDef[0].DrawRectangle;
				}
				else
				{
					_currentFrame = this.FrameDef.Data.Length - 1;
					_currentCount = this.FrameDef[_currentFrame].KeepCount;
					_current = _count;
					this.Target.DrawRectangle = this.FrameDef[_currentFrame].DrawRectangle;
				}
			}
			base.Enable = false;
		}
		/// <summary>
		/// Pauses this animation.
		/// </summary>
		public void Pause()
		{
			_pause = true;
		}
		/// <summary>
		/// Resumes this animation.
		/// </summary>
		public void Resume()
		{
			_pause = false;
		}
		/// <summary>
		/// Sets the animation to first frame.
		/// </summary>
		public void Reset()
		{
			_pause = false;
			_reversing = false;
			_current = 0.0f;
			_currentFrame = 0;
			_currentKeep = 0.0f;
			_currentCount = this.FrameDef[0].KeepCount;
			this.Target.Texture = this.Texture;
			this.Target.DrawRectangle = this.FrameDef[0].DrawRectangle;
		}
		/// <summary>
		/// Updates this animation.
		/// [Called by framework]
		/// </summary>
		public override void Update()
		{
			if (_pause)
			{
				return;
			}
			IAnimation me = (IAnimation)this;
			if (_reversing)
			{
				if (me.Backward())
				{
					_reversing = false;
					this.End();
				}
			}
			else
			{
				if (me.Forward())
				{
					if (this.Reverse)
					{
						_reversing = true;
						me.Backward();
					}
					else this.End();
				}
			}
		}
		/// <summary>
		/// Forward update this animation.
		/// [Called by framework]
		/// </summary>
		/// <returns>
		/// if return <c>true</c> indicate it`s updated to the start; otherwise, <c>false</c>
		/// </returns>
		bool IAnimation.Forward()
		{
			if (_currentKeep < _currentCount)
			{
				_currentKeep += _add;
				_current += _add;
			}
			else
			{
				_currentFrame++;
				if (_currentFrame >= this.FrameDef.Data.Length)
				{
					_current = _count;
					return true;
				}
				else
				{
					_currentKeep = 0.0f;
					_currentCount = this.FrameDef[_currentFrame].KeepCount;
					this.Target.DrawRectangle = this.FrameDef[_currentFrame].DrawRectangle;
				}
			}
			return false;
		}
		/// <summary>
		/// Backward update this animation.
		/// [Called by framework]
		/// </summary>
		/// <returns>
		/// if return <c>true</c> indicate it`s updated to the end; otherwise, <c>false</c>
		/// </returns>
		bool IAnimation.Backward()
		{
			if (_currentKeep > 0.0f)
			{
				_currentKeep -= _add;
				_current -= _add;
			}
			else
			{
				_currentFrame--;
				if (_currentFrame < 0)
				{
					_currentFrame = 0;
					_current = 0.0f;
					return true;
				}
				else
				{
					_currentKeep = this.FrameDef[_currentFrame].KeepCount;
					this.Target.DrawRectangle = this.FrameDef[_currentFrame].DrawRectangle;
				}
			}
			return false;
		}
		/// <summary>
		/// Ends this animation.
		/// </summary>
		private void End()
		{
			if (this.Loop)
			{
				this.Reset();
				((IAnimation)this).Forward();
			}
			else
			{
				base.Enable = false;
				if (OnEnd != null)
				{
					OnEnd(this);
				}
			}
		}
	}
}
