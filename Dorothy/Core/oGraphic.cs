using System.Collections.Generic;
using Dorothy.Effects;
using Microsoft.Xna.Framework.Graphics;

namespace Dorothy.Core
{
	public enum SortMode
	{
		AllSort,
		AlphaSort,
		ZSort,
	};
	/// <summary>
	/// It handles graphic settings.
	/// </summary>
	public static class oGraphic
	{
		#region Field
		private static bool _bZWriteEnable = true;
		private static SortMode _sortMode = SortMode.AllSort;
		private static bool _isPostProcessed;
		#endregion

		/// <summary>
		/// Gets or sets a value indicating whether depth write is enabled.
		/// </summary>
		/// <value>
		///   <c>true</c> if depth write is enabled; otherwise, <c>false</c>.
		///   The default value is true.
		/// </value>
		public static bool ZWriteEnable
		{
			set
			{
				if (_bZWriteEnable != value)
				{
					_bZWriteEnable = value;
					if (value)
					{
						oGame.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
					}
					else
					{
						oGame.GraphicsDevice.DepthStencilState = DepthStencilState.DepthRead;
					}
				}
			}
			get { return _bZWriteEnable; }
		}
		public static SortMode SortMode
		{
			set { _sortMode = value; }
			get { return _sortMode; }
		}
		/// <summary>
		/// Gets or sets a value indicating whether the graphic is post processed.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if the graphic is post processed; otherwise, <c>false</c>.
		/// </value>
		public static bool IsPostProcessed
		{
			set
			{
				if (!_isPostProcessed && value)
				{
					oGame.Instance.UpdateRenderTargets();
				}
				_isPostProcessed = value;
			}
			get { return _isPostProcessed; }
		}
	}
}
