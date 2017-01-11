using System;
using System.Collections.Generic;
using Dorothy.Effects;

namespace Dorothy.Core
{
	/// <summary>
	/// Any drawable item to be drew should get itself up and then be added to a draw queue.
	/// Draw queue handles the order when items are drew and adjust graphic environment for different queues to be drew. 
	/// </summary>
	public static class oDrawQueue
	{
		#region Field
		private static List<Drawable> _firstQueue = new List<Drawable>();
		private static List<Drawable> _secondQueue = new List<Drawable>();
		#endregion

		/// <summary>
		/// Puts a drawable item in the First Queue.
		/// Items in the First Queue will never be sorted and will be drew before drawing the Second Queue items.
		/// Items in the First Queue will write depth value in the depth buffer.
		/// When in dual pass mode, items put in the First Queue shall also be put in the Second Queue by this method.
		/// </summary>
		/// <param name="draw">The drawable item.</param>
		public static void EnFirst(Drawable draw)
		{
			_firstQueue.Add(draw);
		}
		/// <summary>
		/// Puts a drawable item in the Second Queue.
		/// Items in the Second Queue will always be sorted by draw order and drew after drawing the First Queue items.
		/// Items with higher draw order value will be sort to the front.
		/// Items in the Second Queue won`t write depth value in the depth buffer.
		/// </summary>
		/// <param name="draw">The drawable item.</param>
		public static void EnSecond(Drawable draw)
		{
			_secondQueue.Add(draw);
		}
		/// <summary>
		/// Removes a drawable item from draw queue.
		/// </summary>
		/// <param name="draw">The drawable item.</param>
		public static void Remove(Drawable draw)
		{
			if (!_firstQueue.Remove(draw))
			{ _secondQueue.Remove(draw); }
		}
		/// <summary>
		/// Sorts queues.
		/// </summary>
		public static void Sort()
		{
			_secondQueue.Sort(DrawOrderComparer.Comparer);
		}
		/// <summary>
		/// Clears queues.
		/// </summary>
		public static void Clear()
		{
			_firstQueue.Clear();
			_secondQueue.Clear();
		}
		/// <summary>
		/// Draws queues.
		/// </summary>
		public static void Draw()
		{
			switch (oGraphic.SortMode)
			{
				case SortMode.AllSort:
					for (int i = 0; i < _secondQueue.Count; i++)
					{ _secondQueue[i].Draw(); }
					break;
				case SortMode.AlphaSort:
					for (int i = 0; i < _firstQueue.Count; i++)
					{ _firstQueue[i].Draw(); }
					for (int i = 0; i < _secondQueue.Count; i++)
					{ _secondQueue[i].Draw(); }
					break;
				case SortMode.ZSort:
					for (int i = 0; i < _firstQueue.Count; i++)
					{ _firstQueue[i].Draw(); }
					break;
			}
		}
		/// <summary>
		/// Draws the matched items in queues.
		/// </summary>
		/// <param name="match">The matching delegate.</param>
		public static void Draw(Predicate<Drawable> match)
		{
			switch (oGraphic.SortMode)
			{
				case SortMode.AllSort:
					for (int i = 0; i < _secondQueue.Count; i++)
					{
						if (match(_secondQueue[i]))
						{ _secondQueue[i].Draw(); }
					}
					break;
				case SortMode.AlphaSort:
					for (int i = 0; i < _firstQueue.Count; i++)
					{
						if (match(_secondQueue[i]))
						{ _firstQueue[i].Draw(); }
					}
					for (int i = 0; i < _secondQueue.Count; i++)
					{
						if (match(_secondQueue[i]))
						{ _secondQueue[i].Draw(); }
					}
					break;
				case SortMode.ZSort:
					for (int i = 0; i < _firstQueue.Count; i++)
					{
						if (match(_secondQueue[i]))
						{ _firstQueue[i].Draw(); }
					}
					break;
			}
		}
	}

	/// <summary>
	/// Draw order comparer.
	/// </summary>
	class DrawOrderComparer : IComparer<Drawable>
	{
		public static readonly DrawOrderComparer Comparer = new DrawOrderComparer();
		public int Compare(Drawable a, Drawable b)
		{
			return (a.DrawOrder == b.DrawOrder ? 0 : (a.DrawOrder < b.DrawOrder ? 1 : -1));
		}
	}
}
