using System;
using Dorothy.Cameras;
using Dorothy.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Dorothy.Helpers;

namespace Dorothy.Paints
{
	public class LinePaint : Drawable
	{
		private VertexPositionColor[] _vertsLines;
		private Vector2[] _vertices;
		private Color _color;

		public Color Color
		{
			set
			{
				_color = value;
				for (int i = 0; i < _vertsLines.Length; i++)
				{
					_vertsLines[i].Color = value;
				}
			}
			get { return _color; }
		}
		public Vector2[] Vertices
		{
			set
			{
				_vertices = new Vector2[value.Length];
				Array.Copy(value, _vertices, value.Length);
				_vertsLines = new VertexPositionColor[_vertices.Length];
				for (int i = 0; i < _vertices.Length; i++)
				{
					_vertsLines[i].Position = new Vector3(_vertices[i], 0);
					_vertsLines[i].Color = _color;
				}
			}
			get { return _vertices; }
		}
		/// <summary>
		/// Gets or sets a value indicating whether it is 3D item.
		/// A non 3D item won`t write depth value when which can overlap another one.
		/// </summary>
		/// <value>
		///   <c>true</c> if it`s 3D item; otherwise, <c>false</c>.
		/// </value>
		public bool Is3D
		{
			set;
			get;
		}

		public LinePaint(Vector2[] vertices, Color color)
		{
			_color = color;
			this.Vertices = vertices;
		}
		public override void Draw()
		{
			oGame.PaintEffect.World = _mWorld;
			oGame.PaintEffect.Alpha = _finalAlpha;
			oGame.PaintEffect.Apply();
			oGraphic.ZWriteEnable = this.Is3D;
			oGame.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.LineStrip, _vertsLines, 0, _vertices.Length - 1);
		}
		/// <summary>
		/// Gets the item itself ready.
		/// </summary>
		protected override void GetItselfReady()
		{
			base.GetItselfReady();
			switch (oGraphic.SortMode)
			{
				case SortMode.AllSort:
					oDrawQueue.EnSecond(this);
					break;
				case SortMode.AlphaSort:
					if (this.Is3D && _finalAlpha == 1.0f)
					{ oDrawQueue.EnFirst(this); }
					else
					{ oDrawQueue.EnSecond(this); }
					break;
				case SortMode.ZSort:
					oDrawQueue.EnFirst(this);
					break;
			}
		}
	}
}
