namespace AlkalineThunder.CodenameLadouceur.Input
{
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
}

