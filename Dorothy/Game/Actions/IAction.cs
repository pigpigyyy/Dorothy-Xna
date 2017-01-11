using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dorothy.Game;

namespace Dorothy.Game.Actions
{
	public delegate void ActionHandler(IAction sender);
	public interface IAction
	{
		string Name
		{
			get;
		}
		int Priority
		{
			set;
			get;
		}
		bool IsDoing
		{
			get;
		}
		event ActionHandler ActionStart;
		event ActionHandler ActionEnd;

		void Do();
		void Update();
		void Break();
	}
}
