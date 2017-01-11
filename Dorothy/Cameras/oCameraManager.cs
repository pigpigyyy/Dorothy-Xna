using System.Collections.Generic;

namespace Dorothy.Cameras
{
	/// <summary>
	/// Manages the cameras.
	/// </summary>
	public static class oCameraManager
	{
		#region Field
		private static DefaultCamera Default = new DefaultCamera();
		private static ICamera _currentCamera = Default;
		private static Dictionary<string, ICamera> _cameras = new Dictionary<string, ICamera>();
		#endregion

		/// <summary>
		/// Gets the current camera.
		/// </summary>
		public static ICamera CurrentCamera
		{
			get { return _currentCamera; }
		}

		/// <summary>
		/// Adds a camera to its management.
		/// </summary>
		/// <param name="camera">The camera instance.</param>
		/// <returns></returns>
		public static bool Add(ICamera camera)
		{
			if (!_cameras.ContainsKey(camera.Name))
			{
				_cameras.Add(camera.Name, camera);
				return true;
			}
			return false;
		}
		/// <summary>
		/// Removes a camera from its management.
		/// </summary>
		/// <param name="camera">The camera instance.</param>
		/// <returns>If there not exists the camera, returns false.</returns>
		public static bool Remove(ICamera camera)
		{
			if (camera != oGame.DefaultCamera)
			{
				return _cameras.Remove(camera.Name);
			}
			return false;
		}
		/// <summary>
		/// Removes a camera from its management.
		/// </summary>
		/// <param name="name">The camera`s name.</param>
		/// <returns>If there not exists camera with the name, returns false.</returns>
		public static bool Remove(string name)
		{
			if (name.Equals(oGame.DefaultCamera.Name))
			{
				return false;
			}
			return _cameras.Remove(name);
		}
		/// <summary>
		/// Gets a camera.
		/// </summary>
		/// <param name="name">The camera`s name.</param>
		/// <returns></returns>
		public static ICamera GetCamera(string name)
		{
			ICamera camera = null;
			_cameras.TryGetValue(name, out camera);
			return camera;
		}
		/// <summary>
		/// Clears all managed cameras but camera named "Default".
		/// </summary>
		public static void Clear()
		{
			_cameras.Clear();
			oCameraManager.Add(oGame.DefaultCamera);
		}
		/// <summary>
		/// Start to apply the camera with the specific name.
		/// </summary>
		/// <param name="name">The camera`s name.</param>
		/// <returns></returns>
		public static bool Apply(string name)
		{
			ICamera camera = null;
			if (_cameras.TryGetValue(name, out camera))
			{
				_currentCamera = camera;
				_currentCamera.Apply();
				return true;
			}
			return false;
		}
		/// <summary>
		/// Start to apply the specific camera.
		/// </summary>
		/// <param name="camera">The camera.</param>
		public static void Apply(ICamera camera)
		{
			_currentCamera = (camera == null ? oGame.DefaultCamera : camera);
			_currentCamera.Apply();
		}
		/// <summary>
		/// Updates the camera manager.
		/// And the current camera`s Apply() method will be called per frame.
		/// [Called by framework]
		/// </summary>
		public static void Update()
		{
			_currentCamera.Apply();
		}
	}
}
