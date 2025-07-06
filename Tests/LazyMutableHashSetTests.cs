using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Zenseless.Patterns.Tests;

[TestClass()]
public class LazyMutableHashSetTests
{
	[TestMethod()]
	public void AddTest()
	{
		var input = new int[] { 5, 1, 0, 55 };
		LazyMutableHashSet<int> hs =
		[
			.. input,
			//add again
			input[0],
			input[2],
		];
		Assert.AreEqual(input.Length, hs.Count);
		CollectionAssert.AreEquivalent(input, hs.ToArray());
	}

	[TestMethod()]
	public void AddEnumeratorTest()
	{
		LazyMutableHashSet<IEnumerator> hs = [];
		List<IEnumerator> data =
		[
			Data(), Data()
		];

		static IEnumerator Data()
		{
			yield return 1;
			yield return 2;
			yield return 3;
		}
		hs.AddRange(data);
		Assert.AreEqual(data.Count, hs.Count);
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
		LazyMutableHashSet<int> hs = [.. input];

		int count = 0;
		foreach (var i in hs)
		{
			count++;
			hs.Add(i + 1); //delayed add
		}
		var expected = input.Concat(input.Select(i => i + 1)).ToArray();
		Assert.AreEqual(count, input.Length);
		CollectionAssert.AreEquivalent(expected, hs.ToArray());
	}

	[TestMethod()]
	public void GetEnumeratorTestAddAndRemove()
	{
		var input = new int[] { 5, 1, 3, 55 };
		LazyMutableHashSet<int> hs = [.. input];

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
		LazyMutableHashSet<int> hs = [.. input];

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
		LazyMutableHashSet<int> hs = [.. input];

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
		LazyMutableHashSet<int> hs = [.. input];

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