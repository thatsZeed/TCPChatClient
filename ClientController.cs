using System.Net.Sockets;
using System.Text;

namespace TCPChatClient
{
    public class ClientController
    {
        public NetworkStream? Stream { get; set; }

        public void ConnectServer(string server)
        {
            TcpClient chatclient = new TcpClient(server, Convert.ToInt32(Properties.Resources.Port));
            NetworkStream networkStream = chatclient.GetStream();
            this.Stream = networkStream; 
        }

        public void SendMessage(string message)
        {
            if(this.Stream is null)
            {
                Console.WriteLine("Du hast dich noch nicht verbunden!");
                return;
            }

            Byte[] data = System.Text.Encoding.UTF8.GetBytes(message);
            this.Stream.Write(data, 0, data.Length);
            this.Stream.Flush();
        }

        public void Close()
        {
            this.Stream.Close();
        }

        public string Read()
        {
            try
            {
                if (this.Stream is null)
                {
                    Console.WriteLine("Du hast dich noch nicht verbunden!");
                    return "";
                }

                var data = new Byte[256];
                String responseData = String.Empty;
                Int32 bytes = this.Stream.Read(data, 0, data.Length);
                responseData = System.Text.Encoding.UTF8.GetString(data, 0, bytes);
                return responseData;
            }
            catch { return ""; }
        }
    }
}