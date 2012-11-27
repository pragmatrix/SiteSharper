using System.Collections.Generic;

namespace SiteSharper.Model
{
	public interface ISpecifiesReferenceClasses
	{
		List<string> ReferenceClasses { get; }
	}

	public static class HasReferenceClassesExtension
	{
		public static TypeT referenceClass<TypeT>(this TypeT _, string referenceClass)
			where TypeT : ISpecifiesReferenceClasses
		{
			_.ReferenceClasses.Add(referenceClass);
			return _;
		}
	}
}
