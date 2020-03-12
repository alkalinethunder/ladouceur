using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using AlkalineThunder.MultiColorText;

namespace AlkalineThunder.CodenameLadouceur.Rendering
{
    public interface IRenderer : ITextRenderer
    {
        void FillRectangle(Rectangle rect, Texture2D texture, Color color);
        void FillRectangle(Rectangle rect, Color color);
        void DrawString(SpriteFont font, string text, Vector2 pos, Color color);
        void DrawRectangle(Rectangle rect, Color color, int thickness);
    }
}
