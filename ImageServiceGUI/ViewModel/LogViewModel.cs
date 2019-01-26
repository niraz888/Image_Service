using ImageService.Logging.Modal;
using ImageServiceGUI.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ViewModel;

namespace ImageServiceGUI.ViewModel
{
    class LogViewModel : VModel
    {
        private ILogModel LogM;

        public LogViewModel()
        {



            LogM = new LogModel();
            this.LogM.PropertyChanged += delegate (object sender, PropertyChangedEventArgs e)
            {
                NotifyPropertyChanged(e.PropertyName);
            };
            
            
           
        }
        
        public ObservableCollection<MessageRecievedEventArgs> Logs
        {
            get
            {
                return this.LogM.Logs;
            }

            set
            {
                this.LogM.Logs = value;
                NotifyPropertyChanged("Logs");
            }
        }
    }
}
