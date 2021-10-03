using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Zenseless.Patterns.Tests
{
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
	}
}