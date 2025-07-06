using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace Zenseless.Patterns.Tests;

[TestClass()]
public class NoiseTests
{
	[TestMethod()]
	public void ValueTest1dRange()
	{
		Random rnd = new(12);
		Parallel.For(0, 100000, i =>
		{
			var x = (float)rnd.NextFloat(-1000000, 1000000);
			var result = Noise.Value(x);
			Assert.IsTrue(result >= 0.0f && result <= 1.0f, $"Value out of range for x={x}: {result}");
		});
	}

	[DataTestMethod()]
	[DataRow(-0.005f)]
	public void ValueTest1dSmooth(float x)
	{
		var delta = 0.01f;
		var x1 = x + delta; // small step to check smoothness
		var noiseX = Noise.Value(x);
		var noiseX1 = Noise.Value(x1);
		var result = Math.Abs(noiseX - noiseX1);
		Assert.IsTrue(result < delta, $"Output values delta={result} for x={x} and x1={x1}");
	}

	[TestMethod()]
	public void ValueTest1dSmoothMany()
	{
		Random rnd = new(12);
		Parallel.For(0, 1000000, i =>
		{
			var delta = 0.01f;
			var x = (float)rnd.NextFloat(-1000, 1000);
			var x1 = x + delta; // small step to check smoothness
			var noiseX = Noise.Value(x);
			var noiseX1 = Noise.Value(x1);
			var result = Math.Abs(noiseX - noiseX1);
			Assert.IsTrue(result < 1.5f * delta, $"Output values delta={result} for x={x} and x1={x1}");
		});
	}

	[DataTestMethod()]
	[DataRow(0, 0.03f)]
	public void ValueTest1d(float x, float expected)
	{
		var delta = 0.01f;
		var result = Noise.Value(x);
		Assert.AreEqual(expected, result, delta);
	}

	[TestMethod()]
	public void ValueTest2dRange()
	{
		Random rnd = new(12);
		Parallel.For(0, 1000, i =>
		{
			var x = (float)rnd.NextFloat(-1000000, 1000000);
			var y = (float)rnd.NextFloat(-1000000, 1000000);
			var result = Noise.Value(x, y);
			Assert.IsTrue(result >= 0.0f && result <= 1.0f, $"Value out of range for x={x} y={y}: {result}");
		});
	}

	[TestMethod()]
	public void ValueTest2dSmooth()
	{
		Random rnd = new(12);
		Parallel.For(0, 1000000, i =>
		{
			var delta = 0.01f;
			var x = (float)rnd.NextFloat(-1000, 1000);
			var y = (float)rnd.NextFloat(-1000, 1000);
			var x1 = x + delta; // small step to check smoothness
			var y1 = y + delta; // small step to check smoothness
			var noiseX = Noise.Value(x, y);
			var noiseX1 = Noise.Value(x1, y1);
			var result = Math.Abs(noiseX - noiseX1);
			Assert.IsTrue(result < delta * 10, $"Output values delta={result} for x={x}: ");
		});
	}

	[DataTestMethod()]
	[DataRow(0, 0, 0.36f)]
	[DataRow(0, 1, 0.97f)]
	public void ValueTest2d(float x, float y, float expected)
	{
		var delta = 0.01f;
		var result = Noise.Value(x, y);
		Assert.AreEqual(expected, result, delta);
	}
}