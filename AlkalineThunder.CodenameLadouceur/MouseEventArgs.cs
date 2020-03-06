using System;
using System.Collections.Generic;
using System.Text;

namespace AlkalineThunder.CodenameLadouceur
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

    public sealed class MouseMoveEventArgs : MouseEventArgs
    {
        public int MovementX { get; private set; }
        public int MovementY { get; private set; }

        public MouseMoveEventArgs(int x, int y, int dx, int dy) : base(x, y)
        {
            MovementX = dx;
            MovementY = dy;
        }
    }

    public sealed class MouseButtonEventArgs : MouseEventArgs
    {
        public MouseButton Button { get; private set; }
        public bool IsPressed { get; private set; }

        public MouseButtonEventArgs(int x, int y, MouseButton button, bool pressed) : base(x, y)
        {
            Button = button;
            IsPressed = pressed;
        }
    }

    public enum MouseButton
    {
        Left,
        Middle,
        Right,
        XButton1,
        XButton2
    }
}

