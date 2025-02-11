using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Zenseless.Patterns.Tests
{
	[TestClass()]
	public class HandleTests
	{
		[TestMethod()]
		public void HandleEqualTest()
		{
			Handle<double> handleD1 = new(1);
			Handle<double> handleD2 = new(1);
			Assert.AreEqual(handleD1, handleD2);
		}

		[TestMethod()]
		public void HandleTypeTest()
		{
			var typeHandleF = typeof(Handle<float>);
			var typeHandleD = typeof(Handle<double>);
			Assert.IsFalse(typeHandleF.IsAssignableFrom(typeHandleD));
			Assert.IsFalse(typeHandleF.IsAssignableTo(typeHandleD));
			
			Handle<float> handleF = new();
			Assert.IsNotInstanceOfType(handleF, typeHandleD);
		}

		[TestMethod()]
		public void HandleImplicitConvertTest()
		{
			const int value = 77;
			Handle<float> handleF = new(77);
			Assert.AreEqual(value, handleF.Id);
			Assert.AreEqual(value, handleF);
		}
	}
}