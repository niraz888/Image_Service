using System.Collections.ObjectModel;
using System.ComponentModel;
using Communication;
using ImageService.Logging.Modal;
using Infrastructure;
using ImageService.Infrastructure.Enums;
using Newtonsoft.Json;
using System;
using System.Windows;

namespace ImageServiceGUI.Model
{
    class LogModel : ILogModel
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private IClient client;
        private ObservableCollection<MessageRecievedEventArgs> m_Logs;

        /// <summary>
        /// Constructor.  init the client from singelton, create new OBservableList
        /// for logs, sign to event of commandRecieved.
        /// </summary>

        public LogModel()
        {
            this.client = GuiClient.instanceS;
            this.m_Logs = new ObservableCollection<MessageRecievedEventArgs>();
            this.client.CommandRecived += this.OnCommandRecieved;
            string[] args = new string[5];
            MsgCommand cmd = new MsgCommand((int)CommandEnum.LogCommand, args);
            //get the logs
            client.SendAndRecived(cmd);
            
        }

        /// <summary>
        /// property of logs, define list of logs so far.
        /// </summary>

        public ObservableCollection<MessageRecievedEventArgs> Logs
        {
            get
            {
                return this.m_Logs;
            }

            set
            {
                this.m_Logs = value;
                NotifyPropertyChanged("Logs");
            }
        }


        /// <summary>
        /// NotifyPropertyChanged.
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
        /// the function that is enable when the event of commandRecieved is
        /// raised. check between two commands - first if it is to add the whole
        /// logs that occured so far, or update a new log when the GUI is already running
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="m"></param>
        public void OnCommandRecieved(object sender, MsgCommand m)
        {
            //get the list of logs
            if ((int)m.commandID == (int)(CommandEnum.LogCommand))
            {
                ObservableCollection<string> collection = JsonConvert.
                    DeserializeObject<ObservableCollection<string>>(m.args[0]);

                //add logs
                foreach (string log in collection)
                {
                    string[] logInfo = log.Split(';');
                    this.m_Logs.Add(new MessageRecievedEventArgs(this.ConvertType(logInfo[1]), logInfo[0]));
                }
                
                
                
            } else
            {
                //log added after creation.
                if((int)m.commandID == (int)CommandEnum.AddLogCommand)
                {
                    Application.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        this.addNewLog(m);
                    }));
                }
            }

       
        }
        /// <summary>
        /// the function that update a new log
        /// </summary>
        /// <param name="msg">msg command</param>
        private void addNewLog(MsgCommand msg)
        {
            MessageRecievedEventArgs m = MessageRecievedEventArgs.FromJSON(msg.args[0]);
            this.m_Logs.Add(m);
        }

        /// <summary>
        /// convert type of {INFO,WARNING, FAIL} from string to MTE
        /// </summary>
        /// <param name="type">format of string</param>
        /// <returns>format of MessageTypeEnum</returns>
        private MessageTypeEnum ConvertType(string type)
        {
            if (type.Equals("INFO")) {
                return MessageTypeEnum.INFO;
            }
            if (type.Equals("FAIL")) {
                return MessageTypeEnum.FAIL;
            }
            else 
            {
                return MessageTypeEnum.WARNING;
            }
            

        }

    }
}
