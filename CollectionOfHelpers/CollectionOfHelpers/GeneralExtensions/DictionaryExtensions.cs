using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollectionOfHelpers.GeneralExtensions
{
    public static class DictionaryExtensions
    {
        /// <summary>
        /// Add the key-value pair to the dictionary and return true if the key doesn't already exist in the dictionary. Return false if the key already exists.
        /// Similar behaviour to Hashset.add() since no exception is thrown.
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="Dictionary"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool TryAdd<TKey, TValue>(this Dictionary<TKey, TValue> Dictionary, TKey key, TValue value)
        {
            if (!Dictionary.ContainsKey(key))
            {
                Dictionary.Add(key, value);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// If the key exists in the dictionary then update the value associated with the key. If the key doesn't exist then the key-value pari are added to the dictionary.
        /// NOTE: returns dictionary however the original dictionary is updated as well.
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dictionary"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Dictionary<TKey, TValue> AddOrUpdate<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, TValue value)
        {
            if (dictionary.ContainsKey(key))
            {
                dictionary[key] = value;
            }
            else
            {
                dictionary.Add(key, value);
            }

            return dictionary;
        }
    }
}
