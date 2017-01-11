using System.Collections.Generic;
using Dorothy.Core;

namespace Dorothy.Animations
{
	/// <summary>
	/// A set contains animations to be played in a row.
	/// </summary>
	public class Sequence : Component, IAnimation
	{
		#region Field
		private bool _pause;
		private bool _reversing;
		private float _add = 1.0f;
		private float _current;
		private int _currentCount;
		private float _count;
		private float _duration;
		private IAnimation _currentAnimation;
		private List<IAnimation> _animationList = new List<IAnimation>();
		#endregion

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
					if (temp <= _animationList[i].Duration)
					{
						if (_currentAnimation != null)
						{
							int index = _animationList.IndexOf(_currentAnimation);
							if (index < i)
							{
								_currentAnimation.Stop(true);
							}
							else if (index > i)
							{
								_currentAnimation.Stop();
								_currentAnimation.Reset();
							}
						}
						_currentCount = i;
						_currentAnimation = _animationList[i];
						_currentAnimation.Current = temp;
						break;
					}
					else
					{
						temp -= _animationList[i].Duration;
					}
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
			set;
			get;
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
			set;
			get;
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
		/// Gets the sequenced animations.
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
		/// Initializes a new instance of the <see cref="Sequence"/> class.
		/// Its update controller would be initially set to current scene`s controller.
		/// </summary>
		public Sequence()
			: base(oSceneManager.CurrentScene.Controller)
		{ }
		/// <summary>
		/// Adds an animation to the sequence set.
		/// </summary>
		/// <param name="animation">The animation.</param>
		public void Add(IAnimation animation)
		{
			_duration += animation.Duration;
			_count = _duration / oGame.TargetFrameInterval;
			_animationList.Add(animation);
		}
		/// <summary>
		/// Removes an animation from the sequence set.
		/// </summary>
		/// <param name="animation">The animation.</param>
		/// <returns></returns>
		public bool Remove(IAnimation animation)
		{
			if (_animationList.Remove(animation))
			{
				_duration -= animation.Duration;
				_count = _duration / oGame.TargetFrameInterval;
				return true;
			}
			return false;
		}
		/// <summary>
		/// Clears the sequence set.
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
		/// Plays this animation set.
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
			_pause = false;
			_reversing = false;
			if (end)
			{
				if (this.Reverse)
				{
					_current = 0.0f;
					for (int i = 0; i < _animationList.Count; i++)
					{
						_animationList[i].Reset();
					}
				}
				else
				{
					_current = _count;
					for (int i = 0; i < _animationList.Count; i++)
					{
						_animationList[i].Stop(true);
					}
				}
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
			_pause = false;
			_current = 0.0f;
			_currentCount = 0;
			_currentAnimation = _animationList[0];
			for (int i = _animationList.Count - 1; i >= 0; i--)
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
			_current += _add;
			if (_current > _count)
			{
				_current = _count;
			}
			if (_currentAnimation.Forward())
			{
				_currentCount++;
				if (_currentCount < _animationList.Count)
				{
					_currentAnimation = _animationList[_currentCount];
				}
				else
				{
					_currentCount = _animationList.Count - 1;
					return true;
				}
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
			_current -= _add;
			if (_current < 0.0f)
			{
				_current = 0.0f;
			}
			if (_currentAnimation.Backward())
			{
				_currentCount--;
				if (_currentCount >= 0)
				{
					_currentAnimation = _animationList[_currentCount];
				}
				else
				{
					_currentCount = 0;
					return true;
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
