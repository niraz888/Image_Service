using ImageService.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure;

namespace ImageService.ServiceCommunication
{
    public class ImageTransferHandler :IClientHandler
    {

        private List<TcpClient> clientList;
        public static Mutex wMutex;
        private ILoggingService logging;
        string handler;

        /// <summary>
        /// create Image Transfer handler
        /// </summary>
        public ImageTransferHandler(List<TcpClient> c, ILoggingService logs, string h)
        {
            logging = logs;
            clientList = c;
            handler = h;
        }

        /// <summary>
        /// handle client until the client disconnect.
        /// </summary>
        public void HandleClient(TcpClient client)
        {
            new Task(() =>
            {
                while (client.Connected)
                {
                    try
                    {
                        NetworkStream stream = client.GetStream();
                        //get pic name
                        Byte[] name = new byte[6790];
                        //get image name
                        int nameLength = stream.Read(name, 0, name.Length);
                        string picName = Encoding.ASCII.GetString(name, 0, nameLength);

                        //send confirmation that the server got the image name.
                        byte[] confirm = { 1 };
                        //send that confirmation that we got the name
                        stream.Write(confirm, 0, 1);
                        List<byte> ImageByte = new List<byte>();
                        Byte[] temp;
                        int length = 0, i;
                        byte[] imgBytes = new byte[6790];
                        //get the image by chuncks of bytes.
                        do
                        {
                            length = stream.Read(imgBytes, 0, imgBytes.Length);
                            temp = new byte[length];
                            for (i = 0; i < length; i++)
                            {
                                temp[i] = imgBytes[i];
                                ImageByte.Add(temp[i]); 
                            }
                            Thread.Sleep(300);

                        } while (stream.DataAvailable);
                        //remove the image format
                        int index = picName.LastIndexOf(".");
                        picName = picName.Substring(0, index);
                        //add jpg to the image name
                        string path = this.handler + @"\" + picName + ".jpg";
                        //move image to handler folder to handle it.
                        File.WriteAllBytes(path, ImageByte.ToArray());
                    }
                    catch (Exception e)
                    {
                        //close client
                        Console.WriteLine(e.ToString());
                        logging.Log("error in client", Logging.Modal.MessageTypeEnum.FAIL);
                        if (clientList.Contains(client))
                        {
                            clientList.Remove(client);
                        }

                        client.Close();
                    }

                }
            }).Start();
        }

        /// <summary>
        /// send the messgae.
        /// </summary>
        public void notifyClient(TcpClient client, MsgCommand msg)
        {
            throw new NotImplementedException();
        }
    }
    
}
