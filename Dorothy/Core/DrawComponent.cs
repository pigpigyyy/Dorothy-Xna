namespace Dorothy.Core
{
	/// <summary>
	/// Anything needs to update and draw at the same time,
	/// could implements its Update() and Draw() methods.
	/// </summary>
	public abstract class DrawComponent : Drawable, IUpdatable
	{
		#region Field
		private int _updateOrder = UpdateController.INVALID_ORDER;
		private UpdateController _controller;
		#endregion

		/// <summary>
		/// Gets or sets the update order.
		/// Update order is the index of the IUpdatable object in its controller`s update queue.
		/// The value of it is better handled by the framework than the user.
		/// And it`s better implemented intendedly.
		/// </summary>
		/// <value>
		/// The update order.
		/// </value>
		int IUpdatable.UpdateOrder
		{
			set { _updateOrder = value; }
			get { return _updateOrder; }
		}
		/// <summary>
		/// Gets or sets a value indicating whether the Update() method should be called each frame.
		/// </summary>
		/// <value>
		///   <c>true</c> if enable updating; otherwise, <c>false</c>.
		/// </value>
		public bool Enable
		{
			set
			{
				if (value)
				{ _controller.Add(this); }
				else
				{ _controller.Remove(this); }
			}
			get { return (_updateOrder != UpdateController.INVALID_ORDER); }
		}
		/// <summary>
		/// Gets or sets its update controller which arranges its update order
		/// and calls its Update() method when its enabled.
		/// </summary>
		/// <value>
		/// The controller.
		/// </value>
		public UpdateController Controller
		{
			set
			{
				if (_updateOrder != UpdateController.INVALID_ORDER)
				{
					_controller.Remove(this);
					value.Add(this);
				}
				_controller = value;
			}
			get { return _controller; }
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="DrawComponent"/> class.
		/// Its update controller should be specified.
		/// </summary>
		/// <param name="controller">The controller.</param>
		public DrawComponent(UpdateController controller)
		{
			_controller = controller;
		}
		/// <summary>
		/// Interface method for update.
		/// Implements it to get updated each frame.
		/// </summary>
		public abstract void Update();
	}
}
