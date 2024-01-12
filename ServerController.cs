using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TCPChatClient
{
    public class ServerController
    {
        public List<Client> Clients { get; set; } = new List<Client>();
        private TcpListener? listener { get; set; }

        public void StartServer(System.Net.IPAddress ip, int port = 1337)
        {
            // Start Server
            listener = new TcpListener(ip, port);
            listener.Start();
        }

        public async Task<Client> AcceptClient()
        {
            if (listener is null) throw new Exception("Server nicht gestartet.");
            var tcpclient = await listener.AcceptTcpClientAsync();
            var client = new Client(tcpclient);
            Clients.Add(client);

            return client;
        }

        public async Task<string> ReadMessage(Client client)
        {
            try
            {
                if (client is null) return "";

                byte[] buffer = new byte[1024];
                await client.TcpClient.GetStream().ReadAsync(buffer, 0, buffer.Length);
                int received = 0;
                foreach (byte b in buffer)
                {
                    if (b != 0)
                    {
                        received++;
                    }
                }
                string messageRecieved = Encoding.UTF8.GetString(buffer, 0, received);
                return messageRecieved;
            }
            catch
            {
                Clients.Remove(client);
                return "CMD-disconnect";
            }
        }

        public void SendMessageToEveryone(string message)
        {
            foreach(var c in Clients)
            {
                SendMessageToClient(c, message);
            }
        }

        public void SendMessageToClient(Client client, string message)
        {
            var networkStream = client.TcpClient.GetStream();
            Byte[] data = System.Text.Encoding.UTF8.GetBytes(message);
            networkStream.Write(data);
            networkStream.Flush();
        }
    }
}
