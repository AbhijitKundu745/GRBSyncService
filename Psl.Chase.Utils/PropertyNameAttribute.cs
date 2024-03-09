using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Psl.Chase.Utils
{
    [AttributeUsage(AttributeTargets.Property)]
    public class PropertyNameAttribute : System.Attribute
    {
        #region Constructor
        public PropertyNameAttribute(string name)
            : base()
        {
            Name = name;
        }
        #endregion

        #region Properties/Fields
        public string Name { get; set; }
        #endregion
    }
}
