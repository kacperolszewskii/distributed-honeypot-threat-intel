using System.Net;
using System.Net.Sockets;
using System.Text;

namespace NetworkCore
{
    internal class Program
    {
        static void Main(string[] args)
        {

            TcpListener listener = new TcpListener(IPAddress.Loopback, 2121);
            listener.Start();
            Console.WriteLine("Wykrywanie na porcie 2121 włączone !!!");

            while (true)
            {
                Console.WriteLine("\n[Wykrywanie hackera...]");

                TcpClient client = listener.AcceptTcpClient();
                Console.WriteLine($" O godzinie [{DateTime.Now}] wykryto hackera!");

                NetworkStream stream = client.GetStream();
                byte[] buffer = new byte[1024];
                int bytesRead = stream.Read(buffer, 0, buffer.Length);

                string dataFromHacker = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                Console.WriteLine("=== PRZECHWYCONE DANE ===");
                Console.WriteLine(dataFromHacker);
                Console.WriteLine("=========================");

                var remoteEndPoint = (System.Net.IPEndPoint)client.Client.RemoteEndPoint;
                string hackerIP = remoteEndPoint.Address.ToString();
                int hackerPort = remoteEndPoint.Port;

                Console.WriteLine($"Atak z IP: {hackerIP} (port klienta: {hackerPort})");

                string logMessage = $"[{DateTime.Now}] IP: {hackerIP}:{hackerPort} | Dane: {dataFromHacker.Replace("\r\n", " ")}\n";
                string filePath = "honeypot_logs.txt";

                File.AppendAllText(filePath, logMessage);
                Console.WriteLine("[INFO] Dane zostały pomyślnie zapisane do pliku logów.");

                client.Close();
            }
        }

    }
}

