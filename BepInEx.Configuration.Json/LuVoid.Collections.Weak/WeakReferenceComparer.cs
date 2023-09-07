using System.Collections.Generic;

namespace LuVoid.Collections.Weak
{
	public sealed class WeakReferenceComparer<T> : EqualityComparer<WeakReference<T>>
		where T : class
	{
		public static readonly WeakReferenceComparer<T> Instance = new WeakReferenceComparer<T>();

		public override bool Equals(WeakReference<T> x, WeakReference<T> y)
		{
			if (x.IsAlive && y.IsAlive)
			{
				return ReferenceEquals(x.Target, y.Target);
			}

			return x.IsAlive == y.IsAlive;
		}

		public override int GetHashCode(WeakReference<T> obj)
		{
			return obj.IsAlive ? obj.Target.GetHashCode() : 0;
		}
	}
}
