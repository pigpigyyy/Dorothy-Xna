using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Dorothy.Effects
{
	public interface IDrawableEffect : IEffectMatrices
	{
		string Name
		{
			set;
			get;
		}
		float Alpha
		{
			set;
			get;
		}
		void Apply();
	}
	public interface ISpriteEffect : IDrawableEffect
	{
		Vector3 Color
		{
			set;
			get;
		}
		Texture2D Texture
		{
			set;
			get;
		}
	}
	public interface IPostEffect
	{
		string Name
		{
			set;
			get;
		}
		Texture2D Texture
		{
			set;
			get;
		}
		void Apply();
	}
}
