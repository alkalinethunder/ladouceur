using System;

namespace AlkalineThunder.CodenameLadouceur
{
    class Program
    {
        static void Main(string[] args)
        {
            Logger.Log("Initializing game.  Let the cringefest begin...");
            using (var loop = new GameLoop()) loop.Run();
        }
    }
}
