using Dorothy.Core;

namespace Dorothy.Animations
{
	/// <summary>
	/// It does nothing but to wait for specific time to complete.
	/// Use it to make space between two animation when in animation sequences.
	/// </summary>
	public class Idle : Component, IAnimation
	{
		#region Field
		private bool _pause;
		private bool _reversing;
		private float _add = 1.0f;
		private float _current;
		private float _count;
		private float _duration;
		#endregion

		/// <summary>
		/// Gets or sets the current progress time in milliseconds.
		/// Animation`s frame immediate updates after the value is set, regardless the play state.
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
			}
			get { return _current * oGame.TargetFrameInterval; }
		}
		/// <summary>
		/// Gets the duration in milliseconds.
		/// </summary>
		public float Duration
		{
			set
			{
				_duration = value;
				_count = value / oGame.TargetFrameInterval;
			}
			get { return _duration; }
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
		/// Occurs when animation plays over.
		/// </summary>
		public event AnimationHandler OnEnd;

		/// <summary>
		/// Initializes a new instance of the <see cref="Idle"/> class.
		/// Its update controller would be initially set to current scene`s controller.
		/// </summary>
		/// <param name="duration">The duration in milliseconds.</param>
		public Idle(float duration)
			: base(oSceneManager.CurrentScene.Controller)
		{
			this.Duration = duration;
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
					_current = 0.0f;
				}
				else
				{
					_current = _count;
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
			if (_current < _count)
			{
				_current += _add;
			}
			else
			{
				_current = _count;
				return true;
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
			if (_current > 0.0f)
			{
				_current -= _add;
			}
			else
			{
				_current = 0.0f;
				return true;
			}
			return false;
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
