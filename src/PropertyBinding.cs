using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Zenseless.Patterns
{
	/// <summary>
	/// Class that implements binding to properties
	/// </summary>
	/// <typeparam name="DerivedType"></typeparam>
	public class PropertyBinding<DerivedType> : INotifyPropertyChanged
	{
		/// <summary>
		/// Creates an instance. Please use a subclass.
		/// </summary>
		public PropertyBinding()
		{
			PropertyChanged += ViewModel_PropertyChanged;
		}

		/// <summary>
		/// Occurs when a property value changes.
		/// </summary>
		public event PropertyChangedEventHandler? PropertyChanged;

		/// <summary>
		/// Register an Action to be called when the property, given by the name is changed.
		/// </summary>
		/// <param name="propertyName">Name of the property.</param>
		/// <param name="handler">Action to be called when the property changes</param>
		public void AddPropertyChangedHandler(string propertyName, Action handler)
		{
			GetHandlerList(propertyName).Add(new Handler(handler));
		}

		/// <summary>
		/// Register an Action to be called when the property, given by an expression, like <code>() => instance.Property</code> is changed.
		/// </summary>
		/// <typeparam name="PropertyType">Type of the Property</typeparam>
		/// <param name="propertyExpression">An expression, like <code>() => instance.Property</code></param>
		/// <param name="handler">Action to be called when the property changes</param>
		public void AddPropertyChangedHandler<PropertyType>(Expression<Func<PropertyType>> propertyExpression, Action<DerivedType> handler)
		{
			ExtractInstanceAndPropertyName(propertyExpression, out string propertyName, out DerivedType instance);
			GetHandlerList(propertyName).Add(new InstanceHandler(handler, instance));
		}

		/// <summary>
		/// Register an Action to be called when the property, given by an expression, like <code>() => instance.Property</code> is changed.
		/// </summary>
		/// <typeparam name="PropertyType">Type of the Property</typeparam>
		/// <param name="propertyExpression">An expression, like <code>() => instance.Property</code></param>
		/// <param name="handler">Action to be called when the property changes</param>
		public void AddPropertyChangedHandler<PropertyType>(Expression<Func<PropertyType>> propertyExpression, Action<DerivedType, PropertyType> handler)
		{
			ExtractInstanceAndPropertyName(propertyExpression, out string propertyName, out DerivedType instance);
			GetHandlerList(propertyName).Add(new OldValueHandler<PropertyType>(handler, instance));
		}

		/// <summary>
		/// Sets a property if the value has changed and raises the <seealso cref="PropertyChanged"/> event. 
		/// Should be called from inside the property setter.
		/// </summary>
		/// <typeparam name="ValueType">The type of the property.</typeparam>
		/// <param name="backendStore">The backing variable that contains the property value.</param>
		/// <param name="value">The new value of the property.</param>
		/// <param name="propertyName">Auto filled name of the property.</param>
		protected void Set<ValueType>(ref ValueType backendStore, ValueType value, [CallerMemberName] string propertyName = "")
		{
			if (Equals(backendStore, value)) return;
			var oldValue = backendStore;
			backendStore = value;
			RaisePropertyChanged(oldValue, propertyName);
		}

		/// <summary>
		/// Raises the property changed event.
		/// Should be called from inside the property setter.
		/// </summary>
		/// <param name="propertyName">Name of the property. Will be filled automatically with the caller member name.</param>
		protected void RaisePropertyChanged([CallerMemberName] string propertyName = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

		/// <summary>
		/// Raises the property changed event.
		/// Should be called from inside the property setter.
		/// </summary>
		/// <param name="oldValue">The old value of the property.</param>
		/// <param name="propertyName">Name of the property. Will be filled automatically with the caller member name.</param>
		protected void RaisePropertyChanged<T>(T oldValue, [CallerMemberName] string propertyName = "") => PropertyChanged?.Invoke(this, new PropertyChangedOldValueEventArgs<T>(propertyName, oldValue));

		private interface IHandler
		{
			void Invoke(PropertyChangedEventArgs args);
		}

		private class Handler : IHandler
		{
			private readonly Action _handler;

			public Handler(Action handler) => _handler = handler;

			public void Invoke(PropertyChangedEventArgs _) => _handler();
		}

		private class InstanceHandler : IHandler
		{
			private readonly Action<DerivedType> _handler;
			private readonly DerivedType _instance;

			public InstanceHandler(Action<DerivedType> handler, DerivedType instance)
			{
				_handler = handler;
				_instance = instance;
			}

			public void Invoke(PropertyChangedEventArgs _) => _handler(_instance);
		}

		private class OldValueHandler<Type> : IHandler
		{
			private readonly Action<DerivedType, Type> _handler;
			private readonly DerivedType _instance;

			public OldValueHandler(Action<DerivedType, Type> handler, DerivedType instance)
			{
				_handler = handler;
				_instance = instance;
			}

			public void Invoke(PropertyChangedEventArgs args)
			{
				if (args is PropertyChangedOldValueEventArgs<Type> exArgs)
				{
					_handler(_instance, exArgs.OldValue);
				}
			}
		}

		private readonly Dictionary<string, List<IHandler>> handlerData = new();

		private List<IHandler> GetHandlerList(string propertyName)
		{
			if (!handlerData.TryGetValue(propertyName, out var handlerList))
			{
				handlerList = new List<IHandler>();
				handlerData[propertyName] = handlerList;
			}
			return handlerList;
		}

		private static object? Evaluate(Expression e)
		{
			switch (e.NodeType)
			{
				case ExpressionType.Constant:
					return (e as ConstantExpression)?.Value;
				case ExpressionType.MemberAccess:
					{
						if (e is not MemberExpression propertyExpression) return null;
						var field = propertyExpression.Member as FieldInfo;
						var property = propertyExpression.Member as PropertyInfo;
						var container = propertyExpression.Expression == null ? null : Evaluate(propertyExpression.Expression);
						if (field != null)
							return field.GetValue(container);
						else if (property != null)
							return property.GetValue(container, null);
						else
							return null;
					}
				default:
					return null;
			}
		}

		private static void ExtractInstanceAndPropertyName<TProp>(Expression<Func<TProp>> propertyExpression, out string propertyName, out DerivedType instance)
		{
			if (propertyExpression.Body is MemberExpression memberExpression)
			{
				if (memberExpression?.Member is PropertyInfo propertyInfo)
				{
					if (memberExpression.Expression is null) throw new ArgumentException("Invalid expression given");
					if (Evaluate(memberExpression.Expression) is DerivedType instanceObject)
					{
						propertyName = propertyInfo.Name;
						instance = instanceObject;
						return;
					}
				}
			}
			throw new InvalidOperationException("Please provide a valid property expression, like '() => instance.PropertyName'.");
		}

		private void ViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName is null) return;
			if (handlerData.TryGetValue(e.PropertyName, out var handlerList))
			{
				foreach (var handlerData in handlerList)
				{
					handlerData.Invoke(e);
				}
			}
		}
	}
}
