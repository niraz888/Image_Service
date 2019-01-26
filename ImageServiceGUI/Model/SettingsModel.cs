using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Communication;
using Communication.Event;
using ImageService.Infrastructure.Enums;
using Infrastructure;
using System.Windows;

namespace Model {
    public class SettingsModel : ISettingsModel
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private string m_LogName;
        private string m_OutPutDir;
        private string m_SourceName;
        private string m_tumbNailSize;
        private IClient client;
        private string m_SelectedHandler;
        private ObservableCollection<string> handlersModel;
        public void NotifyPropertyChanged(string propname)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propname));
            }
        }

        /// <summary>
        /// constructor.
        /// create list of handlers in OCollection, get instance of client,
        /// sign to event of commandRecieved and ask for config from server.
        /// </summary>
        public SettingsModel()
        {
            handlers = new ObservableCollection<string>();
            this.client = GuiClient.instanceS;
            string[] args = new string[5];
            client.CommandRecived += OnCommandRecived;
            MsgCommand cmd = new MsgCommand((int)CommandEnum.GetConfigCommand, args);
            //get config information
            this.client.SendAndRecived(cmd);
            if(!this.client.IsConnected())
            {
                //no connection
                this.defaultConfig();
            }
            
        }

        /// <summary>
        /// OnCommandReceived.
        /// the function that is called when a command is recieved.
        /// check if is is from type ConfigCommand. if it is -> than get
        /// the config from server.
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="msg">command</param>
        public void OnCommandRecived(object sender, MsgCommand msg) {
            //add information
            if ((int)msg.commandID == (int)CommandEnum.GetConfigCommand)
            {
                OutPutDir = msg.args[0];
                SourceName = msg.args[1];
                LogName = msg.args[2];
                TumbNail = msg.args[3];
                string[] h = msg.args[4].Split(';');
                foreach (string handler in h)
                {
                    if (!handlers.Contains(handler))
                    {
                        handlers.Add(handler);
                    }
                    
                }
                
            }
            else
            {
                //removes the give handler
                if((int)msg.commandID == (int)CommandEnum.RemoveHandlerCommand) {
                    this.removeHandler(msg);
                }
            }    
        }
        /// <summary>
        /// propert of LogName.
        /// </summary>
        public string LogName
        {
            get
            {
                return this.m_LogName;
            }

            set
            {
                this.m_LogName = value;
                NotifyPropertyChanged("LogName");
            }
        }
        /// <summary>
        /// property of outputdir.
        /// </summary>
        public string OutPutDir
        {
            get
            {
                return this.m_OutPutDir;
            }

            set
            {
                this.m_OutPutDir = value;
                NotifyPropertyChanged("OutPutDir");
            }
        }
        /// <summary>
        /// property of SourceName.
        /// </summary>
        public string SourceName
        {
            get
            {
                return this.m_SourceName;
            }

            set
            {
                this.m_SourceName = value;
                NotifyPropertyChanged("SourceName");
            }
        }

        /// <summary>
        /// property of TumbNail.
        /// </summary>
        public string TumbNail
        {
            get
            {
                return this.m_tumbNailSize;
            }

            set
            {
                this.m_tumbNailSize = value;
                NotifyPropertyChanged("TumbNail");
            }
        }
        /// <summary>
        /// property of Handlers.
        /// </summary>

        public ObservableCollection<string> handlers
        {
            get
            {
                return this.handlersModel;
            }
            set
            {
                this.handlersModel = value;
                NotifyPropertyChanged("handlers");
            }
        }
        /// <summary>
        /// property of SelectEDHandler.
        /// </summary>
        public string SelectedHandler
        {
            get
            {
                return this.m_SelectedHandler;
            }
            set
            {
                this.m_SelectedHandler = value;
                NotifyPropertyChanged("SelectedHandler");
            }

        }

        /// <summary>
        /// sendCommand.
        /// send command to the server
        /// </summary>
        /// <param name="msg"></param>
        public void sendCommand(MsgCommand msg)
        {
            this.client.Write(msg);
        }

        /// <summary>
        /// remove handler from list
        /// </summary>
        /// <param name="msg"> msg</param>

        public void removeHandler(MsgCommand msg)
        {
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                string handler = msg.args[0];
                if (handlers.Contains(handler))
                {
                    handlers.Remove(handler);

                }
            }));
            
        }

        /// <summary>
        /// defaultConfig, in case there is no connection.
        /// </summary>
        private void defaultConfig()
        {
            this.OutPutDir = "NO connection";
            this.SourceName = "NO connection";
            this.LogName = "NO connection";
            this.TumbNail = "NO connection";
           
    }

    }
}
