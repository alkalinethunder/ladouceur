using System;
using AlkalineThunder.Nucleus.Gui;
using AlkalineThunder.Nucleus.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace AlkalineThunder.Nucleus.Windowing
{
    public sealed class ThemeWindowDecorator : WindowDecorator
    {
        protected GuiTheme Theme => Control.ActiveTheme;

        public override Padding FrameSize => new Padding(2, 24, 2, 2);

        public override TitleTextLocation TitleTextLocation => TitleTextLocation.Center;

        public override TitleButtonLocation TitleButtonLocation => TitleButtonLocation.Right;

        public override int TitleButtonSpacing => 2;

        public override Vector2 CloseButtonSize => new Vector2(22,22);

        public override Vector2 MinimizeButtonSize => CloseButtonSize;

        public override Vector2 MaximizeButtonSize => CloseButtonSize;

        public override Vector2 IconSize => new Vector2(16, 16);

        public override SpriteFont TitleFont => Theme.DefaultFont;

        public override Brush LeftBrush => Theme.SelectionBrush;

        public override Brush RightBrush => LeftBrush;

        public override Brush BottomtBrush => LeftBrush;

        public override Brush BottomLeftBrush => LeftBrush;

        public override Brush BottomRightBrush => LeftBrush;

        public override Brush ClientBackground => Theme.DefaultBackground;

        public override Brush TitleBrush => LeftBrush;

        public override Brush CloseButtonBrush => Color.Red;
        public override Brush MaximizeButtonBrush => Color.Yellow;
        public override Brush MinimizeButtonBrush => Color.Green;

        public override void LoadContent(ContentManager content)
        {
        }
    }
}
