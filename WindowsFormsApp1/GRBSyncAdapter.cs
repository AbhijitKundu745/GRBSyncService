using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using log4net;
using System.Timers;
using System.IO;
using PSL.GRB.WMS.Service;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Threading;

namespace PSL.GRB.SyncApp
{
    public class GRBSyncAdapter
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private string WarehouseID = ConfigurationManager.AppSettings["WarehouseID"].ToString();
        private string ToDate = ConfigurationManager.AppSettings["ToDate"].ToString();
        private string FromtDate = ConfigurationManager.AppSettings["FromDate"].ToString();
        private string ErrorlogFile = ConfigurationManager.AppSettings["ErrorlogFile"].ToString();

        private System.Timers.Timer syncTimer = null;
        private string token = string.Empty;
        volatile bool isProcessing = false;
        public string centralDBConnString = string.Empty;
        private static Mutex _mutex = new Mutex(false, "Global\\WMSSync2");
        private DateTime tokenExpiration;
        CenteralServerIntraNetDAOImpl _centralDAO = null;

        public GRBSyncAdapter(frmSync mainForm)
        {
            _frmMain = mainForm;
        }

        frmSync _frmMain = null;
        private bool _suspend = false;
        public bool Suspend
        {
            get { return _suspend; }
            set { _suspend = value; }
        }


        private void LogException(string Errormessage)
        {
            //string logFilePath = "error.txt";
            string logMessage = $"\r\n\n[{DateTime.Now}] An exception occurred: {Errormessage}";

            // Write the exception message to a text file
            File.AppendAllText(ErrorlogFile, logMessage);

            Console.WriteLine($"Exception logged to {ErrorlogFile} file.");
        }

        public void start()
        {
            try
            {
                centralDBConnString = ConfigurationManager.ConnectionStrings["CentralDB"].ConnectionString;
                _centralDAO = new CenteralServerIntraNetDAOImpl(centralDBConnString);

                Psl.Chase.Utils.LogManager.Initialize(@".\LoggerConfig.xml");
                Psl.Chase.Utils.ILogger logger = new Psl.Chase.Utils.TextLogger("GRBWMS");
                AppDomain.CurrentDomain.SetData("Logger", logger);

                //#if DEBUG
                //                #region Testing Purpose


                //                if (ToDate == "0")
                //                {
                //                    ToDate = DateTime.Now.ToString("yyyy-MM-dd");
                //                }

                //                if (FromtDate == "0")
                //                    FromtDate = ToDate;

                //                List<WMSLogger> ret = _centralDAO.GetLoggers();

                //                token = GetAuthorizationToken().access_token;

                //                foreach (WMSLogger v in ret)
                //                {
                //                    //string retValue = GetWMSSyncData(v.URL,token);
                //                    ParseStringData(v.URL, v.ParserID);
                //                }

                //                #endregion
                //#else


                int _syntime_Sec = Convert.ToInt32(ConfigurationManager.AppSettings["SyncTimeInSec"].ToString());
                double syncTime = Convert.ToDouble(_syntime_Sec * 1000);
                syncTimer = new System.Timers.Timer();
                this.syncTimer.Interval = syncTime;
                this.syncTimer.Elapsed += new System.Timers.ElapsedEventHandler(this.syncTimer_Tick);
                syncTimer.Enabled = true;
                //#endif
            }
            catch (Exception ex)
            {

                Log.Error("start(): --" + ex);
                LogException("start(): --" + ex.ToString());
                throw;
            }


        }


        public void stop()
        {
            try
            {
                syncTimer.Enabled = false;
                //Library.WriteErrorLog("VGCB DB Sync Service Stop");
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                LogException("stop(): --" + ex.ToString());
            }
        }
        private void syncTimer_Tick(object sender, ElapsedEventArgs e)
        {
            try
            {
                
              Run();
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                LogException("syncTimer_Tick(): --" + ex.ToString());
            }
            finally
            {
                isProcessing = false;
                syncTimer.Enabled = true;
            }
        }

       async void Run()
        {
            try
            {
                if (Suspend)
                    return;

                Suspend = true;

                if(_frmMain != null)
                    _frmMain.SetText("Sync Running Time = " + DateTime.Now.ToString());


                List<WMSLogger> ret = _centralDAO.GetLoggers();
                // Check if the token is expired
                if (DateTime.Now >= tokenExpiration || token == string.Empty)
                {
                    // Token has expired, regenerate it
                    InitializeToken();
                }
               
                //token = GetAuthorizationToken().access_token;
          


                foreach (WMSLogger v in ret)
                {
                    //string retValue = GetWMSSyncData(v.URL,token);
                   ParseStringData(v.URL, v.ParserID);
                }

                Suspend = false;

            }
            catch (Exception ex)
            {
                Suspend = false;
                LogException("Run(): --" + ex.ToString());
                Psl.Chase.Utils.LogManager.Logger.LogError("WMS entry maker process exited." + ex.ToString());
                Log.Error(ex);
            }

        }

