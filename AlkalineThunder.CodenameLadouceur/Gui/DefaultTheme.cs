using AlkalineThunder.Nucleus.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace AlkalineThunder.Nucleus.Gui
{
    public sealed class DefaultTheme : GuiTheme
    {
        private SpriteFont _font = null;
        private SpriteFont _title = null;
        private SpriteFont _big = null;
        private SpriteFont _small = null;
        private SpriteFont _console = null;

        public bool DarkMode { get; set; } = true;

        public override SpriteFont DefaultFont => _font;

        public override SpriteFont TitleFont => _title;

        public override SpriteFont BigFont => _big;

        public override SpriteFont SmallFont => _small;

        public override SpriteFont ConsoleFont => _console;

        public override Color DefaultBackground => DarkMode ? new Color(22,22,22) : Color.WhiteSmoke;

        public override Color DefaultForeground => DarkMode ? Color.WhiteSmoke : Color.Black;

        public override Brush SelectionBrush => Color.CornflowerBlue;

        public override Brush ButtonBrush => new Brush(Color.Blue, new Padding(7, 4));

        public override Brush ButtonHoveredBrush => ButtonBrush.InColor(Color.CornflowerBlue);

        public override Brush ButtonPressedBrush => ButtonBrush.InColor(Color.Black);

        public override SpriteFont TextEntryFont => DefaultFont;

        public override Color TextEntryHintColor => Color.Gray;

        public override Color TextEntryTextColor => DefaultForeground;

        public override Color ListBoxItemColor => DefaultForeground;

        public override Color ListBoxSelectedItemColor => Color.White;

        public override Brush ListBoxBrush => Brush.None;

        public override int CheckSize => 12;

        public override int CheckBorderThickness => 2;

        public override int ScrollBarWidth => 16;

        public override Brush ScrollBarBrush => ButtonBrush;

        public override Brush TextEntryBrush => Brush.Border.InColor(Color.Gray).Pad(new Padding(0, 0, 0, 2));

        public override Brush TextEntryHoveredBrush => TextEntryBrush.InColor(DefaultForeground);

        public override Brush TextEntryFocusedBrush => TextEntryBrush.InColor(Color.CornflowerBlue);

        public override void LoadContent(ContentManager content)
        {
            _font = content.Load<SpriteFont>("Fonts/Default");
            _title = content.Load<SpriteFont>("Fonts/Title");
            _small = content.Load<SpriteFont>("Fonts/Small");
            _big = content.Load<SpriteFont>("Fonts/Big");
            _console = content.Load<SpriteFont>("Fonts/Console");
        }

        public override Color ToggleSwitchForegroundColor => Color.White;
        public override Color ToggleSwitchBackgroundColor => Color.Gray;
        public override Color ToggleSwitchActiveBackgroundColor => Color.CornflowerBlue;

        public override Brush ProgressBarBrush => (DarkMode) ? Brush.Image.InColor(Color.Black) : Brush.Image.InColor(Color.White);

        public override Brush ProgressBrush => Brush.Image.InColor(Color.CornflowerBlue);

        public override Color SliderBarColor => DefaultForeground;

    }
}
