using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Psl.Chase.Utils
{
    public interface IPSLDictionary<TKey, TValue>
    {

        #region Properties

        /// <summary>
        /// Gets the keys.
        /// </summary>
        /// <value>The keys.</value>
        TKey[] Keys { get; }

        /// <summary>
        /// Gets the values.
        /// </summary>
        /// <value>The values.</value>
        TValue[] Values { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Adds the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        void Add(TKey key, TValue value);

        /// <summary>
        /// Removes the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        void Remove(TKey key);

        /// <summary>
        /// Clears this instance.
        /// </summary>
        void Clear();

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        TValue GetValue(TKey key);

        /// <summary>
        /// Sets the value.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        void SetValue(TKey key, TValue value);

        /// <summary>
        /// Gets the key.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        TKey GetKey(TValue value);

        /// <summary>
        /// Gets the keys.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        TKey[] GetKeys(TValue value);

        /// <summary>
        /// Determines whether the specified key contains key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>
        /// 	<c>true</c> if the specified key contains key; otherwise, <c>false</c>.
        /// </returns>
        bool ContainsKey(TKey key);

        /// <summary>
        /// Gets the sorted values.
        /// </summary>
        /// <param name="comparer">The comparer.</param>
        /// <returns></returns>
        TValue[] GetSortedValues(IComparer<TValue> comparer);

        /// <summary>
        /// Gets the sorted values.
        /// </summary>
        /// <returns></returns>
        TValue[] GetSortedValues();

        /// <summary>
        /// Gets the sorted keys.
        /// </summary>
        /// <param name="comparer">The comparer.</param>
        /// <returns></returns>
        TKey[] GetSortedKeys(IComparer<TKey> comparer);

        /// <summary>
        /// Gets the sorted keys.
        /// </summary>
        /// <returns></returns>
        TKey[] GetSortedKeys();

        #endregion

    }
}
