namespace Zenseless.Patterns;

using static MathHelper;

/// <summary>
/// Provides methods for generating pseudo-random numbers, deterministic seeds, and noise.
/// </summary>
public static class Noise
{
	/// <summary>
	/// Generates a pseudo-random unsigned integer using a PCG (Permuted Congruential Generator) algorithm.
	/// </summary>
	/// <remarks>This method implements a PCG algorithm to produce high-quality pseudo-random numbers.  The output
	/// is deterministic for a given seed value, making it suitable for reproducible random number generation.</remarks>
	/// <param name="v">The seed value used to initialize the generator. Must be a non-zero value for meaningful results.</param>
	/// <returns>A pseudo-random unsigned integer generated based on the provided seed.</returns>
	public static uint Pcg(uint v)
	{
		uint state = (v * 747796405u) + 2891336453u;
		uint word = ((state >> (int)((state >> 28) + 4)) ^ state) * 277803737u;
		return (word >> 22) ^ word;
	}

	/// <summary>
	/// Generates a deterministic seed value based on the specified input coordinates.
	/// </summary>
	/// <param name="x">The x-coordinate used to calculate the seed.</param>
	/// <param name="y">The y-coordinate used to calculate the seed.</param>
	/// <returns>A 32-bit unsigned integer representing the calculated seed value.</returns>
	public static uint Seed(uint x, uint y) => (19u * x) + (47u * y) + 101u;

	/// <summary>
	/// Computes a smoothly interpolated value noise based on the input coordinates.
	/// </summary>
	/// <remarks>This method generates a value by performing cubic interpolation between random values at the
	/// nearest integer grid points surrounding the input coordinates. The interpolation is weighted using a smoothstep
	/// function to ensure smooth transitions between values.</remarks>
	/// <param name="x">The x-coordinate of the input position.</param>
	/// <param name="y">The y-coordinate of the input position.</param>
	/// <returns>A floating-point value in the range [0..1] representing the interpolated result at the specified coordinates.</returns>
	public static float Value(float x, float y)
	{
		uint ux = (uint)x.FastTruncate(); // unsigned integer position
		uint uy = (uint)y.FastTruncate();

		//random value at surrounding integer positions
		float v00 = Hash(ux, uy);
		float v10 = Hash(ux + 1, uy);
		float v01 = Hash(ux, uy + 1);
		float v11 = Hash(ux + 1, uy + 1);

		float weightX = Smoothstep(0.0f, 1.0f, x.Fract()); // cubic interpolation

		float x1 = Lerp(v00, v10, weightX);
		float x2 = Lerp(v01, v11, weightX);
		
		float weightY = Smoothstep(0.0f, 1.0f, y.Fract()); // cubic interpolation
		return Lerp(x1, x2, weightY);

	}
	private static float Hash(uint x, uint y)
	{
		uint a = Pcg(Seed(x, y));
		return (float)a / 0xffffffffU;
	}
}
