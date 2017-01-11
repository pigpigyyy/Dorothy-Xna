using Microsoft.Xna.Framework;

namespace Dorothy.Cameras
{
	/// <summary>
	/// Interface of camera
	/// Implements it to make a new camera.
	/// </summary>
	public interface ICamera
	{
		/// <summary>
		/// Gets the camera`s name.
		/// </summary>
		string Name
		{
			get;
		}
		/// <summary>
		/// Gets the position vector.
		/// </summary>
		Vector3 Position
		{
			get;
		}
		/// <summary>
		/// Gets the target vector.
		/// </summary>
		Vector3 Target
		{
			get;
		}
		/// <summary>
		/// Gets the up vector.
		/// </summary>
		Vector3 Up
		{
			get;
		}
		/// <summary>
		/// Applies this camera.
		/// </summary>
		void Apply();
	}
}
