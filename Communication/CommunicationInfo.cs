using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Communication
{
    
    public class CommunicationInfo
    {
        public string IPNumber { get; private set; }
        public int port { get; private set; }

        /// <summary>
        /// create CommunicationInfo object.
        /// </summary>
        public CommunicationInfo()
        {
            try
            {
                this.IPNumber = ConfigurationManager.AppSettings.Get("IP");
                string temp = ConfigurationManager.AppSettings.Get("Port");
                this.port = Int32.Parse(temp);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                this.IPNumber = "127.0.0.1";
                this.port = 9020;
            }
        }
    }
}
