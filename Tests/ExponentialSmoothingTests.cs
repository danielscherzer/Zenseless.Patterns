using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Zenseless.Patterns.Tests
{
	[TestClass]
	public class ExponentialSmoothingTests
	{
		[DataTestMethod()]
		[DataRow(-.01)]
		[DataRow(0)]
		[DataRow(1)]
		[DataRow(1.01)]
		public void WrongWeight(double weight)
		{
			Assert.ThrowsException<ArgumentException>(() => new ExponentialSmoothing(weight));
		}

		[TestMethod]
		public void SmoothedValueConstant()
		{
			for (var value = -1.0; value < 2.0; value += 0.1)
			{
				var smoothing = new ExponentialSmoothing(0.1);
				smoothing.NewSample(value);
				Assert.AreEqual(value, smoothing.SmoothedValue);
				smoothing.NewSample(value);
				Assert.AreEqual(value, smoothing.SmoothedValue);
			}
		}

		[TestMethod]
		public void SmoothedValueRnd()
		{
			var rnd = new Random();
			var samples = 1000;
			for (var j = 0; j < 100; ++j)
			{
				var smoothing = new ExponentialSmoothing(10.0 / samples);
				for (int i = 0; i < samples; ++i)
				{
					var value = rnd.NextDouble();
					smoothing.NewSample(value);
				}
				Assert.AreEqual(0.5, smoothing.SmoothedValue, 0.1);
			}
		}
	}
}
