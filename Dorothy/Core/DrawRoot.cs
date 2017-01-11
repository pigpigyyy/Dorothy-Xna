using System;
using Dorothy.Cameras;
using Dorothy.Helpers;
using Microsoft.Xna.Framework;

namespace Dorothy.Core
{
	/// <summary>
	/// A scene`s root node for drawable items.
	/// Any drawable items of the scene should be attached to it and get drew.
	/// </summary>
	public class DrawRoot : Drawable
	{
		/// <summary>
		/// Draws all items in the scene tree.
		/// Get them ready, sort them in the draw queue by draw orders then clear the queue up after drawing the queue.
		/// </summary>
		public override void Draw()
		{
			base.GetReady();
			oDrawQueue.Sort();
			oDrawQueue.Draw();
			oDrawQueue.Clear();
		}
		/// <summary>
		/// Draws the specified scene items from the scene tree.
		/// </summary>
		/// <param name="match">The matching delegate.</param>
		public void Draw(Predicate<Drawable> match)
		{
			base.GetReady();
			oDrawQueue.Sort();
			oDrawQueue.Draw(match);
			oDrawQueue.Clear();
		}
	}
}
