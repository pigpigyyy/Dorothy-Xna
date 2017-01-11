using Dorothy.Cameras;
using Dorothy.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Dorothy.Helpers;
using System;

namespace Dorothy.Paints
{
	public class RectanglePaint : Drawable
	{
		private bool _isSolid;
		private bool _isOutlined = true;
		private float _width;
		private float _height;
		private VertexPositionColor[] _vertices = new VertexPositionColor[4];
		private short[] _lineIndices = new short[5] { 0, 1, 3, 2, 0 };

		public Color FillColor
		{
			set;
			get;
		}
		public Color OutlineColor
		{
			set;
			get;
		}
		public bool IsSolid
		{
			set { _isSolid = value; }
			get { return _isSolid; }
		}
		public bool IsOutlined
		{
			set { _isOutlined = value; }
			get { return _isOutlined; }
		}
		public float Width
		{
			set
			{
				_width = value;
				this.UpdateVeticesX();
			}
			get { return _width; }
		}
		public float Height
		{
			set
			{
				_height = value;
				this.UpdateVeticesY();
			}
			get { return _height; }
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

		public RectanglePaint(float width, float height, Color fill, Color outline)
		{
			this.Width = width;
			this.Height = height;
			this.FillColor = fill;
			this.OutlineColor = outline;
		}
		public override void Draw()
		{
			oGame.PaintEffect.World = _mWorld;
			oGame.PaintEffect.Alpha = _finalAlpha;
			oGame.PaintEffect.Apply();
			oGraphic.ZWriteEnable = this.Is3D;
			if (_isOutlined)
			{
				this.ChangeColor(this.OutlineColor);
				oGame.GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionColor>(PrimitiveType.LineStrip, _vertices, 0, 4, _lineIndices, 0, 4);
			}
			if (_isSolid)
			{
				this.ChangeColor(this.FillColor);
				oGame.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.TriangleStrip, _vertices, 0, 2);
			}
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
		private void ChangeColor(Color color)
		{
			for (int i = 0; i < 4; i++)
			{
				_vertices[i].Color = color;
			}
		}
		private void UpdateVeticesX()
		{
			float halfWidth = _width * 0.5f;
			_vertices[0].Position.X = -halfWidth;
			_vertices[1].Position.X = halfWidth;
			_vertices[2].Position.X = -halfWidth;
			_vertices[3].Position.X = halfWidth;
		}
		private void UpdateVeticesY()
		{
			float halfHeight = _height * 0.5f;
			_vertices[0].Position.Y = halfHeight;
			_vertices[1].Position.Y = halfHeight;
			_vertices[2].Position.Y = -halfHeight;
			_vertices[3].Position.Y = -halfHeight;
		}
	}
}