        public bool ParseStringData(string url, int parserID)
        {
            bool retValue = false;
            string urlReturnValue = string.Empty;
            try
            {

                if (ToDate == "0")
                {
                    ToDate = DateTime.Now.ToString("yyyy-MM-dd");
                }

                if (FromtDate == "0")
                    FromtDate = ToDate;


                /*if (parserID == 2)
                {
                    urlReturnValue = GetWMSSyncData(url, token);
                    Warehouses W = Newtonsoft.Json.JsonConvert.DeserializeObject<Warehouses>(urlReturnValue);
                    foreach (WHContent wH in W.content)
                    {
                        if (!_centralDAO.CheckifExists(wH.plantNumber, parserID))
                        {
                            _centralDAO.SaveWareHouseContent(wH);
                        }
                    }
                }
                else if (parserID == 3)
                {
                    urlReturnValue = GetWMSSyncData(url, token);
                    BayTypes BT = Newtonsoft.Json.JsonConvert.DeserializeObject<BayTypes>(urlReturnValue);
                    foreach (BayTypesContent BTC in BT.content)
                    {
                        if (!_centralDAO.CheckifExists(BTC.name, parserID))
                        {
                            _centralDAO.SaveBayTypesContent(BTC);
                        }
                    }
                }
                else if (parserID == 4)
                {
                    urlReturnValue = GetWMSSyncData(url, token);
                    Bays bays = Newtonsoft.Json.JsonConvert.DeserializeObject<Bays>(urlReturnValue);
                    foreach (BaysContent BC in bays.content)
                    {
                        if (!_centralDAO.CheckifExists(BC.bayName, parserID))
                        {
                            _centralDAO.SaveBayContent(BC);
                        }
                    }
                }
                else if (parserID == 5)
                {
                    urlReturnValue = GetWMSSyncData(url, token);
                    Bins bin = Newtonsoft.Json.JsonConvert.DeserializeObject<Bins>(urlReturnValue);
                    foreach (BinsContent BinC in bin.content)
                    {
                        if (!_centralDAO.CheckifExists(BinC.name, parserID))
                        {
                            if (_centralDAO.SaveAssetMaster(BinC, parserID))
                            {
                                _centralDAO.SaveBinsContent(BinC);
                            }
                        }
                    }
                }
                else if (parserID == 6)
                {
                    urlReturnValue = GetWMSSyncData(url, token);
                    Pallets pallets = Newtonsoft.Json.JsonConvert.DeserializeObject<Pallets>(urlReturnValue);
                    foreach (PalletsContent PC in pallets.content)
                    {
                        if (!_centralDAO.CheckifExists(PC.name, parserID))
                        {
                            if (_centralDAO.SaveAssetMaster(PC, parserID))
                            {
                                _centralDAO.SavePalletsContent(PC);
                            }
                        }
                    }
                }*/
                if (parserID == 7)
                {
                    string fDate = _centralDAO.GetSTOFromDate();

                    if (fDate != "")
                        FromtDate = fDate;

                    ToDate = DateTime.Now.ToString("yyyy-MM-dd");

                    url = string.Format(url, WarehouseID, FromtDate, ToDate);

                    Psl.Chase.Utils.LogManager.Logger.LogError("URL Call STO " + url);
                    
                    if (_mutex.WaitOne())
                    {
                        try
                        {
                            urlReturnValue = GetWMSSyncData(url, token);

                            Psl.Chase.Utils.LogManager.Logger.LogError("URL Return value STO =" + urlReturnValue);

                            ReceivingByTrucks W = Newtonsoft.Json.JsonConvert.DeserializeObject<ReceivingByTrucks>(urlReturnValue);
                            if (W != null && W.content.Count != 0)
                            {
                                
                                _centralDAO.InsetSTODetails(W);
                               
                            }
                        }
                        finally
                        {
                            // Always release the mutex
                            _mutex.ReleaseMutex();
                        }
                    }
                    else
                    {
                        // Mutex is already acquired by another instance
                        _frmMain.SetText("Another instance is already running.");
                       
                    }

                }
                else if (parserID == 8)
                {
                    string fDate = _centralDAO.GetSOFromDate();

                    if (fDate != "")
                        FromtDate = fDate;
                    //FromtDate = "2024-07-24";

                    ToDate = DateTime.Now.ToString("yyyy-MM-dd");
                    //ToDate = "2024-07-24";
                    if (_mutex.WaitOne())
                    {
                        try
                        {
                            url = string.Format(url, WarehouseID, FromtDate, ToDate);
                            //url = "https://192.168.100.18/wms/api/v1/dispatch?warehouseId=1&fromDateTime=2024-08-03T12:58:00.000Z&toDateTime=2024-08-03T12:59:59.000Z&embedItems=true";
                            urlReturnValue =GetWMSSyncData(url, token);

                            Psl.Chase.Utils.LogManager.Logger.LogError("URL Call SO " + url);

                            Dispatch W = Newtonsoft.Json.JsonConvert.DeserializeObject<Dispatch>(urlReturnValue);

                            Psl.Chase.Utils.LogManager.Logger.LogError("URL Return value SO =" + urlReturnValue);


                            if (W != null && W.content.Count != 0)
                            {
                              
                                _centralDAO.InsetSODetails(W);
                               
                            }
                           
                        }
                        finally
                        {
                            // Always release the mutex
                            _mutex.ReleaseMutex();
                        }
                    }
                    else
                    {
                        // Mutex is already acquired by another instance
                        _frmMain.SetText("Another instance is already running.");
                    }
                }

                return retValue;
            }
            catch (Exception Ex)
            {
                LogException("ParseStringData(): --" + Ex.ToString());
                Psl.Chase.Utils.LogManager.Logger.LogError("WMS Parser Function() ERROR.:" + Ex.ToString());
                Log.Error(Ex);
                retValue = false;
                return retValue;
            }
            finally
            {

            }
        }


