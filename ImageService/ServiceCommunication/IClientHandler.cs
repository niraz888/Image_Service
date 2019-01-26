using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using Infrastructure;

namespace ImageService.ServiceCommunication
{
    public interface IClientHandler
    {
        void HandleClient(TcpClient client);
        void notifyClient(TcpClient client, MsgCommand msg);
    }
}
