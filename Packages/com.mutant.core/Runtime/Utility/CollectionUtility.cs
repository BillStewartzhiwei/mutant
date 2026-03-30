using System.Collections.Generic;

namespace Bill.Mutant.Core
{
    public static class CollectionUtility
    {
        public static bool IsNullOrEmpty<T>(ICollection<T> collection)
        {
            return collection == null || collection.Count == 0;
        }

        public static int SafeCount<T>(ICollection<T> collection)
        {
            return collection?.Count ?? 0;
        }

        public static bool TryGetValue<TKey, TValue>(IReadOnlyDictionary<TKey, TValue> dictionary, TKey key, out TValue value)
        {
            if (dictionary == null)
            {
                value = default;
                return false;
            }

            return dictionary.TryGetValue(key, out value);
        }
    }
}
