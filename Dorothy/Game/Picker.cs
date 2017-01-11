using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Box2D;
using Dorothy.Defs;

namespace Dorothy.Game
{
	public class Picker
	{
		private MouseJoint _joint;
		private World _world;
		private Unit _unit;

		public bool IsPicking
		{
			get { return _joint != null; }
		}
		public Picker(World world)
		{
			_world = world;
			UnitDef unitDef = new UnitDef();
			unitDef.BodyType = BodyType.Static;
			unitDef.Group = Group.Terrain;
			_unit = unitDef.ToUnit(world, 0, 0, 0);
		}
		public Unit Pick(ref Vector2 point)
		{
			if (_joint != null)
			{
				return null;
			}
			//创建一个小矩形
			Vector2 pickPoint = World.B2Value(point);
			AABB aabb;
			Vector2 d = new Vector2(0.001f, 0.001f);
			aabb.lowerBound = pickPoint - d;
			aabb.upperBound = pickPoint + d;
			//查询与该AABB相交的物体
			Fixture result = null;
			_world.B2World.QueryAABB(
			fixtureProxy =>
			{
				if (fixtureProxy.fixture != null)
				{
					Body body = fixtureProxy.fixture.GetBody();
					if (body.GetType() == BodyType.Dynamic)
					{
						bool inside = fixtureProxy.fixture.TestPoint(pickPoint);
						if (inside)
						{
							result = fixtureProxy.fixture;
							return false;
						}
					}
				}
				return true;
			},
			ref aabb);
			if (result != null)
			{
				Body body = result.GetBody();
				MouseJointDef md = new MouseJointDef();
				md.collideConnected = true;
				md.bodyA = _unit.Body;
				md.bodyB = body;
				md.target = pickPoint;
				md.maxForce = 1000.0f * body.GetMass();
				_joint = (MouseJoint)_world.B2World.CreateJoint(md);
				body.SetAwake(true);
				return (Unit)(body.GetUserData());
			}
			return null;
		}
		public void Move(ref Vector2 point)
		{
			if (_joint != null)
			{
				_joint.SetTarget(World.B2Value(point));
			}
		}
		public void Drop()
		{
			if (_joint != null)
			{
				_world.B2World.DestroyJoint(_joint);
				_joint = null;
			}
		}
		public void GetJointLine(out Vector2 start, out Vector2 end)
		{
			if (_joint != null)
			{
				start = World.GameValue(_joint.GetAnchorA());
				end = World.GameValue(_joint.GetAnchorB());
			}
			else
			{
				start = end = Vector2.Zero;
			}
		}
	}
}
