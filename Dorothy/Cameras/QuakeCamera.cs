using Dorothy.Effects;
using Microsoft.Xna.Framework;

namespace Dorothy.Cameras
{
	/// <summary>
	/// The camera for FPS game.
	/// </summary>
	public class QuakeCamera : ICamera
	{
		/// <summary>
		/// Indicating what kind of transform should be applied.
		/// </summary>
		enum TransformFlag
		{
			None = 0,
			Rotate = (1 << 0),
			Translate = (1 << 1)
		};
		#region Field
		private string _name = string.Empty;
		private TransformFlag _flag = TransformFlag.Rotate | TransformFlag.Translate;
		private float _rotateY;
		private float _rotateX;
		private Vector3 _move;
		private Matrix _cameraRotation = Matrix.Identity;
		private Vector3 _position;
		private Vector3 _up;
		private Vector3 _target;
		private Matrix _view;
		#endregion

		/// <summary>
		/// Gets or sets the camera`s name.
		/// </summary>
		public string Name
		{
			set { _name = value; }
			get { return _name; }
		}
		/// <summary>
		/// Gets or sets the rotation around X axis.
		/// </summary>
		/// <value>
		/// The rotation in radius.
		/// </value>
		public float RotateX
		{
			set
			{
				_rotateX = value;
				_flag |= TransformFlag.Rotate;
			}
			get { return _rotateX; }
		}
		/// <summary>
		/// Gets or sets the rotation around Y axis.
		/// </summary>
		/// <value>
		/// The rotation in radius.
		/// </value>
		public float RotateY
		{
			set
			{
				_rotateY = value;
				_flag |= TransformFlag.Rotate;
			}
			get { return _rotateY; }
		}
		/// <summary>
		/// Gets or sets the position vector.
		/// </summary>
		public Vector3 Position
		{
			set
			{
				_position = value;
				_flag |= TransformFlag.Translate;
			}
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
		/// Moves forward by some value.
		/// </summary>
		/// <param name="value">The value.</param>
		public void MoveForwardBy(float value)
		{
			_move.Z = -value;
			_flag |= TransformFlag.Translate;
		}
		/// <summary>
		/// Moves right by some value.
		/// </summary>
		/// <param name="value">The value.</param>
		public void MoveRightBy(float value)
		{
			_move.X = value;
			_flag |= TransformFlag.Translate;
		}
		/// <summary>
		/// Moves up by some value.
		/// </summary>
		/// <param name="value">The value.</param>
		public void MoveUpBy(float value)
		{
			_move.Y = value;
			_flag |= TransformFlag.Translate;
		}
		/// <summary>
		/// Applies this camera.
		/// </summary>
		public void Apply()
		{
			if (_flag != TransformFlag.None)
			{
				if ((_flag & TransformFlag.Rotate) != 0)
				{
					_flag &= ~TransformFlag.Rotate;
					_cameraRotation = Matrix.CreateRotationX(_rotateX) * Matrix.CreateRotationY(_rotateY);
				}
				if ((_flag & TransformFlag.Translate) != 0)
				{
					_flag &= ~TransformFlag.Translate;
					_position += Vector3.Transform(_move, _cameraRotation);
					_move = Vector3.Zero;
				}
				_target = _position + Vector3.Transform(Vector3.Forward, _cameraRotation);
				_up = Vector3.Transform(Vector3.Up, _cameraRotation);
				Matrix.CreateLookAt(ref _position, ref _target, ref _up, out _view);
			}
			oEffectManager.SetView(ref _view);
		}
	}
}
