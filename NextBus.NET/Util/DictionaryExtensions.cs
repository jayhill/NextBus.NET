using System.Collections.Generic;
using System.Diagnostics;

namespace NextBus.NET.Util
{
    public static class DictionaryExtensions
    {
      [DebuggerHidden]
      public static TValue NullSafeGet<TKey, TValue>(this IDictionary<TKey,TValue> dictionary, TKey key) where TValue : class
      {
        TValue output;
        return dictionary.TryGetValue(key, out output) ? output : null;
      }
    }
}