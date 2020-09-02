using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace PingPongServer
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                StartServer(args);
                Thread.Sleep(100000);
            }
        }
        public static void StartServer(string[] args)
        {

            int port = int.Parse(args[0]);
            IPHostEntry host = Dns.GetHostEntry("10.1.0.25");
            IPAddress ipAddress = host.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, port);

            try
            {
                Socket listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                listener.Bind(localEndPoint);
                listener.Listen(20);


                Console.WriteLine("Waiting for a connection...");

                while (true)
                {
                    Socket handler = listener.Accept();
                    Task task = Task.Run(() => getMessage(handler));
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }   


        }
        public static void getMessage(Socket handler)
        {

            string data = null;
            byte[] bytes = null;

            while (true)
            {
                bytes = new byte[1024];
                int bytesRec = handler.Receive(bytes);
                data = Encoding.ASCII.GetString(bytes, 0, bytesRec);

                if (data.IndexOf("exit") > -1)
                {
                    break;
                }
                else
                {
                    Console.WriteLine($"Text Received: {data}");
                    byte[] msg = Encoding.ASCII.GetBytes(data);
                    handler.Send(msg);
                }
            }

            handler.Shutdown(SocketShutdown.Both);
            handler.Close();
        }
    }
}
          
