using System;
using Realtime.Messaging.Internal;

public static class ExtensionMethods {

	public static Func<T, TResult> Memoize<T, TResult>(this Func<T, TResult> f) {
		var cache = new ConcurrentDictionary<T, TResult>();
		return key => cache.GetOrAdd(key, f);
	}

}
