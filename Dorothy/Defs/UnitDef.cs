using System;
using Box2D;
using Dorothy.Game;
using Microsoft.Xna.Framework;

namespace Dorothy.Defs
{
	/// <summary>
	/// Unit`s definition.
	/// </summary>
	[Serializable]
	public class UnitDef
	{
		/// <summary>
		/// Unit`s name.
		/// </summary>
		public string Name = string.Empty;
		/// <summary>
		/// Is fixed rotation.
		/// When it`s fixed, it won`t rotate.
		/// </summary>
		public bool FixedRotation = false;
		/// <summary>
		/// Body type.
		/// </summary>
		public BodyType BodyType = BodyType.Dynamic;
		/// <summary>
		/// Unit`s group.
		/// </summary>
		public Group Group = Group.PlayerOne;
		/// <summary>
		/// Convert this definition to a new unit`s instance.
		/// </summary>
		/// <param name="world">The world.</param>
		/// <param name="x">The x position in the world.</param>
		/// <param name="y">The y position in the world.</param>
		/// <param name="angle">The angle.</param>
		/// <returns></returns>
		public virtual Unit ToUnit(Dorothy.Game.World world, float x, float y, float angle)
		{
			BodyDef bodydef = new BodyDef();
			bodydef.type = this.BodyType;
			bodydef.angle = angle;
			bodydef.fixedRotation = this.FixedRotation;
			bodydef.position.X = Game.World.B2Value(x);
			bodydef.position.Y = Game.World.B2Value(y);
			Body body = world.B2World.CreateBody(bodydef);

			Unit unit = new Unit(body, world);
			unit.Group = this.Group;
			body.SetUserData(unit);

			return unit;
		}
	}
	/// <summary>
	/// Rectangle unit`s definition.
	/// </summary>
	[Serializable]
	public class RectangleUnitDef : UnitDef
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
	/// Circle unit`s definition.
	/// </summary>
	[Serializable]
	public class CircleUnitDef : UnitDef
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
		/// Unit`s friction, 0.0f no friction, 1.0f max friction.
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
	/// Polygon unit`s definition.
	/// </summary>
	[Serializable]
	public class PolygonUnitDef : UnitDef
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
		/// Unit`s friction, 0.0f no friction, 1.0f max friction.
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
	/// <summary>
	/// Shape`s definition.
	/// </summary>
	[Serializable]
	public abstract class ShapeDef
	{
		/// <summary>
		/// Convert this definition to a new instance.
		/// </summary>
		/// <param name="unit">Unit to hold the shape.</param>
		public abstract void ToShape(Unit unit);
	}
	/// <summary>
	/// Edge definition.
	/// </summary>
	[Serializable]
	public class EdgeDef : ShapeDef
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="EdgeDef"/> struct.
		/// </summary>
		/// <param name="x1">The x1.</param>
		/// <param name="y1">The y1.</param>
		/// <param name="x2">The x2.</param>
		/// <param name="y2">The y2.</param>
		/// <param name="friction">The friction, 0.0f no friction, 1.0f max friction.</param>
		/// <param name="restitution">The restitution, 0.0f no restitution, 1.0f max restitution.</param>
		public EdgeDef(float x1, float y1, float x2, float y2, float friction = 0.4f, float restitution = 0.4f)
		{
			this.X1 = x1;
			this.Y1 = y1;
			this.X2 = x2;
			this.Y2 = y2;
			this.Friction = friction;
			this.Restitution = restitution;
		}
		/// <summary>
		/// x1
		/// </summary>
		public float X1;
		/// <summary>
		/// y1
		/// </summary>
		public float Y1;
		/// <summary>
		/// x2
		/// </summary>
		public float X2;
		/// <summary>
		/// y2
		/// </summary>
		public float Y2;
		/// <summary>
		/// Unit`s friction, 0.0f no friction, 1.0f max friction.
		/// </summary>
		public float Friction;
		/// <summary>
		/// Unit`s restitution, 0.0f no restitution, 1.0f max restitution.
		/// </summary>
		public float Restitution;
		/// <summary>
		/// Convert this definition to a new instance.
		/// </summary>
		/// <param name="unit">Unit to hold the shape.</param>
		public override void ToShape(Unit unit)
		{
			unit.AttachEdge(X1, Y1, X2, Y2, Friction, Restitution);
		}
	}
	/// <summary>
	/// Edge unit`s definition.
	/// </summary>
	[Serializable]
	public class EdgeUnitDef : UnitDef
	{
		/// <summary>
		/// Its edge definitions.
		/// </summary>
		public EdgeDef[] Edges;
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
			if (Edges != null)
			{
				foreach (var e in Edges)
				{
					e.ToShape(unit);
				}
			}
			return unit;
		}
	}
	/// <summary>
	/// Circle shape definition.
	/// </summary>
	[Serializable]
	public class CircleDef : ShapeDef
	{
		/// <summary>
		/// Its center position on unit.
		/// </summary>
		public Vector2 Center;
		/// <summary>
		/// Its radius.
		/// </summary>
		public float Radius;
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
		/// Convert this definition to a new instance.
		/// </summary>
		/// <param name="unit">Unit to hold the shape.</param>
		public override void ToShape(Unit unit)
		{
			unit.AttachCircle(Center, Radius, Density, Friction, Restitution);
		}
	}
	/// <summary>
	/// Rectangle shape definition.
	/// </summary>
	[Serializable]
	public class RectangleDef : ShapeDef
	{
		/// <summary>
		/// Its center position on unit.
		/// </summary>
		public Vector2 Center;
		/// <summary>
		/// Rectangle`s width.
		/// </summary>
		public float Width;
		/// <summary>
		/// Rectangle`s height.
		/// </summary>
		public float Height;
		/// <summary>
		/// Its initial angle.
		/// </summary>
		public float Angle;
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
		/// Convert this definition to a new instance.
		/// </summary>
		/// <param name="unit">Unit to hold the shape.</param>
		public override void ToShape(Unit unit)
		{
			unit.AttachRectangle(Center, Width, Height, Angle, Density, Friction, Restitution);
		}
	}
	/// <summary>
	/// Polygon shape definition.
	/// </summary>
	[Serializable]
	public class PolygonDef : ShapeDef
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
		/// Unit`s friction, 0.0f no friction, 1.0f max friction.
		/// </summary>
		public float Friction = 0.4f;
		/// <summary>
		/// Unit`s restitution, 0.0f no restitution, 1.0f max restitution.
		/// </summary>
		public float Restitution = 0.4f;
		/// <summary>
		/// Convert this definition to a new instance.
		/// </summary>
		/// <param name="unit">Unit to hold the shape.</param>
		public override void ToShape(Unit unit)
		{
			unit.AttachPolygon(Verteces, Density, Friction, Restitution);
		}
	}
	/// <summary>
	/// A unit being composed with different shapes.
	/// </summary>
	[Serializable]
	public class ShapeUnitDef : UnitDef
	{
		/// <summary>
		/// The shape definitions.
		/// </summary>
		public ShapeDef[] Shapes;
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
			if (this.Shapes != null)
			{
				foreach (var shape in this.Shapes)
				{
					shape.ToShape(unit);
				}
			}
			return unit;
		}
	}
}
