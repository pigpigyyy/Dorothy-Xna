namespace Dorothy.Events
{
	public class Event
	{
		public string Name
		{
			set;
			get;
		}
		public Event(string name)
		{
			this.Name = name;
		}
	}
}
