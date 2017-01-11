using System;
using Dorothy.Core;
using Dorothy.Data;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Dorothy.Defs
{
	/// <summary>
	/// Sprite`s definition.
	/// </summary>
	[Serializable]
	public class SpriteDef
	{
		/// <summary>
		/// Sprite`s name.
		/// </summary>
		public string Name = string.Empty;
		/// <summary>
		/// Sprite`s texture.
		/// </summary>
		public string TexName = string.Empty;
		/// <summary>
		/// Its alpha.
		/// </summary>
		public float Alpha = 1.0f;
		/// <summary>
		/// 
		/// </summary>
		public float ScaleX = 1.0f;
		/// <summary>
		/// 
		/// </summary>
		public float ScaleY = 1.0f;
		/// <summary>
		/// 
		/// </summary>
		public float RotateX;
		/// <summary>
		/// 
		/// </summary>
		public float RotateY;
		/// <summary>
		/// 
		/// </summary>
		public float RotateZ;
		/// <summary>
		/// 
		/// </summary>
		public float OffsetX;
		/// <summary>
		/// 
		/// </summary>
		public float OffsetY;
		/// <summary>
		/// 
		/// </summary>
		public float OffsetZ;
		/// <summary>
		/// 
		/// </summary>
		public float R = 1.0f;
		/// <summary>
		/// 
		/// </summary>
		public float G = 1.0f;
		/// <summary>
		/// 
		/// </summary>
		public float B = 1.0f;
		/// <summary>
		/// 
		/// </summary>
		public Rectangle DrawRectangle;
		public Vector2 TransformOrigin = new Vector2(0.5f, 0.5f);
		public SpriteDef[] Children;
		public bool Is3D;
		public virtual Sprite ToSprite(Resource<Texture2D> resource)
		{
			Texture2D tex = resource[this.TexName];
			Sprite sprite = new Sprite(tex);
			sprite.Name = this.Name;
			sprite.DrawRectangle = this.DrawRectangle;
			sprite.Alpha = this.Alpha;
			sprite.ScaleX = this.ScaleX;
			sprite.ScaleY = this.ScaleY;
			sprite.RotateX = this.RotateX;
			sprite.RotateY = this.RotateY;
			sprite.RotateZ = this.RotateZ;
			sprite.OffsetX = this.OffsetX;
			sprite.OffsetY = this.OffsetY;
			sprite.OffsetZ = this.OffsetZ;
			sprite.R = this.R;
			sprite.G = this.G;
			sprite.B = this.B;
			sprite.TransformOrigin = this.TransformOrigin;
			sprite.Is3D = this.Is3D;
			if (this.Children != null)
			{
				foreach (var sd in this.Children)
				{
					Sprite s = sd.ToSprite(resource);
					sprite.Add(s);
				}
			}
			return sprite;
		}
	}

	[Serializable]
	public class RepeatSpriteDef : SpriteDef
	{
		public float RepeatX = 1.0f;
		public float RepeatY = 1.0f;
		public override Sprite ToSprite(Resource<Texture2D> resource)
		{
			Texture2D tex = resource[this.TexName];
			RepeatSprite rSprite = new RepeatSprite(tex);
			rSprite.Name = this.Name;
			rSprite.RepeatX = this.RepeatX;
			rSprite.RepeatY = this.RepeatY;
			rSprite.Alpha = this.Alpha;
			rSprite.ScaleX = this.ScaleX;
			rSprite.ScaleY = this.ScaleY;
			rSprite.RotateX = this.RotateX;
			rSprite.RotateY = this.RotateY;
			rSprite.RotateZ = this.RotateZ;
			rSprite.OffsetX = this.OffsetX;
			rSprite.OffsetY = this.OffsetY;
			rSprite.OffsetZ = this.OffsetZ;
			rSprite.R = this.R;
			rSprite.G = this.G;
			rSprite.B = this.B;
			rSprite.TransformOrigin = this.TransformOrigin;
			rSprite.Is3D = this.Is3D;
			if (this.Children != null)
			{
				foreach (var sd in this.Children)
				{
					Sprite s = sd.ToSprite(resource);
					rSprite.Add(s);
				}
			}
			return rSprite;
		}
	}
}
