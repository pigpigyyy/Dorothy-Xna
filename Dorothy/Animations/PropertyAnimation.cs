using Dorothy.Core;

namespace Dorothy.Animations
{
	/// <summary>
	/// This animation is to gradually change an instance`s property in float value with ease function.
	/// Implements it to make a new property animation.
	/// </summary>
	public abstract class PropertyAnimation : Component, IAnimation
	{
		#region Field
		private bool _pause;
		private bool _reversing;
		private float _current;
		private float _duration;
		private float _change;
		private float _from;
		private float _add = 1.0f;
		private float _count;
		private Easer _easer = Easer.NoEasing;
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
				this.SetProperty(_easer.Func(_current, _from, _change, _count));
			}
			get
			{
				if (_current < 0.0f)
				{
					return 0.0f;
				}
				else if (_current > _count)
				{
					return _duration;
				}
				else return _current * oGame.TargetFrameInterval;
			}
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
		/// Gets or sets the easer.
		/// </summary>
		/// <value>
		/// The easer.
		/// </value>
		public Easer Easer
		{
			set { _easer = value; }
			get { return _easer; }
		}
		/// <summary>
		/// Gets or sets the begin value.
		/// </summary>
		/// <value>
		/// The begin value.
		/// </value>
		public float From
		{
			set { _from = value; }
			get { return _from; }
		}
		/// <summary>
		/// Gets or sets the end value.
		/// </summary>
		/// <value>
		/// The end value.
		/// </value>
		public float To
		{
			set { _change = value - _from; }
			get { return _from + _change; }
		}
		/// <summary>
		/// Occurs when animation plays over.
		/// </summary>
		public event AnimationHandler OnEnd;

		/// <summary>
		/// Initializes a new instance of the <see cref="PropertyAnimation"/> class.
		/// Its update controller would be initially set to current scene`s controller.
		/// </summary>
		/// <param name="from">The begin value.</param>
		/// <param name="to">The end value.</param>
		/// <param name="duration">The duration in millisecends.</param>
		public PropertyAnimation(float from, float to, float duration)
			: base(oSceneManager.CurrentScene.Controller)
		{
			this.From = from;
			this.To = to;
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
					this.SetProperty(_from);
				}
				else
				{
					_current = _count;
					this.SetProperty(this.To);
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
			this.SetProperty(_from);
		}
		/// <summary>
		/// Updates this animation set.
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
			if (_current < _count)
			{
				this.SetProperty(_easer.Func(_current, _from, _change, _count));
				_current += _add;
			}
			else
			{
				this.SetProperty(this.To);
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
				this.SetProperty(_easer.Func(_current, _from, _change, _count));
				_current -= _add;
			}
			else
			{
				this.SetProperty(_easer.Func(_current, _from, _change, _count));
				_current = 0.0f;
				return true;
			}
			return false;
		}
		/// <summary>
		/// Ends this instance.
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
		/// <summary>
		/// Interface for setting the target property.
		/// Implements it and the specific property will be set in animation.
		/// </summary>
		/// <param name="value">The changed value to set.</param>
		protected abstract void SetProperty(float value);
	}
}
