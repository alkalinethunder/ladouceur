using System;
using AlkalineThunder.Nucleus;

namespace AlkalineThunder.Nucleus.Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            GameLoop.GameReady += GameLoop_GameReady;

            using (var game = new GameLoop())
                game.Run();
        }

        private static void GameLoop_GameReady(object sender, EventArgs e)
        {
            GameLoop.Instance.ScreenManager.AddScreen<DemoScreen>();
        }
    }
}
