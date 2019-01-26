using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageService.Logging;
using System.Net.Sockets;
using System.Net;

namespace ImageService.ServiceCommunication
{
    public class ImageTransferServer : IServiceServer
    {
        private ILoggingService log;
        private List<TcpClient> clients;
        private ImageTransferHandler handler;
        private TcpListener listener;

        /// <summary>
        /// create a Image Transfer server.
        /// </summary>
        public ImageTransferServer(ILoggingService l, string[] handlers)
        {
            this.log = l;
            clients = new List<TcpClient>();
            handler = new ImageTransferHandler(clients, log,handlers[0]);
        }

        /// <summary>
        /// start the server.listens to clients and handle them.
        /// </summary>
        public void Start()
        {
            try
            {
                IPEndPoint pnt = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 2500);
                listener = new TcpListener(pnt);
                listener.Start();
                // print of connections
                Task tsk = new Task(() =>
                {
                    while (true)
                    {
                        try
                        {
                            TcpClient client = listener.AcceptTcpClient();
                            log.Log("client connect", Logging.Modal.MessageTypeEnum.INFO);
                            // success in recieve
                            clients.Add(client);
                            handler.HandleClient(client);
                        }
                        catch (Exception e)
                        {
                            //end server.
                            log.Log("stop server", Logging.Modal.MessageTypeEnum.FAIL);
                            Console.WriteLine(e.ToString());
                            this.Stop();
                            break;
                        }

                    }
                });
                tsk.Start();
            }
            catch (Exception t)
            {
                Console.WriteLine(t.ToString());
            }
        }

        /// <summary>
        /// stop listeninig
        /// </summary>
        public void Stop()
        {
            listener.Stop();
            foreach(TcpClient client in clients)
            {
                if (client.Connected)
                {
                    client.Close();
                }
            }
        }
    }
}
