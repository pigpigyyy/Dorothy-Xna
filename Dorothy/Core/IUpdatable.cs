namespace Dorothy.Core
{
	/// <summary>
	/// Anything needs to be updated each frame should implement this interface.
	/// </summary>
	public interface IUpdatable
	{
		/// <summary>
		/// Gets or sets the update order.
		/// Update order is the index of the IUpdatable object in its controller`s update queue.
		/// The value of it is better handled by the framework than the user.
		/// And it`s better implemented intendedly.
		/// </summary>
		/// <value>
		/// The update order.
		/// </value>
		int UpdateOrder
		{
			set;
			get;
		}
		/// <summary>
		/// Gets or sets a value indicating whether the Update() method should be called by frame.
		/// </summary>
		/// <value>
		///   <c>true</c> if enable updating; otherwise, <c>false</c>.
		/// </value>
		bool Enable
		{
			set;
			get;
		}
		/// <summary>
		/// Gets or sets its update controller which arranges its update order
		/// and calls its Update() method when its enabled.
		/// </summary>
		/// <value>
		/// The controller.
		/// </value>
		UpdateController Controller
		{
			set;
			get;
		}
		/// <summary>
		/// Interface for update.
		/// Implements it to get updated each frame.
		/// </summary>
		void Update();
	}
}
