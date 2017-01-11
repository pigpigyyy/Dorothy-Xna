using System;
using Box2D;
using Microsoft.Xna.Framework;

namespace Dorothy.Game
{
	public class Unit : IDisposable
	{
		#region 成员
		private bool _bEnableCollision = true;
		private Body _body;
		private World _world;
		private Group _group = Group.PlayerOne;
		#endregion

		#region 属性
		public Body Body
		{
			get { return _body; }
		}
		public Vector2 Position
		{
			get { return World.GameValue(_body.Position); }
		}
		public float VelocityX
		{
			set { _body.LinearVelocityX = World.B2Value(value); }
			get { return World.GameValue(_body.LinearVelocityX); }
		}
		public float VelocityY
		{
			set { _body.LinearVelocityY = World.B2Value(value); }
			get { return World.GameValue(_body.LinearVelocityY); }
		}
		public Vector2 Velocity
		{
			set
			{
				_body.SetLinearVelocity(World.B2Value(ref value));
			}
			get
			{
				return World.GameValue(_body.GetLinearVelocity());
			}
		}
		public Group Group
		{
			set { _group = value; }
			get { return _group; }
		}
		public World World
		{
			get { return _world; }
		}
		public bool EnableCollision
		{
			set { _bEnableCollision = value; }
			get { return _bEnableCollision; }
		}
		public bool IsDisposed
		{
			get { return (_body == null); }
		}
		#endregion

		public Unit(Body body, World world)
		{
			_body = body;
			_world = world;
		}
		public void ApplyForce(Vector2 direction, float power)
		{
			float angle = (float)Math.Atan2(direction.Y, direction.X);
			this.ApplyForce(angle, power);
		}
		public void ApplyForce(float angle, float power)
		{
			_body.ApplyForce(
				new Vector2(
					power * (float)Math.Cos(angle),
					power * (float)Math.Sin(angle)),
				_body.GetWorldCenter());
		}
		public void ApplyImpulse(Vector2 direction, float impulse)
		{
			float angle = (float)Math.Atan2(direction.Y, direction.X);
			this.ApplyImpulse(angle, impulse);
		}
		public void ApplyImpulse(float angle, float impulse)
		{
			_body.ApplyLinearImpulse(
				new Vector2(
					impulse * (float)Math.Cos(angle),
					impulse * (float)Math.Sin(angle)),
				_body.GetWorldCenter());
		}
		public Fixture AttachRectangle(Vector2 center, float width, float height, float angle,
			float density, float friction, float restitution)
		{
			PolygonShape shape = new PolygonShape();
			shape.SetAsBox(
				World.B2Value(width * 0.5f),
				World.B2Value(height * 0.5f),
				World.B2Value(center),
				angle);
			FixtureDef fixtureDef = new FixtureDef();
			fixtureDef.shape = shape;
			fixtureDef.density = density;
			fixtureDef.friction = friction;
			fixtureDef.restitution = restitution;
			return _body.CreateFixture(fixtureDef);
		}
		public Fixture AttachRectangle(float width, float height,
			float density, float friction, float restitution)
		{
			return this.AttachRectangle(Vector2.Zero, width, height, 0, density, friction, restitution);
		}
		public Fixture AttachPolygon(Vector2[] vertices,
			float density, float friction, float restitution)
		{
			PolygonShape shape = new PolygonShape();
			int length = vertices.Length;
			Vector2[] vs = new Vector2[length];
			for (int i = 0; i < vertices.Length; i++)
			{
				vs[i] = Game.World.B2Value(vertices[i]);
			}
			shape.Set(vs);
			FixtureDef fixtureDef = new FixtureDef();
			fixtureDef.shape = shape;
			fixtureDef.density = density;
			fixtureDef.friction = friction;
			fixtureDef.restitution = restitution;
			return _body.CreateFixture(fixtureDef);
		}
		public Fixture AttachPolygonNoMass(Vector2[] vertices,
			float friction, float restitution)
		{
			LoopShape shape = new LoopShape();
			Vector2[] vs = new Vector2[vertices.Length];
			for (int i = 0; i < vertices.Length; i++)
			{
				vs[i] = Game.World.B2Value(vertices[i]);
			}
			shape._vertices = vs;
			shape._count = vertices.Length;
			FixtureDef fixtureDef = new FixtureDef();
			fixtureDef.shape = shape;
			fixtureDef.friction = friction;
			fixtureDef.restitution = restitution;
			return _body.CreateFixture(fixtureDef);
		}
		public Fixture AttachCircle(Vector2 center, float radius,
			float density, float friction, float restitution)
		{
			CircleShape shape = new CircleShape();
			shape._p = World.B2Value(center);
			shape._radius = World.B2Value(radius);
			FixtureDef fixtureDef = new FixtureDef();
			fixtureDef.shape = shape;
			fixtureDef.density = density;
			fixtureDef.friction = friction;
			fixtureDef.restitution = restitution;
			return _body.CreateFixture(fixtureDef);
		}
		public Fixture AttachCircle(float radius,
			float density, float friction, float restitution)
		{
			return this.AttachCircle(Vector2.Zero, radius, density, friction, restitution);
		}
		public Fixture AttachEdge(float x1, float y1, float x2, float y2,
			float friction, float restitution)
		{
			PolygonShape shape = new PolygonShape();
			shape.SetAsEdge(
				World.B2Value(new Vector2(x1, y1)),
				World.B2Value(new Vector2(x2, y2)));
			FixtureDef fixtureDef = new FixtureDef();
			fixtureDef.shape = shape;
			fixtureDef.density = 0.0f;
			fixtureDef.friction = friction;
			fixtureDef.restitution = restitution;
			return _body.CreateFixture(fixtureDef);
		}
		public Sensor AttachRectangleSensor(float width, float height,
			Vector2 center, float angle, int maxSense = 10)
		{
			Fixture fixture = this.AttachRectangle(center, width, height, angle, 0, 0, 0);
			fixture.SetSensor(true);
			Sensor info = new Sensor(this, fixture, maxSense);
			return info;
		}
		public Sensor AttachPolygonSensor(Vector2[] verteces, int maxSense = 10)
		{
			Fixture fixture = this.AttachPolygon(verteces, 0, 0, 0);
			fixture.SetSensor(true);
			Sensor info = new Sensor(this, fixture, maxSense);
			return info;
		}
		public Sensor AttachCircleSensor(Vector2 center, float radius, int maxSense = 10)
		{
			Fixture fixture = this.AttachCircle(center, radius, 0, 0, 0);
			fixture.SetSensor(true);
			Sensor info = new Sensor(this, fixture, maxSense);
			return info;
		}
		public virtual void Dispose()
		{
			if (_body != null)
			{
				_world.B2World.DestroyBody(_body);
				_body = null;
				_world = null;
			}
		}
	}
}
