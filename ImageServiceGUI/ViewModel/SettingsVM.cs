
using Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Prism.Commands;
using Communication.Event;
using ImageService.Infrastructure.Enums;
using System.Windows.Input;
using Infrastructure;



namespace ViewModel
{
    public class SettingsVM : VModel
    {
        private ISettingsModel  model;
       

        public SettingsVM()
        {
            this.model = new SettingsModel();
            model.PropertyChanged += delegate (object sender, PropertyChangedEventArgs p)
            {
                NotifyPropertyChanged(p.PropertyName);
            };
            this.Remove = new DelegateCommand<object>(this.onRemove, this.canBeRemoved);
            
        }

        public string OutPutDir
        {
            get {
                return model.OutPutDir;
            }
            set
            {
                model.OutPutDir = value;
                NotifyPropertyChanged("OutPutDir");
            }
        }

        public string SourceName
        {
            get {
                return model.SourceName;
            }
            set
            {
                model.SourceName = value;
                NotifyPropertyChanged("SourceName");
            }
        }

        public string LogName
        {
            get {
                return model.LogName;
            }
            set
            {
                model.LogName = value;
                NotifyPropertyChanged("LogName");
            }
        }

        public string TumbNail
        {
            get {
                return model.TumbNail;
            }
            set
            {
                model.TumbNail = value;
                NotifyPropertyChanged("TumbNail");
            }
        }

        public ObservableCollection<string> handlers
        {
            get
            {
                return model.handlers;
            }
            set
            {
                model.handlers = value;
                NotifyPropertyChanged("handlers");
            }
        }

        public string SelectedHandler
        {
            get
            {
                return this.model.SelectedHandler;
            }
            set
            {
                this.model.SelectedHandler = value;
                var command = this.Remove as DelegateCommand<object>;
                command.RaiseCanExecuteChanged();
                NotifyPropertyChanged("selectedHandler");
            }
        }
        public ICommand Remove { get; private set; }


        /// <summary>
        /// check if can be removed
        /// </summary>
        private bool canBeRemoved(object obj)
        {
            if (this.model.SelectedHandler != null)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// remove handler
        /// </summary>
        private void onRemove(object obj)
        {
            string[] arr = { this.SelectedHandler };
            this.model.sendCommand(new MsgCommand((int)CommandEnum.RemoveHandlerCommand, arr
                ));
        }

    }
}