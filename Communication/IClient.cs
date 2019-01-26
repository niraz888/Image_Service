using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Communication.Event;
using Infrastructure;

namespace Communication
{
    public interface IClient
    {
        event EventHandler<MsgCommand> CommandRecived;
        void Connect();
        void Disconnect();
        void Write(MsgCommand msg);
        void Read();
        bool IsConnected();
        void HandleRecived();
        void SendAndRecived(MsgCommand msg);
    }
}
