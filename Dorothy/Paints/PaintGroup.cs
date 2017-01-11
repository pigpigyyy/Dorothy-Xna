using System;
using Dorothy.Cameras;
using Dorothy.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Dorothy.Effects;
using Dorothy.Helpers;

namespace Dorothy.Paints
{
	public class PaintGroup : Drawable
	{
		private bool _isAutoClear = true;
		private int _lCapacity;
		private int _fCapacity;
		private int _lvCount;
		private int _fvCount;
		private VertexPositionColor[] _vertsLines;
		private VertexPositionColor[] _vertsFills;

		public bool IsAutoClear
		{
			set { _isAutoClear = value; }
			get { return _isAutoClear; }
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

		public PaintGroup(int lCapacity = 129, int fCapacity = 129)
		{
			_lCapacity = lCapacity;
			_fCapacity = fCapacity;
			_vertsLines = new VertexPositionColor[lCapacity];
			_vertsFills = new VertexPositionColor[fCapacity];
		}
		public void AddPolygon(Vector2[] vertices, Color color)
		{
			this.CheckLineCapacity(vertices.Length * 2);
			for (int i = 0; i < vertices.Length - 1; i++)
			{
				_vertsLines[_lvCount].Position = new Vector3(vertices[i], 0.0f);
				_vertsLines[_lvCount].Color = color;
				_lvCount++;
				_vertsLines[_lvCount].Position = new Vector3(vertices[i + 1], 0.0f);
				_vertsLines[_lvCount].Color = color;
				_lvCount++;
			}
			_vertsLines[_lvCount].Position = new Vector3(vertices[vertices.Length - 1], 0.0f);
			_vertsLines[_lvCount].Color = color;
			_lvCount++;
			_vertsLines[_lvCount].Position = new Vector3(vertices[0], 0.0f);
			_vertsLines[_lvCount].Color = color;
			_lvCount++;
		}
		public void AddCircle(Vector2 center, float radius, Color color, int segments = 16)
		{
			this.CheckLineCapacity(segments * 2);
			double increment = Math.PI * 2.0 / (double)segments;
			double theta = 0.0;
			for (int i = 0; i < segments; i++)
			{
				Vector2 v1 = center + radius * new Vector2((float)Math.Cos(theta), (float)Math.Sin(theta));
				Vector2 v2 = center + radius * new Vector2((float)Math.Cos(theta + increment), (float)Math.Sin(theta + increment));
				_vertsLines[_lvCount].Position = new Vector3(v1, 0.0f);
				_vertsLines[_lvCount].Color = color;
				_lvCount++;
				_vertsLines[_lvCount].Position = new Vector3(v2, 0.0f);
				_vertsLines[_lvCount].Color = color;
				_lvCount++;
				theta += increment;
			}
		}
		public void AddLine(Vector2 p1, Vector2 p2, Color color)
		{
			this.CheckLineCapacity(2);
			_vertsLines[_lvCount].Position = new Vector3(p1, 0.0f);
			_vertsLines[_lvCount].Color = color;
			_lvCount++;
			_vertsLines[_lvCount].Position = new Vector3(p2, 0.0f);
			_vertsLines[_lvCount].Color = color;
			_lvCount++;
		}
		public void AddSolidPolygon(Vector2[] vertices, Color color)
		{
			if (vertices.Length < 2)
			{
				return;
			}
			else if (vertices.Length == 2)
			{
				this.AddPolygon(vertices, color);
				return;
			}
			this.CheckFillCapacity((vertices.Length - 2) * 3);
			for (int i = 1; i < vertices.Length - 1; i++)
			{
				_vertsFills[_fvCount].Position = new Vector3(vertices[0], 0.0f);
				_vertsFills[_fvCount].Color = color;
				_fvCount++;
				_vertsFills[_fvCount].Position = new Vector3(vertices[i], 0.0f);
				_vertsFills[_fvCount].Color = color;
				_fvCount++;
				_vertsFills[_fvCount].Position = new Vector3(vertices[i + 1], 0.0f);
				_vertsFills[_fvCount].Color = color;
				_fvCount++;
			}
		}
		public void AddSolidCircle(Vector2 center, float radius, Color color, int segments = 16)
		{
			this.CheckFillCapacity((segments - 2) * 3);
			double increment = Math.PI * 2.0 / (double)segments;
			double theta = 0.0;
			Vector2 v0 = center + radius * new Vector2((float)Math.Cos(theta), (float)Math.Sin(theta));
			theta += increment;
			for (int i = 1; i < segments - 1; i++)
			{
				Vector2 v1 = center + radius * new Vector2((float)Math.Cos(theta), (float)Math.Sin(theta));
				Vector2 v2 = center + radius * new Vector2((float)Math.Cos(theta + increment), (float)Math.Sin(theta + increment));
				_vertsFills[_fvCount].Position = new Vector3(v0, 0.0f);
				_vertsFills[_fvCount].Color = color;
				_fvCount++;
				_vertsFills[_fvCount].Position = new Vector3(v1, 0.0f);
				_vertsFills[_fvCount].Color = color;
				_fvCount++;
				_vertsFills[_fvCount].Position = new Vector3(v2, 0.0f);
				_vertsFills[_fvCount].Color = color;
				_fvCount++;
				theta += increment;
			}
		}
		public void AddPoint(Vector2 p, float size, Color color)
		{
			float hs = size / 2.0f;
			Vector2[] verts = new Vector2[4]
            {
                new Vector2(p.X-hs, p.Y-hs),
                new Vector2(p.X+hs, p.Y-hs),
                new Vector2(p.X+hs, p.Y+hs),
                new Vector2(p.X-hs, p.Y+hs)
            };
			this.AddSolidPolygon(verts, color);
		}
		public void ClearAllLine()
		{
			_lvCount = 0;
		}
		public void ClearAllFill()
		{
			_fvCount = 0;
		}
		public override void Draw()
		{
			oGame.PaintEffect.World = _mWorld;
			oGame.PaintEffect.Alpha = _finalAlpha;
			oGame.PaintEffect.Apply();
			oGraphic.ZWriteEnable = this.Is3D;
			if (_lvCount > 0)
			{
				oGame.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.LineList, _vertsLines, 0, _lvCount / 2);
			}
			if (_fvCount > 0)
			{
				oGame.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.TriangleList, _vertsFills, 0, _fvCount / 3);
			}
			if (_isAutoClear)
			{
				_lvCount = _fvCount = 0;
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
		private void CheckLineCapacity(int verticesToAdd)
		{
			int need = _lvCount + verticesToAdd;
			if (need > _lCapacity)
			{
				while (_lCapacity < need)
				{
					_lCapacity = _lCapacity * 2 + 1;
				}
				VertexPositionColor[] vertices = new VertexPositionColor[_lCapacity];
				Array.Copy(_vertsLines, vertices, _lvCount);
				_vertsLines = vertices;
			}
		}
		private void CheckFillCapacity(int verticesToAdd)
		{
			int need = _fvCount + verticesToAdd;
			if (need > _fCapacity)
			{
				while (_fCapacity < need)
				{
					_fCapacity = _fCapacity * 2 + 1;
				}
				VertexPositionColor[] vertices = new VertexPositionColor[_fCapacity];
				Array.Copy(_vertsFills, vertices, _fvCount);
				_vertsFills = vertices;
			}
		}
	}
}
