using Microsoft.Xna.Framework.Input;

namespace Dorothy.Core
{
	/// <summary>
	/// Gets all input details from it.
	/// </summary>
	public static class oInput
	{
		#region Field
		private static KeyboardState _oldKeyState;
		private static KeyboardState _newKeyState;
		private static MouseState _oldMouseState;
		private static MouseState _newMouseState;
		#endregion

		/// <summary>
		/// Gets the mouse X position on the screen.
		/// </summary>
		public static int MouseX
		{
			get { return _newMouseState.X; }
		}
		/// <summary>
		/// Gets the mouse Y position on the screen.
		/// </summary>
		public static int MouseY
		{
			get { return _newMouseState.Y; }
		}
		/// <summary>
		/// Gets a value indicating whether left mouse button is down.
		/// </summary>
		/// <value>
		/// 	<c>true</c> the moment left button is clicked; holding or released, <c>false</c>.
		/// </value>
		public static bool IsLeftButtonDown
		{
			get { return (_newMouseState.LeftButton == ButtonState.Pressed && _oldMouseState.LeftButton == ButtonState.Released); }
		}
		/// <summary>
		/// Gets a value indicating whether left mouse button is up.
		/// </summary>
		/// <value>
		/// 	<c>true</c> the moment left button is releasing; holding or released, <c>false</c>.
		/// </value>
		public static bool IsLeftButtonUp
		{
			get { return (_newMouseState.LeftButton == ButtonState.Released && _oldMouseState.LeftButton == ButtonState.Pressed); }
		}
		/// <summary>
		/// Gets a value indicating whether left mouse button is pressing.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if left mouse button is pressing; otherwise, <c>false</c>.
		/// </value>
		public static bool IsLeftButtonPressed
		{
			get { return _newMouseState.LeftButton == ButtonState.Pressed; }
		}
		/// <summary>
		/// Gets a value indicating whether right mouse button is down.
		/// </summary>
		/// <value>
		/// 	<c>true</c> the moment right button is clicked; holding or released, <c>false</c>.
		/// </value>
		public static bool IsRightButtonDown
		{
			get { return (_newMouseState.RightButton == ButtonState.Pressed && _oldMouseState.RightButton == ButtonState.Released); }
		}
		/// <summary>
		/// Gets a value indicating whether right mouse button is up.
		/// </summary>
		/// <value>
		/// 	<c>true</c> the moment right button is releasing; holding or released, <c>false</c>.
		/// </value>
		public static bool IsRightButtonUp
		{
			get { return (_newMouseState.RightButton == ButtonState.Released && _oldMouseState.RightButton == ButtonState.Pressed); }
		}
		/// <summary>
		/// Gets a value indicating whether right mouse button is pressing.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if right mouse button is pressing; otherwise, <c>false</c>.
		/// </value>
		public static bool IsRightButtonPressed
		{
			get { return _newMouseState.RightButton == ButtonState.Pressed; }
		}
		/// <summary>
		/// Gets whether the specified key is pressing down.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <returns>
		///   <c>true</c> the moment key is pressing down; otherwise, <c>false</c>.
		/// </returns>
		public static bool IsKeyDown(Keys key)
		{
			return (_newKeyState.IsKeyDown(key) && !_oldKeyState.IsKeyDown(key));
		}
		/// <summary>
		/// Gets whether the specified key is releasing.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <returns>
		///   <c>true</c> the moment key is releasing; otherwise, <c>false</c>.
		/// </returns>
		public static bool IsKeyUp(Keys key)
		{
			return (!_newKeyState.IsKeyDown(key) && _oldKeyState.IsKeyDown(key)); ;
		}
		/// <summary>
		/// Gets the state of the key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <returns>
		///   <c>true</c> when key is pressing; otherwise, <c>false</c>.
		/// </returns>
		public static bool GetKeyState(Keys key)
		{
			return _newKeyState.IsKeyDown(key);
		}
		/// <summary>
		/// Gets the Pressed keys.
		/// </summary>
		public static Keys[] PressedKeys()
		{
			return _newKeyState.GetPressedKeys();
		}
		/// <summary>
		/// Updates input states.
		/// [Called by framework]
		/// </summary>
		public static void Update()
		{
			_oldKeyState = _newKeyState;
			_newKeyState = Keyboard.GetState();
			_oldMouseState = _newMouseState;
			_newMouseState = Mouse.GetState();
		}
	}
}
