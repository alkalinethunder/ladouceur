using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using AlkalineThunder.MultiColorText;
using AlkalineThunder.CodenameLadouceur.Gui;

namespace AlkalineThunder.CodenameLadouceur.Rendering
{
    public interface IRenderer : ITextRenderer
    {
        void FillRectangle(Rectangle rect, Texture2D texture, Color color);
        void FillRectangle(Rectangle rect, Color color);
        void DrawString(SpriteFont font, string text, Vector2 pos, Color color);
        void DrawRectangle(Rectangle rect, Color color, int thickness);
        void DrawRectangle(Rectangle rect, Color color, Padding edges);
        void DrawBrush(Rectangle rect, Brush brush);

        void Clear(Color color);

        void DrawRectangle(Rectangle rect, Texture2D texture, Color color, Padding edges);
        void FillRectangle(Rectangle rect, Texture2D texture, Color color, Padding edges);

        void DrawRoundedRectangle(Rectangle rect, Color color, float roundingPercentage);
    }
}
