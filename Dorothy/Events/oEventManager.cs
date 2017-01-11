using System.Collections.Generic;

namespace Dorothy.Events
{
	public static class oEventManager
	{
		private static Dictionary<string, EventType> _eventMap = new Dictionary<string, EventType>();
		private static Queue<Event> _eventQueue = new Queue<Event>();

		public static void AddEventType(EventType type)
		{
			if (!_eventMap.ContainsKey(type.EventName))
			{
				_eventMap.Add(type.EventName, type);
			}
		}
		public static void RemoveEventType(string typename)
		{
			EventType type = null;
			if (_eventMap.TryGetValue(typename, out type))
			{
				type.Clear();
				_eventMap.Remove(typename);
			}
		}
		public static void PostEvent(Event e)
		{
			_eventQueue.Enqueue(e);
		}
		public static void SendEvent(Event e)
		{
			_eventMap[e.Name].Update(e);
		}
		public static void Register(EventListener listener)
		{
			_eventMap[listener.EventName].Add(listener);
		}
		public static void UnRegister(EventListener listener)
		{
			_eventMap[listener.EventName].Remove(listener);
		}
		public static void Update()
		{
			while (_eventQueue.Count != 0)
			{
				Event e = _eventQueue.Dequeue();
				_eventMap[e.Name].Update(e);
			}
		}
	}
}
