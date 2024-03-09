using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net.NetworkInformation;
using System.Diagnostics;

namespace Psl.Chase.Utils
{
    public class NetworkConnectivityChecker : IDisposable
    {
        #region Constructor
        public NetworkConnectivityChecker(string hostAddress, int interval, ConnectionStatus initialStatus)
        {
            HostAddress = hostAddress;
            Status = initialStatus;
            Interval = interval;
            _thread = new Thread(Run);
            _thread.IsBackground = true;
            _thread.Start();
        }
        #endregion

        #region Delegate
        public delegate void ConnectionStatusChangedEventHandler(object sender, ConnectionStatus status);
        #endregion

        #region Events
        public event ConnectionStatusChangedEventHandler ConnectionStatusChanged;
        #endregion

        #region Enum
        public enum ConnectionStatus
        {
            Connected,
            NotConnected
        }
        #endregion

        #region Private Methods
        private void Run()
        {
            while (true)
            {
                try
                {
                    using (Ping ping = new Ping())
                    {
                        PingReply reply = ping.Send(HostAddress);
                        if (reply.Status == IPStatus.Success)
                        {
                            if (Status != ConnectionStatus.Connected)
                            {
                                Status = ConnectionStatus.Connected;
                                try
                                {
                                    if (ConnectionStatusChanged != null)
                                    {
                                        ConnectionStatusChanged(this, Status);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Debug.WriteLine("Error ocurred while notifying connection status changed." + ex.ToString());
                                }
                            }
                        }
                        else
                        {
                            if (Status != ConnectionStatus.NotConnected)
                            {
                                Status = ConnectionStatus.NotConnected;
                                try
                                {
                                    if (ConnectionStatusChanged != null)
                                    {
                                        ConnectionStatusChanged(this, Status);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Debug.WriteLine("Error ocurred while notifying connection status changed." + ex.ToString());
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Error occurred while checking for connection status." + ex.ToString());
                }
                Thread.Sleep(Interval);
            }
        }
        #endregion

        #region Properties/Fields
        private ConnectionStatus _status = ConnectionStatus.NotConnected;
        public ConnectionStatus Status
        {
            get { return _status; }
            set { _status = value; }
        }

        private string _hostAddress = string.Empty;
        public string HostAddress
        {
            get { return _hostAddress; }
            set { _hostAddress = value; }
        }

        private int _interval = 1000;
        public int Interval
        {
            get { return _interval; }
            set { _interval = value; }
        }

        private Thread _thread = null;
        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            try
            {
                if (_thread != null)
                {
                    _thread.Abort();
                }
            }
            catch { }
            _thread = null;
        }

        #endregion
    }
}
