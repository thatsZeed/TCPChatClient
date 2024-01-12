using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TCPChatClient
{
    /// <summary>
    /// Client Klasse die der Server benutzt um Clients zu identifizieren
    /// </summary>
    public class Client
    {
        public Client(TcpClient client)
        {
            this.TcpClient = client;
        }

        public TcpClient TcpClient { get; set; }

        public string UserName { get; set; } = "unbekannter client";
    }
}
