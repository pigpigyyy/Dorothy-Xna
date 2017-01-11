using System;

namespace Dorothy.Defs
{
	/// <summary>
	/// A level`s definition.
	/// </summary>
	[Serializable]
	public class LevelDef
	{
		/// <summary>
		/// The level`s name.
		/// </summary>
		public string Name = string.Empty;
		/// <summary>
		/// This level`s texture resource information.
		/// </summary>
		public TextureDef[] Textures;
		/// <summary>
		/// This level`s model2d resource information.
		/// </summary>
		public string[] Model2Ds;
		/// <summary>
		/// This level`s sprite definitions.
		/// </summary>
		public SpriteDef[] SpriteDefs;
		/// <summary>
		/// This level`s unit definitions.
		/// </summary>
		public UnitDef[] UnitDefs;
	}
}
