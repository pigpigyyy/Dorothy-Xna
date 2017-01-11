using System;
using System.Collections.Generic;
using System.IO;
using Dorothy.Animations;
using Dorothy.Core;
using Dorothy.Data;
using Dorothy.Game;
using ICSharpCode.SharpZipLib.Zip;
using Microsoft.Xna.Framework.Graphics;

namespace Dorothy.Defs
{
	/// <summary>
	/// Model 2D`s definition.
	/// </summary>
	[Serializable]
	public class Model2DDef
	{
		/// <summary>
		/// Model name.
		/// </summary>
		public string Name = string.Empty;
		/// <summary>
		/// Model alpha.
		/// </summary>
		public float Alpha = 1.0f;
		/// <summary>
		/// Its scale X.
		/// </summary>
		public float ScaleX = 1.0f;
		/// <summary>
		/// Its scale Y.
		/// </summary>
		public float ScaleY = 1.0f;
		/// <summary>
		/// Its rotation X.
		/// </summary>
		public float RotateX;
		/// <summary>
		/// Its rotation Y.
		/// </summary>
		public float RotateY;
		/// <summary>
		/// Its rotation Z.
		/// </summary>
		public float RotateZ;
		/// <summary>
		/// Model`s texture resource information.
		/// </summary>
		public TextureDef[] Textures;
		/// <summary>
		/// Model`s sprite definition.
		/// </summary>
		public SpriteDef SpriteDef;
		/// <summary>
		/// Model`s animation definition.
		/// </summary>
		public AnimationDef[] AnimationDefs;
		/// <summary>
		/// Convert this definition to a new model2D instance.
		/// </summary>
		/// <param name="resource">The resource contains model`s textures.</param>
		/// <returns>A new model2D instance.</returns>
		public Model2D ToModel2D(Resource<Texture2D> resource)
		{
			Sprite sprite = this.SpriteDef.ToSprite(resource);
			Resource<object> res = new Resource<object>();
			Drawable.Traverse
			(
				sprite,
				drawable =>
				{
					res.Add(drawable.Name, (Sprite)drawable);
				}
			);
			Queue<object> que = new Queue<object>();
			int subscript = 0;
			SpriteState state = new SpriteState(res.Count);
			que.Enqueue(this.SpriteDef);
			while (que.Count != 0)
			{
				SpriteDef sd = (SpriteDef)que.Dequeue();
				state.Set(subscript, (Sprite)res[sd.Name], sd);
				subscript++;
				if (sd.Children != null)
				{
					foreach (var s in sd.Children)
					{
						que.Enqueue(s);
					}
				}
			}
			Model2D model = new Model2D(sprite, state);
			model.Alpha = this.Alpha;
			model.ScaleX = this.ScaleX;
			model.ScaleY = this.ScaleY;
			model.RotateX = this.RotateX;
			model.RotateY = this.RotateY;
			model.RotateZ = this.RotateZ;
			if (this.AnimationDefs != null)
			{
				foreach (var ad in this.AnimationDefs)
				{
					IAnimation animation = ad.ToAnimation(res, resource);
					model.Animations.Add(ad.Name, animation);
				}
			}
			return model;
		}
	}
	/// <summary>
	/// Model2D`s data.
	/// </summary>
	public class Model2DData : IDisposable
	{
		/// <summary>
		/// Model2D`s definition.
		/// </summary>
		public Model2DDef Model2DDef;
		/// <summary>
		/// Model2D`s texture resource.
		/// </summary>
		public Resource<Texture2D> Resource;
		/// <summary>
		/// Gets a value indicating whether this instance is disposed.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this instance is disposed; otherwise, <c>false</c>.
		/// </value>
		public bool IsDisposed
		{
			get { return this.Resource == null; }
		}
		/// <summary>
		/// Load a model data item from file.
		/// The Loaded data is isolated from content.
		/// </summary>
		/// <param name="filename">The filename.</param>
		/// <returns>A model data item</returns>
		public static Model2DData FromFile(string filename)
		{
			Model2DData modData = new Model2DData();
			using (ZipFile modelFile = new ZipFile(filename))
			{
				int i = 0;
				while (!modelFile[i].Name.EndsWith(".def"))
				{
					i++;
					if (i >= modelFile.Count)
					{
						return null;
					}
				}
				Model2DDef modDef;
				using (Stream stream = modelFile.GetInputStream(i))
				{
					modDef = oFormatter.Binary.Deserialize(stream) as Model2DDef;
				}
				Resource<Texture2D> res = new Resource<Texture2D>();
				foreach (var texDef in modDef.Textures)
				{
					int o = modelFile.FindEntry(texDef.FileName, true);
					using (Stream stream = modelFile.GetInputStream(o))
					{
						Texture2D tex = Texture2D.FromStream(oGame.GraphicsDevice, stream);
						res.Add(texDef.Name, tex);
					}
				}
				modData.Model2DDef = modDef;
				modData.Resource = res;
			}
			return modData;
		}
		/// <summary>
		/// Convert this data to a new model2D instance.
		/// </summary>
		/// <returns>A new model2D instance.</returns>
		public Model2D ToModel2D()
		{
			return this.Model2DDef.ToModel2D(this.Resource);
		}
		/// <summary>
		/// Releases this item.
		/// </summary>
		public void Dispose()
		{
			if (this.Resource != null)
			{
				this.Resource.Dispose();
				this.Resource = null;
			}
		}
	}
}
