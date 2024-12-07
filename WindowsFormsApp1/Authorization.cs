using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSL.GRB.WMS.Service
{
    public class Authorization
    {
        public string access_token { get; set; }
        public string expires_in { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public string username { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public List<string> roles { get; set; }
    }
}
