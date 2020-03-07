using AlkalineThunder.CodenameLadouceur.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace AlkalineThunder.CodenameLadouceur.Screens
{
    public abstract class Screen : IRenderer
    {
        private Renderer _renderer = null;

        protected virtual void OnUpdate(GameTime gameTime) { }
        protected virtual void OnDraw(GameTime gameTime) { }

        public void Update(GameTime gameTime)
        {
            OnUpdate(gameTime);
        }

        public void Draw(GameTime gameTime, Renderer renderer)
        {
            if (_renderer != null) throw new InvalidOperationException("Screen is already rendering.");
            _renderer = renderer;
            _renderer.Begin();
            OnDraw(gameTime);
            _renderer.End();
            _renderer = null;
        }

        public void FillRectangle(Rectangle rect, Texture2D texture, Color color)
        {
            _renderer.FillRectangle(rect, texture, color);
        }

        public void FillRectangle(Rectangle rect, Color color)
        {
            _renderer.FillRectangle(rect, color);
        }

        public void DrawString(SpriteFont font, string text, Vector2 pos, Color color)
        {
            _renderer.DrawString(font, text, pos, color);
        }
    }
}
