using System;
using System.Collections.Generic;
using System.Text;

namespace AlkalineThunder.CodenameLadouceur.Input
{
    public abstract class MouseEventArgs : EventArgs
    {
        public int X { get; private set; }
        public int Y { get; private set; }

        public MouseEventArgs(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}

