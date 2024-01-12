using System.Net.Sockets;
using System.Text;

namespace TCPChatClient
{
    public class ClientController
    {
        // Methode:
        // - Verbinden (Server)
        // - Schreiben (Message)
        // - Lesen  ()
        // - Schliessen ()

        public NetworkStream? Stream { get; set; }

        public static void ClientCode(String message, String server)
        {
            try
            {

                //int port = 1337;

                //TcpClient chatclient = new TcpClient(server, port);
                ////message = "Hello there!";

                //Byte[] data = System.Text.Encoding.UTF8.GetBytes(message);

                ////Connect
                //NetworkStream networkStream = chatclient.GetStream();
                //// Send message
                //networkStream.Write(data, 0, data.Length);
                //Console.WriteLine("Sent: {0}", message);

                //// Get response
                //data = new Byte[256];
                //String responseData = String.Empty;
                //Int32 bytes = networkStream.Read(data, 0, data.Length);
                //responseData = System.Text.Encoding.UTF8.GetString(data, 0, bytes);
                //Console.WriteLine("Received: {0}", responseData);

                //// Close
                //chatclient.Close();
                //networkStream.Close();
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException: {0}", e);
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }            
        }

        public void ConnectServer(string server)
        {
            int port = 1337;
            TcpClient chatclient = new TcpClient(server, port);
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
            //chatclient.Close();
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