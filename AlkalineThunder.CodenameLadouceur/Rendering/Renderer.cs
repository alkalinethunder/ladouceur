using AlkalineThunder.CodenameLadouceur.Gui;
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

        public Color Tint { get; set; } = Color.White;

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

        private Color TintColor(Color source)
        {
            float r = (float)Tint.R / 255;
            float g = (float)Tint.G / 255;
            float b = (float)Tint.B / 255;
            float a = (float)Tint.A / 255;

            var tintedR = source.R * r;
            var tintedG = source.G * g;
            var tintedB = source.B * b;
            var tintedA = source.A * a;

            return new Color((int)tintedR, (int)tintedG, (int)tintedB, (int)tintedA);
        }

        public void FillRectangle(Rectangle rect, Texture2D texture, Color color)
        {
            _batch.Draw(texture, rect, TintColor(color));
        }

        public void FillRectangle(Rectangle rect, Color color)
        {
            FillRectangle(rect, _white, TintColor(color));
        }

        public void DrawRectangle(Rectangle rect, Color color, Padding edges)
        {
            var tinted = TintColor(color);

            _batch.Draw(_white, new Rectangle(
                    rect.Left,
                    rect.Top,
                    edges.Left,
                    rect.Height
                ), tinted);

            _batch.Draw(_white, new Rectangle(
                    rect.Right - edges.Right,
                    rect.Top,
                    edges.Right,
                    rect.Height
                ), tinted);

            _batch.Draw(_white, new Rectangle(
                    rect.Left + edges.Left,
                    rect.Top,
                    rect.Width - edges.Width,
                    edges.Top
                ), tinted);

            _batch.Draw(_white, new Rectangle(
                    rect.Left + edges.Left,
                    rect.Bottom - edges.Bottom,
                    rect.Width - edges.Width,
                    edges.Bottom
                ), tinted);


        }

        public void DrawRectangle(Rectangle rect, Color color, int thickness)
        {
            if (thickness < 1) return;

            var tinted = TintColor(color);

            FillRectangle(new Rectangle(rect.X, rect.Y, thickness, rect.Height), tinted);
            FillRectangle(new Rectangle(rect.X + thickness, rect.Y, rect.Width - (thickness * 2), thickness), tinted);
            FillRectangle(new Rectangle(rect.Right - thickness, rect.Y, thickness, rect.Height), tinted);
            FillRectangle(new Rectangle(rect.X + thickness, rect.Bottom - thickness, rect.Width - (thickness * 2), thickness), tinted);

        }

        public void DrawBrush(Rectangle rect, Brush brush)
        {
            if(!rect.IsEmpty && brush.BrushType != BrushType.None)
            {
                if(brush.BrushType == BrushType.Image)
                {
                    FillRectangle(rect, brush.Texture ?? _white, brush.BrushColor);
                }
                else
                {
                    if(brush.Texture == null)
                    {
                        if(brush.BrushType == BrushType.Box)
                        {
                            FillRectangle(rect, brush.BrushColor);
                        }
                        else
                        {
                            DrawRectangle(rect, brush.BrushColor, brush.Margin);
                        }
                    }
                    else
                    {
                        var tex = brush.Texture;
                        var tinted = TintColor(brush.BrushColor);
                        var edges = brush.Margin;

                        _batch.Draw(tex, new Rectangle(
                                rect.Left,
                                rect.Top,
                                edges.Left,
                                edges.Top
                            ), new Rectangle(
                                0, 0,
                                edges.Left,
                                edges.Top
                            ), tinted);

                        _batch.Draw(tex, new Rectangle(
                                rect.Right - edges.Right,
                                rect.Top,
                                edges.Right,
                                edges.Top
                            ), new Rectangle(
                                tex.Width - edges.Right, 0,
                                edges.Right,
                                edges.Top
                            ), tinted);

                        _batch.Draw(tex, new Rectangle(
                                rect.Left,
                                rect.Bottom - edges.Bottom,
                                edges.Left,
                                edges.Bottom
                            ), new Rectangle(
                                0, tex.Height - edges.Bottom,
                                edges.Left,
                                edges.Bottom
                            ), tinted);

                        _batch.Draw(tex, new Rectangle(
                                rect.Right - edges.Right,
                                rect.Bottom - edges.Bottom,
                                edges.Right,
                                edges.Bottom
                            ), new Rectangle(
                                tex.Width - edges.Right, tex.Height - edges.Bottom,
                                edges.Right,
                                edges.Top
                            ), tinted);

                        _batch.Draw(tex, new Rectangle(
                                rect.Left + edges.Left,
                                rect.Top,
                                rect.Width - edges.Width,
                                edges.Top
                            ), new Rectangle(
                                edges.Left,
                                0,
                                tex.Width - edges.Width,
                                edges.Top
                            ), tinted);

                        _batch.Draw(tex, new Rectangle(
                                rect.Left + edges.Left,
                                rect.Bottom - edges.Bottom,
                                rect.Width - edges.Width,
                                edges.Bottom
                            ), new Rectangle(
                                edges.Left,
                                tex.Height - edges.Bottom,
                                tex.Width - edges.Width,
                                edges.Bottom
                            ), tinted);

                        _batch.Draw(tex, new Rectangle(
                                rect.Left,
                                rect.Top + edges.Top,
                                edges.Left,
                                rect.Height - edges.Height
                            ), new Rectangle(
                                0,
                                edges.Top,
                                edges.Left,
                                tex.Height - edges.Height
                            ), tinted);

                        _batch.Draw(tex, new Rectangle(
                                rect.Right - edges.Right,
                                rect.Top + edges.Top,
                                edges.Right,
                                rect.Height - edges.Height
                            ), new Rectangle(
                                tex.Width - edges.Right,
                                edges.Top,
                                edges.Right,
                                tex.Height - edges.Height
                            ), tinted);

                        if(brush.BrushType == BrushType.Box)
                        {
                            _batch.Draw(tex, new Rectangle(
                                    rect.Left + edges.Left,
                                    rect.Top + edges.Top,
                                    rect.Width - edges.Width,
                                    rect.Height - edges.Height
                                ), new Rectangle(
                                    edges.Left,
                                    edges.Top,
                                    tex.Width - edges.Width,
                                    tex.Height - edges.Height
                                ), tinted);
                        }
                    }
                }
            }
        }

        public void DrawString(SpriteFont font, string text, Vector2 pos, Color color)
        {
            _batch.DrawString(font, text, pos, TintColor(color));
        }
    }
}
