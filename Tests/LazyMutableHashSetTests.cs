using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Zenseless.Patterns.Tests
{
	[TestClass()]
	public class LazyMutableHashSetTests
	{
		[TestMethod()]
		public void AddTest()
		{
			var input = new int[] { 5, 1, 0, 55 };
			var hs = new LazyMutableHashSet<int> { input };
			//add again
			hs.Add(input[0]);
			hs.Add(input[2]);
			CollectionAssert.AreEquivalent(input, hs.ToArray());
		}


		[TestMethod()]
		public void RemoveTest()
		{
			var hs = new LazyMutableHashSet<int> { 5, 1, 0, 55 };
			hs.Remove(0);
			hs.Remove(5);
			CollectionAssert.AreEquivalent(new int[] { 1, 55 }, hs.ToArray());
		}

		[TestMethod()]
		public void GetEnumeratorTestAdd()
		{
			var input = new int[] { 5, 1, 3, 55 };

			var hs = new LazyMutableHashSet<int> { input };
			int count = 0;
			foreach (var i in hs)
			{
				count++;
				hs.Add(i + 1); //delay add
			}
			var expected = input.Concat(input.Select(i => i + 1)).ToArray();
			Assert.AreEqual(count, input.Count());
			CollectionAssert.AreEquivalent(expected, hs.ToArray());
		}

		[TestMethod()]
		public void GetEnumeratorTestAddAndRemove()
		{
			var input = new int[] { 5, 1, 3, 55 };

			var hs = new LazyMutableHashSet<int> { input };
			foreach (var i in hs)
			{
				hs.Add(i + 1); //delay add
				hs.Remove(i + 1);
			}
			CollectionAssert.AreEquivalent(input, hs.ToArray());
		}

		[TestMethod()]
		public void GetEnumeratorTestRemoveAndAdd()
		{
			var input = new int[] { 5, 1, 3, 55 };

			var hs = new LazyMutableHashSet<int> { input };
			foreach (var i in hs)
			{
				hs.Remove(i + 1);
				hs.Add(i + 1); //delay add
			}
			var expected = input.Concat(input.Select(i => i + 1));
			CollectionAssert.AreEquivalent(expected.ToArray(), hs.ToArray());
		}


		[TestMethod()]
		public void GetEnumeratorTestRecursive()
		{
			var input = new int[] { 5, 1, 3, 55 };

			var hs = new LazyMutableHashSet<int> { input };
			foreach (var i in hs)
			{
				hs.Add(i + 1); //delayed add
				foreach (var j in hs)
				{
					hs.Remove(j + 1); //delayed remove
				}
			}
			CollectionAssert.AreEquivalent(input, hs.ToArray());
		}

		[TestMethod()]
		public void GetEnumeratorTestRecursive2()
		{
			var input = new int[] { 5, 1, 3, 55 };

			var hs = new LazyMutableHashSet<int> { input };
			foreach (var i in hs)
			{
				hs.Add(i + 1); //delayed add
				foreach (var j in hs)
				{
					hs.Add(j + 100); //delayed add
				}
			}
			var expected = input.Concat(input.Select(i => i + 1)).Concat(input.Select(i => i + 100));
			CollectionAssert.AreEquivalent(expected.ToArray(), hs.ToArray());
		}
	}
}