        public Authorization GetAuthorizationToken()
        {
            Authorization authorization = new Authorization();
            try
            {
                string authorizationUrl = System.Configuration.ConfigurationManager.AppSettings["AuthorizationUrl"];
                string username = System.Configuration.ConfigurationManager.AppSettings["Username"];
                string password = System.Configuration.ConfigurationManager.AppSettings["Password"];

                System.Net.HttpWebRequest request = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(authorizationUrl);
                string jsonData = $"{{\r\n    \"username\": \"{username}\",\r\n    \"password\": \"{password}\"\r\n}}";


                request.Method = "POST";


                request.ContentType = "application/json";
                byte[] byteArray = Encoding.UTF8.GetBytes(jsonData);
                request.ContentLength = byteArray.Length;


                using (Stream dataStream = request.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                }


                System.Net.HttpWebResponse response = (System.Net.HttpWebResponse)request.GetResponse();


                using (Stream responseStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream);
                    string apiResponse = reader.ReadToEnd();
                    authorization = Newtonsoft.Json.JsonConvert.DeserializeObject<Authorization>(apiResponse);
                }


                response.Close();


            }
            catch (Exception ex)
            {
                LogException("GetAuthorizationToken(): --" + ex.ToString());
            }
            return authorization;
        }

        public string GetWMSSyncData(string URL, string Token)
        {
            string ReponseConvert = string.Empty;
            try
            {

                //var client = new HttpClient();
                using (var client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromMinutes(10);
                    client.DefaultRequestHeaders.ConnectionClose = true;
                    var request = new HttpRequestMessage(HttpMethod.Get, URL);
                    request.Headers.Add("Accept", "*/*");
                    request.Headers.Add("Authorization", "Bearer " + Token);
                    request.Headers.Add("X-TenantId", WarehouseID);
                    try
                    {
                        var response = client.SendAsync(request);
                        //response.EnsureSuccessStatusCode();
                        //ReponseConvert = await response.Content.ReadAsStringAsync();
                        Task<string> responseBody = response.Result.Content.ReadAsStringAsync();
                        ReponseConvert = responseBody.Result.ToString();
                    }
                    catch (TaskCanceledException ex) when (ex.CancellationToken.IsCancellationRequested == false)
                    {
                        // Handle timeout specifically
                        LogException("PutReceivingItemInfo(): Timeout occurred --" + ex.ToString());
                        throw;
                    }
                    catch (HttpRequestException ex)
                    {
                        // handle exception
                        LogException("GetWMSSyncData(): --" + ex.ToString());
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                LogException("GetWMSSyncData(): --" + ex.ToString());
            }
            
            return ReponseConvert;
        }
        private void InitializeToken()
        {
            token = GetAuthorizationToken().access_token;

            // Parse the expiration time from the response
            if (DateTime.TryParse(GetAuthorizationToken().expires_in, out DateTime expirationTime))
            {
                tokenExpiration = expirationTime; // Set the expiration time
            }
            else
            {
                // Handle parsing error (e.g., log it or throw an exception)
                LogException("Failed to parse expiration time from token response.");
                tokenExpiration = DateTime.Now.AddMinutes(300); // Fallback to a default value
            }
        }

    }

}
