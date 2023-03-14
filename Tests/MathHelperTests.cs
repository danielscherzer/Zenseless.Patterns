using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Numerics;

namespace Zenseless.Patterns.Tests
{
	[TestClass()]
	public class MathHelperTests
	{
		[DataTestMethod()]
		[DataRow(0f, 0)]
		[DataRow(float.Epsilon, 0)]
		[DataRow(0.25f, 0)]
		[DataRow(-0.5f, 0)]
		[DataRow(0.75f, 0)]
		[DataRow(0.99f, 0)]
		[DataRow(-0.99999f, 0)]
		[DataRow(12345f, 12345)]
		[DataRow(12345.99f, 12345)]
		[DataRow(-1f, -1)]
		[DataRow(-1.99f, -1)]
		[DataRow(-12345.99f, -12345)]
		public void FastTruncateTest(float input, int expected) => Assert.AreEqual(expected, input.FastTruncate());

		[DataTestMethod()]
		[DataRow(0, 1)]
		[DataRow(1, 1)]
		[DataRow(2, 2)]
		[DataRow(3, 2)]
		[DataRow(4, 3)]
		[DataRow(16, 5)]
		[DataRow(256, 9)]
		public void MipMapLevelsSquareTest(int inputSize, int expected)
		{
			var output = MathHelper.MipMapLevelCount(inputSize, inputSize);
			Assert.AreEqual(expected, output);
		}

		[DataTestMethod()]
		[DataRow(new int[] { 1, 3, 5 }, 1, 0, 0)]
		[DataRow(new int[] { 1, 3, 5 }, 3, 1, 1)]
		[DataRow(new int[] { 1, 3, 5 }, 5, 2, 2)]
		[DataRow(new int[] { 1, 3, 5 }, 2, 0, 1)]
		[DataRow(new int[] { 1, 3, 5 }, 4, 1, 2)]
		[DataRow(new int[] { 1, 3, 5 }, 0, 0, 0)]
		[DataRow(new int[] { 1, 3, 5 }, 6, 2, 2)]
		public void FindEncompassingRangeTest(int[] values, int value, int lowerId, int upperId)
		{
			var output = values.FindEncompassingIndices(value);
			Assert.AreEqual((lowerId, upperId), output);
		}

		[DataTestMethod()]
		[DataRow(0, 0)]
		[DataRow(0.3f, 0.3f)]
		[DataRow(1.3f, 0.3f)]
		[DataRow(-1.3f, 0.3f)]
		public void FractTest(float input, float expected)
		{
			var delta = 0.0000001f;
			Assert.AreEqual(expected, input.Fract(), delta);
		}
	}
}