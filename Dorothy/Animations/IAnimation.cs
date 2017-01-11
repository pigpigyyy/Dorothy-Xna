namespace Dorothy.Animations
{
	/// <summary>
	/// Function to handle animation event such as end of a play.
	/// </summary>
	/// <param name="sender">The sender animation.</param>
	public delegate void AnimationHandler(IAnimation sender);
	/// <summary>
	/// Interface of animation.
	/// Implements it to make a new animation.
	/// </summary>
	public interface IAnimation
	{
		/// <summary>
		/// Gets or sets the current progress time in milliseconds.
		/// Animation`s frame immediate updates after the value is set, regardless the play state.
		/// </summary>
		/// <value>
		/// The current progress time.
		/// </value>
		float Current
		{
			set;
			get;
		}
		/// <summary>
		/// Gets the duration in milliseconds.
		/// </summary>
		float Duration
		{
			get;
		}
		/// <summary>
		/// Gets or sets the speed.
		/// 1.0f is normal speed, 0.5f to slow down by half of normal speed, 2.0f to double the speed.
		/// </summary>
		/// <value>
		/// The speed.
		/// </value>
		float Speed
		{
			set;
			get;
		}
		/// <summary>
		/// Gets or sets a value indicating whether this animation is in loop mode.
		/// When in loop mode, the animation OnEnd event won`t happen.
		/// </summary>
		/// <value>
		///   <c>true</c> if loop; otherwise, <c>false</c>.
		/// </value>
		bool Loop
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
		bool Reverse
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
		bool IsPlaying
		{
			get;
		}
		/// <summary>
		/// Gets a value indicating whether this animation is paused.
		/// </summary>
		/// <value>
		///   <c>true</c> if this animation is paused; otherwise, <c>false</c>.
		/// </value>
		bool IsPaused
		{
			get;
		}
		/// <summary>
		/// Occurs when animation plays over.
		/// </summary>
		event AnimationHandler OnEnd;

		/// <summary>
		/// Plays this animation.
		/// </summary>
		void Play();
		/// <summary>
		/// Stops the animation.
		/// </summary>
		/// <param name="end">if set to <c>true</c> the animation immediateley jumps to its last frame.</param>
		void Stop(bool end = false);
		/// <summary>
		/// Pauses this animation.
		/// </summary>
		void Pause();
		/// <summary>
		/// Resumes this animation.
		/// </summary>
		void Resume();
		/// <summary>
		/// Sets the animation to first frame.
		/// </summary>
		void Reset();
		/// <summary>
		/// Updates this animation.
		/// [Called by framework]
		/// </summary>
		void Update();
		/// <summary>
		/// Forward update this animation.
		/// [Called by framework]
		/// </summary>
		/// <returns>if return <c>true</c> indicate it`s updated to the start; otherwise, <c>false</c></returns>
		bool Forward();
		/// <summary>
		/// Backward update this animation.
		/// [Called by framework]
		/// </summary>
		/// <returns>if return <c>true</c> indicate it`s updated to the end; otherwise, <c>false</c></returns>
		bool Backward();
	}
}
