using System;
using System.Collections.Generic;

namespace Dorothy.Core
{
	/// <summary>
	/// Manages the game scenes.
	/// There is only one game scene updating and drawing at the same time.
	/// So different game scenes are isolated worlds doing their own logic and rendering.
	/// </summary>
	public static class oSceneManager
	{
		#region Field
		private static Scene _currentScene;
		private static Dictionary<string, Scene> _scene = new Dictionary<string, Scene>();
		#endregion

		/// <summary>
		/// Gets the current scene.
		/// </summary>
		public static Scene CurrentScene
		{
			get { return _currentScene; }
		}

		/// <summary>
		/// Adds a scene to manager.
		/// Never add a scene with a name already exists in manager.
		/// </summary>
		/// <param name="scene">The scene instance.</param>
		public static void Add(Scene scene)
		{
			_scene.Add(scene.Name, scene);
		}
		/// <summary>
		/// Removes a scene from manager.
		/// </summary>
		/// <param name="scene">The scene instance.</param>
		/// <returns>
		/// 	<c>true</c> if there exist the specified scene; otherwise, <c>false</c>.
		/// </returns>
		public static bool Remove(Scene scene)
		{
			return _scene.Remove(scene.Name);
		}
		/// <summary>
		/// Removes a scene from manager by its name.
		/// </summary>
		/// <param name="scene">The scene`s name.</param>
		/// <returns>
		/// 	<c>true</c> if there exist a scene with the name; otherwise, <c>false</c>.
		/// </returns>
		public static bool Remove(string name)
		{
			return _scene.Remove(name);
		}
		/// <summary>
		/// Switches to a scene with the specified name from manager.
		/// When switching to a new scene, first the last scene`s Dispose() method is called
		/// then the new scene`s Initialize() method.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <returns>
		/// 	<c>true</c> if there exist a scene with the name; otherwise, <c>false</c>.
		/// </returns>
		public static bool SwitchScene(string name)
		{
			Scene scene;
			if (_scene.TryGetValue(name, out scene))
			{
				if (_currentScene != null)
				{
					_currentScene.Dispose();
				}
				_currentScene = scene;
				_currentScene.Initialize();
				return true;
			}
			return false;
		}
		/// <summary>
		/// Gets a managed scene by name.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <returns>The named scene.</returns>
		public static Scene GetScene(string name)
		{
			Scene scene;
			_scene.TryGetValue(name, out scene);
			return scene;
		}
		/// <summary>
		/// Draws the current scene.
		/// [Called by framework]
		/// </summary>
		public static void Draw()
		{
			_currentScene.Draw();
		}
		/// <summary>
		/// Draws matched items in current scene.
		/// </summary>
		/// <param name="match">The matching delegate.</param>
		public static void Draw(Predicate<Drawable> match)
		{
			_currentScene.Draw(match);
		}
		/// <summary>
		/// Updates the current scene
		/// [Called by framework].
		/// </summary>
		public static void Update()
		{
			_currentScene.Update();
		}
	}
}
