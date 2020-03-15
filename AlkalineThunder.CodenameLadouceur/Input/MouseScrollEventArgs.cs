namespace AlkalineThunder.CodenameLadouceur.Input
{
    public class MouseScrollEventArgs : MouseEventArgs
    {
        public int ScrollWheelValue { get; }
        public int ScrollWheelDelta { get; }

        public MouseScrollEventArgs(int x, int y, int value, int delta) : base(x, y)
        {
            ScrollWheelValue = value;
            ScrollWheelDelta = delta;
        }
    }
}

