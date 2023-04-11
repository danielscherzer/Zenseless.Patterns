using System;
using System.Collections.Generic;

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
			return min + (max - min) * (float)random.NextDouble(); //TODO: net6 has NextSingle, but speed seems same
		}

		/// <summary>
		/// Fisher-Yates shuffle algorithm: https://en.wikipedia.org/wiki/Fisher%E2%80%93Yates_shuffle#Fisher_and_Yates'_original_method
		/// </summary>
		/// <typeparam name="TType">Element type</typeparam>
		/// <param name="random">An instance of the <see cref="Random"/> class.</param>
		/// <param name="input">Input list of numbers</param>
		public static void Shuffle<TType>(this Random random, IList<TType> input)
		{
			Shuffle(input, random.Next);
		}

		/// <summary>
		/// Fisher-Yates shuffle algorithm: https://en.wikipedia.org/wiki/Fisher%E2%80%93Yates_shuffle#Fisher_and_Yates'_original_method
		/// </summary>
		/// <typeparam name="TType">Element type</typeparam>
		/// <param name="input">List of numbers to shuffle</param>
		/// <param name="random">A <seealso cref="Func{T, TResult}"/> that returns a random integer in the range [0, parameter - 1]</param>
		public static void Shuffle<TType>(this IList<TType> input, Func<int, int> random)
		{
			for (int i = input.Count - 1; i > 0; --i)
			{
				int rndId = random(i + 1);
				(input[rndId], input[i]) = (input[i], input[rndId]);
			}
		}
	}
}
