using Dorothy.Core;
using Microsoft.Xna.Framework;
using Dorothy.Cameras;

namespace Dorothy.Game
{
	public class World : DrawComponent
	{
		public const float B2FACTOR = 100.0f;
		private int _velocityIterations = 8;
		private int _positionIterations = 6;
		private float _deltaTime;
		private Box2D.World _world;

		public int VelocityIterations
		{
			set { _velocityIterations = value; }
			get { return _velocityIterations; }
		}
		public int PositionIterations
		{
			set { _positionIterations = value; }
			get { return _positionIterations; }
		}
		public float DeltaTime
		{
			set { _deltaTime = value; }
			get { return _deltaTime; }
		}
		public bool IsRunning
		{
			set { this.Enable = value; }
			get { return this.Enable; }
		}
		public Box2D.World B2World
		{
			get { return _world; }
		}

		public static float B2Value(float value)
		{
			return value / B2FACTOR;
		}
		public static Vector2 B2Value(Vector2 value)
		{
			return new Vector2(value.X / B2FACTOR, value.Y / B2FACTOR);
		}
		public static Vector2 B2Value(ref Vector2 value)
		{
			return new Vector2(value.X / B2FACTOR, value.Y / B2FACTOR);
		}
		public static float GameValue(float value)
		{
			return value * B2FACTOR;
		}
		public static Vector2 GameValue(Vector2 value)
		{
			return new Vector2(value.X * B2FACTOR, value.Y * B2FACTOR);
		}
		public static Vector2 GameValue(ref Vector2 value)
		{
			return new Vector2(value.X * B2FACTOR, value.Y * B2FACTOR);
		}

		public World(Vector2 gravity)
			: base(oSceneManager.CurrentScene.Controller)
		{
			_world = new Box2D.World(gravity, true);
			_world.ContactListener = new ContactListener();
			_deltaTime = oGame.TargetFrameInterval / 1000.0f;
			oSceneManager.CurrentScene.Root.Add(this);
			base.Enable = true;
		}
		public override void Update()
		{
			_world.Step(_deltaTime, _velocityIterations, _positionIterations);
		}
		protected override void GetReady()
		{
			base.GetReady();
			if (_world.DebugDraw != null)
			{
				_world.DrawDebugData();
			}
		}
		public override void Draw()
		{ }
	}
}
