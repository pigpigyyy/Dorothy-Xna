using Box2D;
using Dorothy.Core;

namespace Dorothy.Game
{
	public class DrawUnit : Unit, IUpdatable
	{
		private int _updateOrder = UpdateController.INVALID_ORDER;
		private UpdateController _controller;
		private Sprite _sprite;

		int IUpdatable.UpdateOrder
		{
			set { _updateOrder = value; }
			get { return _updateOrder; }
		}
		public bool Enable
		{
			set
			{
				if (value)
				{
					_controller.Add(this);
				}
				else
				{
					_controller.Remove(this);
				}
			}
			get
			{
				return (_updateOrder != UpdateController.INVALID_ORDER);
			}
		}
		public UpdateController Controller
		{
			set
			{
				if (_updateOrder != UpdateController.INVALID_ORDER)
				{
					_controller.Remove(this);
					value.Add(this);
				}
				_controller = value;
			}
			get { return _controller; }
		}
		public Sprite Sprite
		{
			set { _sprite = value; }
			get { return _sprite; }
		}

		public DrawUnit(Sprite sprite, Body body, World world)
			: base(body, world)
		{
			_sprite = sprite;
			_controller = world.Controller;
			this.Enable = true;
		}
		public virtual void Update()
		{
			Body body = base.Body;
			if (body.IsAwake() && body.GetType() != BodyType.Static)
			{
				_sprite.X = World.GameValue(body.Position.X);
				_sprite.Y = World.GameValue(body.Position.Y);
				_sprite.RotateZ = body.Rotation;
			}
		}
		public override void Dispose()
		{
			this.Enable = false;
			if (_sprite != null)
			{
				_sprite.Parent.Remove(_sprite);
				_sprite = null;
			}
			base.Dispose();
		}
	}
}
