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

        public override Color DefaultBackground => DarkMode ? new Color(32, 32, 32) : Color.WhiteSmoke;

        public override Color DefaultForeground => DarkMode ? Color.WhiteSmoke : new Color(32, 32, 32);

        public override int ButtonBorderThickness => 2;

        public override SpriteFont TextEntryFont => DefaultFont;

        public override Color ButtonBorderColor => DefaultForeground;

        public override Color ButtonBackgroundColor => DarkMode ? new Color(72, 72, 72) : Color.LightGray;

        public override Color ButtonHoveredColor => Color.Gray;

        public override Color ButtonPressedColor => DarkMode ? Color.Black : Color.White;

        public override void LoadContent(ContentManager content)
        {
            _font = content.Load<SpriteFont>("ConsoleFont");
        }
    }
}
