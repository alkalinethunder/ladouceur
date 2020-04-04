using System.Collections.Generic;
using System.Text;
using AlkalineThunder.Nucleus.Gui;
using AlkalineThunder.Nucleus.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace AlkalineThunder.Nucleus.Windowing
{
    public abstract class WindowDecorator
    {
        public abstract Padding FrameSize { get; }
        public abstract TitleTextLocation TitleTextLocation { get; }
        public abstract TitleButtonLocation TitleButtonLocation { get; }
        public abstract int TitleButtonSpacing { get; }
        public abstract Vector2 CloseButtonSize { get; }
        public abstract Vector2 MinimizeButtonSize { get; }
        public abstract Vector2 MaximizeButtonSize { get; }
        public abstract Vector2 IconSize { get; }
        public abstract SpriteFont TitleFont { get; }

        public abstract Brush LeftBrush { get; }
        public abstract Brush RightBrush { get; }
        public abstract Brush BottomtBrush { get; }
        public abstract Brush BottomLeftBrush { get; }
        public abstract Brush BottomRightBrush { get; }

        public virtual Color ActiveFrameColor => Color.CornflowerBlue;
        public virtual Color InactiveFrameColor => Color.Gray;

        public abstract Brush ClientBackground { get; }
        public abstract Brush TitleBrush { get; }

        public abstract Brush CloseButtonBrush { get; }
        public abstract Brush MaximizeButtonBrush { get; }
        public abstract Brush MinimizeButtonBrush { get; }
        public virtual Brush RestoreButtonBrush => MaximizeButtonBrush;

        public virtual Brush CloseHoverBrush => CloseButtonBrush;
        public virtual Brush MaximizeHoverBrush => MaximizeButtonBrush;
        public virtual Brush MinimizeHoverBrush => MinimizeButtonBrush;
        public virtual Brush RestoreHoverBrush => RestoreButtonBrush;

        public virtual Brush ClosePressedBrush => CloseButtonBrush;
        public virtual Brush MaximizePressedBrush => MaximizeButtonBrush;
        public virtual Brush MinimizePressedBrush => MinimizeButtonBrush;
        public virtual Brush RestorePressedBrush => RestoreButtonBrush;

        public abstract void LoadContent(ContentManager content);
    }
}
