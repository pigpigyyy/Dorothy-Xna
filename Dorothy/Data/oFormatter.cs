using System.Runtime.Serialization.Formatters.Binary;

namespace Dorothy.Data
{
	/// <summary>
	/// Formatter for serialization.
	/// </summary>
	public static class oFormatter
	{
		/// <summary>
		/// Binary formatter for serialization.
		/// </summary>
		public static readonly BinaryFormatter Binary = new BinaryFormatter();
	}
}
