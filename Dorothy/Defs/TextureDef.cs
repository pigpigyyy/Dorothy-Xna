using System;
using Dorothy.Data;
using Microsoft.Xna.Framework.Graphics;

namespace Dorothy.Defs
{
	[Serializable]
	public class TextureDef
	{
		public string Name = string.Empty;
		public string FileName = string.Empty;

		public TextureDef(string name, string filename)
		{
			Name = name;
			FileName = filename;
		}
		public Texture2D ToTexture()
		{
			return oContent.GetTexture(this.Name);
		}
	}
}
