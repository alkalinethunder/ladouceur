using AlkalineThunder.DevConsole;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlkalineThunder.CodenameLadouceur.Rendering
{
    public sealed class Renderer : IRenderer, IConsoleRenderer
    {
        private SpriteBatch _batch = null;
        private Texture2D _white = null;
        private RasterizerState _rasterizerState = null;

        public Renderer(GraphicsDevice device)
        {
            _batch = new SpriteBatch(device);
            _white = new Texture2D(device, 1, 1);
            _white.SetData<uint>(new[] { 0xffffffff });

            _rasterizerState = new RasterizerState
            {
                ScissorTestEnable = true
            };
        }

        public void SetScissorRect(Rectangle rect)
        {
            if (rect == Rectangle.Empty)
            {
                _batch.GraphicsDevice.ScissorRectangle = new Rectangle(0, 0, _batch.GraphicsDevice.PresentationParameters.BackBufferWidth, _batch.GraphicsDevice.PresentationParameters.BackBufferHeight);
            }
            else
            {
                _batch.GraphicsDevice.ScissorRectangle = rect;
            }
        }

        public void Begin()
        {
            _batch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, _rasterizerState); ;
        }
        
        public void End()
        {
            _batch.End();
        }

        public void FillRectangle(Rectangle rect, Texture2D texture, Color color)
        {
            _batch.Draw(texture, rect, color);
        }

        public void FillRectangle(Rectangle rect, Color color)
        {
            FillRectangle(rect, _white, color);
        }

        public void DrawRectangle(Rectangle rect, Color color, int thickness)
        {
            if (thickness < 1) return;

            FillRectangle(new Rectangle(rect.X, rect.Y, thickness, rect.Height), color);
            FillRectangle(new Rectangle(rect.X + thickness, rect.Y, rect.Width - (thickness * 2), thickness), color);
            FillRectangle(new Rectangle(rect.Right - thickness, rect.Y, thickness, rect.Height), color);
            FillRectangle(new Rectangle(rect.X + thickness, rect.Bottom - thickness, rect.Width - (thickness * 2), thickness), color);

        }

        public void DrawString(SpriteFont font, string text, Vector2 pos, Color color)
        {
            _batch.DrawString(font, text, pos, color);
        }
    }
}
