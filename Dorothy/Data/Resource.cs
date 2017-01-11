using System;
using System.Collections.Generic;

namespace Dorothy.Data
{
	/// <summary>
	/// A wrapped dictionary for storing and getting resource items.
	/// </summary>
	/// <typeparam name="T">Resource type.</typeparam>
	public class Resource<T> : IDisposable
	{
		#region Field
		private Dictionary<string, T> _objects = new Dictionary<string, T>();
		#endregion

		/// <summary>
		/// Gets the <see cref="T"/> with the specified name.
		/// </summary>
		public T this[string name]
		{
			get
			{
				T obj = default(T);
				_objects.TryGetValue(name, out obj);
				return obj;
			}
		}
		/// <summary>
		/// Gets the number of contained items.
		/// </summary>
		public int Count
		{
			get { return _objects.Count; }
		}
		/// <summary>
		/// Gets a value indicating whether this resource is disposed.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this resource is disposed; otherwise, <c>false</c>.
		/// </value>
		public bool IsDisposed
		{
			get { return (_objects == null); }
		}
		/// <summary>
		/// Gets the items.
		/// </summary>
		public Dictionary<string, T> Items
		{
			get { return _objects; }
		}
		/// <summary>
		/// Adds a resource item.
		/// </summary>
		/// <param name="name">The item`s name.</param>
		/// <param name="item">The item.</param>
		public void Add(string name, T item)
		{
			_objects.Add(name, item);
		}
		/// <summary>
		/// Removes a item with the specified name.
		/// </summary>
		/// <param name="name">The item`s name.</param>
		/// <returns>
		/// 	<c>true</c> if there is item with the name; otherwise <c>false</c>.
		/// </returns>
		public bool Remove(string name)
		{
			return _objects.Remove(name);
		}
		/// <summary>
		/// Clears and disposes all items in the resource.
		/// </summary>
		public void Clear()
		{
			foreach (var o in _objects)
			{
				IDisposable i = o.Value as IDisposable;
				if (i != null)
				{
					i.Dispose();
				}
			}
			_objects.Clear();
		}
		/// <summary>
		/// Releases this resource.
		/// </summary>
		public void Dispose()
		{
			if (_objects != null)
			{
				this.Clear();
				_objects = null;
			}
		}
	}
}
