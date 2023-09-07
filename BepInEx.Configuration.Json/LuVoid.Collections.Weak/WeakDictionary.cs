using System;
using System.Collections.Generic;

namespace LuVoid.Collections.Weak
{
	public class WeakDictionary<TKey, TValue> : Dictionary<WeakReference<TKey>, TValue> //, IDictionary<TKey, TValue>
		where TKey : class
	{

		public WeakDictionary() : this(0) { }

		public WeakDictionary(int capacity) : base(capacity, WeakReferenceComparer<TKey>.Instance) { }

		public WeakDictionary(IDictionary<TKey, TValue> dictionary)
			: this(dictionary?.Count ?? 0)
		{
			if (dictionary == null) throw new ArgumentNullException("dictionary");

			foreach (KeyValuePair<TKey, TValue> item in dictionary)
			{
				Add(new WeakReference<TKey>(item.Key), item.Value);
			}
		}

		public TValue this[TKey key] { get => this[new WeakReference<TKey>(key)]; set => this[new WeakReference<TKey>(key)] = value; }

		//bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly => ((IDictionary<WeakReference<TKey>, TValue>)this).IsReadOnly;

		//ICollection<TValue> IDictionary<TKey, TValue>.Values => base.Values;

		public void Add(TKey key, TValue value)
		{
			Add(new WeakReference<TKey>(key), value);
		}

		//void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item)
		//{
		//	Add(item.Key, item.Value);
		//}

		public bool Contains(KeyValuePair<TKey, TValue> item)
		{
			ICollection<KeyValuePair<WeakReference<TKey>, TValue>> collection = this;
			WeakReference<TKey> weakReference = new WeakReference<TKey>(item.Key);
			KeyValuePair<WeakReference<TKey>, TValue> pair = new KeyValuePair<WeakReference<TKey>, TValue>(weakReference, item.Value);
			return collection.Contains(pair);
		}

		bool ContainsKey(TKey key)
		{
			return ContainsKey(new WeakReference<TKey>(key));
		}

		public bool Remove(TKey key)
		{
			return Remove(new WeakReference<TKey>(key));
		}

		//bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
		//{
		//	ICollection<KeyValuePair<WeakReference<TKey>, TValue>> collection = this;
		//	WeakReference<TKey> weakReference = new WeakReference<TKey>(item.Key);
		//	KeyValuePair<WeakReference<TKey>, TValue> pair = new KeyValuePair<WeakReference<TKey>, TValue>(weakReference, item.Value);
		//	return collection.Remove(pair);
		//}

		public bool TryGetValue(TKey key, out TValue value)
		{
			return TryGetValue(new WeakReference<TKey>(key), out value);
		}
	}
}
