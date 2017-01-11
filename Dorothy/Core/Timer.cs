using System;

namespace Dorothy.Core
{
	/// <summary>
	/// The timer for game logic use.
	/// </summary>
	public class Timer : Component
	{
		#region Field
		private uint _loop;
		private uint _currentLoop;
		private uint _count;
		private uint _currentCount;
		private object _eventData;
		#endregion

		/// <summary>
		/// Sets the delay time to trigger the on timer event.
		/// </summary>
		/// <value>
		/// The delay in milliseconds.
		/// </value>
		public uint Delay
		{
			set
			{
				_count = (uint)Math.Round((double)value / oGame.TargetFrameInterval);
			}
		}
		/// <summary>
		/// Sets the loop times.
		/// </summary>
		/// <value>
		/// The loop times.
		/// When set to 0, the timer will always loop.
		/// </value>
		public uint LoopTimes
		{
			set { _loop = value; }
		}
		/// <summary>
		/// Gets the current loop number.
		/// The first loop`s number is 0.
		/// </summary>
		public uint CurrentLoop
		{
			get { return _currentLoop; }
		}
		/// <summary>
		/// Gets or sets the event data.
		/// </summary>
		/// <value>
		/// The event data.
		/// </value>
		public object EventData
		{
			set { _eventData = value; }
			get { return _eventData; }
		}
		/// <summary>
		/// Occurs when it is on time.
		/// </summary>
		public event Action<Timer> OnTimer;

		/// <summary>
		/// Initializes a new instance of the <see cref="Timer"/> class.
		/// Its update controller is initially the current scenes controller.
		/// </summary>
		/// <param name="delay">The delay time in milliseconds.</param>
		/// <param name="loop">The loop times.</param>
		public Timer(uint delay, uint loop = 1)
			: base(oGame.Controller)
		{
			this.Delay = delay;
			_loop = loop;
		}
		/// <summary>
		/// Starts timer.
		/// </summary>
		public void Start()
		{
			_currentLoop = 0;
			_currentCount = 0;
			this.Enable = true;
		}
		/// <summary>
		/// Stops timer.
		/// </summary>
		public void Stop()
		{
			this.Enable = false;
		}
		/// <summary>
		/// Updates timer.
		/// [Called by framework]
		/// </summary>
		public override void Update()
		{
			if (++_currentCount > _count)
			{
				if (++_currentLoop < _loop || _loop == 0)
				{
					_currentCount = 0;
				}
				else
				{
					this.Enable = false;
				}
				if (OnTimer != null)
				{
					OnTimer(this);
				}
			}
		}
	}
}
