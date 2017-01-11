using System;
using Microsoft.Xna.Framework;

namespace Dorothy.Helpers
{
	public static class oHelper
	{
		private static uint _seed = (uint)DateTime.Now.Ticks;
		private static Random _rand = new Random();

		/// <summary>
		/// The plane whose normal is backward(0,0,1) and distance is 0 to origin , parallel to screen.
		/// </summary>
		public static readonly Plane StandardPlane = new Plane(Vector3.Backward, 0);

		public static float NextFloat(float min, float max)
		{
			_seed = 214013 * _seed + 2531011;
			return min + (_seed >> 16) * (1.0f / 65535.0f) * (max - min);
		}
		public static int NextInt(int min, int max)
		{
			return _rand.Next(min, max);
		}
		public static bool PointInRectangle(float px, float py, float left, float top, float right, float bottom)
		{
			return ((bottom <= py && py <= top) && (left <= px && px <= right));
		}
	}
}
