using System;
using Dorothy.Animations;
using Dorothy.Game;
using Microsoft.Xna.Framework;

namespace Dorothy.Cameras
{
	/// <summary>
	/// This camera is for platform game (Mario-like game) and inspired by game Maple Story Online.
	/// Instead of simply lock view on the character all the time, it uses four bounds lock,
	/// the keep bound, the follow bound, the reset bound and the map bound.
	/// The keep bound is a rectangle area makes camera`s XY position as its center.
	/// When character moves within keep bound area, the camera won`t do anything.
	/// The follow bound is another rectangle area makes camera`s XY position as its center.
	/// When character move out of follow bound the camera will immediately move to insure the character always in its follow bound.
	/// The reset bound indicates the camera should start an animation and gradually move to make character as its center in reset time
	/// when character`s velocity is below certain value.
	/// Because whenever the character slows down to stop the camera always needs to reset.
	/// The camera won`t move out of the map bound.
	/// The camera always look at plane(Z = 0).
	/// All this may make platform game looks more comfortable.
	/// </summary>
	public class DrawUnitCamera : ICamera
	{
		#region Field
		private string _name = string.Empty;
		private DrawUnit _target;
		private bool _bResetX = false;
		private bool _bResetY = false;
		private float _currentX = 0.0f;
		private float _currentY = 0.0f;
		private float _count = 300.0f;
		private BasicCamera _cam = new BasicCamera();
		private Vector2 _resetBound;
		private Vector3 _position;
		private Vector3 _offset;
		private Vector2 _upperBound;
		private Vector2 _lowerBound;
		private Vector2 _keepBound;
		private Vector2 _followBound;
		private Vector3 _resetStart;
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
			get { return _cam.Target; }
		}
		/// <summary>
		/// Gets the up vector.
		/// </summary>
		public Vector3 Up
		{
			get { return _cam.Up; }
		}
		/// <summary>
		/// Sets the reset time in milliseconds.
		/// </summary>
		/// <value>
		/// The reset time.
		/// </value>
		public float ResetTime
		{
			set { _count = value / oGame.TargetFrameInterval; }
		}
		/// <summary>
		/// Gets or sets the distance to target.
		/// </summary>
		/// <value>
		/// The distance.
		/// </value>
		public float Distance
		{
			set { _offset.Z = value; }
			get { return _offset.Z; }
		}
		/// <summary>
		/// Gets or sets the offset X.
		/// Offset to make camera won`t right lock at target`s position.
		/// </summary>
		/// <value>
		/// The offset X.
		/// </value>
		public float OffsetX
		{
			set { _offset.X = value; }
			get { return _offset.X; }
		}
		/// <summary>
		/// Gets or sets the offset Y.
		/// Offset to make camera won`t right lock at target`s position.
		/// </summary>
		/// <value>
		/// The offset Y.
		/// </value>
		public float OffsetY
		{
			set { _offset.Y = value; }
			get { return _offset.Y; }
		}
		/// <summary>
		/// Gets or sets the offset.
		/// Offset to make camera won`t right lock at target`s position.
		/// </summary>
		/// <value>
		/// The offset.
		/// </value>
		public Vector2 Offset
		{
			set { _offset = new Vector3(value, _offset.Z); }
			get { return new Vector2(_offset.X, _offset.Y); }
		}
		/// <summary>
		/// Gets or sets the map bound`s left and top position.
		/// </summary>
		/// <value>
		/// The upper map bound.
		/// </value>
		public Vector2 UpperMapBound
		{
			set { _upperBound = value; }
			get { return _upperBound; }
		}
		/// <summary>
		/// Gets or sets the map bound`s right and bottom position.
		/// </summary>
		/// <value>
		/// The lower bound.
		/// </value>
		public Vector2 LowerMapBound
		{
			set { _lowerBound = value; }
			get { return _lowerBound; }
		}
		/// <summary>
		/// Gets or sets the keep bound area`s width and height.
		/// Camera position is always keep bound`s center.
		/// </summary>
		/// <value>
		/// The keep bound.
		/// </value>
		public Vector2 KeepBound
		{
			set
			{
				_keepBound = value;
			}
			get
			{
				return _keepBound;
			}
		}
		/// <summary>
		/// Gets or sets the follow bound area`s width and height.
		/// Camera position is always follow bound`s center.
		/// </summary>
		/// <value>
		/// The follow bound.
		/// </value>
		public Vector2 FollowBound
		{
			set
			{
				_followBound = value;
			}
			get
			{
				return _followBound;
			}
		}
		/// <summary>
		/// Gets or sets the reset bound speed.
		/// The camera will gradually reset when target`s velocity is below reset bound speed.
		/// </summary>
		/// <value>
		/// The reset bound.
		/// </value>
		public Vector2 ResetBound
		{
			set { _resetBound = value; }
			get { return _resetBound; }
		}
		/// <summary>
		/// Gets or sets the target unit.
		/// </summary>
		/// <value>
		/// The target unit.
		/// </value>
		public DrawUnit TargetUnit
		{
			set { _target = value; }
			get { return _target; }
		}
		/// <summary>
		/// Gets or sets a value indicating whether it should gradually reset X position to the target`s X position.
		/// </summary>
		/// <value>
		///   <c>true</c> if [reset X]; otherwise, <c>false</c>.
		/// </value>
		private bool ResetX
		{
			set
			{
				if (value)
				{
					Vector3 targetPos = Vector3.Transform(_target.Sprite.Position, _target.Sprite.Parent.World);
					if (Math.Abs(targetPos.X - _position.X) > _keepBound.X)
					{
						_bResetX = true;
						_currentX = 0.0f;
						_resetStart.X = _position.X;
					}
				}
				else
				{
					_bResetX = false;
				}
			}
			get
			{
				return _bResetX;
			}
		}
		/// <summary>
		/// Gets or sets a value indicating whether it should gradually reset Y position to the target`s Y position.
		/// </summary>
		/// <value>
		///   <c>true</c> if [reset Y]; otherwise, <c>false</c>.
		/// </value>
		private bool ResetY
		{
			set
			{
				if (value)
				{
					Vector3 targetPos = Vector3.Transform(_target.Sprite.Position, _target.Sprite.Parent.World);
					if (Math.Abs(targetPos.Y - _position.Y) > _keepBound.Y)
					{
						_bResetY = true;
						_currentY = 0.0f;
						_resetStart.Y = _position.Y;
					}
				}
				else
				{
					_bResetY = false;
				}
			}
			get
			{
				return _bResetY;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="DrawUnitCamera"/> class.
		/// </summary>
		/// <param name="target">The target.</param>
		/// <param name="keepBound">The keep bound.</param>
		/// <param name="followBound">The follow bound.</param>
		/// <param name="upperMapBound">The upper map bound.</param>
		/// <param name="lowerMapBound">The lower map bound.</param>
		/// <param name="resetBound">The reset bound.</param>
		/// <param name="offset">The offset.</param>
		public DrawUnitCamera(
			DrawUnit target,
			Vector2 keepBound,
			Vector2 followBound,
			Vector2 upperMapBound,
			Vector2 lowerMapBound,
			Vector2 resetBound,
			Vector3 offset)
		{
			_target = target;
			_offset = offset;
			_keepBound = keepBound;
			_followBound = followBound;
			_upperBound = upperMapBound;
			_lowerBound = lowerMapBound;
			_resetBound = resetBound;
		}
		/// <summary>
		/// Applies this camera.
		/// </summary>
		public void Apply()
		{
			Vector2 v = _target.Velocity;
			if (Math.Abs(v.X) < _resetBound.X)
			{
				if (!this.ResetX)
				{
					this.ResetX = true;
				}
			}
			else
			{
				this.ResetX = false;
			}
			if (Math.Abs(v.Y) < _resetBound.Y)
			{
				if (!this.ResetY)
				{
					this.ResetY = true;
				}
			}
			else
			{
				this.ResetY = false;
			}

			Vector3 targetPos = Vector3.Transform(_target.Sprite.Position, _target.Sprite.Parent.World);
			if (Math.Abs(targetPos.X - _position.X) > _followBound.X)
			{
				_bResetX = false;
				_position.X = targetPos.X + (targetPos.X - _position.X > 0 ? -_followBound.X : _followBound.X);
			}
			if (Math.Abs(targetPos.Y - _position.Y) > _followBound.Y)
			{
				_bResetY = false;
				_position.Y = targetPos.Y + (targetPos.Y - _position.Y > 0 ? -_followBound.Y : _followBound.Y);
			}
			if (_bResetX)
			{
				float changeX = targetPos.X - _resetStart.X;
				_position.X = Easer.Out_Cubic.Func(_currentX, _resetStart.X, changeX, _count);
				_currentX++;
				if (_currentX >= _count)
				{
					_bResetX = false;
				}
			}
			if (_bResetY)
			{
				float changeY = targetPos.Y - _resetStart.Y;
				_position.Y = Easer.Out_Cubic.Func(_currentY, _resetStart.Y, changeY, _count);
				_currentY++;
				if (_currentY >= _count)
				{
					_bResetY = false;
				}
			}
			_position.X += _offset.X;
			_position.Y += _offset.Y;
			if (_position.X < _upperBound.X)
			{
				_position.X = _upperBound.X;
			}
			else if (_position.X > _lowerBound.X)
			{
				_position.X = _lowerBound.X;
			}
			if (_position.Y < _lowerBound.Y)
			{
				_position.Y = _lowerBound.Y;
			}
			else if (_position.Y > _upperBound.Y)
			{
				_position.Y = _upperBound.Y;
			}
			Vector3 target = _position;
			target.Z = targetPos.Z;
			_position.Z = targetPos.Z + _offset.Z;
			_cam.Set(_position, target, Vector3.Up);
			_cam.Apply();
		}
	}
}
