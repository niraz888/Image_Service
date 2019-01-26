using Communication;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Communication.Event;
using Infrastructure;

public interface ISettingsModel : INotifyPropertyChanged
{
    string OutPutDir { get; set; }
    string SourceName { get; set; }
    string LogName { get; set; }
    string TumbNail { get; set; }
    string SelectedHandler { get; set; }
    ObservableCollection<string> handlers { get; set; }
    void sendCommand(MsgCommand msg);
}


