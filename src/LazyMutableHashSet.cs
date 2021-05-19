using System.Collections;
using System.Collections.Generic;

namespace Zenseless.Patterns
{
	/// <summary>
	///  Uses <see cref="HashSet{T}"/> with O(1) add and removal to implement a lazy mutable iteration
	///  When iterating add and removes are buffered and applied after iteration.
	/// </summary>
	/// <typeparam name="DataType"></typeparam>
	public class LazyMutableHashSet<DataType> : IEnumerable<DataType>, ICollection<DataType>
	{
		/// <summary>
		/// Queue a new item to add to the hash set
		/// </summary>
		/// <param name="item">the item to add</param>
		public void Add(DataType item)
		{
			if (IsIterating)
			{
				toAdd.Add(item);
				toRemove.Remove(item);
			}
			else
			{
				items.Add(item);
			}
		}

		/// <summary>
		/// Queue a list of items to add to the hash set
		/// </summary>
		/// <param name="other">a lsit of items to add</param>
		public void Add(IEnumerable<DataType> other)
		{
			if (IsIterating)
			{
				toAdd.UnionWith(other);
				toRemove.ExceptWith(other);
			}
			else
			{
				items.UnionWith(other);
			}
		}

		/// <summary>
		/// Queue an item for removal from the hash set
		/// </summary>
		/// <param name="item">the item to remove</param>
		public void Remove(DataType item)
		{
			if (IsIterating)
			{
				toRemove.Add(item);
				toAdd.Remove(item);
			}
			else
			{
				items.Remove(item);
			}
		}

		/// <summary>
		/// Get an enumerator on the items of the hash set. After iteration is finished queued items for adding or removal are resolved
		/// </summary>
		/// <returns></returns>
		public IEnumerator<DataType> GetEnumerator()
		{
			++iterationCount; // could be called recursively, so a boolean is not enough (iterate while iterating)
			foreach (var item in items)
			{
				yield return item;
			}
			--iterationCount;
			if (!IsIterating)
			{
				items.ExceptWith(toRemove);
				toRemove.Clear();
				items.UnionWith(toAdd);
				toAdd.Clear();
			}
		}

		// Explicit for IEnumerable because weakly typed collections are Bad
		IEnumerator IEnumerable.GetEnumerator()
		{
			// uses the strongly typed IEnumerable<T> implementation
			return GetEnumerator();
		}

		/// <summary>
		/// Clears the hash set
		/// </summary>
		public void Clear()
		{

			items.Clear();
			toAdd.Clear();
			toAdd.Clear();
		}

		/// <summary>
		/// Determines if the given item is contained in the hash set.
		/// </summary>
		/// <param name="item">The item to query</param>
		/// <returns></returns>
		public bool Contains(DataType item) => items.Contains(item);

		/// <summary>
		/// Copies the elements of the hash set to an array, starting at the specified array index.
		/// </summary>
		/// <param name="array">The one-dimensional array that is the destination of the elements copied from the hash set. The array must have zero-based indexing.</param>
		/// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
		public void CopyTo(DataType[] array, int arrayIndex) => items.CopyTo(array, arrayIndex);

		bool ICollection<DataType>.Remove(DataType item)
		{
			var contains = items.Contains(item);
			Remove(item);
			return contains;
		}

		private int iterationCount = 0;
		private readonly HashSet<DataType> items = new(); // O(1) add and removal
		private readonly HashSet<DataType> toRemove = new();
		private readonly HashSet<DataType> toAdd = new();
		private bool IsIterating => 0 < iterationCount;

		/// <summary>
		/// Gets the number of elements that are contained in a set.
		/// </summary>
		public int Count => items.Count;

		bool ICollection<DataType>.IsReadOnly => false;
	}
}
