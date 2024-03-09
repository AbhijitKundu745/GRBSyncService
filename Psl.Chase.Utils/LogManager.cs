using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Psl.Chase.Utils
{
    public class LogManager
    {
        #region Properties/Fields
        static Psl.Chase.Utils.ILogger _logger = null;
        public static Psl.Chase.Utils.ILogger Logger
        {
            get
            {
                try
                {
                    if(_logger == null)
                        _logger = AppDomain.CurrentDomain.GetData("Logger") as Psl.Chase.Utils.ILogger;
                }                               
                catch
                {
                    _logger = new Psl.Chase.Utils.NullLogger();
                }
                if (_logger == null)
                    _logger = new Psl.Chase.Utils.NullLogger();
                return _logger;
            }
        }

        static Dictionary<string, Psl.Chase.Utils.ILogger> _loggerMappings = new Dictionary<string,ILogger>();
        #endregion

        #region Public Methods
        /// <summary>
        /// Initializes the specified logger config path.
        /// </summary>
        /// <param name="loggerConfigPath">The logger config path.</param>
        public static void Initialize(string loggerConfigPath)
        {
            log4net.Config.XmlConfigurator.Configure(new System.IO.FileInfo(loggerConfigPath));
        }

        /// <summary>
        /// Gets the logger.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public static ILogger GetLogger(string name)
        {
            ILogger retValue = null;
            if (!_loggerMappings.ContainsKey(name))
            {
                retValue = new TextLogger(name);
                _loggerMappings.Add(name, retValue);
            }
            else
            {
                retValue = _loggerMappings[name];
            }
            return retValue;
        }
        #endregion
    }
}
