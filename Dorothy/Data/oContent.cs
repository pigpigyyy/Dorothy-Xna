using System.IO;
using Dorothy.Core;
using Dorothy.Defs;
using Dorothy.Game;
using ICSharpCode.SharpZipLib.Zip;
using Microsoft.Xna.Framework.Graphics;

namespace Dorothy.Data
{
	/// <summary>
	/// Loads and gets data from it.
	/// </summary>
	public static class oContent
	{
		#region Field
		private static int _texToLoad;
		private static int _currentTex;
		private static int _modelToLoad;
		private static int _currentModel;
		private static string _levelFilename;
		private static ZipFile _zipFile;
		private static Resource<Texture2D> _textures = new Resource<Texture2D>();
		private static Resource<Model2DData> _model2DData = new Resource<Model2DData>();
		private static Resource<SpriteDef> _sprites = new Resource<SpriteDef>();
		private static Resource<UnitDef> _units = new Resource<UnitDef>();
		#endregion

		/// <summary>
		/// Gets the current level data.
		/// </summary>
		public static LevelDef LevelDef
		{
			private set;
			get;
		}
		/// <summary>
		/// Gets a value indicating whether current level data is loaded up.
		/// </summary>
		/// <value>
		///   <c>true</c> if current level data is loaded; otherwise, <c>false</c>.
		/// </value>
		public static bool IsLoaded
		{
			private set;
			get;
		}

