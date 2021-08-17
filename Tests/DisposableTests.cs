using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;

namespace Zenseless.Patterns.Tests
{
	[TestClass()]
	public class DisposableTests
	{
		private class Test : Disposable
		{
			public Stream stream = new MemoryStream();
			public List<Stream> streams = new() { new MemoryStream(), new MemoryStream() };
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
			Assert.ThrowsException<ObjectDisposedException>(() => Provoke(result.stream));
			Assert.ThrowsException<ObjectDisposedException>(() => Provoke(result.streams[0]));
			Assert.ThrowsException<ObjectDisposedException>(() => Provoke(result.streams[1]));
		}
	}
}