using System;
using System.Collections.Generic;
using System.Diagnostics;
using Dorothy.Cameras;
using Dorothy.Core;
using Dorothy.Data;
using Dorothy.Effects;
using Dorothy.Events;
using Dorothy.Game;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Dorothy
{
	/// <summary>
	/// The game class, singleton.
	/// </summary>
	public class oGame : Microsoft.Xna.Framework.Game
	{
		#region Field
		private float _interval;
		private float _fieldOfView = MathHelper.PiOver4;
		private float _nearPlaneDistance = 1.0f;
		private float _farPlaneDistance = 10000.0f;
		private DrawRoot _root;
		private UpdateController _updateController;
		private GraphicsDeviceManager _graphics;
		private SpriteBatch _spriteBatch;
		private VertexPositionTexture[] _screenVertices;
		private RenderTarget2D[] _targets;
		private static GraphicsDevice _graphicDevice;
		private static oGame _instance;
		#endregion

		/// <summary>
		/// Gets or sets a value indicating whether game is in full screen mode.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if game is full screen; otherwise, <c>false</c>.
		/// </value>
		public static bool IsFullScreen
		{
			set
			{
				if (value)
				{
					if (!_instance._graphics.IsFullScreen)
					{
						_instance._graphics.IsFullScreen = true;
						_instance._graphics.ApplyChanges();
						_instance.UpdateProjection();
						_instance.UpdateRenderTargets();
					}
				}
				else if (_instance._graphics.IsFullScreen)
				{
					_instance._graphics.IsFullScreen = false;
					_instance._graphics.ApplyChanges();
					_instance.UpdateProjection();
					_instance.UpdateRenderTargets();
				}
			}
			get { return _instance._graphics.IsFullScreen; }
		}
		/// <summary>
		/// Gets or sets the field of view in radius.
		/// </summary>
		/// <value>
		/// The field of view.
		/// </value>
		public static float FieldOfView
		{
			set
			{
				_instance._fieldOfView = value;
				_instance.UpdateProjection();
			}
			get { return _instance._fieldOfView; }
		}
		/// <summary>
		/// Gets or sets the target interval between two frames in millisecond.
		/// </summary>
		public static float TargetFrameInterval
		{
			set
			{
				_instance._interval = value;
				_instance.TargetElapsedTime = TimeSpan.FromMilliseconds(value);
			}
			get { return _instance._interval; }
		}
		/// <summary>
		/// Gets or sets the near plane distance.
		/// </summary>
		/// <value>
		/// The near plane distance.
		/// </value>
		public static float NearPlaneDistance
		{
			set
			{
				_instance._nearPlaneDistance = value;
				_instance.UpdateProjection();
			}
			get { return _instance._nearPlaneDistance; }
		}
		/// <summary>
		/// Gets or sets the far plane distance.
		/// </summary>
		/// <value>
		/// The far plane distance.
		/// </value>
		public static float FarPlaneDistance
		{
			set
			{
				_instance._farPlaneDistance = value;
				_instance.UpdateProjection();
			}
			get { return _instance._farPlaneDistance; }
		}
		/// <summary>
		/// Gets the standard distance. When setting distance between the camera and a sprite to a standard distance,
		/// the sprite`s size seen on the screen is its pixel size.
		/// </summary>
		public static float StandardDistance
		{
			get { return (float)(_graphicDevice.Viewport.Height * 0.5 / Math.Tan(_instance._fieldOfView * 0.5)); }
		}
		/// <summary>
		/// Gets the controller of the update queue.
		/// </summary>
		public static UpdateController Controller
		{
			get { return _instance._updateController; }
		}
		/// <summary>
		/// Gets the draw root. Things to draw under scene should be attached to it.
		/// </summary>
		public static Drawable Root
		{
			get { return _instance._root; }
		}
		/// <summary>
		/// Gets the default camera.
		/// </summary>
		public static ICamera DefaultCamera
		{
			private set;
			get;
		}
		/// <summary>
		/// Gets the default effect.
		/// </summary>
		public static SpriteEffect DefaultSpriteEffect
		{
			private set;
			get;
		}
		/// <summary>
		/// Gets the paint effect.
		/// </summary>
		public static PaintEffect PaintEffect
		{
			private set;
			get;
		}
		/// <summary>
		/// Gets the current GraphicsDevice.
		/// </summary>
		public static new GraphicsDevice GraphicsDevice
		{
			get { return _graphicDevice; }
		}
		/// <summary>
		/// Gets the graphics device manager.
		/// </summary>
		public static GraphicsDeviceManager GraphicsDeviceManager
		{
			get { return _instance._graphics; }
		}
		/// <summary>
		/// Gets the sprite batch.
		/// </summary>
		public static SpriteBatch SpriteBatch
		{
			get { return _instance._spriteBatch; }
		}
		/// <summary>
		/// Gets the game instance.
		/// </summary>
		public static oGame Instance
		{
			get { return _instance; }
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="oGame"/> class.
		/// There should exist only one oGame instance.
		/// </summary>
		public oGame()
		{
			Debug.Assert(_instance == null, "One more oGame instance appeared!");
			Content.RootDirectory = "Content";
			_graphics = new GraphicsDeviceManager(this);
			_instance = this;
		}
		/// <summary>
		/// Called after the Game and GraphicsDevice are created, but before LoadContent.
		/// </summary>
		protected override void Initialize()
		{
			base.Initialize();
			_interval = (float)base.TargetElapsedTime.TotalMilliseconds;

			_graphicDevice = base.GraphicsDevice;
			_graphicDevice.RasterizerState = RasterizerState.CullNone;
			_graphicDevice.SamplerStates[0] = SamplerState.LinearClamp;
			_graphicDevice.BlendState = BlendState.AlphaBlend;
			_graphicDevice.DepthStencilState = DepthStencilState.Default;

			_spriteBatch = new SpriteBatch(_graphicDevice);

			oGame.DefaultCamera = new DefaultCamera();
			oCameraManager.Add(oGame.DefaultCamera);

			oGame.DefaultSpriteEffect = new SpriteEffect(_graphicDevice);
			oEffectManager.Add(oGame.DefaultSpriteEffect);

			oGame.PaintEffect = new PaintEffect(_graphicDevice);
			oEffectManager.Add(oGame.PaintEffect);

			_updateController = new UpdateController();
			_root = new DrawRoot();

			_screenVertices = new VertexPositionTexture[4]
            {
                new VertexPositionTexture(new Vector3(-1.0f, 1.0f, 0.0f), new Vector2(0.0f, 0.0f)),
                new VertexPositionTexture(new Vector3(1.0f, 1.0f, 0.0f), new Vector2(1.0f, 0.0f)),
                new VertexPositionTexture(new Vector3(-1.0f, -1.0f, 0.0f), new Vector2(0.0f, 1.0f)),
                new VertexPositionTexture(new Vector3(1.0f, -1.0f, 0.0f), new Vector2(1.0f, 1.0f))
            };

			this.UpdateProjection();
			this.UpdateRenderTargets();
		}
		/// <summary>
		/// Reference page contains links to related conceptual articles.
		/// </summary>
		/// <param name="gameTime">Time passed since the last call to Update.</param>
		protected override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
			oInput.Update();
			oCameraManager.Update();
			oEventManager.Update();
			oSceneManager.Update();
			_updateController.Update();
		}
		/// <summary>
		/// Reference page contains code sample.
		/// </summary>
		/// <param name="gameTime">Time passed since the last call to Draw.</param>
		protected override void Draw(GameTime gameTime)
		{
			base.Draw(gameTime);
			Mirror.Fill();
			if (oGraphic.IsPostProcessed)
			{
				List<IPostEffect> postEffects = oEffectManager.PostProcessEffects;
				int index = 0;
				_graphicDevice.SetRenderTarget(_targets[index]);
				oSceneManager.Draw();
				int last = postEffects.Count - 1;
				for (int i = 0; i < last; i++)
				{
					int prevIndex = index;
					index = (i + 1) % 2;
					_graphicDevice.SetRenderTarget(_targets[index]);
					postEffects[i].Texture = _targets[prevIndex];
					postEffects[i].Apply();
					_graphicDevice.DrawUserPrimitives<VertexPositionTexture>(PrimitiveType.TriangleStrip, _screenVertices, 0, 2);
				}
				_graphicDevice.SetRenderTarget(null);
				postEffects[last].Texture = _targets[index];
				postEffects[last].Apply();
				_graphicDevice.DrawUserPrimitives<VertexPositionTexture>(PrimitiveType.TriangleStrip, _screenVertices, 0, 2);
			}
			else
			{
				oSceneManager.Draw();
			}
			_root.Draw();
		}
		/// <summary>
		/// Called when graphics resources need to be unloaded. Override this method to unload any game-specific graphics resources.
		/// </summary>
		protected override void UnloadContent()
		{
			base.UnloadContent();
			oContent.Unload();
		}
		/// <summary>
		/// Updates the projection.
		/// </summary>
		public void UpdateProjection()
		{
			Matrix proj = Matrix.CreatePerspectiveFieldOfView(
							_fieldOfView,
							GraphicsDevice.Viewport.AspectRatio,
							_nearPlaneDistance,
							_farPlaneDistance);
			oEffectManager.SetProjection(ref proj);
		}
		public void UpdateRenderTargets()
		{
			PresentationParameters pp = _graphicDevice.PresentationParameters;
			if (_targets != null)
			{
				if (_targets[0].Width == pp.BackBufferWidth
					&& _targets[0].Height == pp.BackBufferHeight)
				{
					return;
				}
				for (int i = 0; i < 2; i++)
				{
					_targets[i].Dispose();
					_targets[i] = null;
				}
				_targets = null;
			}
			_targets = new RenderTarget2D[2];
			for (int i = 0; i < 2; i++)
			{
				_targets[i] = new RenderTarget2D
				(
					_graphicDevice,
					pp.BackBufferWidth,
					pp.BackBufferHeight,
					false,
					pp.BackBufferFormat,
					pp.DepthStencilFormat
				);
			}
		}
	}
}
