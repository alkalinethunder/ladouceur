using AlkalineThunder.Nucleus.Debugging;
using System;

namespace AlkalineThunder.Nucleus
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
