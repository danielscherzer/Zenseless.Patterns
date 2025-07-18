﻿using System;

namespace Zenseless.Patterns;

/// <summary>
/// Contains static/extension methods for System.Math[F]
/// </summary>
public static partial class MathHelper
{
	/// <summary>
	/// Clamp the input value x in between min and max. 
	/// If x smaller min return min; 
	/// if x bigger max return max; 
	/// else return x unchanged
	/// </summary>
	/// <param name="x">input value that will be clamped</param>
	/// <param name="min">lower limit</param>
	/// <param name="max">upper limit</param>
	/// <returns>clamped version of x</returns>
	public static int Clamp(this int x, int min, int max) => Math.Min(max, Math.Max(min, x));

	/// <summary>
	/// Clamp the input value x in between min and max. 
	/// If x smaller min return min; 
	/// if x bigger max return max; 
	/// else return x unchanged
	/// </summary>
	/// <param name="x">input value that will be clamped</param>
	/// <param name="min">lower limit</param>
	/// <param name="max">upper limit</param>
	/// <returns>clamped version of x</returns>
	public static float Clamp(this float x, float min, float max) => MathF.Min(max, MathF.Max(min, x));

	/// <summary>
	/// Clamp the input value x in between min and max. 
	/// If x smaller min return min; 
	/// if x bigger max return max; 
	/// else return x unchanged
	/// </summary>
	/// <param name="x">input value that will be clamped</param>
	/// <param name="min">lower limit</param>
	/// <param name="max">upper limit</param>
	/// <returns>clamped version of x</returns>
	public static double Clamp(this double x, double min, double max) => Math.Min(max, Math.Max(min, x));

	/// <summary>
	/// Re-maps a number from one range to another.
	/// </summary>
	/// <param name="input">the incoming value to be converted</param>
	/// <param name="inputLower">lower bound of the value's current range</param>
	/// <param name="inputUpper">upper bound of the value's current range</param>
	/// <param name="outputMin">lower bound of the value's target range</param>
	/// <param name="outputMax">upper bound of the value's target range</param>
	/// <returns></returns>
	public static float Map(this float input, float inputLower, float inputUpper, float outputMin, float outputMax)
	{
		return Lerp(outputMin, outputMax, Normalize(input, inputLower, inputUpper));
	}

	/// <summary>
	/// Returns the number of mipmap levels (floor convention) required for MIP mapped filtering of an image.
	/// </summary>
	/// <param name="width">The image width in pixels.</param>
	/// <param name="height">The image height in pixels.</param>
	/// <returns>Number of mipmap levels</returns>
	public static int MipMapLevelCount(int width, int height) => (int)MathF.Log(MathF.Floor(Math.Max(1, Math.Max(width, height))), 2f) + 1;

	/// <summary>
	/// Convert input uint from range [0,255] into float in range [0,1]
	/// </summary>
	/// <param name="v">input in range [0,255]</param>
	/// <returns>range [0,1]</returns>
	public static float Normalize(this uint v) => v / 255f;

	/// <summary>
	/// Finds the indices of a sorted array that encompass a given value
	/// </summary>
	/// <typeparam name="TValue"></typeparam>
	/// <param name="sorted">a sorted array of values</param>
	/// <param name="value">a value</param>
	/// <returns>two indices</returns>
	public static (int lower, int upper) FindEncompassingIndices<TValue>(this TValue[] sorted, TValue value)
	{
		var ipos = Array.BinarySearch(sorted, value);
		if (ipos >= 0)
		{
			// exact target found at position "ipos"
			return (ipos, ipos);
		}
		else
		{
			// Exact key not found: BinarySearch returns negative when the 
			// exact target is not found, which is the bitwise complement 
			// of the next index in the list larger than the target.
			ipos = ~ipos;
			if (0 == ipos)
			{
				return (0, 0);
			}
			if (ipos < sorted.Length)
			{
				return (ipos - 1, ipos);
			}
			else
			{
				return (sorted.Length - 1, sorted.Length - 1);
			}
		}
	}

	/// <summary>
	/// Transform the input value into the range [0..1]
	/// </summary>
	/// <param name="inputValue">the input value</param>
	/// <param name="inputMin">the lower input range bound</param>
	/// <param name="inputMax">the upper input range bound</param>
	/// <returns></returns>
	public static float Normalize(this float inputValue, float inputMin, float inputMax)
	{
		var inputRange = inputMax - inputMin;
		return float.Epsilon >= inputRange ? 0f : (inputValue - inputMin) / inputRange;
	}

	/// <summary>
	/// Linear interpolation of two known values a and b according to weight
	/// </summary>
	/// <param name="a">First value</param>
	/// <param name="b">Second value</param>
	/// <param name="weight">Interpolation weight</param>
	/// <returns>Linearly interpolated value</returns>
	public static float Lerp(float a, float b, float weight) => (a * (1 - weight)) + (b * weight);

	/// <summary>
	/// Linear interpolation of two known values a and b according to weight
	/// </summary>
	/// <param name="a">First value</param>
	/// <param name="b">Second value</param>
	/// <param name="weight">Interpolation weight</param>
	/// <returns>Linearly interpolated value</returns>
	public static double Lerp(double a, double b, double weight) => (a * (1 - weight)) + (b * weight);

	/// <summary>
	/// Returns the integer part of the specified floating-point number. 
	/// Works not for constructs like <code>1f - float.epsilon</code> because this is outside of floating point precision
	/// </summary>
	/// <param name="v">Input floating-point number</param>
	/// <returns>The integer part.</returns>
	public static int FastTruncate(this float v) => (int)v;

	/// <summary>
	/// Returns the fractional part of the floating-point number.
	/// </summary>
	/// <param name="v">Input floating-point number</param>
	/// <returns></returns>
	public static float Fract(this float v)
	{
		var abs = MathF.Abs(v);
		return abs - (int)abs;
	}

	/// <summary>
	/// Perform Hermite interpolation between 0 and 1 starting at the lower edge <paramref name="min"/> and the upper edge <paramref name="max"/>."/>
	/// </summary>
	/// <param name="min">The lower edge of the hermite interpolation</param>
	/// <param name="max"></param>
	/// <param name="x"></param>
	/// <returns></returns>
	public static float Smoothstep(float min, float max, float x)
	{
		var t = Math.Clamp((x - min) / (max - min), 0f, 1f);
		return t * t * (3f - (2f * t));
	}
}
