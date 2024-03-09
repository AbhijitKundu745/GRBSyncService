using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Psl.Chase.Utils
{
    public class TextLogger : ILogger
    {
        #region Constructor
        public TextLogger(string name)
        {
            _name = name;
            _logger = log4net.LogManager.GetLogger(name);
        }
        #endregion

        #region Properties/Fields
        private log4net.ILog _logger = null;
        #endregion

        #region ILogger Members

        private string _name = string.Empty;
        public string Name
        {
            get
            {
                return _name;
            }
        }

        public void Log(string text)
        {
            if(_logger != null)
                _logger.Debug(text);
        }

        public void LogError(string text)
        {
            if(_logger != null)
                _logger.Error(text);
        }

        public void LogInfo(string text)
        {
            if(_logger != null)
                _logger.Info(text);
        }

        #endregion
    }
}
