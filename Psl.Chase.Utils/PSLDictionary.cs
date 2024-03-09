using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Psl.Chase.Utils
{
    public class PSLDictionary<TKey, TValue> : IPSLDictionary<TKey, TValue>
    {

        #region Constructor

        public PSLDictionary()
        {
            _pslDictionary = new Dictionary<TKey, TValue>();
        }

        #endregion

        #region Helper Methods


        #endregion

        #region Properties/Fields

        private Dictionary<TKey, TValue> _pslDictionary = null;

        #endregion

        #region IPSLDictionary<TKey,TValue> Members

        /// <summary>
        /// Adds the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.Synchronized)]
        public void Add(TKey key, TValue value)
        {
            if (!_pslDictionary.ContainsKey(key))
            {
                _pslDictionary.Add(key, value);
            }
        }

        /// <summary>
        /// Removes the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.Synchronized)]
        public void Remove(TKey key)
        {
            _pslDictionary.Remove(key);
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.Synchronized)]
        public TValue GetValue(TKey key)
        {
            try
            {
                TValue returnValue;
                _pslDictionary.TryGetValue(key, out returnValue);
                return returnValue;
            }
            catch (Exception)
            {
                return default(TValue);
            }
        }

        /// <summary>
        /// Sets the value.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.Synchronized)]
        public void SetValue(TKey key, TValue value)
        {
            if (_pslDictionary.ContainsKey(key))
            {
                _pslDictionary.Remove(key);
                _pslDictionary.Add(key, value);
            }
        }

        /// <summary>
        /// Gets the key.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.Synchronized)]
        public TKey GetKey(TValue value)
        {
            TKey retKey = default(TKey);
            foreach (System.Collections.Generic.KeyValuePair<TKey, TValue> keyValuePair in _pslDictionary)
            {
                if (keyValuePair.Value.Equals(value))
                {
                    retKey = keyValuePair.Key;
                    break;
                }
            }
            //TValue[] values = new TValue[_pslDictionary.Values.Count];
            //_pslDictionary.Values.CopyTo(values, 0);
            //int index = -1;
            //bool isFound = false;
            //foreach (TValue tValue in values)
            //{
            //    index++;
            //    if (tValue.Equals(value))
            //    {
            //        isFound = true;
            //        break;
            //    }
            //}
            //if (index != -1 && isFound)
            //{
            //    TKey[] keys = new TKey[_pslDictionary.Keys.Count];
            //    _pslDictionary.Keys.CopyTo(keys, 0);
            //    retKey = (TKey)keys.GetValue(index);
            //}
            return retKey;
        }

        /// <summary>
        /// Gets the key.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.Synchronized)]
        public TKey[] GetKeys(TValue value)
        {
            List<TKey> keys = new List<TKey>();
            foreach (System.Collections.Generic.KeyValuePair<TKey, TValue> keyValuePair in _pslDictionary)
            {
                if (keyValuePair.Value.Equals(value))
                    keys.Add(keyValuePair.Key);
            }

            TKey[] retKeys = new TKey[keys.Count];
            keys.CopyTo(retKeys, 0);
            return retKeys;
        }

        /// <summary>
        /// Determines whether the specified key contains key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>
        /// 	<c>true</c> if the specified key contains key; otherwise, <c>false</c>.
        /// </returns>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.Synchronized)]
        public bool ContainsKey(TKey key)
        {
            return _pslDictionary.ContainsKey(key);
        }

        /// <summary>
        /// Gets the keys.
        /// </summary>
        /// <value>The keys.</value>
        public TKey[] Keys
        {
            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.Synchronized)]
            get
            {
                TKey[] keys = new TKey[_pslDictionary.Keys.Count];
                _pslDictionary.Keys.CopyTo(keys, 0);
                return keys;
            }
        }

        /// <summary>
        /// Gets the values.
        /// </summary>
        /// <value>The values.</value>
        public TValue[] Values
        {
            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.Synchronized)]
            get
            {
                TValue[] values = new TValue[_pslDictionary.Values.Count];
                _pslDictionary.Values.CopyTo(values, 0);
                return values;
            }
        }

        /// <summary>
        /// Clears this instance.
        /// </summary>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.Synchronized)]
        public void Clear()
        {
            _pslDictionary.Clear();
        }

        /// <summary>
        /// Gets the sorted values.
        /// </summary>
        /// <param name="comparer">The comparer.</param>
        /// <returns></returns>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.Synchronized)]
        public TValue[] GetSortedValues(IComparer<TValue> comparer)
        {
            TValue[] sortedValues = new TValue[_pslDictionary.Values.Count];
            List<TValue> toBeSortedValues = new List<TValue>(_pslDictionary.Values);
            toBeSortedValues.Sort(comparer);
            toBeSortedValues.CopyTo(sortedValues, 0);
            return sortedValues;
        }

        /// <summary>
        /// Gets the sorted values.
        /// </summary>
        /// <returns></returns>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.Synchronized)]
        public TValue[] GetSortedValues()
        {
            TValue[] sortedValues = new TValue[_pslDictionary.Values.Count];
            List<TValue> toBeSortedValues = new List<TValue>(_pslDictionary.Values);
            toBeSortedValues.Sort();
            toBeSortedValues.CopyTo(sortedValues, 0);
            return sortedValues;
        }

        /// <summary>
        /// Gets the sorted keys.
        /// </summary>
        /// <param name="comparer">The comparer.</param>
        /// <returns></returns>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.Synchronized)]
        public TKey[] GetSortedKeys(IComparer<TKey> comparer)
        {
            TKey[] sortedKeys = new TKey[_pslDictionary.Keys.Count];
            List<TKey> toBeSortedKeys = new List<TKey>(_pslDictionary.Keys);
            toBeSortedKeys.Sort(comparer);
            toBeSortedKeys.CopyTo(sortedKeys, 0);
            return sortedKeys;
        }

        /// <summary>
        /// Gets the sorted keys.
        /// </summary>
        /// <returns></returns>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.Synchronized)]
        public TKey[] GetSortedKeys()
        {
            TKey[] sortedKeys = new TKey[_pslDictionary.Keys.Count];
            List<TKey> toBeSortedKeys = new List<TKey>(_pslDictionary.Keys);
            toBeSortedKeys.Sort();
            toBeSortedKeys.CopyTo(sortedKeys, 0);
            return sortedKeys;
        }

        #endregion

    }
}
