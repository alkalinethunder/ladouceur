using System;

namespace AlkalineThunder.CodenameLadouceur
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var loop = new GameLoop()) loop.Run();
        }
    }
}
