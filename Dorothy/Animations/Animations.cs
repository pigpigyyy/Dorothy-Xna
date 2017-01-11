using Dorothy.Core;

namespace Dorothy.Animations
{
	/// <summary>
	/// Transparency animation.
	/// </summary>
	public class Fade : PropertyAnimation
	{
		/// <summary>
		/// Gets or sets the target.
		/// </summary>
		/// <value>
		/// The Drawable node.
		/// </value>
		public Drawable Target
		{
			set;
			get;
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="Fade"/> class.
		/// </summary>
		/// <param name="alphaFrom">The alpha from.</param>
		/// <param name="alphaTo">The alpha to.</param>
		/// <param name="duration">The duration.</param>
		public Fade(float alphaFrom, float alphaTo, float duration)
			: base(alphaFrom, alphaTo, duration)
		{ }
		/// <summary>
		/// Set the alpha property. Override method to be called by property animation.
		/// </summary>
		/// <param name="value">The alpha.</param>
		protected override void SetProperty(float value)
		{
			this.Target.Alpha = value;
		}
	}
	/// <summary>
	/// Rotation around x-axis` animation.
	/// </summary>
	public class RotateX : PropertyAnimation
	{
		/// <summary>
		/// Gets or sets the target.
		/// </summary>
		/// <value>
		/// The Drawable node.
		/// </value>
		public Drawable Target
		{
			set;
			get;
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="RotateX"/> class.
		/// </summary>
		/// <param name="angleXFrom">The angle around x-axis from.</param>
		/// <param name="angleXTo">The angle around x-axis to.</param>
		/// <param name="duration">The duration.</param>
		public RotateX(float angleXFrom, float angleXTo, float duration)
			: base(angleXFrom, angleXTo, duration)
		{ }
		/// <summary>
		/// Set the rotateX property. Override method to be called by property animation.
		/// </summary>
		/// <param name="value">The rotateX.</param>
		protected override void SetProperty(float value)
		{
			this.Target.RotateX = value;
		}
	}
	/// <summary>
	/// Rotation around y-axis` animation.
	/// </summary>
	public class RotateY : PropertyAnimation
	{
		/// <summary>
		/// Gets or sets the target.
		/// </summary>
		/// <value>
		/// The Drawable node.
		/// </value>
		public Drawable Target
		{
			set;
			get;
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="RotateY"/> class.
		/// </summary>
		/// <param name="angleYFrom">The angle around y-axis from.</param>
		/// <param name="angleYTo">The angle around y-axis to.</param>
		/// <param name="duration">The duration.</param>
		public RotateY(float angleYFrom, float angleYTo, float duration)
			: base(angleYFrom, angleYTo, duration)
		{ }
		/// <summary>
		/// Set the rotateY property. Override method to be called by property animation.
		/// </summary>
		/// <param name="value">The rotateY.</param>
		protected override void SetProperty(float value)
		{
			this.Target.RotateY = value;
		}
	}
	/// <summary>
	/// Rotation around z-axis` animation.
	/// </summary>
	public class RotateZ : PropertyAnimation
	{
		/// <summary>
		/// Gets or sets the target.
		/// </summary>
		/// <value>
		/// The Drawable node.
		/// </value>
		public Drawable Target
		{
			set;
			get;
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="RotateZ"/> class.
		/// </summary>
		/// <param name="angleZFrom">The angle around z-axis from.</param>
		/// <param name="angleZTo">The angle around z-axis to.</param>
		/// <param name="duration">The duration.</param>
		public RotateZ(float angleZFrom, float angleZTo, float duration)
			: base(angleZFrom, angleZTo, duration)
		{ }
		/// <summary>
		/// Set the rotateZ property. Override method to be called by property animation.
		/// </summary>
		/// <param name="value">The rotateZ.</param>
		protected override void SetProperty(float value)
		{
			this.Target.RotateZ = value;
		}
	}
	/// <summary>
	/// X position animation.
	/// </summary>
	public class MoveX : PropertyAnimation
	{
		/// <summary>
		/// Gets or sets the target.
		/// </summary>
		/// <value>
		/// The Drawable node.
		/// </value>
		public Drawable Target
		{
			set;
			get;
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="MoveX"/> class.
		/// </summary>
		/// <param name="XFrom">The position X from.</param>
		/// <param name="XTo">The position X to.</param>
		/// <param name="duration">The duration.</param>
		public MoveX(float XFrom, float XTo, float duration)
			: base(XFrom, XTo, duration)
		{ }
		/// <summary>
		/// Set the position X property. Override method to be called by property animation.
		/// </summary>
		/// <param name="value">The position X.</param>
		protected override void SetProperty(float value)
		{
			this.Target.X = value;
		}
	}
	/// <summary>
	/// Y position animation.
	/// </summary>
	public class MoveY : PropertyAnimation
	{
		/// <summary>
		/// Gets or sets the target.
		/// </summary>
		/// <value>
		/// The Drawable node.
		/// </value>
		public Drawable Target
		{
			set;
			get;
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="MoveY"/> class.
		/// </summary>
		/// <param name="YFrom">The position Y from.</param>
		/// <param name="YTo">The position Y to.</param>
		/// <param name="duration">The duration.</param>
		public MoveY(float YFrom, float YTo, float duration)
			: base(YFrom, YTo, duration)
		{ }
		/// <summary>
		/// Set the position Y property. Override method to be called by property animation.
		/// </summary>
		/// <param name="value">The position Y.</param>
		protected override void SetProperty(float value)
		{
			this.Target.Y = value;
		}
	}
	/// <summary>
	/// Z position animation.
	/// </summary>
	public class MoveZ : PropertyAnimation
	{
		/// <summary>
		/// Gets or sets the target.
		/// </summary>
		/// <value>
		/// The Drawable node.
		/// </value>
		public Drawable Target
		{
			set;
			get;
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="MoveZ"/> class.
		/// </summary>
		/// <param name="ZFrom">The position Z from.</param>
		/// <param name="ZTo">The position Z to.</param>
		/// <param name="duration">The duration.</param>
		public MoveZ(float ZFrom, float ZTo, float duration)
			: base(ZFrom, ZTo, duration)
		{ }
		/// <summary>
		/// Set the position Z property. Override method to be called by property animation.
		/// </summary>
		/// <param name="value">The position Z.</param>
		protected override void SetProperty(float value)
		{
			this.Target.Z = value;
		}
	}
	/// <summary>
	/// Scale X animation. 
	/// </summary>
	public class ScaleX : PropertyAnimation
	{
		/// <summary>
		/// Gets or sets the target.
		/// </summary>
		/// <value>
		/// The Drawable node.
		/// </value>
		public Drawable Target
		{
			set;
			get;
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="ScaleX"/> class.
		/// </summary>
		/// <param name="scaleXFrom">The scale X from.</param>
		/// <param name="scaleXTo">The scale X to.</param>
		/// <param name="duration">The duration.</param>
		public ScaleX(float scaleXFrom, float scaleXTo, float duration)
			: base(scaleXFrom, scaleXTo, duration)
		{ }
		/// <summary>
		/// Set the scale X property. Override method to be called by property animation.
		/// </summary>
		/// <param name="value">The scale X.</param>
		protected override void SetProperty(float value)
		{
			this.Target.ScaleX = value;
		}
	}
	/// <summary>
	/// Scale Y animation. 
	/// </summary>
	public class ScaleY : PropertyAnimation
	{
		/// <summary>
		/// Gets or sets the target.
		/// </summary>
		/// <value>
		/// The Drawable node.
		/// </value>
		public Drawable Target
		{
			set;
			get;
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="ScaleY"/> class.
		/// </summary>
		/// <param name="scaleYFrom">The scale Y from.</param>
		/// <param name="scaleYTo">The scale Y to.</param>
		/// <param name="duration">The duration.</param>
		public ScaleY(float scaleYFrom, float scaleYTo, float duration)
			: base(scaleYFrom, scaleYTo, duration)
		{ }
		/// <summary>
		/// Set the scale Y property. Override method to be called by property animation.
		/// </summary>
		/// <param name="value">The scale Y.</param>
		protected override void SetProperty(float value)
		{
			this.Target.ScaleY = value;
		}
	}
	/// <summary>
	/// Offset X animation. 
	/// </summary>
	public class OffsetX : PropertyAnimation
	{
		/// <summary>
		/// Gets or sets the target.
		/// </summary>
		/// <value>
		/// The Drawable node.
		/// </value>
		public Drawable Target
		{
			set;
			get;
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="OffsetX"/> class.
		/// </summary>
		/// <param name="offsetXFrom">The offset X from.</param>
		/// <param name="offsetXTo">The offset X to.</param>
		/// <param name="duration">The duration.</param>
		public OffsetX(float offsetXFrom, float offsetXTo, float duration)
			: base(offsetXFrom, offsetXTo, duration)
		{ }
		/// <summary>
		/// Set the offset X property. Override method to be called by property animation.
		/// </summary>
		/// <param name="value">The offset X.</param>
		protected override void SetProperty(float value)
		{
			this.Target.OffsetX = value;
		}
	}
	/// <summary>
	/// Offset Y animation. 
	/// </summary>
	public class OffsetY : PropertyAnimation
	{
		/// <summary>
		/// Gets or sets the target.
		/// </summary>
		/// <value>
		/// The Drawable node.
		/// </value>
		public Drawable Target
		{
			set;
			get;
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="OffsetY"/> class.
		/// </summary>
		/// <param name="offsetYFrom">The offset Y from.</param>
		/// <param name="offsetYTo">The offset Y to.</param>
		/// <param name="duration">The duration.</param>
		public OffsetY(float offsetYFrom, float offsetYTo, float duration)
			: base(offsetYFrom, offsetYTo, duration)
		{ }
		/// <summary>
		/// Set the offset Y property. Override method to be called by property animation.
		/// </summary>
		/// <param name="value">The offset Y.</param>
		protected override void SetProperty(float value)
		{
			this.Target.OffsetY = value;
		}
	}
	/// <summary>
	/// Offset Z animation. 
	/// </summary>
	public class OffsetZ : PropertyAnimation
	{
		/// <summary>
		/// Gets or sets the target.
		/// </summary>
		/// <value>
		/// The Drawable node.
		/// </value>
		public Drawable Target
		{
			set;
			get;
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="OffsetZ"/> class.
		/// </summary>
		/// <param name="offsetZFrom">The offset Z from.</param>
		/// <param name="offsetZTo">The offset Z to.</param>
		/// <param name="duration">The duration.</param>
		public OffsetZ(float offsetZFrom, float offsetZTo, float duration)
			: base(offsetZFrom, offsetZTo, duration)
		{ }
		/// <summary>
		/// Set the offset Z property. Override method to be called by property animation.
		/// </summary>
		/// <param name="value">The offset Z.</param>
		protected override void SetProperty(float value)
		{
			this.Target.OffsetZ = value;
		}
	}
}
