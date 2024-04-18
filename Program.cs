using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace PRI_lab4_Server
{
    internal static class Program
    {
        static HashSet<IPEndPoint> iPAddress = new HashSet<IPEndPoint>();
        static UdpClient udpServer = new UdpClient(1234);
        static UdpClient udpForSend = new UdpClient();
        static void Main(string[] args)
        {
            while (true)
            {
                var result = udpServer.ReceiveAsync().Result;//добавь await
                iPAddress.Add(result.RemoteEndPoint);
                // предположим, что отправлена строка, преобразуем байты в строку
                var message = Encoding.UTF8.GetString(result.Buffer);
                Console.WriteLine($"Получено {result.Buffer.Length} байт");
                Console.WriteLine($"Удаленный адрес: {result.RemoteEndPoint}");
                Console.WriteLine(message);
                byte[] broadcastMessage = Encoding.ASCII.GetBytes($"{result.RemoteEndPoint.Address}:{message}");
                Console.WriteLine("ipendpoints:");
                foreach (var ip_end_point in iPAddress)
                {
                    Console.WriteLine(ip_end_point+":"+ message);
                    udpForSend.Send(broadcastMessage, broadcastMessage.Length, ip_end_point);
                }
                Console.WriteLine("-------------------------");
            }
        }
        //удали эту фигню
        static async Task workNiger()
        {
            
        }
        static  void SendToEveryOne(byte[] sendBytes)
        {
            foreach (var ip_end_point in iPAddress)
            {
                  udpServer.Send(sendBytes, sendBytes.Length, ip_end_point);
            }
        }
    }
}

