using Dorothy.Effects;
using Microsoft.Xna.Framework;

namespace Dorothy.Cameras
{
	/// <summary>
	/// The default camera which keeps object with size value 1 on the plane(Z = 0) to look 1 pixel size on the screen.
	/// It always faces the plane(Z = 0).
	/// </summary>
	public class DefaultCamera : ICamera
	{
		#region Field
		private Vector3 _position;
		private Vector3 _target;
		private Vector3 _up = Vector3.Up;
		private Matrix _view;
		#endregion

		/// <summary>
		/// Gets the camera`s name, the string "Default".
		/// </summary>
		public string Name
		{
			private set;
			get;
		}
		/// <summary>
		/// Gets the position vector.
		/// </summary>
		public Vector3 Position
		{
			get { return _position; }
		}
		/// <summary>
		/// Gets the target vector.
		/// </summary>
		public Vector3 Target
		{
			get { return _target; }
		}
		/// <summary>
		/// Gets the up vector.
		/// </summary>
		public Vector3 Up
		{
			get { return _up; }
		}
		/// <summary>
		/// Gets or sets the X position.
		/// </summary>
		/// <value>
		/// The X.
		/// </value>
		public float X
		{
			set { _position.X = _target.X = value; }
			get { return _position.X; }
		}
		/// <summary>
		/// Gets or sets the Y position.
		/// </summary>
		/// <value>
		/// The Y.
		/// </value>
		public float Y
		{
			set { _position.Y = _target.Y = value; }
			get { return _position.Y; }
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="DefaultCamera"/> class.
		/// </summary>
		public DefaultCamera()
		{
			this.Name = "Default";
		}
		/// <summary>
		/// Applies this camera.
		/// </summary>
		public void Apply()
		{
			_position.Z = oGame.StandardDistance;
			Matrix.CreateLookAt(ref _position, ref _target, ref _up, out _view);
			oEffectManager.SetView(ref _view);
		}
	}
}
