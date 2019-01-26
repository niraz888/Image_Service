using Communication;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageServiceGUI.Model
{
    public class MainWindowModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private bool m_IsConnected;
        private IClient client;

        /// <summary>
        /// constructor.
        /// </summary>
        public MainWindowModel()
        {
            this.client = GuiClient.instanceS;
            this.IsConnected = client.IsConnected();
            this.client.HandleRecived();
            
        }

        /// <summary>
        /// notifyPropertyChanged.
        /// </summary>
        /// <param name="propname"></param>
        public void NotifyPropertyChanged(string propname)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propname));
            }
        }

        /// <summary>
        ///property to check ifConnected.
        /// </summary>
        public bool IsConnected
        {
            get
            {
                return this.m_IsConnected;
            }

            set
            {
                this.m_IsConnected = value;
                this.NotifyPropertyChanged("IsConnected");
            }
        }

        /// <summary>
        /// OnClose function, is called when an event of close client is raised
        /// </summary>
        public void OnClose()
        {
            this.client.Disconnect();
        }
        
         
    }
}
