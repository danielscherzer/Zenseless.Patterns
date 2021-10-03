using System;

namespace Zenseless.Patterns
{
	/// <summary>
	/// This static class contains extension methods to the <see cref="Random"/> class for <see cref="float"/> variables.
	/// </summary>
	public static class RandomExtensions
	{
		/// <summary>
		/// Returns a random float in the range from <paramref name="min"/>(default = 0) to <paramref name="max"/> default = 1.
		/// </summary>
		/// <param name="random">An instance of the <see cref="Random"/> class.</param>
		/// <param name="min">The inclusive lower bound of the random number returned.</param>
		/// <param name="max">The exclusive upper bound of the random number returned. <paramref name="max"/> must be greater than or equal to minValue.</param>
		/// <returns>A 32-bit <see cref="float"/> greater than or equal to <paramref name="min"/> and less than <paramref name="max"/></returns>
		public static float NextFloat(this Random random, float min = 0f, float max = 1f)
		{
			return min + (max - min) * (float)random.NextDouble();
		}
	}
}
