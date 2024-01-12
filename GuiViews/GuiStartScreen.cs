using Terminal.Gui;

namespace TCPChatClient.GuiViews
{
    public class GuiStartScreen : IGuiWindow
    {
        private static GuiStartScreen? _instance;

        public static void Display()
        {
            if (_instance is null)
            {
                _instance = new GuiStartScreen();
            }

            _instance.Show();
        }

        Window window;
        Label lblServerText;
        Label lblUsername;
        TextField usernameText;
        TextField serverText;
        Button btnConnect;
        Button btnServer;

        public GuiStartScreen()
        {
            window = new Window("TCPChat");

            lblUsername = new Label("Username:")
            {
                X = 10,
                Y = Pos.Percent(44),
                Width = 10,
                Height = 1
            };

            usernameText = new TextField("")
            {
                X = Pos.Right(lblUsername),
                Y = Pos.Top(lblUsername),
                Width = 25,
                Height = 1,
                Id = "tbUsername",
                Text = "Anonym"
            };

            lblServerText = new Label("Server:")
            {
                X = 10,
                Y = Pos.Percent(50),
                Width = 10,
                Height = 1
            };

            serverText = new TextField("")
            {
                X = Pos.Right(lblServerText),
                Y = Pos.Top(lblServerText),
                Width = 25,
                Height = 1,
                Id = "tbServer",
                Text = "localhost"
            };

            btnConnect = new Button("Verbinden")
            {
                X = Pos.Left(lblServerText),
                Y = Pos.Bottom(lblServerText),
                Width = 12,
                Height = 1
            };

            btnConnect.Clicked += BtnConnect_Clicked;

            btnServer = new Button("Weiter als Server")
            {
                X = Pos.Right(btnConnect) + 1,
                Y = Pos.Top(btnConnect),
                Width = Dim.Width(btnConnect),
                Height = 1
            };
            btnServer.Clicked += BtnServer_Clicked;

            window.Add(lblServerText, lblUsername,serverText, usernameText,btnConnect, btnServer);
        }

        void SetupMenuBar()
        {
            var top = Application.Top;
            var menu = new MenuBar(new MenuBarItem[]
            {
                new MenuBarItem("Start", new MenuBarItem[]
                {
                    new MenuBarItem("_Verlassen", "", () => {if (Quit()) top.Running = false; })
                })
            });

            top.Add(menu);

            static bool Quit()
            {
                var n = MessageBox.Query(50, 7, "Quit", "Are you sure you want to quit?", "Yes", "No");
                return n == 0;
            }
        }

        public void Show()
        {
            Application.Top.RemoveAll();
            SetupMenuBar();
            Application.Top.Add(window);
        }

        private void BtnServer_Clicked()
        {
            GuiServerWindow.Display();
        }

        private void BtnConnect_Clicked()
        {
            // get server input
            var serverIp = serverText.Text.ToString();
            var username = usernameText.Text.ToString() ?? "unbekannt";
            if (string.IsNullOrEmpty(serverIp))
            {
                var n = MessageBox.ErrorQuery(50, 7, "Achtung", "Du hast keine Server-Adresse und Username eingegeben!", "Ok");
                return;
            }

            GuiClientWindow.Display(serverIp, username);
            //GuiClientWindow.Display(username);
        }
    }
}
