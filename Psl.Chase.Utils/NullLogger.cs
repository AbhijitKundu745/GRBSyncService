using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Psl.Chase.Utils
{
    public class NullLogger : ILogger
    {
        #region Properties/Fields
        private string _name = string.Empty;
        public string Name { get { return _name; } set { _name = value; } }
        #endregion

        #region ILogger Members

        /// <summary>
        /// Logs the specified text.
        /// </summary>
        /// <param name="text">The text.</param>
        public void Log(string text)
        {
            return;
        }

        /// <summary>
        /// Logs the error.
        /// </summary>
        /// <param name="text">The text.</param>
        public void LogError(string text)
        {
            return;
        }

        /// <summary>
        /// Logs the info.
        /// </summary>
        /// <param name="text">The text.</param>
        public void LogInfo(string text)
        {
            return;
        }

        #endregion
    }
}
