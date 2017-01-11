using Dorothy.Effects;
using Microsoft.Xna.Framework;

namespace Dorothy.Cameras
{
	/// <summary>
	/// The camera for space capsule like.
	/// </summary>
	public class FreeLancerCamera : ICamera
	{
		/// <summary>
		/// Indicating what kind of transform should be applied.
		/// </summary>
		enum TransformFlag
		{
			None = 0,
			RotateV = (1 << 0),
			RotateH = (1 << 1),
			Translate = (1 << 2)
		};
		#region Field
		private string _name = string.Empty;
		private TransformFlag _flag = TransformFlag.RotateH | TransformFlag.RotateV | TransformFlag.Translate;
		private float _rotateV;
		private float _rotateH;
		private Vector3 _move;
		private Quaternion _cameraRotation = Quaternion.Identity;
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
		/// Gets the up vector.
		/// </summary>
		public Vector3 Up
		{
			get { return _up; }
		}
		/// <summary>
		/// Gets the target vector.
		/// </summary>
		public Vector3 Target
		{
			get { return _target; }
		}

		/// <summary>
		/// Sets the rotation.
		/// </summary>
		/// <param name="rotateV">The vertical rotation in radius.</param>
		/// <param name="rotateH">The horizontal rotation in radius.</param>
		public void SetRotation(float rotateV, float rotateH)
		{
			_cameraRotation = Quaternion.Identity;
			_rotateV = rotateV;
			_rotateH = rotateH;
			_flag |= (TransformFlag.RotateH | TransformFlag.RotateV);
		}
		/// <summary>
		/// Rotates horizontally by some value.
		/// </summary>
		/// <param name="value">The value in radius, positive value to rotate upward.</param>
		public void RotateHorizontalBy(float value)
		{
			_rotateH = value;
			_flag |= TransformFlag.RotateH;
		}
		/// <summary>
		/// Rotates vertically by some value.
		/// </summary>
		/// <param name="value">The value in radius, positive value to rotate rightward.</param>
		public void RotateVerticalBy(float value)
		{
			_rotateV = value;
			_flag |= TransformFlag.RotateV;
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
				if ((_flag & TransformFlag.RotateH) != 0)
				{
					_flag &= ~TransformFlag.RotateH;
					_cameraRotation = _cameraRotation * Quaternion.CreateFromAxisAngle(Vector3.Up, _rotateH);
				}
				if ((_flag & TransformFlag.RotateV) != 0)
				{
					_flag &= ~TransformFlag.RotateV;
					_cameraRotation = _cameraRotation * Quaternion.CreateFromAxisAngle(Vector3.Right, _rotateV);
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
