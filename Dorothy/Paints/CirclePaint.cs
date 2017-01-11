using System;
using Dorothy.Cameras;
using Dorothy.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Dorothy.Helpers;

namespace Dorothy.Paints
{
	public class CirclePaint : Drawable
	{
		private bool _isSolid;
		private bool _isOutlined = true;
		private int _segments;
		private float _radius;
		private VertexPositionColor[] _vertsLines;
		private VertexPositionColor[] _vertsFills;

		public Color FillColor
		{
			set
			{
				for (int i = 0; i < _vertsFills.Length; i++)
				{
					_vertsFills[i].Color = value;
				}
			}
			get { return _vertsFills[0].Color; }
		}
		public Color OutlineColor
		{
			set
			{
				for (int i = 0; i < _vertsLines.Length; i++)
				{
					_vertsLines[i].Color = value;
				}
			}
			get { return _vertsLines[0].Color; }
		}
		public bool IsSolid
		{
			set
			{
				_isSolid = value;
			}
			get { return _isSolid; }
		}
		public bool IsOutlined
		{
			set { _isOutlined = value; }
			get { return _isOutlined; }
		}
		public float Radius
		{
			set
			{
				_radius = value;
				this.UpdateVetices();
			}
			get { return _radius; }
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

		public CirclePaint(float radius, Color fill, Color outline, int segments = 16)
		{
			_segments = segments;
			_vertsFills = new VertexPositionColor[(segments - 2) * 3];
			_vertsLines = new VertexPositionColor[segments * 2];
			this.Radius = radius;
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
				oGame.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.LineList, _vertsLines, 0, _segments);
			}
			if (_isSolid)
			{
				oGame.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.TriangleList, _vertsFills, 0, _segments - 2);
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
		private void UpdateVetices()
		{
			double increment = Math.PI * 2.0 / (double)_segments;

			int count = 0;
			double theta = 0.0;
			Vector2 v0 = _radius * new Vector2((float)Math.Cos(theta), (float)Math.Sin(theta));
			theta += increment;
			for (int i = 1; i < _segments - 1; i++)
			{
				Vector2 v1 = _radius * new Vector2((float)Math.Cos(theta), (float)Math.Sin(theta));
				Vector2 v2 = _radius * new Vector2((float)Math.Cos(theta + increment), (float)Math.Sin(theta + increment));
				_vertsFills[count].Position = new Vector3(v0, 0.0f);
				count++;
				_vertsFills[count].Position = new Vector3(v1, 0.0f);
				count++;
				_vertsFills[count].Position = new Vector3(v2, 0.0f);
				count++;
				theta += increment;
			}

			count = 0;
			theta = 0.0;
			for (int i = 0; i < _segments; i++)
			{
				Vector2 v1 = _radius * new Vector2((float)Math.Cos(theta), (float)Math.Sin(theta));
				Vector2 v2 = _radius * new Vector2((float)Math.Cos(theta + increment), (float)Math.Sin(theta + increment));
				_vertsLines[count].Position = new Vector3(v1, 0.0f);
				count++;
				_vertsLines[count].Position = new Vector3(v2, 0.0f);
				count++;
				theta += increment;
			}
		}
	}
}
