using System;
using Box2D;
using Dorothy.Data;
using Dorothy.Game;
using Microsoft.Xna.Framework;

namespace Dorothy.Defs
{
	/// <summary>
	/// Game role`s definition.
	/// </summary>
	[Serializable]
	public class RoleDef : UnitDef
	{
		/// <summary>
		/// Model2D`s name.
		/// </summary>
		public string ModName = string.Empty;
		/// <summary>
		/// Its width.
		/// </summary>
		public float Width;
		/// <summary>
		/// Its height.
		/// </summary>
		public float Height;
		/// <summary>
		/// Unit`s density in kg/m^3.
		/// </summary>
		public float Density = 1.0f;
		/// <summary>
		/// Unit`s friction, 0.0f no friction, 1.0f max friction.
		/// </summary>
		public float Friction = 0.4f;
		/// <summary>
		/// Unit`s restitution, 0.0f no restitution, 1.0f max restitution.
		/// </summary>
		public float Restitution = 0.4f;
		/// <summary>
		/// Action definitions.
		/// </summary>
		public ActionDef[] Actions;
		/// <summary>
		/// Convert this definition to a new unit`s instance.
		/// </summary>
		/// <param name="world">The world.</param>
		/// <param name="x">The x position in the world.</param>
		/// <param name="y">The y position in the world.</param>
		/// <param name="angle">The angle.</param>
		/// <returns></returns>
		public override Unit ToUnit(Game.World world, float x, float y, float angle)
		{
			BodyDef bodydef = new BodyDef();
			bodydef.type = this.BodyType;
			bodydef.angle = angle;
			bodydef.fixedRotation = this.FixedRotation;
			bodydef.position.X = Game.World.B2Value(x);
			bodydef.position.Y = Game.World.B2Value(y);
			Body body = world.B2World.CreateBody(bodydef);

			float halfWidth = this.Width * 0.5f;
			float halfHeight = this.Height * 0.5f;
			if (halfWidth != 0.0f && halfHeight != 0.0f)
			{
				float x0 = Game.World.B2Value(-halfWidth + 2);
				float x1 = Game.World.B2Value(halfWidth - 2);
				float x2 = Game.World.B2Value(halfWidth);
				float x3 = Game.World.B2Value(-halfWidth);
				float y0 = Game.World.B2Value(-halfHeight);
				float y1 = y0;
				float y2 = Game.World.B2Value(halfHeight);
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

				PolygonShape shape = new PolygonShape();
				shape.Set(vertices);
				FixtureDef fixtureDef = new FixtureDef();
				fixtureDef.shape = shape;
				fixtureDef.density = this.Density;
				fixtureDef.friction = this.Friction;
				fixtureDef.restitution = this.Restitution;
				body.CreateFixture(fixtureDef);
			}

			Model2D model = oContent.GetModel2D(this.ModName);
			Role role = new Role(model, body, world, halfWidth, halfHeight);
			role.Group = this.Group;
			body.SetUserData(role);
			if (this.Actions != null)
			{
				foreach (var a in this.Actions)
				{
					role.AttachAction(a.ToAction(role));
				}
			}
			world.Add(model);

			return role;
		}
	}
}
