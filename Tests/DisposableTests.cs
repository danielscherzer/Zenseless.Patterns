using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;

namespace Zenseless.Patterns.Tests;

[TestClass()]
public class DisposableTests
{
	private class Test : Disposable
	{
		public Stream stream = new MemoryStream();
		public List<Stream> streams = [new MemoryStream(), new MemoryStream()];
		protected override void DisposeResources() => DisposeAllFields(this);
	}

	[TestMethod()]
	public void DisposeAllFieldsTest()
	{
		var result = new Test();
		result.Dispose();

		static void Provoke(Stream s)
		{
			int i = s.ReadByte();
		}
		Assert.ThrowsExactly<ObjectDisposedException>(() => Provoke(result.stream));
		Assert.ThrowsExactly<ObjectDisposedException>(() => Provoke(result.streams[0]));
		Assert.ThrowsExactly<ObjectDisposedException>(() => Provoke(result.streams[1]));
	}
}