		/// <summary>
		/// Loads a level.
		/// If content is loaded before. The previous loaded data will be all cleared before loading again.
		/// </summary>
		/// <param name="filename">The filename.</param>
		public static void LoadLevel(string filename)
		{
			if (oContent.IsLoaded)
			{
				oContent.Unload();
			}
			_levelFilename = filename;
			using (ZipFile zipFile = new ZipFile(filename))
			{
				int index = 0;
				for (; index < zipFile.Count; index++)
				{
					if (zipFile[index].Name.EndsWith(".def"))
					{
						break;
					}
				}
				using (Stream stream = zipFile.GetInputStream(index))
				{
					oContent.LevelDef = oFormatter.Binary.Deserialize(stream) as LevelDef;
				}
				if (oContent.LevelDef.Textures != null)
				{
					foreach (var texDef in oContent.LevelDef.Textures)
					{
						index = zipFile.FindEntry(texDef.FileName, true);
						using (Stream stream = zipFile.GetInputStream(index))
						{
							Texture2D tex = Texture2D.FromStream(oGame.GraphicsDevice, stream);
							_textures.Add(texDef.Name, tex);
						}
					}
				}
				if (oContent.LevelDef.Model2Ds != null)
				{
					foreach (var modname in oContent.LevelDef.Model2Ds)
					{
						index = zipFile.FindEntry(modname, true);
						using (Stream modStream = zipFile.GetInputStream(index))
						{
							using (ZipFile modelFile = new ZipFile(modStream))
							{
								int i = 0;
								for (; i < modelFile.Count; i++)
								{
									if (modelFile[i].Name.EndsWith(".def"))
									{
										break;
									}
								}
								Model2DDef modDef = null;
								using (Stream defStream = modelFile.GetInputStream(i))
								{
									modDef = oFormatter.Binary.Deserialize(defStream) as Model2DDef;
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
								Model2DData modData = new Model2DData();
								modData.Model2DDef = modDef;
								modData.Resource = res;
								_model2DData.Add(modDef.Name, modData);
							}
						}
					}
				}
				if (oContent.LevelDef.SpriteDefs != null)
				{
					foreach (var sd in oContent.LevelDef.SpriteDefs)
					{
						_sprites.Add(sd.Name, sd);
					}
				}
				if (oContent.LevelDef.UnitDefs != null)
				{
					foreach (var ud in oContent.LevelDef.UnitDefs)
					{
						_units.Add(ud.Name, ud);
					}
				}
			}
			_currentTex = _currentModel = _texToLoad = _modelToLoad = 0;
			oContent.IsLoaded = true;
		}
		/// <summary>
		/// Loads a Texture2D or Model2D from specified file.
		/// </summary>
		/// <typeparam name="T">Texture2D or Model2D</typeparam>
		/// <param name="filename">The filename.</param>
		/// <returns>The loaded item.</returns>
		public static T Load<T>(string filename)
		{
			if (typeof(T).Equals(typeof(Texture2D)))
			{
				string name = Path.GetFileNameWithoutExtension(filename);
				object tex = _textures[name];
				if (tex != null)
				{
					return (T)tex;
				}
				using (FileStream stream = new FileStream(filename, FileMode.Open))
				{
					tex = Texture2D.FromStream(oGame.GraphicsDevice, stream);
					_textures.Add(name, (Texture2D)tex);
				}
				return (T)tex;
			}
			else if (typeof(T).Equals(typeof(Model2D)))
			{
				Model2DData modData = null;
				using (ZipFile modelFile = new ZipFile(filename))
				{
					int i = 0;
					string name = string.Empty;
					for (; i < modelFile.Count; i++)
					{
						if (modelFile[i].Name.EndsWith(".def"))
						{
							name = Path.GetFileNameWithoutExtension(modelFile[i].Name);
							break;
						}
					}
					modData = _model2DData[name];
					if (modData == null)
					{
						Model2DDef modDef = null;
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
						modData = new Model2DData();
						modData.Model2DDef = modDef;
						modData.Resource = res;
						_model2DData.Add(modDef.Name, modData);
					}
				}
				object mod = modData.ToModel2D();
				return (T)mod;
			}
			return default(T);
		}
		/// <summary>
		/// Unloads all data in content.
		/// </summary>
		public static void Unload()
		{
			if (_zipFile != null)
			{
				_zipFile.Close();
				_zipFile = null;
			}
			oContent.IsLoaded = false;
			_textures.Clear();
			_model2DData.Clear();
			_sprites.Clear();
			_units.Clear();
		}
		/// <summary>
		/// Unloads a Texture2D or Model2D.
		/// </summary>
		/// <typeparam name="T">Texture2D or Model2D</typeparam>
		/// <param name="name">The item`s name.</param>
		/// <returns></returns>
		public static bool Unload<T>(string name)
		{
			if (typeof(T).Equals(typeof(Texture2D)))
			{
				return _textures.Remove(name);
			}
			else if (typeof(T).Equals(typeof(Model2D)))
			{
				return _model2DData.Remove(name);
			}
			return false;
		}
		/// <summary>
		/// Begins loading a level`s data.
		/// </summary>
		/// <param name="filename">The filename.</param>
		/// <example>
		/// oContent.BeginLoadLevel("level.zip");
		/// bool isLoading = true;
		/// while (isLoading)
		/// {
		///     isLoading = oContent.LoadNextItem();
		/// }
		/// </example>
		public static void BeginLoadLevel(string filename)
		{
			if (oContent.IsLoaded)
			{
				oContent.Unload();
			}
			_levelFilename = filename;
			_zipFile = new ZipFile(filename);
			int index = 0;
			for (; index < _zipFile.Count; index++)
			{
				if (_zipFile[index].Name.EndsWith(".def"))
				{
					break;
				}
			}
			using (Stream stream = _zipFile.GetInputStream(index))
			{
				oContent.LevelDef = oFormatter.Binary.Deserialize(stream) as LevelDef;
			}
			if (oContent.LevelDef.SpriteDefs != null)
			{
				foreach (var sd in oContent.LevelDef.SpriteDefs)
				{
					_sprites.Add(sd.Name, sd);
				}
			}
			if (oContent.LevelDef.UnitDefs != null)
			{
				foreach (var ud in oContent.LevelDef.UnitDefs)
				{
					_units.Add(ud.Name, ud);
				}
			}
			_currentTex = _currentModel = 0;
			_texToLoad = (oContent.LevelDef.Textures == null ? 0 : oContent.LevelDef.Textures.Length);
			_modelToLoad = (oContent.LevelDef.Model2Ds == null ? 0 : oContent.LevelDef.Model2Ds.Length);
		}
		/// <summary>
		/// Loads the next item.
		/// </summary>
		/// <returns>
		/// 	<c>true</c> if all data are loaded up; otherwise <c>false</c>.
		/// </returns>
		public static bool LoadNextItem()
		{
			if (_currentTex < _texToLoad)
			{
				int index = _zipFile.FindEntry(oContent.LevelDef.Textures[_currentTex].FileName, true);
				Texture2D tex = Texture2D.FromStream(oGame.GraphicsDevice, _zipFile.GetInputStream(index));
				_textures.Add(oContent.LevelDef.Textures[_currentTex].Name, tex);
				_currentTex++;
				return true;
			}
			if (_currentModel < _modelToLoad)
			{
				int index = _zipFile.FindEntry(oContent.LevelDef.Model2Ds[_currentModel], true);
				using (ZipFile modelFile = new ZipFile(_zipFile.GetInputStream(index)))
				{
					int i = 0;
					for (; i < modelFile.Count; i++)
					{
						if (modelFile[i].Name.EndsWith(".def"))
						{
							break;
						}
					}
					Model2DDef modDef = oFormatter.Binary.Deserialize(modelFile.GetInputStream(i)) as Model2DDef;
					Resource<Texture2D> res = new Resource<Texture2D>();
					foreach (var texDef in modDef.Textures)
					{
						int o = modelFile.FindEntry(texDef.FileName, true);
						Texture2D tex = Texture2D.FromStream(oGame.GraphicsDevice, modelFile.GetInputStream(o));
						res.Add(texDef.Name, tex);
					}
					Model2DData modData = new Model2DData();
					modData.Model2DDef = modDef;
					modData.Resource = res;
					_model2DData.Add(modDef.Name, modData);
				}
				_currentModel++;
				return true;
			}
			if (!oContent.IsLoaded)
			{
				oContent.IsLoaded = true;
				_zipFile.Close();
				_zipFile = null;
			}
			return false;
		}
		/// <summary>
		/// Gets a loaded texture item.
		/// </summary>
		/// <param name="name">The texture item`s name.</param>
		/// <returns>A texture item.</returns>
		public static Texture2D GetTexture(string name)
		{
			return _textures[name];
		}
		/// <summary>
		/// Gets a new model2D instance from loaded data.
		/// </summary>
		/// <param name="name">The model2D`s name.</param>
		/// <returns>A new model2D instance.</returns>
		public static Model2D GetModel2D(string name)
		{
			Model2DData data = _model2DData[name];
			if (data != null)
			{ return data.ToModel2D(); }
			return null;
		}
		/// <summary>
		/// Gets a new sprite instance from loaded data.
		/// </summary>
		/// <param name="name">The sprite`s name.</param>
		/// <returns>A new sprite instance.</returns>
		public static Sprite GetSprite(string name)
		{
			return _sprites[name].ToSprite(_textures);
		}
		/// <summary>
		/// Gets a new unit instance from loaded data.
		/// </summary>
		/// <param name="name">The unit`s name.</param>
		/// <param name="world">The world.</param>
		/// <param name="x">The x position in world.</param>
		/// <param name="y">The y position in world.</param>
		/// <param name="angle">The angle.</param>
		/// <returns>A new unit instance</returns>
		public static Unit GetUnit(string name, World world, float x, float y, float angle)
		{
			return _units[name].ToUnit(world, x, y, angle);
		}
	}
}
