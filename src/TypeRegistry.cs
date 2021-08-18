using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Zenseless.Patterns
{
	/// <summary>
	/// Holds a dictionary of type instances that can be registered and requested.
	/// </summary>
	public class TypeRegistry
	{
		/// <summary>
		/// Returns <c>true</c> if the given type is registered.
		/// </summary>
		/// <typeparam name="TYPE">An unique type</typeparam>
		/// <returns></returns>
		public bool Contains<TYPE>() => _types.ContainsKey(typeof(TYPE));

		/// <summary>
		/// Register an instance of a unique type with this type registry. If the type was already registered it is overwritten.
		/// </summary>
		/// <typeparam name="TYPE">An unique type</typeparam>
		/// <param name="instance">An instance</param>
		/// <exception cref="ArgumentNullException"/>
		public void RegisterTypeInstance<TYPE>(TYPE instance) where TYPE : class
		{
			if (instance is null) throw new ArgumentNullException(nameof(instance));
			var type = typeof(TYPE);
			Debug.WriteLineIf(Contains<TYPE>(), $"Overwriting registered type instance {type}.");
			_types[type] = instance;
		}

		/// <summary>
		/// Unregisters the given type.
		/// </summary>
		/// <typeparam name="TYPE">An unique type</typeparam>
		/// <returns>Returns <c>true</c> if the type is successfully found and removed; otherwise, <c>false</c>.</returns>
		public bool UnregisterTypeInstance<TYPE>() where TYPE : class => _types.Remove(typeof(TYPE));

		/// <summary>
		/// Returns the registered instance of the given type.
		/// </summary>
		/// <typeparam name="TYPE">An unique type</typeparam>
		/// <returns>An instance of the given type or <c>null</c> if no such instance is registered</returns>
		public TYPE? GetInstance<TYPE>() where TYPE : class
		{
			var type = typeof(TYPE);
			if (_types.TryGetValue(type, out var instance))
			{
				return (TYPE)instance;
			}
			return null;
		}

		private readonly Dictionary<Type, object> _types = new();
	}
}
