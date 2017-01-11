using System.Collections.Generic;
using System;

namespace Dorothy.Events
{
	public class EventType
	{
		private IListener[] _listeners = new IListener[0];
		private int _capacity = 0;
		private int _count;

		public string EventName
		{
			set;
			get;
		}

		public EventType(string name)
		{
			this.EventName = name;
		}
		public void Add(IListener listener)
		{
			if (listener.Order == EventListener.INVALID_ORDER)
			{
				if (_count + 1 > _capacity)
				{
					_capacity = _capacity * 2 + 1;
					IListener[] list = new IListener[_capacity];
					Array.Copy(_listeners, list, _count);
					_listeners = list;
				}
				_listeners[_count] = listener;
				listener.Order = _count;
				_count++;
			}
		}
		public void Remove(IListener listener)
		{
			int order = listener.Order;
			if (order != EventListener.INVALID_ORDER)
			{
				_count--;
				_listeners[order] = _listeners[_count];
				listener.Order = EventListener.INVALID_ORDER;
			}
		}
		public void Clear()
		{
			foreach (var listener in _listeners)
			{
				listener.Order = EventListener.INVALID_ORDER;
			}
			_count = 0;
		}
		public void Trim()
		{
			if (_count < _capacity)
			{
				_capacity = _count;
				IListener[] list = new IListener[_count];
				Array.Copy(_listeners, list, _count);
				_listeners = list;
			}
		}
		public void Update(Event e)
		{
			for (int i = 0; i < _listeners.Length; i++)
			{
				IListener listener = _listeners[i];
				listener.Action(e);
				if (listener.Order == EventListener.INVALID_ORDER)
				{
					i--;
				}
			}
		}
	}
}
