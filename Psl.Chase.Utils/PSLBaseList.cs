using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Reflection;
using System.Collections;


namespace Psl.Chase.Utils
{
    public class PSLBaseList<T> : System.ComponentModel.BindingList<T>
    {
        #region Constructor
        public PSLBaseList(ISynchronizeInvoke invoke)
        {
            this.Invoke = invoke;
        }
        #endregion

        #region Properties/Fields
        ISynchronizeInvoke Invoke;

        private ListSortDescriptionCollection _SortDescriptions;

        private List<PropertyComparer<T>> comparers;

        public ListSortDescriptionCollection SortDescriptions
        {
            get { return _SortDescriptions; }
        }

        public bool SupportsAdvancedSorting
        {
            get { return true; }
        }
        #endregion

        #region Delagates
        delegate void ListChangedDelegate(ListChangedEventArgs e);

        public delegate T GetItemDelegate(int index);
        #endregion

        #region Public Methods
        public T GetItemByIndex(int index)
        {
            if (!Invoke.InvokeRequired)
                return this.Items[index];
            else
                return (T)Invoke.Invoke(new GetItemDelegate(GetItemByIndex), new object[] { index });
        }

        /// <summary>
        /// Finds the specified property.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <param name="key">The key.</param>
        /// <returns>If found returns index else -1. On any exception returns -2.</returns>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.Synchronized)]
        public int Find(string property, object key)
        {
            try
            {
                // Check the properties for a property with the specified name.
                PropertyDescriptorCollection properties =
                TypeDescriptor.GetProperties(typeof(T));
                PropertyDescriptor prop = properties.Find(property, true);
                int foundIndex = -1;
                // If there is not a match, return -1 otherwise pass search to
                // FindCore method.
                if (prop == null)
                    foundIndex = -1;
                else
                {
                    foundIndex = FindCore(prop, key);

                }
                return foundIndex;
            }
            catch
            {
                return -2;
            }
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.Synchronized)]
        public void ApplySort(ListSortDescriptionCollection sorts)
        {
            // Get list to sort
            // Note: this.Items is a non-sortable ICollection<T>
            List<T> items = this.Items as List<T>;

            // Apply and set the sort, if items to sort
            if (items != null)
            {
                _SortDescriptions = sorts;
                comparers = new List<PropertyComparer<T>>();
                foreach (ListSortDescription sort in sorts)
                    comparers.Add(new PropertyComparer<T>(sort.PropertyDescriptor,
                        sort.SortDirection));
                items.Sort(CompareValuesByProperties);
                //_isSorted = true;
            }
            else
            {
                //_isSorted = false;
            }

            this.OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
        }
        #endregion

        #region Overrides

        protected override bool SupportsChangeNotificationCore
        {
            get
            {
                return true;
            }
        }

        protected override void OnListChanged(ListChangedEventArgs e)
        {
            try
            {

                if (Invoke.InvokeRequired)
                {
                    Invoke.Invoke(new ListChangedDelegate(base.OnListChanged), new object[] { e });
                }
                else
                {
                    base.OnListChanged(e);

                }
            }
            catch (Exception ex)
            {
                //To Be Logged

            }
        }

        protected override bool SupportsSearchingCore
        {
            get { return true; }
        }

        //[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.Synchronized)]
        protected override void RemoveItem(int index)
        {
            if (this[index] != null)
                base.RemoveItem(index);
        }
        //private ArrayList selectedIndices = null;
        //private int[] returnIndices = null;

        protected override int FindCore(PropertyDescriptor prop, object key)
        {
            // Get the property info for the specified property. 
            PropertyInfo propInfo = typeof(T).GetProperty(prop.Name);
            T item;
            int found = -1;
            //selectedIndices = new ArrayList();
            if (key != null)
            {
                // Loop through the items to see if the key 
                // value matches the property value. 
                for (int i = 0; i < Count; ++i)
                {
                    item = (T)Items[i];
                    if (propInfo.GetValue(item, null).Equals(key))
                    {
                        //found = 0;
                        //selectedIndices.Add(i);
                        found = i;
                    }
                }
            }
            return found;
        }
        #endregion

        #region Private Methods
        private int CompareValuesByProperties(T x, T y)
        {
            if (x == null)
                return (y == null) ? 0 : -1;
            else
            {
                if (y == null)
                    return 1;
                else
                {
                    foreach (PropertyComparer<T> comparer in comparers)
                    {
                        int retval = comparer.Compare(x, y);
                        if (retval != 0)
                            return retval;
                    }
                    return 0;
                }
            }

        }


        #endregion
    }
}
