//
//  Copyright (C) 2016 Fluendo S.A.
using System;
using VAS.Core.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace VAS.Core.Filters
{
	/// <summary>
	/// This is a simple predicate.
	/// Contains a settable function that receives a T object, and returns a boolean.
	/// If it is not set, it will return true.
	/// </summary>
	public class Predicate<T> : IPredicate <T>
	{
		#region IPredicate implementation

		public Func<T, bool> Filter {
			get;
			set;
		} = _ => true;

		#endregion
	}

	/// <summary>
	/// This is a composite predicate.
	/// This predicate applies the OR operation to all the predicates it contains.
	/// </summary>
	public class OrPredicate<T> : IPredicate<T>
	{
		public List<IPredicate<T>> Filters { get; set; } = new List<IPredicate<T>>();

		#region IPredicate implementation

		public Func<T, bool> Filter {
			get {
				return (evt) => Filters.Any (f => f.Filter (evt));
			}
		}

		#endregion
	}

	/// <summary>
	/// This is a composite predicate.
	/// This predicate applies the AND operation to all the predicates it contains.
	/// </summary>
	public class AndPredicate<T> : IPredicate<T>
	{
		public List<IPredicate<T>> Filters { get; set; } = new List<IPredicate<T>>();

		#region IPredicate implementation

		public Func<T, bool> Filter {
			get {
				return (evt) => Filters.All (f => f.Filter (evt));
			}
		}

		#endregion
	}
}

