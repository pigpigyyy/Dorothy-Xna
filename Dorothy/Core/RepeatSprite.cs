using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Dorothy.Core
{
	/// <summary>
	/// Sprite with texture drew repeatedly.
	/// Its texture`s size must be 2^n.
	/// </summary>
	public class RepeatSprite : Sprite
	{
		#region Field
		private float _repeatX = 1.0f;
		private float _repeatY = 1.0f;
		#endregion

		/// <summary>
		/// Gets or sets the horizontal repeat time.
		/// </summary>
		/// <value>
		/// The repeat time.
		/// </value>
		public float RepeatX
		{
			set
			{
				_repeatX = value;
				_vertices[1].TextureCoordinate.X = value;
				_vertices[3].TextureCoordinate.X = value;
			}
			get { return _repeatX; }
		}
		/// <summary>
		/// Gets or sets the vertical repeat time.
		/// </summary>
		/// <value>
		/// The repeat time.
		/// </value>
		public float RepeatY
		{
			set
			{
				_repeatY = value;
				_vertices[2].TextureCoordinate.Y = value;
				_vertices[3].TextureCoordinate.Y = value;
			}
			get { return _repeatY; }
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="RepeatSprite"/> class.
		/// </summary>
		/// <param name="texture">The texture.</param>
		public RepeatSprite(Texture2D texture)
			: base(texture)
		{
			base.DrawRectangle = new Rectangle(0, 0, texture.Width, texture.Height);
		}
		/// <summary>
		/// Gets the draw rectangle.
		/// </summary>
		public new Rectangle DrawRectangle
		{
			get { return base.DrawRectangle; }
		}
		/// <summary>
		/// Draws this instance.
		/// [Called by framework]
		/// </summary>
		public override void Draw()
		{
			oGame.GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
			base.Draw();
			oGame.GraphicsDevice.SamplerStates[0] = SamplerState.LinearClamp;
		}
	}
}
