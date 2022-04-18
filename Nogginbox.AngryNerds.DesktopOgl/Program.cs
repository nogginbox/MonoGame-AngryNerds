using System;
using Nogginbox.AngryNerds;


namespace Nogginbox.AngryNerds.DesktopOgl
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using var game = new AngryNerdGame();
            game.Run();
        }
    }
}