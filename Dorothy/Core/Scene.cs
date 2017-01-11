using System;

namespace Dorothy.Core
{
	/// <summary>
	/// Game scene.
	/// </summary>
	public abstract class Scene : IDisposable
	{
		#region Field
		private string _name = string.Empty;
		private DrawRoot _root = new DrawRoot();
		private UpdateController _controller = new UpdateController();
		#endregion

		/// <summary>
		/// Gets the scene`s name.
		/// </summary>
		public string Name
		{
			get { return _name; }
		}
		/// <summary>
		/// Gets the scene root.
		/// </summary>
		public DrawRoot Root
		{
			get { return _root; }
		}
		/// <summary>
		/// Gets the scene`s controller.
		/// </summary>
		public UpdateController Controller
		{
			get { return _controller; }
		}
		/// <summary>
		/// Gets the game instance.
		/// </summary>
		protected Microsoft.Xna.Framework.Game Game
		{
			set;
			get;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Scene"/> class.
		/// </summary>
		/// <param name="game">The game instance.</param>
		/// <param name="name">The scene`s name.</param>
		public Scene(Microsoft.Xna.Framework.Game game, string name)
		{
			this.Game = game;
			_name = name;
		}
		/// <summary>
		/// Interface method for initializing the scene.
		/// Implements it to get the scene initialized.
		/// </summary>
		public abstract void Initialize();
		/// <summary>
		/// Draws the scene.
		/// Override it to attach your own drawing code.
		/// [Called by framework]
		/// </summary>
		public virtual void Draw()
		{
			_root.Draw();
		}
		/// <summary>
		/// Draws the matched items in the scene.
		/// </summary>
		/// <param name="match">The matching delegate.</param>
		public virtual void Draw(Predicate<Drawable> match)
		{
			_root.Draw(match);
		}
		/// <summary>
		/// Updates this scene.
		/// Override it to attach your own updating code.
		/// [Call by framework]
		/// </summary>
		public virtual void Update()
		{
			_controller.Update();
		}
		/// <summary>
		/// Interface method for disposing the scene.
		/// Implements it to get the scene disposed.
		/// </summary>
		public abstract void Dispose();
	}
}
