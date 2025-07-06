using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Zenseless.Patterns.Tests;

[TestClass()]
public class NoiseTests
{
	[DataTestMethod()]
	[DataRow(0, 0, 0.36f)]
	[DataRow(0, 1, 0.97f)]
	public void ValueTest(float x, float y, float expected)
	{
		var delta = 0.01f;
		var result = Noise.Value(x, y);
		Assert.AreEqual(expected, result, delta);
	}
}