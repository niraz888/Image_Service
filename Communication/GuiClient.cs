using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Configuration;
using System.Threading;

using Communication.Event;
using Infrastructure;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace Communication
{
    public class GuiClient : IClient
    {
        public event EventHandler<MsgCommand> CommandRecived;
        private System.Net.Sockets.TcpClient TClient;
        private int portNumber;
        private static Mutex rMutex = new Mutex();
        private static Mutex wMutex = new Mutex();
        private IPEndPoint ipEndPoint;
        private static GuiClient instance;

        /// <summary>
        /// create GuiClient singelton.
        /// </summary>
        public static GuiClient instanceS 
        {
            get {

                if (instance == null)
                {
                    instance = new GuiClient();
                }
                return instance;
            }
        }

        /// <summary>
        /// GuiClient Constructor.
        /// </summary>
        private GuiClient()
        {
            CommunicationInfo com = new CommunicationInfo();
            this.portNumber = com.port;
            this.ipEndPoint = new IPEndPoint(IPAddress.Parse(com.IPNumber), this.portNumber);
            this.Connect();

        }

        /// <summary>
        /// connect to the server.
        /// </summary>
        public void Connect()
        {
            //may be will be changed
            try 
            {
                  TClient = new System.Net.Sockets.TcpClient();
                  TClient.Connect(this.ipEndPoint);
                  Console.WriteLine("connect sucessfully");
             
            } catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }


        /// <summary>
        /// disconnect from server.
        /// </summary>
        public void Disconnect()
        {
            try
            {
                TClient.Close();
                Console.WriteLine("disconnect successfully");
            }catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }


        /// <summary>
        /// write message to the server.
        /// </summary>
        public void Write(MsgCommand msg)
        {
            Task t = new Task(() =>
            {
                try
                {
                    if (this.IsConnected())
                    {
                        NetworkStream stream = TClient.GetStream();
                        BinaryWriter writer = new BinaryWriter(stream);
                        wMutex.WaitOne();
                        //conver from message format to string.
                        string send = JsonConvert.SerializeObject(msg);
                        writer.Write(send);
                        wMutex.ReleaseMutex();

                    }
                }
                catch (Exception writeExp)
                {
                    Console.WriteLine(writeExp.ToString());
                }
            });
            t.Start();
            t.Wait();
        }


        /// <summary>
        /// read string from server.
        /// </summary>
        public void Read()
        {
            Task t = new Task(() =>
             {
                 try
                 {


                     if (this.IsConnected())
                     {
                         string buffer;
                         NetworkStream stream = TClient.GetStream();
                         BinaryReader reader = new BinaryReader(stream);
                         rMutex.WaitOne();
                         buffer = reader.ReadString();
                         rMutex.ReleaseMutex();
                         if (buffer != null)
                         {
                             //conver message from string to messag format
                             MsgCommand msg = JsonConvert.DeserializeObject<MsgCommand>(buffer);
                             //invoke the message.
                             CommandRecived?.Invoke(this, msg);
                         }
                     }
                 }
                 catch (Exception readExp)
                 {
                     Console.WriteLine(readExp.ToString());

                 }

             });
            t.Start();
            t.Wait();
            
        }


        /// <summary>
        /// check if client is connected.
        /// </summary>
        public bool IsConnected()
        {
            return this.TClient.Connected;
        }


        /// <summary>
        /// send a message to the server and wait to a respond.
        /// </summary>
        public void SendAndRecived(MsgCommand msg)
        {
            
            this.Write(msg);
            this.Read();
        }


        /// <summary>
        /// get new informatin from server while the client is connected.
        /// </summary>
        public void HandleRecived()
        {
            new Task(() =>
            {
                while (this.IsConnected())
                {
                    this.Read();
                }


            }).Start();
        }

    }
}