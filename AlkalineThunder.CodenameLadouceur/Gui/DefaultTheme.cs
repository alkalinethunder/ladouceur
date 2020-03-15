using AlkalineThunder.CodenameLadouceur.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace AlkalineThunder.CodenameLadouceur.Gui
{
    public sealed class DefaultTheme : GuiTheme
    {
        private SpriteFont _font = null;

        public bool DarkMode { get; set; } = true;

        public override SpriteFont DefaultFont => _font;

        public override SpriteFont TitleFont => _font;

        public override SpriteFont BigFont => _font;

        public override SpriteFont SmallFont => _font;

        public override SpriteFont ConsoleFont => _font;

        public override Color DefaultBackground => DarkMode ? new Color(22,22,22) : Color.WhiteSmoke;

        public override Color DefaultForeground => DarkMode ? Color.WhiteSmoke : Color.Black;

        public override Brush SelectionBrush => Color.CornflowerBlue;

        public override Brush ButtonBrush => new Brush(Color.Blue, new Padding(7, 4));

        public override Brush ButtonHoveredBrush => ButtonBrush.InColor(Color.CornflowerBlue);

        public override Brush ButtonPressedBrush => ButtonBrush.InColor(Color.Black);

        public override SpriteFont TextEntryFont => _font;

        public override Color TextEntryHintColor => Color.Gray;

        public override Color TextEntryTextColor => DefaultForeground;

        public override Color ListBoxItemColor => DefaultForeground;

        public override Color ListBoxSelectedItemColor => Color.White;

        public override Brush ListBoxBrush => Brush.None;

        public override int CheckSize => 12;

        public override int CheckBorderThickness => 2;

        public override int ScrollBarWidth => 16;

        public override Brush ScrollBarBrush => DefaultForeground;

        public override void LoadContent(ContentManager content)
        {
            _font = content.Load<SpriteFont>("ConsoleFont");
        }
    }
}
