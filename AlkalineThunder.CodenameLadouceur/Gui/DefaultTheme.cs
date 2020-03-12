using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace AlkalineThunder.CodenameLadouceur.Gui
{
    public sealed class DefaultTheme : GuiTheme
    {
        private SpriteFont _font = null;

        public bool DarkMode { get; set; } = false;

        public override SpriteFont DefaultFont => _font;

        public override SpriteFont TitleFont => _font;

        public override SpriteFont BigFont => _font;

        public override SpriteFont SmallFont => _font;

        public override SpriteFont ConsoleFont => _font;

        public override Color DefaultBackground => DarkMode ? new Color(32, 32, 32) : Color.WhiteSmoke;

        public override Color DefaultForeground => DarkMode ? Color.WhiteSmoke : new Color(32, 32, 32);

        public override void LoadContent(ContentManager content)
        {
            _font = content.Load<SpriteFont>("ConsoleFont");
        }
    }
}
