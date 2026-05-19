using System;

namespace Example;

/// <summary>
/// A for loop extension class
/// </summary>
public static class ForLoopExtension
{
	/// <summary>
	/// Create a for loop from an integer that gives the iteration count.
	/// </summary>
	/// <param name="count">how often the loop is iterated</param>
	/// <param name="action">to perform per iteration</param>
	public static void TimesDo(this int count, Action<int> action)
	{
		for (int i = 0; i < count; ++i) action(i);
	}

}
