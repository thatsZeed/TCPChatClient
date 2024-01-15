using Terminal.Gui;
using NStack;
using TCPChatClient.GuiViews;
using System.Text;

namespace TCPChatClient
{
    public class Program
    {
        public static void Main()
        {
            Console.OutputEncoding = Encoding.UTF8;
            Application.Init();
            Colors.Base.Normal = Application.Driver.MakeAttribute(Terminal.Gui.Color.Green, Terminal.Gui.Color.Black);

            GuiStartScreen.Display();

            Application.Run();
            Application.Shutdown();
            return;
        }

        private static void OldMain()
        {
            Console.WriteLine("d888888b  .o88b. d8888b.  .o88b. db      d888888b d88888b d8b   db d888888b \r\n`~~88~~' d8P  Y8 88  `8D d8P  Y8 88        `88'   88'     888o  88 `~~88~~' \r\n   88    8P      88oodD' 8P      88         88    88ooooo 88V8o 88    88    \r\n   88    8b      88~~~   8b      88         88    88~~~~~ 88 V8o88    88    \r\n   88    Y8b  d8 88      Y8b  d8 88booo.   .88.   88.     88  V888    88    \r\n   YP     `Y88P' 88       `Y88P' Y88888P Y888888P Y88888P VP   V8P    YP ");

        }

       
    }
}
