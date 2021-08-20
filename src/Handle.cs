namespace Zenseless.Patterns
{
	/// <summary>
	/// Implements a type safe <see cref="int"/> handle. A handle is an abstract reference to a resource that is used when application software references blocks of memory or objects that are managed by another system. <see href="https://en.wikipedia.org/wiki/Handle_%28computing%29">Wikipedia</see>
	/// </summary>
	/// <typeparam name="DataType"></typeparam>
	public struct Handle<DataType>
	{
		/// <summary>
		/// Create a new instance with the given id
		/// </summary>
		/// <param name="id">handle id</param>
		public Handle(int id)
		{
			Id = id;
		}

		/// <summary>
		/// The handle id
		/// </summary>
		public int Id { get; }

		/// <summary>
		/// Auto convert <see cref="Handle{DataType}"/> instance to the <see cref="int"/> handle.
		/// </summary>
		/// <param name="handle"></param>
		public static implicit operator int(Handle<DataType> handle) { return handle.Id; }

		/// <summary>
		/// Writes the Handle id to a string
		/// </summary>
		/// <returns>A string.</returns>
		public override string ToString() => $"{Id}";
	}
}
