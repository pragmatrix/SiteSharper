using System;
using System.Collections.Generic;

namespace Toolbox
{
	/**
		A two element tuple where the elements have the same type.
	**/

	public struct Two<ElementT> : IComparable<Two<ElementT>>
	{
		public readonly ElementT First;
		public readonly ElementT Second;

		private static readonly IComparer<ElementT> ElementComparer = Comparer<ElementT>.Default;
		
		public Two(ElementT first, ElementT second)
		{
			First = first;
			Second = second;
		}

		public int CompareTo(Two<ElementT> other)
		{
			var first = ElementComparer.Compare(First, other.First);
			return first != 0 ? first : ElementComparer.Compare(Second, other.Second);
		}

		public override bool Equals(object obj)
		{
			return obj is Two<ElementT> && (Two<ElementT>)obj == this;
		}

		public override int GetHashCode()
		{
			int r = 0;

			if (First != null)
				r ^= First.GetHashCode();

			if (Second != null)
				r ^= Second.GetHashCode();

			return r;
		}

		public static bool operator ==(Two<ElementT> l, Two<ElementT> r)
		{
			return Equals(l.First, r.First) && Equals(l.Second, r.Second);
		}

		public static bool operator !=(Two<ElementT> l, Two<ElementT> r)
		{
			return !(l == r);
		}

		public ElementT[] ToArray()
		{
			return new[] { First, Second };
		}

		public Two<OtherT> map<OtherT>(Func<ElementT, OtherT> f)
		{
			return Two.make(
				f(First),
				f(Second)
				);
		}

		public Two<OtherT> map<OtherT>(Func<ElementT, OtherT> fFirst, Func<ElementT, OtherT> fSecond)
		{
			return Two.make(
				fFirst(First),
				fSecond(Second)
				);
		}

		public Two<CastedT> cast<CastedT>()
		{
			return Two.make(
				(CastedT)(object)First,
				(CastedT)(object)Second
				);
		}

		public Two<ResultT> combine<OtherT, ResultT>(Two<OtherT> other, Func<ElementT, OtherT, ResultT> f)
		{
			return Two.make(
				f(First, other.First),
				f(Second, other.Second));
		}
	}

	public static class Two
	{
		public static Two<FirstT> make<FirstT>(FirstT first, FirstT second)
		{
			return new Two<FirstT>(first, second);
		}

		public static T first<T>(this Two<T> t)
		{
			return t.First;
		}

		public static T second<T>(this Two<T> t)
		{
			return t.Second;
		}
	}
}
