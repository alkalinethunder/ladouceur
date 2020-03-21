namespace AlkalineThunder.Nucleus.Input
{
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
}

