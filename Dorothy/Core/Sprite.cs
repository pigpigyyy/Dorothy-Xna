using System;
using Dorothy.Cameras;
using Dorothy.Effects;
using Dorothy.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Dorothy.Core
{
	/// <summary>
	/// An entity with a texture.
	/// </summary>
	public class Sprite : Drawable
	{
		#region Field
		private int _width;
		private int _height;
		private Vector3 _origin;
		private Vector2 _transformOrigin = new Vector2(0.5f, 0.5f);
		private Vector3 _color = Vector3.One;
		private Rectangle _drawRectangle;
		private ISpriteEffect _effect = oGame.DefaultSpriteEffect;
		private Texture2D _texture;
		protected VertexPositionTexture[] _vertices = new VertexPositionTexture[4];
		#endregion

		/// <summary>
		/// Gets or sets the texture.
		/// </summary>
		/// <value>
		/// The texture.
		/// </value>
		public Texture2D Texture
		{
			set { _texture = value; }
			get { return _texture; }
		}
		/// <summary>
		/// Gets or sets the transform origin.
		/// </summary>
		/// <value>
		/// The transform origin.
		/// </value>
		public Vector2 TransformOrigin
		{
			set
			{
				_transformOrigin = value;
				this.UpdateVertices();
			}
			get { return _transformOrigin; }
		}
		/// <summary>
		/// Gets or sets the actual width.
		/// The scale X value will be set along.
		/// </summary>
		/// <value>
		/// The width.
		/// </value>
		public float Width
		{
			set { this.ScaleX = value / _width; }
			get { return _width * base.ScaleX; }
		}
		/// <summary>
		/// Gets or sets the actual height.
		/// The scale Y value will be set along.
		/// </summary>
		/// <value>
		/// The height.
		/// </value>
		public float Height
		{
			set { this.ScaleY = value / _height; }
			get { return _height * base.ScaleY; }
		}
		/// <summary>
		/// Gets or sets the R value.
		/// When R, G, B are set to 0.0f, this sprite will look totally black.
		/// When R, G, B are set to 1.0f, this sprite will look totally white.
		/// </summary>
		/// <value>
		/// The R.
		/// </value>
		public float R
		{
			set { _color.X = value; }
			get { return _color.X; }
		}
		/// <summary>
		/// Gets or sets the G value.
		/// When R, G, B are set to 0.0f, this sprite will look totally black.
		/// When R, G, B are set to 1.0f, this sprite will look totally white.
		/// </summary>
		/// <value>
		/// The G.
		/// </value>
		public float G
		{
			set { _color.Y = value; }
			get { return _color.Y; }
		}
		/// <summary>
		/// Gets or sets the B value.
		/// When R, G, B are set to 0.0f, this sprite will look totally black.
		/// When R, G, B are set to 1.0f, this sprite will look totally white.
		/// </summary>
		/// <value>
		/// The B.
		/// </value>
		public float B
		{
			set { _color.Z = value; }
			get { return _color.Z; }
		}
		/// <summary>
		/// Gets or sets the draw rectangle.
		/// </summary>
		/// <value>
		/// The draw rectangle.
		/// </value>
		public Rectangle DrawRectangle
		{
			set
			{
				_drawRectangle = value;
				_width = _drawRectangle.Width;
				_height = _drawRectangle.Height;
				float left = (float)_drawRectangle.X / _texture.Width;
				float top = (float)_drawRectangle.Y / _texture.Height;
				float right = (float)(_drawRectangle.X + _drawRectangle.Width) / _texture.Width;
				float bottom = (float)(_drawRectangle.Y + _drawRectangle.Height) / _texture.Height;
				_vertices[0].TextureCoordinate.X = left;
				_vertices[0].TextureCoordinate.Y = top;
				_vertices[1].TextureCoordinate.X = right;
				_vertices[1].TextureCoordinate.Y = top;
				_vertices[2].TextureCoordinate.X = left;
				_vertices[2].TextureCoordinate.Y = bottom;
				_vertices[3].TextureCoordinate.X = right;
				_vertices[3].TextureCoordinate.Y = bottom;
				this.UpdateVertices();
			}
			get { return _drawRectangle; }
		}
		/// <summary>
		/// Gets the vertices.
		/// </summary>
		public VertexPositionTexture[] Vertices
		{
			get { return _vertices; }
		}
		/// <summary>
		/// Gets or sets the sprite effect.
		/// </summary>
		/// <value>
		/// The sprite effect.
		/// </value>
		public ISpriteEffect Effect
		{
			set { _effect = value; }
			get { return _effect; }
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
		/// <summary>
		/// Initializes a new instance of the <see cref="Sprite"/> class.
		/// </summary>
		/// <param name="texture">The texture.</param>
		public Sprite(Texture2D texture)
		{
			_texture = texture;
		}
		/// <summary>
		/// Draws the sprite.
		/// [Called by framework]
		/// </summary>
		public override void Draw()
		{
			_effect.Color = _color;
			_effect.Texture = _texture;
			_effect.World = _mWorld;
			_effect.Alpha = _finalAlpha;
			_effect.Apply();
			oGraphic.ZWriteEnable = this.Is3D;
			oGame.GraphicsDevice.DrawUserPrimitives<VertexPositionTexture>(PrimitiveType.TriangleStrip, _vertices, 0, 2);
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
		/// <summary>
		/// Updates the vertices.
		/// </summary>
		private void UpdateVertices()
		{
			float hotX = _drawRectangle.Width * _transformOrigin.X;
			_vertices[0].Position.X = -hotX;
			_vertices[1].Position.X = _drawRectangle.Width - hotX;
			_vertices[2].Position.X = -hotX;
			_vertices[3].Position.X = _drawRectangle.Width - hotX;
			float hotY = _drawRectangle.Height * _transformOrigin.Y;
			_vertices[0].Position.Y = hotY;
			_vertices[1].Position.Y = hotY;
			_vertices[2].Position.Y = hotY - _drawRectangle.Height;
			_vertices[3].Position.Y = hotY - _drawRectangle.Height;
			_origin = new Vector3
			(
				(_vertices[0].Position.X + _vertices[1].Position.X) * 0.5f,
				(_vertices[0].Position.Y + _vertices[2].Position.Y) * 0.5f,
				0.0f
			);
		}
		/// <summary>
		/// Test whether a ray is running through the sprite.
		/// </summary>
		/// <param name="ray">A ray.</param>
		/// <returns>
		/// When the ray is through the sprite it returns the distance between ray start point
		/// and the point where they intersect. Otherwise returns null.
		/// </returns>
		public override float? Intersects(Ray ray)
		{
			Matrix world = Matrix.Invert(_mWorld);
			Vector3 rayStart, rayEnd, end = ray.Position + ray.Direction;
			Vector3.Transform(ref ray.Position, ref world, out rayStart);
			Vector3.Transform(ref end, ref world, out rayEnd);
			Ray finalRay = new Ray(rayStart, rayEnd - rayStart);
			float? distance = finalRay.Intersects(oHelper.StandardPlane);
			if (distance != null)
			{
				Vector3 interPoint = finalRay.Position + (float)distance * finalRay.Direction;
				if (oHelper.PointInRectangle(
					interPoint.X, interPoint.Y,
					_vertices[0].Position.X,
					_vertices[0].Position.Y,
					_vertices[3].Position.X,
					_vertices[3].Position.Y))
				{
					return distance;
				}
			}
			return null;
		}
	}
}
