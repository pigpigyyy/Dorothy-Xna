using System;
using Box2D;
using Dorothy.Core;
using Dorothy.Data;
using Dorothy.Game;
using Microsoft.Xna.Framework;

namespace Dorothy.Defs
{
	/// <summary>
	/// Draw unit`s definition.
	/// </summary>
	[Serializable]
	public class DrawUnitDef : UnitDef
	{
		/// <summary>
		/// Sprite`s name.
		/// </summary>
		public string SpriteName = string.Empty;
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
			Sprite sprite = oContent.GetSprite(this.SpriteName);
			sprite.RotateZ = angle;

			BodyDef bodydef = new BodyDef();
			bodydef.type = this.BodyType;
			bodydef.angle = angle;
			bodydef.fixedRotation = this.FixedRotation;
			bodydef.position.X = Game.World.B2Value(x);
			bodydef.position.Y = Game.World.B2Value(y);
			Body body = world.B2World.CreateBody(bodydef);

			DrawUnit drawUnit = new DrawUnit(sprite, body, world);
			drawUnit.Group = this.Group;
			body.SetUserData(drawUnit);

			world.Add(sprite);

			return drawUnit;
		}
	}
	/// <summary>
	/// Rectangle draw unit`s definition.
	/// </summary>
	[Serializable]
	public class RectDrawUnitDef : DrawUnitDef
	{
		/// <summary>
		/// Rectangle`s width.
		/// </summary>
		public float Width;
		/// <summary>
		/// Rectangle`s height.
		/// </summary>
		public float Height;
		/// <summary>
		/// Unit`s density in kg/m^3.
		/// </summary>
		public float Density = 1.0f;
		/// <summary>
		/// Unit`s friction.
		/// </summary>
		public float Friction = 0.4f;
		/// <summary>
		/// Unit`s restitution, 0.0f no restitution, 1.0f max restitution.
		/// </summary>
		public float Restitution = 0.4f;
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
			Unit unit = base.ToUnit(world, x, y, angle);
			unit.AttachRectangle(Width, Height, Density, Friction, Restitution);
			return unit;
		}
	}
	/// <summary>
	/// Circle draw unit`s definition.
	/// </summary>
	[Serializable]
	public class CircleDrawUnitDef : DrawUnitDef
	{
		/// <summary>
		/// Unit`s radius.
		/// </summary>
		public float Radius;
		/// <summary>
		/// Unit`s density in kg/m^3.
		/// </summary>
		public float Density = 1.0f;
		/// <summary>
		/// Unit`s friction.
		/// </summary>
		public float Friction = 0.4f;
		/// <summary>
		/// Unit`s restitution, 0.0f no restitution, 1.0f max restitution.
		/// </summary>
		public float Restitution = 0.4f;
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
			Unit unit = base.ToUnit(world, x, y, angle);
			unit.AttachCircle(Radius, Density, Friction, Restitution);
			return unit;
		}
	}
	/// <summary>
	/// Polygon draw unit`s definition.
	/// </summary>
	[Serializable]
	public class PolygonDrawUnitDef : DrawUnitDef
	{
		/// <summary>
		/// Vertices in counterclockwise order.
		/// </summary>
		public Vector2[] Verteces;
		/// <summary>
		/// Unit`s density in kg/m^3.
		/// </summary>
		public float Density = 1.0f;
		/// <summary>
		/// Unit`s friction.
		/// </summary>
		public float Friction = 0.4f;
		/// <summary>
		/// Unit`s restitution, 0.0f no restitution, 1.0f max restitution.
		/// </summary>
		public float Restitution = 0.4f;
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
			Unit unit = base.ToUnit(world, x, y, angle);
			unit.AttachPolygon(Verteces, Density, Friction, Restitution);
			return unit;
		}
	}
}
