using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace Zenseless.Patterns.Tests;

[TestClass()]
public class RandomExtensionsTests
{
	[DataTestMethod()]
	[DataRow(0f, 1f)]
	[DataRow(-1f, 1f)]
	[DataRow(-1f, 10f)]
	[DataRow(-4f, -1f)]
	public void NextFloatTest(float min, float max)
	{
		Random rnd = new();
		for (int i = 0; i < 100; ++i)
		{
			float result = rnd.NextFloat(min, max);
			Assert.IsTrue(result < max);
			Assert.IsTrue(min <= result);
		}
	}

	[TestMethod()]
	public void ShuffleContiansInputTest()
	{
		Random rnd = new();
		var input = Enumerable.Range(1, 9).ToArray();
		void CheckIfShuffledInput()
		{
			var temp = new int[input.Length];
			input.CopyTo(temp, 0);
			Array.Sort(temp);
			for (int i = 0; i < input.Length; ++i)
			{
				Assert.AreEqual(i + 1, temp[i]);
			}
		}
		for (int i = 0; i < 100; ++i)
		{
			rnd.Shuffle(input);
			CheckIfShuffledInput();
		}
	}

	[TestMethod()]
	public void ShuffleEntropyTest()
	{
		Random rnd = new();
		const int range = 10;
		var counts = new int[range, range];
		const int count = 1000000;
		for (int i = 0; i < count; ++i)
		{
			var input = Enumerable.Range(1, range).ToArray();
			rnd.Shuffle(input);
			for (int j = 0; j < range; ++j)
			{
				counts[j, input[j] - 1]++;
			}
		}
		var expected = count / (double)range;
		var delta = 0.1 * expected; // below 10% error
		foreach (var cell in counts)
		{
			Assert.AreEqual(expected, cell, delta);
		}
	}
}