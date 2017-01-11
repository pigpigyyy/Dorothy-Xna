using System.Collections.Generic;
using Dorothy.Core;

namespace Dorothy.Animations
{
	/// <summary>
	/// A set contains animations which can be used as one animation.
	/// </summary>
	public class Parallel : Component, IAnimation
	{
		#region Field
		private bool _loop;
		private bool _pause;
		private bool _reversing;
		private bool _reverse;
		private float _add = 1.0f;
		private float _current;
		private float _count;
		private float _duration;
		private List<IAnimation> _animationList = new List<IAnimation>();
		private int _animationCount;
		#endregion

		/// <summary>
		/// Gets or sets the speed.
		/// 1.0f is normal speed, 0.5f to slow down by half of normal speed, 2.0f to double the speed.
		/// </summary>
		/// <value>
		/// The speed.
		/// </value>
		public float Speed
		{
			set
			{
				_add = (value < 0.0f ? 0.0f : value);
				for (int i = 0; i < _animationList.Count; i++)
				{
					_animationList[i].Speed = value;
				}
			}
			get { return _add; }
		}
		/// <summary>
		/// Gets or sets a value indicating whether this animation set is in loop mode.
		/// When in loop mode, the animation set OnEnd event won`t happen.
		/// </summary>
		/// <value>
		///   <c>true</c> if loop; otherwise, <c>false</c>.
		/// </value>
		public bool Loop
		{
			set
			{
				_loop = value;
				for (int i = 0; i < _animationList.Count; i++)
				{
					_animationList[i].Loop = value;
				}
			}
			get { return _loop; }
		}
		/// <summary>
		/// Gets or sets a value indicating whether this animation set is in reverse mode.
		/// When in reverse mode, the animation set reversely plays right after normal playing.
		/// The OnEnd event happens when reverse play completes.
		/// </summary>
		/// <value>
		///   <c>true</c> if reverse; otherwise, <c>false</c>.
		/// </value>
		public bool Reverse
		{
			set
			{
				_reverse = value;
				for (int i = 0; i < _animationList.Count; i++)
				{
					_animationList[i].Reverse = value;
				}
			}
			get { return _reverse; }
		}
		/// <summary>
		/// Gets or sets the current progress time in milliseconds.
		/// Animation set`s frame immediate updates after the value is set, regardless the play state.
		/// </summary>
		/// <value>
		/// The current progress time.
		/// </value>
		public float Current
		{
			set
			{
				float temp = value;
				if (temp < 0.0f)
				{
					temp = 0.0f;
				}
				else if (temp > _duration)
				{
					temp = _duration;
				}
				_current = temp / oGame.TargetFrameInterval;
				for (int i = 0; i < _animationList.Count; i++)
				{
					_animationList[i].Current = temp;
				}
			}
			get { return _current * oGame.TargetFrameInterval; }
		}
		/// <summary>
		/// Gets the duration in milliseconds.
		/// </summary>
		public float Duration
		{
			get { return _duration; }
		}
		/// <summary>
		/// Gets a value indicating whether this animation set is playing.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this aniamtion set is playing; otherwise, <c>false</c>.
		/// </value>
		public bool IsPlaying
		{
			get { return base.Enable; }
		}
		/// <summary>
		/// Gets a value indicating whether this animation set is paused.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this animation set is paused; otherwise, <c>false</c>.
		/// </value>
		public bool IsPaused
		{
			get { return _pause; }
		}
		/// <summary>
		/// Gets the paralleled animations.
		/// </summary>
		public List<IAnimation> Animations
		{
			get { return _animationList; }
		}
		/// <summary>
		/// Occurs when animation set plays over.
		/// </summary>
		public event AnimationHandler OnEnd;

		/// <summary>
		/// Initializes a new instance of the <see cref="Parallel"/> class.
		/// Its update controller would be initially set to current scene`s controller.
		/// </summary>
		public Parallel()
			: base(oSceneManager.CurrentScene.Controller)
		{ }
		/// <summary>
		/// Adds an animation to the parallel set.
		/// </summary>
		/// <param name="animation">The animation.</param>
		public void Add(IAnimation animation)
		{
			animation.Loop = _loop;
			animation.Reverse = _reverse;
			if (_duration < animation.Duration)
			{
				_duration = animation.Duration;
				_count = _duration / oGame.TargetFrameInterval;
			}
			_animationList.Add(animation);
		}
		/// <summary>
		/// Removes an animation from the parallel set.
		/// </summary>
		/// <param name="animation">The animation.</param>
		public void Remove(IAnimation animation)
		{
			if (_animationList.Remove(animation))
			{
				if (_duration == animation.Duration)
				{
					_duration = 0.0f;
					for (int i = 0; i < _animationList.Count; i++)
					{
						if (_duration < _animationList[i].Duration)
						{
							_duration = _animationList[i].Duration;
						}
					}
				}
				_count = _duration / oGame.TargetFrameInterval;
			}
		}
		/// <summary>
		/// Clears the parallel set.
		/// </summary>
		public void Clear()
		{
			if (this.IsPlaying)
			{
				this.Stop();
			}
			_animationList.Clear();
		}
		/// <summary>
		/// Plays the animation set.
		/// </summary>
		public void Play()
		{
			this.Reset();
			base.Enable = true;
		}
		/// <summary>
		/// Stops the animation set.
		/// </summary>
		/// <param name="end">if set to <c>true</c> the animation set immediateley jumps to its last frame.</param>
		public void Stop(bool end = false)
		{
			_animationCount = 0;
			if (this.Reverse)
			{
				_current = 0.0f;
			}
			else
			{
				_current = _count;
			}
			for (int i = 0; i < _animationList.Count; i++)
			{
				_animationList[i].Stop(end);
			}
			base.Enable = false;
		}
		/// <summary>
		/// Pauses this animation set.
		/// </summary>
		public void Pause()
		{
			_pause = true;
		}
		/// <summary>
		/// Resumes this animation set.
		/// </summary>
		public void Resume()
		{
			_pause = false;
		}
		/// <summary>
		/// Sets the animation set to first frame.
		/// </summary>
		public void Reset()
		{
			_current = 0.0f;
			_reversing = false;
			_animationCount = _animationList.Count;
			for (int i = 0; i < _animationList.Count; i++)
			{
				_animationList[i].Reset();
			}
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
		/// Forward update this animation set.
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
				for (int i = 0; i < _animationList.Count; i++)
				{
					_animationList[i].Forward();
				}
			}
			else
			{
				_current = _count;
				return true;
			}
			return false;
		}
		/// <summary>
		/// Backward update this animation set.
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
				for (int i = 0; i < _animationList.Count; i++)
				{
					_animationList[i].Backward();
				}
			}
			else
			{
				_current = 0.0f;
				return true;
			}
			return false;
		}
		/// <summary>
		/// Ends this animation set.
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
