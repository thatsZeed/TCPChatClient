using System.ComponentModel;
using Terminal.Gui;

namespace TCPChatClient.GuiViews
{
    public class GuiChatWindow : IGuiWindow
    {
        Window window;

        private bool IsServer = false;
        private string ServerIP = "";

        protected ListView? lvUsers;
        protected ListView? lvChatLog;
        protected TextField? tbChatMessage;
        protected Button? btnSendMessage;
        protected FrameView? currentUsers=null; 

        protected List<string> Messages = new List<string>();
        protected List<string> Users = new List<string>();

        public GuiChatWindow(bool isServer, string serverIp = "0.0.0.0")
        {
            window = new Window($"TCPChat / {(isServer ? "Server" : "Client")} ({serverIp})");

            IsServer = isServer;
            ServerIP = serverIp;


            currentUsers = new FrameView("Userliste")
            {
                X = 0,
                Y = 0,
                Width = Dim.Percent(20),
                Height = Dim.Fill()
            };

            lvUsers = new ListView(Users)
            {
                X = 0,
                Y = 0,
                Width = Dim.Fill(),
                Height = Dim.Fill()
            };

            currentUsers.Add(lvUsers);

            FrameView ChatFrame = new FrameView("Chat")
            {
                X = Pos.Right(currentUsers),
                Y = 0,
                Width = Dim.Fill(),
                Height = Dim.Fill()
            };
            lvChatLog = new ListView(Messages)
            {
                X = 0,
                Y = 0,
                Width = Dim.Fill(),
                Height = Dim.Fill(1),
                CanFocus = false
            };

            tbChatMessage = new TextField()
            {
                X = 0,
                Y = Pos.Bottom(lvChatLog),
                Width = Dim.Fill(12),
                Height =1
            };
            tbChatMessage.KeyPress += TbChatMessage_KeyPress;

            btnSendMessage = new Button("senden")
            {
                X = Pos.Right(tbChatMessage),
                Y = Pos.Bottom(lvChatLog),
                Width = 12,
                Height = 1
            };


            ChatFrame.Add(lvChatLog, tbChatMessage, btnSendMessage);

            

            window.Add(ChatFrame, currentUsers);
        }

        private void TbChatMessage_KeyPress(View.KeyEventEventArgs obj)
        {
            if(obj.KeyEvent.Key == Key.Enter && btnSendMessage is not null)
            {
                obj.Handled = true;
                btnSendMessage.OnClicked();
            }
        }

        public void Show()
        {
            Application.Top.RemoveAll();
            SetupMenuBar();

            Application.Top.Add(window);     
        }

        protected void AddChatMessage(string text)
        {
            Application.MainLoop.Invoke(() => { 
                try
                {
                    Messages.Add(text);
                    if (lvChatLog is not null)
                    {
                        try
                        {
                            lvChatLog.MoveDown();
                            lvChatLog.MoveEnd();
                            Application.Refresh();
                        }
                        catch { }
                    }
                }
                catch(Exception ex)
                {
                    
                }
            });
        }

        void SetupMenuBar()
        {
            var top = Application.Top;
            var menu = new MenuBar(new MenuBarItem[]
            {
                new MenuBarItem("Start", new MenuBarItem[]
                {
                    new MenuBarItem("_New Chat", "", () => { window.Dispose(); GuiStartScreen.Display(); }),
                    new MenuBarItem("_Quit", "", () => {if (Quit()) top.Running = false; })
                })
            });

            top.Add(menu);

            static bool Quit()
            {
                var n = MessageBox.Query(50, 7, "Quit", "Are you sure you want to quit?", "Yes", "No");
                return n == 0;
            }
        }
    }
}
