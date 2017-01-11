using System;

namespace Dorothy.Core
{
	/// <summary>
	/// Gets items updated or not updated.
	/// </summary>
	public class UpdateController
	{
		#region Field
		public const int INVALID_ORDER = -1;
		private IUpdatable[] _updates = new IUpdatable[31];
		private int _capacity = 31;
		private int _count;
		#endregion

		/// <summary>
		/// Adds an update item in the update queue.
		/// </summary>
		/// <param name="update">The update item.</param>
		public void Add(IUpdatable update)
		{
			if (update.UpdateOrder == UpdateController.INVALID_ORDER)
			{
				if (_count + 1 > _capacity)
				{
					_capacity = _capacity * 2 + 1;
					IUpdatable[] list = new IUpdatable[_capacity];
					Array.Copy(_updates, list, _count);
					_updates = list;
				}
				_updates[_count] = update;
				update.UpdateOrder = _count;
				_count++;
			}
		}
		/// <summary>
		/// Removes an update item from queue.
		/// </summary>
		/// <param name="update">The update item.</param>
		public void Remove(IUpdatable update)
		{
			int order = update.UpdateOrder;
			if (order != UpdateController.INVALID_ORDER)
			{
				_count--;
				_updates[order] = _updates[_count];
				_updates[order].UpdateOrder = order;
				update.UpdateOrder = UpdateController.INVALID_ORDER;
			}
		}
		/// <summary>
		/// Removes all update items from update queue.
		/// </summary>
		public void Clear()
		{
			foreach (var u in _updates)
			{
				u.UpdateOrder = UpdateController.INVALID_ORDER;
			}
			_count = 0;
		}
		/// <summary>
		/// Sets its capacity to its item`s number.
		/// </summary>
		public void Trim()
		{
			if (_count < _capacity)
			{
				_capacity = _count;
				IUpdatable[] list = new IUpdatable[_count];
				Array.Copy(_updates, list, _count);
				_updates = list;
			}
		}
		/// <summary>
		/// Updates this controller.
		/// [Called by framework]
		/// </summary>
		public void Update()
		{
			for (int i = 0; i < _count; i++)
			{
				IUpdatable u = _updates[i];
				u.Update();
				if (u.UpdateOrder == UpdateController.INVALID_ORDER)
				{
					i--;
				}
			}
		}
	}
}
