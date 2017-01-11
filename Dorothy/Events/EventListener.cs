using System;
namespace Dorothy.Events
{
	public interface IListener
	{
		int Order
		{
			set;
			get;
		}
		Action<Event> Action
		{
			set;
			get;
		}
	}
	public class EventListener : IListener
	{
		public const int INVALID_ORDER = -1;
		private int _order = EventListener.INVALID_ORDER;

		int IListener.Order
		{
			set { _order = value; }
			get { return _order; }
		}
		public string EventName
		{
			set;
			get;
		}
		public bool Register
		{
			set
			{
				if (value)
				{
					oEventManager.Register(this);
				}
				else
				{
					oEventManager.UnRegister(this);
				}
			}
			get { return (_order != EventListener.INVALID_ORDER); }
		}
		public Action<Event> Action
		{
			set;
			get;
		}

		public EventListener(string name, Action<Event> action = null)
		{
			this.EventName = name;
			this.Action = action;
		}
	}
}
