using System;
using System.Net;
using System.Net.Sockets;
using System.Text;


namespace PingPongClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Please enter the IP dect: ");
            string ip = Console.ReadLine();
            Console.WriteLine("Please enter the port number");
            int port = int.Parse(Console.ReadLine());
            StartClient(ip,port);
        }
        public static void StartClient(string ip , int port)
        {
            byte[] bytes = new byte[1024];
            try
            {
                IPHostEntry host = Dns.GetHostEntry(ip);
                IPAddress ipAddress = host.AddressList[0];
                IPEndPoint remoteEP = new IPEndPoint(ipAddress, port);

                Socket sender = new Socket(ipAddress.AddressFamily,SocketType.Stream, ProtocolType.Tcp);
                try
                {
                    // Connect to Remote EndPoint  
                    sender.Connect(remoteEP);

                    Console.WriteLine($"Socket connected to {sender.RemoteEndPoint.ToString()}");


                    while (true)
                    {
                        Console.WriteLine("Please enter what you want to send: ");
                        string message = Console.ReadLine();
                        // Encode the data string into a byte array.    
                        byte[] msg = Encoding.ASCII.GetBytes(message);

                        // Send the data through the socket.    
                        int bytesSent = sender.Send(msg);
                        if(message=="exit")
                        {
                            break;
                        }
                        // Receive the response from the remote device.    
                        int bytesRec = sender.Receive(bytes);
                        Console.WriteLine($"Server sent = {Encoding.ASCII.GetString(bytes, 0, bytesRec)}");
                    }
                    // Release the socket.    
                    sender.Shutdown(SocketShutdown.Both);
                    sender.Close();

                }
                catch (ArgumentNullException ane)
                {
                    Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
                }
                catch (SocketException se)
                {
                    Console.WriteLine("SocketException : {0}", se.ToString());
                }
                catch (Exception e)
                {
                    Console.WriteLine("Unexpected exception : {0}", e.ToString());
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

        }
    }
}
