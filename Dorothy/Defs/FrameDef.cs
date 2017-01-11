using System;
using Dorothy.Animations;
using Dorothy.Core;
using Dorothy.Data;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Dorothy.Defs
{
	/// <summary>
	/// Frame animation`s data item.
	/// </summary>
	[Serializable]
	public struct FrameData
	{
		/// <summary>
		/// Sprite`s draw rectangle.
		/// </summary>
		public Rectangle DrawRectangle;
		/// <summary>
		/// How many frames the draw rectangle should be keep.
		/// </summary>
		public float KeepCount;
	}
	/// <summary>
	/// Frame animation`s definition.
	/// </summary>
	[Serializable]
	public class FrameDef : AnimationDef
	{
		/// <summary>
		/// Gets the frame count.
		/// </summary>
		public int FrameCount
		{
			private set;
			get;
		}
		/// <summary>
		/// Target`s name.
		/// </summary>
		public string TargetName = string.Empty;
		/// <summary>
		/// Animation`s texture name.
		/// </summary>
		public string TexName = string.Empty;
		/// <summary>
		/// Gets the frame data.
		/// </summary>
		public FrameData[] Data
		{
			private set;
			get;
		}
		/// <summary>
		/// Gets the <see cref="Dorothy.Defs.FrameData"/> with the subscript.
		/// </summary>
		public FrameData this[int subScript]
		{
			get { return this.Data[subScript]; }
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="FrameDef"/> class.
		/// </summary>
		/// <param name="texName">Name of the texture.</param>
		/// <param name="frameCount">The draw rectangles count.</param>
		public FrameDef(string texName, uint frameCount)
		{
			this.TexName = texName;
			this.Data = new FrameData[frameCount];
			this.FrameCount = (int)frameCount;
		}
		/// <summary>
		/// Sets a frame.
		/// </summary>
		/// <param name="subScript">The sub script.</param>
		/// <param name="rect">The rect.</param>
		/// <param name="keepTime">The keep time.</param>
		public void SetFrame(int subScript, Rectangle rect, uint keepTime)
		{
			this.Data[subScript].DrawRectangle = rect;
			this.Data[subScript].KeepCount = (float)keepTime / oGame.TargetFrameInterval;
		}
		/// <summary>
		/// Convert the definition to a new animation instance.
		/// </summary>
		/// <param name="targets">The resource contains its target.</param>
		/// <param name="textures">The resource contains textures.</param>
		/// <returns>
		/// A new animation instance.
		/// </returns>
		public override IAnimation ToAnimation(
			Resource<object> targets,
			Resource<Texture2D> textures)
		{
			Frame frame = new Frame(this, textures[this.TexName]);
			frame.Target = (Sprite)targets[TargetName];
			frame.Loop = this.Loop;
			frame.Reverse = this.Reverse;
			frame.Speed = this.Speed;
			return frame;
		}
	}
}
