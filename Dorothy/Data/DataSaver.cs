using System;
using System.IO;
using Dorothy.Defs;
using ICSharpCode.SharpZipLib.Zip;

namespace Dorothy.Data
{
	/// <summary>
	/// Class used by SharpZipLib.
	/// </summary>
	class ZipSource : IStaticDataSource
	{
		private Stream _stream;
		/// <summary>
		/// Initializes a new instance of the <see cref="ZipSource"/> class.
		/// </summary>
		/// <param name="stream">The stream.</param>
		public ZipSource(Stream stream)
		{
			_stream = stream;
		}
		/// <summary>
		/// Get a source of data by creating a new stream.
		/// </summary>
		/// <returns>
		/// Returns a <see cref="Stream"/> to use for compression input.
		/// </returns>
		public Stream GetSource()
		{
			return _stream;
		}
	}

	/// <summary>
	/// Tool for saving data.
	/// </summary>
	public class DataSaver : IDisposable
	{
		#region Field
		private int _currentTex;
		private int _currentMod;
		private int _texToSave;
		private int _modToSave;
		private LevelDef _currentDef;
		private ZipFile _zipFile;
		private string _filename;
		#endregion

		/// <summary>
		/// Initializes a new instance of the <see cref="DataSaver"/> class.
		/// </summary>
		public DataSaver()
		{
			_filename = string.Empty;
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="DataSaver"/> class.
		/// </summary>
		/// <param name="filename">The filename.</param>
		public DataSaver(string filename)
		{
			this.Filename = filename;
		}
		/// <summary>
		/// Gets a value indicating whether data is saved.
		/// </summary>
		/// <value>
		///   <c>true</c> if data is saved; otherwise, <c>false</c>.
		/// </value>
		public bool IsSaved
		{
			private set;
			get;
		}
		/// <summary>
		/// Gets or sets the target filename.
		/// </summary>
		/// <value>
		/// The target filename.
		/// </value>
		public string Filename
		{
			set
			{
				if (_zipFile != null)
				{
					_zipFile.Close();
					_zipFile = null;
				}
				_filename = value;
				if (File.Exists(value))
				{
					_zipFile = new ZipFile(value);
				}
				else
				{
					_zipFile = ZipFile.Create(value);
				}
			}
			get { return _filename; }
		}
		/// <summary>
		/// Adds a file to current save archive.
		/// </summary>
		/// <param name="filename">The filename.</param>
		/// <returns>
		/// 	<c>true</c> if the file is added successfully; otherwise <c>false</c>.
		/// </returns>
		public bool AddFile(string filename)
		{
			int index = _zipFile.FindEntry(filename, true);
			if (index != -1)
			{
				return false;
			}
			_zipFile.Add(filename, CompressionMethod.Stored);
			return true;
		}
		/// <summary>
		/// Adds or replaces a file to current save archive.
		/// </summary>
		/// <param name="filename">The filename.</param>
		public void AddOrReplaceFile(string filename)
		{
			_zipFile.BeginUpdate();
			_zipFile.Add(filename, CompressionMethod.Stored);
			_zipFile.CommitUpdate();
		}
		/// <summary>
		/// Saves the specified level data up.
		/// </summary>
		/// <param name="levelDef">The level data.</param>
		public void Save(LevelDef levelDef)
		{
			using (MemoryStream memStream = new MemoryStream())
			{
				oFormatter.Binary.Serialize(memStream, levelDef);
				ZipSource zipSource = new ZipSource(memStream);
				memStream.Seek(0, SeekOrigin.Begin);
				_zipFile.BeginUpdate();
				_zipFile.Add(zipSource, levelDef.Name + ".def");
				if (levelDef.Textures != null)
				{
					foreach (var texDef in levelDef.Textures)
					{
						if (File.Exists(texDef.FileName))
						{
							_zipFile.Add(texDef.FileName, CompressionMethod.Stored);
						}
					}
				}
				if (levelDef.Model2Ds != null)
				{
					foreach (var modfile in levelDef.Model2Ds)
					{
						if (File.Exists(modfile))
						{
							_zipFile.Add(modfile, CompressionMethod.Stored);
						}
					}
				}
				_zipFile.CommitUpdate();
			}
			this.IsSaved = true;
		}
		/// <summary>
		/// Begins saving the level data.
		/// </summary>
		/// <param name="levelDef">The level data.</param>
		/// <example>
		/// DataSaver saver = DataSaver("targetfile.zip");
		/// saver.BeginSave(levelDef);
		/// bool isSaving = true;
		/// while (isSaving)
		/// {
		///     isSaving = saver.SaveNextItem();
		/// }
		/// </example>
		public void BeginSave(LevelDef levelDef)
		{
			_currentDef = levelDef;
			_currentTex = _currentMod = 0;
			if (levelDef.Textures != null)
			{
				_texToSave = levelDef.Textures.Length;
			}
			if (levelDef.Model2Ds != null)
			{
				_modToSave = levelDef.Model2Ds.Length;
			}
			using (MemoryStream memStream = new MemoryStream())
			{
				oFormatter.Binary.Serialize(memStream, levelDef);
				ZipSource zipSource = new ZipSource(memStream);
				memStream.Seek(0, SeekOrigin.Begin);
				_zipFile.BeginUpdate();
				_zipFile.Add(zipSource, levelDef.Name + ".def");
				_zipFile.CommitUpdate();
			}
			this.IsSaved = false;
		}
		/// <summary>
		/// Saves the next item.
		/// </summary>
		/// <returns>
		/// 	<c>true</c> if all data are saved up; otherwise <c>false</c>.
		/// </returns>
		public bool SaveNextItem()
		{
			if (_currentTex < _texToSave)
			{
				if (File.Exists(_currentDef.Textures[_currentTex].FileName))
				{
					_zipFile.BeginUpdate();
					_zipFile.Add(_currentDef.Textures[_currentTex].FileName, CompressionMethod.Stored);
					_zipFile.CommitUpdate();
				}
				_currentTex++;
				return true;
			}
			if (_currentMod < _modToSave)
			{
				if (File.Exists(_currentDef.Model2Ds[_currentTex]))
				{
					_zipFile.BeginUpdate();
					_zipFile.Add(_currentDef.Model2Ds[_currentTex], CompressionMethod.Stored);
					_zipFile.CommitUpdate();
				}
				_currentMod++;
				return true;
			}
			if (!this.IsSaved)
			{
				this.IsSaved = true;
			}
			return false;
		}
		/// <summary>
		/// Saves the specified 2d model data.
		/// </summary>
		/// <param name="modDef">The 2d modle data.</param>
		public void Save(Model2DDef modDef)
		{
			using (MemoryStream memStream = new MemoryStream())
			{
				oFormatter.Binary.Serialize(memStream, modDef);
				ZipSource zipSource = new ZipSource(memStream);
				memStream.Seek(0, SeekOrigin.Begin);
				_zipFile.BeginUpdate();
				_zipFile.Add(zipSource, modDef.Name + ".def");
				if (modDef.Textures != null)
				{
					foreach (var texDef in modDef.Textures)
					{
						if (File.Exists(texDef.FileName))
						{
							_zipFile.Add(texDef.FileName, CompressionMethod.Stored);
						}
					}
				}
				_zipFile.CommitUpdate();
			}
			this.IsSaved = true;
		}
		/// <summary>
		/// Closes the save archive file.
		/// </summary>
		public void Dispose()
		{
			if (_zipFile != null)
			{
				_zipFile.Close();
				_zipFile = null;
			}
		}
	}
}
