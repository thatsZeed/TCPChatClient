using Terminal.Gui;

namespace TCPChatClient.GuiViews
{
    public class GuiClientWindow : GuiChatWindow
    {
        public static void Display(string serverIp, string username)
        {
            var instance = new GuiClientWindow(serverIp, username);
            instance.Show();
        }

        public string Username { get; private set; } = "unbekannt";

        ClientController client = new ClientController();

        public GuiClientWindow(string serverIp, string username)
            : base(false, serverIp)
        {
            try
            {
                Username = username;
                StartReadingMessage(serverIp);
                if (btnSendMessage is not null)
                {
                    btnSendMessage.Clicked += BtnSendMessage_Clicked;
                }
            }
            catch (Exception ex)
            {
                AddChatMessage(ex.Message);
            }
        }

        private void BtnSendMessage_Clicked()
        {
            string? msg = tbChatMessage?.Text.ToString();
            if (string.IsNullOrEmpty(msg))
            {
                return;
            }

            try
            {
                client.SendMessage(msg);
                if (tbChatMessage is not null)
                {
                    tbChatMessage.Text = "";
                    tbChatMessage.SetFocus();
                }
            }
            catch (Exception ex)
            {
                AddChatMessage("Fehler beim Senden einer Nachricht...");
                AddChatMessage(ex.ToString());
            }
        }

        private void StartReadingMessage(string server)
        {
            Task taskRead = new Task(() =>
            {
                client.ConnectServer(server);
                AddChatMessage("Client gestartet..");
                client.SendMessage($"CMD-name:{Username}");
                while (true)
                {
                    string msg = client.Read();
                    UserListeAktualisieren(msg);
                    if (msg.StartsWith("CMD-"))
                        continue;
                    AddChatMessage($"{msg}");
                }
            });
            taskRead.Start();
        }

        private void UserListeAktualisieren(string msg)
        {
            if (msg.StartsWith("CMD-UserList:"))
            {
                Application.MainLoop.Invoke(() =>
                {
                    Users.Clear();
                    // befehl auseinander nehmem
                    var msgGetrennt = msg.Split("///");

                    foreach (var client in msgGetrennt)
                    {
                        if (client != "CMD-UserList:")
                        {
                            Users.Add(client);
                        }
                    }
                });
            }

        }
    }
}
