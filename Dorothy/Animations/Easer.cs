using System;
using System.Collections.Generic;

namespace Dorothy.Animations
{
	/// <summary>
	/// Class with ease function interface and built-in ease functions.
	/// Implements it to make a new ease function.
	/// </summary>
	public abstract class Easer
	{
		/// <summary>
		/// Gets the ID.
		/// </summary>
		/// <value>
		/// The ID.
		/// </value>
		public int ID
		{
			private set;
			get;
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="Easer"/> class.
		/// </summary>
		public Easer()
		{
			this.ID = Easer.List.Count;
			Easer.List.Add(this);
		}
		/// <summary>
		/// Interface for ease function.
		/// Implements it to write self-defined ease equation.
		/// </summary>
		/// <param name="time">The current time.</param>
		/// <param name="begin">The begin value.</param>
		/// <param name="change">The change value equals to the final value minus the begin value.</param>
		/// <param name="duration">The duration.</param>
		/// <returns>The current value.</returns>
		public abstract float Func(float time, float begin, float change, float duration);
		/// <summary>
		/// List contains all easers.
		/// </summary>
		protected static List<Easer> List = new List<Easer>();
		public static readonly NoEasing NoEasing = new NoEasing();
		public static readonly In_Out_Cubic In_Out_Cubic = new In_Out_Cubic();
		public static readonly In_Out_Quintic In_Out_Quintic = new In_Out_Quintic();
		public static readonly Out_Elastic Out_Elastic = new Out_Elastic();
		public static readonly EaseG94 EaseG94 = new EaseG94();
		public static readonly Out_Cubic Out_Cubic = new Out_Cubic();
		public static readonly In_Cubic In_Cubic = new In_Cubic();
		public static readonly In_Elastic In_Elastic = new In_Elastic();
		public static readonly Back_In_Cubic Back_In_Cubic = new Back_In_Cubic();
		public static readonly Back_Out_Cubic Back_Out_Cubic = new Back_Out_Cubic();
		/// <summary>
		/// Gets the easer by ID.
		/// </summary>
		/// <param name="id">The id.</param>
		/// <returns>Easer</returns>
		public static Easer GetEaserByID(int id)
		{
			if (0 <= id && id < Easer.List.Count)
			{
				return Easer.List[id];
			}
			return Easer.NoEasing;
		}
	}

	/// <summary>
	/// Ease function 
	/// return b + c * (t / d)
	/// </summary>
	public class NoEasing : Easer
	{
		public override float Func(float time, float begin, float change, float duration)
		{
			return begin + change * (time / duration);
		}
	}
	/// <summary>
	/// Ease function
	/// ts = (t /= d) * t
	/// tc = ts * t
	/// return b + c * (-2 * tc + 3 * ts)
	/// </summary>
	public class In_Out_Cubic : Easer
	{
		public override float Func(float time, float begin, float change, float duration)
		{
			float ts = (time /= duration) * time;
			float tc = ts * time;
			return begin + change * (-2 * tc + 3 * ts);
		}
	}
	/// <summary>
	/// Ease function
	/// ts = (t /= d) * t
	/// tc = ts * t
	/// return b + c * ( 6 * tc * ts + -15 * ts * ts + 10 * tc )
	/// </summary>
	public class In_Out_Quintic : Easer
	{
		public override float Func(float time, float begin, float change, float duration)
		{
			float ts = (time /= duration) * time;
			float tc = ts * time;
			return begin + change * (6 * tc * ts + -15 * ts * ts + 10 * tc);
		}
	}
	/// <summary>
	/// Ease function
	/// ts = (t /= d) * t
	/// tc = ts * 
	/// return b + c * (33 * tc * ts + -106 * ts * ts + 126 * tc + -67 * ts + 15 * t)
	/// </summary>
	public class Out_Elastic : Easer
	{
		public override float Func(float time, float begin, float change, float duration)
		{
			float ts = (time /= duration) * time;
			float tc = ts * time;
			return begin + change * (33 * tc * ts + -106 * ts * ts + 126 * tc + -67 * ts + 15 * time);
		}
	}
	/// <summary>
	/// Ease function
	/// r = 0.1
	/// if (t /= d) LESS_THAN r
	/// return (1 - Math.Cos(t / r * Math.PI / 2)) * c * (1 - r) + b
	/// else
	/// return Math.Sin((t - r) / (1 - r) * Math.PI / 2) * c * r + c * (1 - r) + b
	/// </summary>
	public class EaseG94 : Easer
	{
		public override float Func(float time, float begin, float change, float duration)
		{
			float r = 0.1f;
			return (time /= duration) < r ?
				(1 - (float)Math.Cos(time / r * Math.PI / 2)) * change * (1 - r) + begin :
				(float)Math.Sin((time - r) / (1 - r) * Math.PI / 2) * change * r + change * (1 - r) + begin;
		}
	}
	/// <summary>
	/// Ease function
	/// ts = (t /= d) * t
	/// tc = ts * t
	/// return b + c * (tc + -3 * ts + 3 * t)
	/// </summary>
	public class Out_Cubic : Easer
	{
		public override float Func(float time, float begin, float change, float duration)
		{
			float ts = (time /= duration) * time;
			float tc = ts * time;
			return begin + change * (tc + -3 * ts + 3 * time);
		}
	}
	/// <summary>
	/// Ease function
	/// tc = (t /= d) * t * t
	/// return b + c * tc
	/// </summary>
	public class In_Cubic : Easer
	{
		public override float Func(float time, float begin, float change, float duration)
		{
			float tc = (time /= duration) * time * time;
			return begin + change * tc;
		}
	}
	/// <summary>
	/// Ease function
	/// ts = (t /= d) * t
	/// tc = ts * t
	/// return b + c * (33 * tc * ts + -59 * ts * ts + 32 * tc + -5 * ts)
	/// </summary>
	public class In_Elastic : Easer
	{
		public override float Func(float time, float begin, float change, float duration)
		{
			float ts = (time /= duration) * time;
			float tc = ts * time;
			return begin + change * (33 * tc * ts + -59 * ts * ts + 32 * tc + -5 * ts);
		}
	}
	/// <summary>
	/// Ease function
	/// ts = (t /= d) * t
	/// tc = ts * t
	/// return b + c * (4 * tc + -3 * ts)
	/// </summary>
	public class Back_In_Cubic : Easer
	{
		public override float Func(float time, float begin, float change, float duration)
		{
			float ts = (time /= duration) * time;
			float tc = ts * time;
			return begin + change * (4 * tc + -3 * ts);
		}
	}
	/// <summary>
	/// Ease function
	/// ts = (t /= d) * t
	/// tc = ts * t
	/// return b + c * (4 * tc + -9 * ts + 6 * t)
	/// </summary>
	public class Back_Out_Cubic : Easer
	{
		public override float Func(float time, float begin, float change, float duration)
		{
			float ts = (time /= duration) * time;
			float tc = ts * time;
			return begin + change * (4 * tc + -9 * ts + 6 * time);
		}
	}
}
