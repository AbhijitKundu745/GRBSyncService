using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Psl.Chase.Utils
{
    public class SQLServerConnStrBuilder
    {
        #region Constructor
        public SQLServerConnStrBuilder(string connStr, bool isPasswordEncrypted)
        {
            try
            {
                string[] splittedConnStrings = connStr.Split(';');
                if (splittedConnStrings != null)
                {
                    foreach (string splittedConnString in splittedConnStrings)
                    {
                        if (splittedConnString.ToUpper().Contains(DATA_SOURCE.ToUpper()))
                        {
                            string[] dataSourceStrings = splittedConnString.Split('=');
                            if (dataSourceStrings != null &&
                                dataSourceStrings.Length > 1)
                            {
                                DataSource = (string)dataSourceStrings.GetValue(1);
                            }
                        }
                        else if (splittedConnString.ToUpper().Contains(INITIAL_CATALOG.ToUpper()))
                        {
                            string[] initialCatalogStrings = splittedConnString.Split('=');
                            if (initialCatalogStrings != null &&
                                initialCatalogStrings.Length > 1)
                            {
                                InitialCatalog = (string)initialCatalogStrings.GetValue(1);
                            }
                        }
                        else if (splittedConnString.ToUpper().Contains(USER_ID.ToUpper()))
                        {
                            string[] userIDStrings = splittedConnString.Split('=');
                            if (userIDStrings != null &&
                                userIDStrings.Length > 1)
                            {
                                UserId = (string)userIDStrings.GetValue(1);
                            }
                        }
                        else if (splittedConnString.ToUpper().Contains(PASSWORD.ToUpper()))
                        {
                            string[] passwordStrings = splittedConnString.Split('=');
                            if (passwordStrings != null &&
                                passwordStrings.Length > 1)
                            {
                                string password = (string)passwordStrings.GetValue(1);
                                if (passwordStrings.Length > 2)
                                {
                                    for (int i = 2; i < passwordStrings.Length; i++)
                                    {
                                        string passwordString = (string)passwordStrings.GetValue(i);
                                        if (passwordString == string.Empty)
                                        {
                                            password = password + "=";
                                        }
                                        else
                                        {
                                            password = password + passwordString;
                                        }
                                    }
                                }
                                if (isPasswordEncrypted)
                                {
                                    password = CryptorEngine.Decrypt(password, true);
                                }
                                Password = password;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Could not parse connection string. " + ex.ToString());
            }
        }
        #endregion

        #region Properties/Fields
        public string DataSource { get; set; }

        public string InitialCatalog { get; set; }

        public string UserId { get; set; }

        public string Password { get; set; }
        #endregion

        #region Constants
        const string DATA_SOURCE = "Data Source";
        const string INITIAL_CATALOG = "Initial Catalog";
        const string USER_ID = "User Id";
        const string PASSWORD = "Password";
        #endregion

        #region Overriden Methods
        public override string ToString()
        {
            return string.Format("Data Source={0};Initial Catalog={1};User Id={2};Password={3};", DataSource, InitialCatalog, UserId, Password);
        }
        #endregion
    }
}
