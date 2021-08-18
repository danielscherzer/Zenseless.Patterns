using System;

namespace Zenseless.Patterns
{
	/// <summary>
	/// A singleton service locator
	/// </summary>
	public sealed class ServiceLocator
	{
		private ServiceLocator() { }

		/// <summary>
		/// Register a service with a unique type. If the type was already registered it is overwritten.
		/// </summary>
		/// <typeparam name="TYPE">The unique type of the service</typeparam>
		/// <param name="service">A service instance</param>
		/// <exception cref="ArgumentNullException"/>
		public static void AddService<TYPE>(TYPE service) where TYPE : class
		{
			_registry.RegisterTypeInstance(service);
		}

		/// <summary>
		/// Returns the registered service instance of the given type.
		/// </summary>
		/// <typeparam name="TYPE">An unique type</typeparam>
		/// <returns>An instance of the given type or <c>null</c> if no such instance is registered</returns>
		public static TYPE? GetService<TYPE>() where TYPE : class
		{
			return _registry.GetInstance<TYPE>();
		}

		/// <summary>
		/// Returns the registered service instance of the given type.
		/// </summary>
		/// <typeparam name="Type"></typeparam>
		/// <returns>An instance of the given type or throws a <see cref="NotImplementedException"/> if no such instance is registered</returns>
		/// <exception cref="NotImplementedException"/>
		public static Type RequireService<Type>() where Type : class => GetService<Type>() ?? throw new NotImplementedException($"Service {typeof(Type)} not found.");

		private static readonly TypeRegistry _registry = new();
	}
}
