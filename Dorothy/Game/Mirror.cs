using Dorothy.Cameras;
using Dorothy.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System;
using Dorothy.Effects;
using Dorothy.Helpers;
using Dorothy.Data;

namespace Dorothy.Game
{
	public class Mirror : Drawable, IDisposable
	{
		private int _width;
		private int _height;
		private VertexPositionTexture[] _vertices;
		private BasicCamera _camera = new BasicCamera();
		public RenderTarget2D _face;
		private MirrorEffect _effect = MirrorEffect.Instance;
		private static List<Mirror> _mirrors = new List<Mirror>();

		public event Action Before;
		public event Action After;

		public static void Fill()
		{
			if (_mirrors.Count == 0) { return; }
			ICamera currentCamera = oCameraManager.CurrentCamera;
			SortMode mode = oGraphic.SortMode;
			oGraphic.SortMode = SortMode.AllSort;
			for (int i = 0; i < _mirrors.Count; i++)
			{
				if (_mirrors[i].Before != null) { _mirrors[i].Before(); }
				Plane mirrorPlane = Plane.Transform(oHelper.StandardPlane, _mirrors[i]._mWorld);
				float distance = mirrorPlane.DotCoordinate(currentCamera.Position);
				Vector3 camPos = currentCamera.Position - 2 * distance * mirrorPlane.Normal;
				distance = mirrorPlane.DotCoordinate(currentCamera.Target);
				Vector3 camTarget = currentCamera.Target - 2 * distance * mirrorPlane.Normal;
				Vector3 upPosition = currentCamera.Position + currentCamera.Up;
				mirrorPlane.DotCoordinate(ref upPosition, out distance);
				Vector3 camUp = upPosition - 2 * distance * mirrorPlane.Normal - camPos;
				_mirrors[i]._camera.Set(ref camPos, ref camTarget, ref camUp);
				bool visible = _mirrors[i].IsVisible;
				_mirrors[i].IsVisible = false;
				oCameraManager.Apply(_mirrors[i]._camera);
				oGame.GraphicsDevice.SetRenderTarget(_mirrors[i]._face);
				oSceneManager.Draw
				(
					drawable =>
					{
						return !(drawable is Mirror) && mirrorPlane.DotCoordinate(Vector3.Transform(Vector3.Zero, drawable.World)) > 0;
					}
				);
				_mirrors[i].IsVisible = visible;
				if (_mirrors[i].After != null) { _mirrors[i].After(); }
			}
			oCameraManager.Apply(currentCamera);
			oGame.GraphicsDevice.SetRenderTarget(null);
			oGraphic.SortMode = mode;
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

		public Mirror(int width, int height)
			: this(width, height,
			oGame.GraphicsDevice.PresentationParameters.BackBufferWidth,
			oGame.GraphicsDevice.PresentationParameters.BackBufferHeight)
		{ }
		public Mirror(int width, int height, int bufferWidth, int bufferHeight)
		{
			_width = width;
			_height = height;
			PresentationParameters pp = oGame.GraphicsDevice.PresentationParameters;
			_face = new RenderTarget2D
			(
				oGame.GraphicsDevice,
				bufferWidth,
				bufferHeight,
				false,
				pp.BackBufferFormat,
				pp.DepthStencilFormat
			);
			float halfWidth = (float)width / 2;
			float halfHeight = (float)height / 2;
			_vertices = new VertexPositionTexture[4]
            {
                new VertexPositionTexture(new Vector3(-halfWidth, halfHeight, 0),new Vector2(0, 0)),
                new VertexPositionTexture(new Vector3(halfWidth, halfHeight, 0),new Vector2(0, 0)),
                new VertexPositionTexture(new Vector3(-halfWidth, -halfHeight, 0),new Vector2(0, 0)),
                new VertexPositionTexture(new Vector3(halfWidth, -halfHeight, 0),new Vector2(0, 0))
            };
			_mirrors.Add(this);
		}
		public override void Draw()
		{
			_effect.MirrorTexture = _face;
			_effect.World = _mWorld;
			_effect.View = oEffectManager.View;
			_effect.Projection = oEffectManager.Projection;
			_effect.MirrorView = Matrix.CreateLookAt(_camera.Position, _camera.Target, _camera.Up);
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
		public void Dispose()
		{
			if (_face != null)
			{
				_mirrors.Remove(this);
				_face.Dispose();
			}
		}
	}
}
