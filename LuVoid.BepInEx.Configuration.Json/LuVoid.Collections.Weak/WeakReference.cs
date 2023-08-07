using System;

namespace LuVoid.Collections.Weak
{
	public class WeakReference<T> : WeakReference, IEquatable<T>
		where T : class
	{
		public WeakReference(T target) : base(target) { }

		public WeakReference(T target, bool trackResurrection) : base(target, trackResurrection) { }

		public new T Target => base.Target as T;

		public bool Equals(T other)
		{
			return Target.Equals(other);
		}
	}
}
