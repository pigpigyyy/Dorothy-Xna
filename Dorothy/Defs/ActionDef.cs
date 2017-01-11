using System;
using Dorothy.Game;
using Dorothy.Game.Actions;

namespace Dorothy.Defs
{
	/// <summary>
	/// Role action`s definition.
	/// </summary>
	[Serializable]
	public abstract class ActionDef
	{
		/// <summary>
		/// Action name.
		/// </summary>
		public string Name = string.Empty;
		/// <summary>
		/// Convert the definition to a new action instance.
		/// </summary>
		/// <param name="role">The role.</param>
		/// <returns>A new action instance.</returns>
		public abstract IAction ToAction(Role role);
	}
}
