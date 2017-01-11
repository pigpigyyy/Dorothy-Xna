using System;
using Dorothy.Effects;
using Microsoft.Xna.Framework;

namespace Dorothy.Cameras
{
	/// <summary>
	/// Basic camera. Can be set by position, target or up vector.
	/// </summary>
	public class BasicCamera : ICamera
	{
		#region Field
		private string _name = string.Empty;
		private Vector3 _target;
		private Vector3 _up = Vector3.Up;
		private Vector3 _position;
		private Matrix _view;
		#endregion

		/// <summary>
		/// Gets or sets the camera`s name.
		/// </summary>
		/// <value>
		/// The name.
		/// </value>
		public string Name
		{
			set { _name = value; }
			get { return _name; }
		}
		/// <summary>
		/// Gets or sets the position vector.
		/// </summary>
		public Vector3 Position
		{
			set
			{
				_position = value;
				this.SetTarget(ref _target);
			}
			get { return _position; }
		}
		/// <summary>
		/// Gets or sets the target vector.
		/// </summary>
		public Vector3 Target
		{
			set { this.SetTarget(ref value); }
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
		/// Sets the specified position and target. Up vector will be adjust by these two vectors.
		/// </summary>
		/// <param name="position">The position.</param>
		/// <param name="target">The target.</param>
		public void Set(Vector3 position, Vector3 target)
		{
			_position = position;
			this.SetTarget(ref target);
		}
		/// <summary>
		/// Sets the specified position and target. Up vector will be adjust by these two vectors.
		/// </summary>
		/// <param name="position">The position.</param>
		/// <param name="target">The target.</param>
		public void Set(ref Vector3 position, ref Vector3 target)
		{
			_position = position;
			this.SetTarget(ref target);
		}
		/// <summary>
		/// Sets the specified position, target and up vectors.
		/// </summary>
		/// <param name="position">The position vector.</param>
		/// <param name="target">The target vector.</param>
		/// <param name="up">The up vector.</param>
		public void Set(ref Vector3 position, ref Vector3 target, ref Vector3 up)
		{
			_position = position;
			_target = target;
			_up = up;
		}
		/// <summary>
		/// Sets the specified position, target and up vectors.
		/// </summary>
		/// <param name="position">The position vector.</param>
		/// <param name="target">The target vector.</param>
		/// <param name="up">The up vector.</param>
		public void Set(Vector3 position, Vector3 target, Vector3 up)
		{
			_position = position;
			_target = target;
			_up = up;
		}
		/// <summary>
		/// Moves to the specified position.
		/// </summary>
		/// <param name="position">The position.</param>
		public void Move(ref Vector3 position)
		{
			_target = (_target - _position + position);
			_position = position;
		}
		/// <summary>
		/// Moves to the specified position.
		/// </summary>
		/// <param name="position">The position.</param>
		public void Move(Vector3 position)
		{
			this.Move(ref position);
		}
		/// <summary>
		/// Applies this camera.
		/// </summary>
		public void Apply()
		{
			Matrix.CreateLookAt(ref _position, ref _target, ref _up, out _view);
			oEffectManager.SetView(ref _view);
		}
		/// <summary>
		/// Sets the target and adjust the up vector.
		/// </summary>
		/// <param name="target">The target.</param>
		private void SetTarget(ref Vector3 target)
		{
			Vector3 dest = target - _position;
			float l = dest.Length();
			if (l != 0.0f)
			{
				float rotateX = (float)Math.Asin(dest.Y / l);
				float rotateY = 0.0f;
				if (dest.X != 0.0f)
				{
					rotateY = -(float)Math.Atan(dest.Z / dest.X);
				}
				_up = Vector3.Transform(Vector3.Up, Matrix.CreateRotationX(rotateX) * Matrix.CreateRotationY(rotateY));
				_target = target;
			}
		}
	}
}
