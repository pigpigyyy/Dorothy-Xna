using Box2D;
using Dorothy.Paints;
using Microsoft.Xna.Framework;

namespace TestGame.Debugs
{
    public class MyDebugDraw : PaintGroup, IDebugDraw
    {
        /// <summary>
        /// Gets or sets the flags.
        /// </summary>
        /// <value>
        /// The flags.
        /// </value>
        public DebugDrawFlags Flags
        {
            set;
            get;
        }
        /// <summary>
        /// Append flags to the current flags.
        /// </summary>
        /// <param name="flags">The flags.</param>
	    public void AppendFlags(DebugDrawFlags flags)
        {
            this.Flags |= flags;
        }
        /// <summary>
        /// Clear flags from the current flags.
        /// </summary>
        /// <param name="flags">The flags.</param>
	    public  void ClearFlags(DebugDrawFlags flags)
        {
            this.Flags &= ~flags;
        }
        public MyDebugDraw()
        {
            base.ScaleX = base.ScaleY = Dorothy.Game.World.B2FACTOR;
        }
        public void DrawPolygon(Vector2[] vertices, Color color)
        {
            base.AddPolygon(vertices, color);
        }
        public void DrawSolidPolygon(Vector2[] vertices, Color color)
        {
            color.A = 128;
            base.AddSolidPolygon(vertices, color);
            color.A = 255;
            base.AddPolygon(vertices, color);
        }
        public void DrawSolidCircle(Vector2 center, float radius, Vector2 axis, Color color)
        {
            base.AddSolidCircle(center, radius, color);
            base.AddCircle(center, radius, color);
            base.AddLine(center, center + axis * radius, color);
        }
        public void DrawCircle(Vector2 center, float radius, Color color)
        {
            base.AddCircle(center, radius, color);
        }
        public void DrawSegment(Vector2 p1, Vector2 p2, Color color)
        {
            base.AddLine(p1, p2, color);
        }
        public void DrawTransform(ref Transform xf)
        {
            float axisScale = 0.4f;
            Vector2 p1 = xf.Position;
            Vector2 p2 = p1 + axisScale * xf.R.col1;
            base.AddLine(p1, p2, Color.Red);
            p2 = p1 + axisScale * xf.R.col2;
            base.AddLine(p1, p2, Color.Green);
        }
    }
}
