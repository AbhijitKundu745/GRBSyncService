using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Psl.Chase.Utils
{
    /// <summary>
    /// 
    /// </summary>
    public interface ILogger
    {
        #region Properties/Fields
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        string Name { get; }
        #endregion

        #region Public Methods
        /// <summary>
        /// Logs the specified text.
        /// </summary>
        /// <param name="text">The text.</param>
        void Log(string text);

        /// <summary>
        /// Logs the error.
        /// </summary>
        /// <param name="text">The text.</param>
        void LogError(string text);

        /// <summary>
        /// Logs the info.
        /// </summary>
        /// <param name="text">The text.</param>
        void LogInfo(string text);
        #endregion
    }
}
