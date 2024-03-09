using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PSL.GRB.SyncApp
{
    public partial class frmSync : Form
    {
        public frmSync()
        {
            InitializeComponent();
        }

        GRBSyncAdapter service = null;
        private void btnStart_Click(object sender, EventArgs e)
        {
            if(btnStart.Text == "Start")
            {
                lblStart.Text = "Sync Service Started on " + DateTime.Now.ToString();
                btnStart.Text = "Stop";
                service = new GRBSyncAdapter(this);
                service.start();
                while (true)
                {
                    System.Threading.Thread.Sleep(100);

                    Application.DoEvents();
                }
                
            }
            else
            {
               if(service != null)
                {
                    service.stop();
                }
                btnStart.Text = "Start";
            }
                    

            
        }

        public delegate void UpdateTextDel(string message);
        public void SetText(string message)
        {
            try
            {
                this.Invoke(new UpdateTextDel(UpdateText), message);
            }
            catch (Exception ex)
            {
                int j = 50;
            }

        }

        public void UpdateText(string message)
        {
            try
            {
                lblUpdate.Text = message;
            }
            catch (Exception ex)
            {
                int j = 50;

            }
        }
    }
}
