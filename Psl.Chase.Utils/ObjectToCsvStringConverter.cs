using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Psl.Chase.Utils
{
    /// <summary>
    /// Utility which coverts collection object to Csv string including header
    /// </summary>
    public class ObjectToCsvStringConverter
    {
        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectToCsvStringConverter"/> class.
        /// </summary>
        /// <param name="sepepator">The sepepator.</param>
        public ObjectToCsvStringConverter(char sepepator)
        {
            _sepepator = sepepator;
        }
        #endregion

        #region Properties/Fields
        private char _sepepator = ',';
        #endregion

        #region Public Methods
        /// <summary>
        /// Collections to CSV.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <returns></returns>
        public string CollectionToCsv(System.Collections.IList collection)
        {
            if (collection == null)
            {
                throw new ArgumentNullException("Collection can not be null.");
            }

            StringBuilder sb = new StringBuilder();
            for (int index = 0; index < collection.Count; index++)
            {
                object item = collection[index];
                if (index == 0)
                {
                    sb.Append(ObjectToCsvHeader(item));
                }
                sb.Append(ObjectToCsvData(item));
                sb.Append(Environment.NewLine);
            }
            return sb.ToString();
        }

        /// <summary>
        /// Objects to CSV data.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">The obj.</param>
        /// <returns></returns>
        public string ObjectToCsvData(object obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("obj", "Value can not be null or Nothing!");
            }

            StringBuilder sb = new StringBuilder();
            Type t = obj.GetType();
            PropertyInfo[] pi = t.GetProperties();

            for (int index = 0; index < pi.Length; index++)
            {
                sb.Append(pi[index].GetValue(obj, null));

                if (index < pi.Length - 1)
                {
                    sb.Append(_sepepator.ToString());
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Objects to CSV header.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">The obj.</param>
        /// <returns></returns>
        public string ObjectToCsvHeader(object obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("obj", "Value can not be null or Nothing!");
            }

            StringBuilder sb = new StringBuilder();
            Type t = obj.GetType();
            PropertyInfo[] pi = t.GetProperties();

            for (int index = 0; index < pi.Length; index++)
            {
                PropertyInfo propertyInfo = pi[index];

                string name = string.Empty;
                object[] attributes = propertyInfo.GetCustomAttributes(true);
                if (attributes != null &&
                    attributes.Length > 0)
                {
                    PropertyNameAttribute attribute = attributes.GetValue(0) as PropertyNameAttribute;
                    if (attribute != null)
                        name = attribute.Name;
                }

                if (string.IsNullOrEmpty(name))
                {
                    name = propertyInfo.Name;
                }

                sb.Append(name);

                if (index < pi.Length - 1)
                {
                    sb.Append(_sepepator.ToString());
                }
            }
            sb.Append(Environment.NewLine);

            return sb.ToString();
        }
        #endregion
    }
}
