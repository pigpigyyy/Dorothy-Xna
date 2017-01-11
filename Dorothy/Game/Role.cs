using System.Collections.Generic;
using Box2D;
using Dorothy.Core;
using Dorothy.Game.Actions;
using Microsoft.Xna.Framework;

namespace Dorothy.Game
{
	public class Role : Unit, IUpdatable
	{
		private int _updateOrder = UpdateController.INVALID_ORDER;
		private UpdateController _controller;
		private Model2D _model;
		private IAction _currentAction;
		private Sensor _sensor;
		private Dictionary<string, IAction> _actionList = new Dictionary<string, IAction>();

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
			get { return (_updateOrder != UpdateController.INVALID_ORDER); }
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
		public Model2D Model2D
		{
			set { _model = value; }
			get { return _model; }
		}
		public IAction CurrentAction
		{
			get { return _currentAction; }
		}
		public Dictionary<string, IAction> Actions
		{
			get { return _actionList; }
		}
		public bool IsOnSurface
		{
			set
			{
				if (value == false)
				{
					_sensor.Clear();
				}
			}
			get { return (_sensor.SensedCount > 0); }
		}

		public Role(Model2D model, Body body, World world, float halfWidth, float halfHeight)
			: base(body, world)
		{
			_model = model;
			_model.X = World.GameValue(body.Position.X);
			_model.Y = World.GameValue(body.Position.Y);
			_controller = world.Controller;
			if (halfWidth != 0.0f && halfHeight != 0.0f)
			{
				float x0 = -halfWidth + 2;
				float x1 = halfWidth - 2;
				float x2 = x1;
				float x3 = x0;
				float y0 = -halfHeight - 2.0f;
				float y1 = y0;
				float y2 = -halfHeight - 1.0f;
				float y3 = y2;
				Vector2[] vertices = new Vector2[4];
				vertices[0].X = x0;
				vertices[0].Y = y0;
				vertices[1].X = x1;
				vertices[1].Y = y1;
				vertices[2].X = x2;
				vertices[2].Y = y2;
				vertices[3].X = x3;
				vertices[3].Y = y3;
				_sensor = this.AttachPolygonSensor(vertices);
			}
			this.Enable = true;
		}
		public void AttachAction(IAction action)
		{
			_actionList.Add(action.Name, action);
		}
		public void Do(string name)
		{
			IAction action = _actionList[name];
			if (!action.IsDoing)
			{
				if (_currentAction != null)
				{
					if (_currentAction.IsDoing)
					{
						if (_currentAction.Priority <= action.Priority)
						{
							_currentAction.Break();
						}
						else
						{
							return;
						}
					}
				}
				_currentAction = action;
				action.Do();
			}
		}
		public virtual void Update()
		{
			Body body = base.Body;
			if (body.IsAwake() && body.GetType() != BodyType.Static)
			{
				_model.X = World.GameValue(body.Position.X);
				_model.Y = World.GameValue(body.Position.Y);
				_model.RotateZ = body.Rotation;
			}
			if (_currentAction != null)
			{
				_currentAction.Update();
				if (!_currentAction.IsDoing)
				{
					_currentAction = null;
				}
			}
		}
		public override void Dispose()
		{
			this.Enable = false;
			if (_model != null)
			{
				_model.Parent.Remove(_model);
				_model.Stop(true);
				_model = null;
			}
			base.Dispose();
		}
	}
}
