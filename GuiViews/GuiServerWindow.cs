using System.Net.Sockets;

namespace TCPChatClient.GuiViews
{
    public class GuiServerWindow : GuiChatWindow
    {
        public static void Display()
        {
            var instance = new GuiServerWindow();
            instance.Show();
        }

        ServerController server = new ServerController();

        public GuiServerWindow(): base(true)
        {
            try
            {
                _=StartAcceptingClients();
                if(btnSendMessage is not null)
                    btnSendMessage.Clicked += BtnSendMessage_Clicked;
            }
            catch (Exception ex)
            {
                AddChatMessage("Fehler beim Starten des Servers..");
                AddChatMessage(ex.ToString());
            }
        }

        private void BtnSendMessage_Clicked()
        {
            string? msg = tbChatMessage?.Text.ToString();
            if (string.IsNullOrEmpty(msg)) return;

            AddChatMessage($"[{DateTime.Now:dd.MM HH:mm}] Server: {msg}");
            try
            {
                server.SendMessageToEveryone(msg);
                if (tbChatMessage is not null)
                {
                    tbChatMessage.Text = "";
                    tbChatMessage.SetFocus();
                }
            }
            catch (Exception ex)
            {
                AddChatMessage("Fehler beim Senden einer Nachricht..");
                AddChatMessage(ex.ToString());
            }
        }

        private async Task StartAcceptingClients()
        {
            server.StartServer(System.Net.IPAddress.Any);
            AddChatMessage("Server gestartet..");

            while (true)
            {
                var client = await server.AcceptClient();                
                AddChatMessage($"{client.UserName} Client ist beigetreten..");
                UserListeAktualisieren();
                _ = StartReadingMessages(client);
            }
        }

        private async Task StartReadingMessages(Client client)
        {
            while (client.TcpClient.Connected)
            {
                // Nachricht Lesen
                string msg = await server.ReadMessage(client);


                // msg testen auf "CMD-name:{username}"
                // client.UserName = username;
                if (msg.Trim().StartsWith("CMD-name:"))
                {
                    string username = msg.Substring(9);
                    client.UserName = username;
                    UserListeAktualisieren();
                }
                if(msg.StartsWith("CMD-disconnect"))
                {
                    UserListeAktualisieren();
                }

                var echoMessage = $"[{DateTime.Now:dd.MM HH:mm}] {client.UserName}: {msg}";
                server.SendMessageToEveryone(echoMessage);
                AddChatMessage(echoMessage);
            }

            if (server.Clients.Contains(client))
                server.Clients.Remove(client);
        }

        private void UserListeAktualisieren()
        {
            Users.Clear();
            var verbundendeClients = server.Clients;
            foreach (var client in verbundendeClients)
            {
                Users.Add(client.UserName);
            }
            server.SendMessageToEveryone("CMD-UserList:///" + string.Join("///", Users));
        }
    }
}
