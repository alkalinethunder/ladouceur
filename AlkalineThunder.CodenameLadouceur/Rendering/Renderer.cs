using AlkalineThunder.CodenameLadouceur.Gui;
using AlkalineThunder.DevConsole;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace AlkalineThunder.CodenameLadouceur.Rendering
{
    public sealed class Renderer : IRenderer, IConsoleRenderer
    {
        private GraphicsDevice _gfx = null;
        private List<VertexPositionColorTexture> _vertexBuffer = new List<VertexPositionColorTexture>();
        private List<int> _indexBuffer = new List<int>();
        private SpriteEffect _basicEffect = null;
        private Texture2D _texture = null;
        private Texture2D _white = null;
        private const int CIRCLE_SEGMENTS = 240;

        public Renderer(GraphicsDevice device)
        {
            _gfx = device;

            _basicEffect = new SpriteEffect(_gfx);

            _white = new Texture2D(_gfx, 1, 1);
            _white.SetData<uint>(new[] { 0xffffffff });
        }

        public Color Tint { get; set; } = Color.White;

        public void SetScissorRect(Rectangle rect)
        {

        }

        public void Begin()
        {
            _vertexBuffer.Clear();
            _indexBuffer.Clear();
        }

        public void Clear(Color color)
        {
            _gfx.Clear(color);
        }

        private void SetGraphicsState()
        {
            _gfx.SamplerStates[0] = SamplerState.PointClamp;
            _gfx.BlendState = BlendState.AlphaBlend;
            _gfx.DepthStencilState = DepthStencilState.None;
            _gfx.RasterizerState = new RasterizerState
            {
                CullMode = CullMode.None,
                ScissorTestEnable = true
            };

            _basicEffect.CurrentTechnique.Passes[0].Apply();

            _gfx.Textures[0] = _texture ?? _white;
        }
    
        public void End()
        {
            if (_vertexBuffer.Count > 0 && _indexBuffer.Count > 0)
            {
                SetGraphicsState();

                _gfx.DrawUserIndexedPrimitives(PrimitiveType.TriangleList,
                    _vertexBuffer.ToArray(),
                    0,
                    _vertexBuffer.Count,
                    _indexBuffer.ToArray(),
                    0,
                    _indexBuffer.Count / 3);
            }
        }

        private int GetVertex(Vector3 position, Color color, Vector2 texture)
        {
            var vertex = new VertexPositionColorTexture(position, color, texture);

            if(_vertexBuffer.Contains(vertex))
            {
                return _vertexBuffer.IndexOf(vertex);
            }
            else
            {
                int c = _vertexBuffer.Count;
                _vertexBuffer.Add(vertex);
                return c;
            }
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

        private void SetTexture(Texture2D texture)
        {
            if(_texture != texture)
            {
                End();
                _texture = texture;
                Begin();
            }
        }

        public void FillRectangle(Rectangle rect, Texture2D texture, Color color)
        {
            SetTexture(texture);

            var tl = GetVertex(new Vector3(rect.Left, rect.Top, 0), color, Vector2.Zero);
            var tr = GetVertex(new Vector3(rect.Right, rect.Top, 0), color, new Vector2(1, 0));
            var bl = GetVertex(new Vector3(rect.Left, rect.Bottom, 0), color, new Vector2(0, 1));
            var br = GetVertex(new Vector3(rect.Right, rect.Bottom, 0), color, Vector2.One);

            _indexBuffer.Add(tl);
            _indexBuffer.Add(tr);
            _indexBuffer.Add(bl);
            _indexBuffer.Add(bl);
            _indexBuffer.Add(tr);
            _indexBuffer.Add(br);
        }



        public void FillRectangle(Rectangle rect, Color color)
        {
            FillRectangle(rect, null, color);
        }

        public void DrawRectangle(Rectangle rect, Color color, Padding edges)
        {
            DrawRectangle(rect, null, color, edges);
        }

        public void DrawRectangle(Rectangle rect, Texture2D texture, Color color, Padding edges)
        {
            // Set the texture.
            SetTexture(texture);

            // because I'm FUCKING LAZY.
            texture = texture ?? _white;

            // Left edge
            var tl = GetVertex(new Vector3(rect.Left, rect.Top, 0), color, TexCoord(0, 0));
            var tr = GetVertex(new Vector3(rect.Left + edges.Left, rect.Top, 0), color, TexCoord(edges.Left, 0));
            var bl = GetVertex(new Vector3(rect.Left, rect.Bottom, 0), color, TexCoord(0, texture.Height));
            var br = GetVertex(new Vector3(rect.Left + edges.Left, rect.Bottom, 0), color, TexCoord(edges.Left, texture.Height));

            // add the triangles...
            _indexBuffer.Add(tl);
            _indexBuffer.Add(tr);
            _indexBuffer.Add(bl);
            _indexBuffer.Add(bl);
            _indexBuffer.Add(br);
            _indexBuffer.Add(tr);

            // right edge
            tl = GetVertex(new Vector3(rect.Right - edges.Right, rect.Top, 0), color, TexCoord(texture.Width - edges.Right, 0));
            tr = GetVertex(new Vector3(rect.Right, rect.Top, 0), color, TexCoord(texture.Width, 0));
            bl = GetVertex(new Vector3(rect.Right - edges.Right, rect.Bottom, 0), color, TexCoord(texture.Width - edges.Right, texture.Height));
            br = GetVertex(new Vector3(rect.Right, rect.Bottom, 0), color, TexCoord(texture.Width, texture.Height));

            // add the triangles...
            _indexBuffer.Add(tl);
            _indexBuffer.Add(tr);
            _indexBuffer.Add(bl);
            _indexBuffer.Add(bl);
            _indexBuffer.Add(br);
            _indexBuffer.Add(tr);

            // Top edge.
            tl = GetVertex(new Vector3(rect.Left + edges.Left, rect.Top, 0), color, TexCoord(edges.Left, 0));
            tr = GetVertex(new Vector3(rect.Right - edges.Right, rect.Top, 0), color, TexCoord(texture.Width - edges.Right, 0));
            bl = GetVertex(new Vector3(rect.Left + edges.Left, rect.Top + edges.Top, 0), color, TexCoord(edges.Left, edges.Top));
            br = GetVertex(new Vector3(rect.Right - edges.Right, rect.Top + edges.Top, 0), color, TexCoord(texture.Width - edges.Right, edges.Top));

            // add the triangles...
            _indexBuffer.Add(tl);
            _indexBuffer.Add(tr);
            _indexBuffer.Add(bl);
            _indexBuffer.Add(bl);
            _indexBuffer.Add(br);
            _indexBuffer.Add(tr);

            // Bottom edge.
            tl = GetVertex(new Vector3(rect.Left + edges.Left, rect.Bottom - edges.Bottom, 0), color, TexCoord(edges.Left, texture.Height - edges.Bottom));
            tr = GetVertex(new Vector3(rect.Right - edges.Right, rect.Bottom - edges.Bottom, 0), color, TexCoord(texture.Width - edges.Right, texture.Height - edges.Bottom));
            bl = GetVertex(new Vector3(rect.Left + edges.Left, rect.Bottom, 0), color, TexCoord(edges.Left, texture.Height));
            br = GetVertex(new Vector3(rect.Right - edges.Right, rect.Bottom, 0), color, TexCoord(texture.Width - edges.Right, texture.Height));

            // add the triangles...
            _indexBuffer.Add(tl);
            _indexBuffer.Add(tr);
            _indexBuffer.Add(bl);
            _indexBuffer.Add(bl);
            _indexBuffer.Add(br);
            _indexBuffer.Add(tr);
        }

        public void FillRectangle(Rectangle rect, Texture2D texture, Color color, Padding edges)
        {
            // Draw the outline of the rectangle.
            DrawRectangle(rect, texture, color, edges);

            // again, because I'm lazy.

            texture = texture ?? _white;

            // No need to set texture, DrawRectangle did it for us.
            // But now we do need to get vertices for the inner rectangle.
            var innerRect = edges.Affect(rect);

            var tl = GetVertex(new Vector3(innerRect.Left, innerRect.Top, 0), color, new Vector2(edges.Left, edges.Top));
            var tr = GetVertex(new Vector3(innerRect.Right, innerRect.Top, 0), color, new Vector2(texture.Width - edges.Right, edges.Top));
            var bl = GetVertex(new Vector3(innerRect.Left, innerRect.Bottom, 0), color, new Vector2(edges.Left, texture.Height - edges.Bottom));
            var br = GetVertex(new Vector3(innerRect.Right, innerRect.Bottom, 0), color, new Vector2(texture.Width - edges.Right, texture.Height - edges.Bottom));

            // And now we add the triangles.
            _indexBuffer.Add(tl);
            _indexBuffer.Add(tr);
            _indexBuffer.Add(bl);

            _indexBuffer.Add(bl);
            _indexBuffer.Add(br);
            _indexBuffer.Add(tr);
        }

        public void DrawRectangle(Rectangle rect, Color color, int thickness)
        {
            DrawRectangle(rect, color, new Padding(thickness));
        }

        public void DrawBrush(Rectangle rect, Brush brush)
        {
            if(!rect.IsEmpty && brush.BrushType != BrushType.None)
            {
                if(brush.BrushType == BrushType.Image)
                {
                    FillRectangle(rect, brush.Texture, brush.BrushColor);
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
                        var edges = brush.Margin;

                        if(brush.BrushType == BrushType.Box)
                        {
                            DrawRectangle(rect, tex, brush.BrushColor, edges);
                        }
                        else
                        {
                            FillRectangle(rect, tex, brush.BrushColor, edges);
                        }
                    }
                }
            }
        }

        public void DrawString(SpriteFont font, string text, Vector2 pos, Color color)
        {
            if(font != null && !string.IsNullOrWhiteSpace(text))
            {
                var tex = font.Texture;

                SetTexture(tex);

                // Sorta borrowed from MonoGame's source code.
                var offset = Vector2.Zero;
                bool isFirstInLine = true;

                var glyphs = font.GetGlyphs();

                foreach(char c in text)
                {
                    if (c == '\r') continue;
                    if(c == '\n')
                    {
                        offset.X = 0;
                        offset.Y += font.LineSpacing;
                        isFirstInLine = true;
                        continue;
                    }

                    var glyph = glyphs.ContainsKey(c) ? glyphs[c] : glyphs[' '];

                    if(isFirstInLine)
                    {
                        offset.X = Math.Max(glyph.LeftSideBearing, 0);
                        isFirstInLine = false;
                    }
                    else
                    {
                        offset.X += glyph.LeftSideBearing;
                    }

                    var rPos = offset;
                    rPos.X += glyph.Cropping.X;
                    rPos.Y += glyph.Cropping.Y;
                    rPos += pos;

                    var tl = GetVertex(new Vector3(rPos.X, rPos.Y, 0), color, TexCoord(glyph.BoundsInTexture.Left, glyph.BoundsInTexture.Top));
                    var tr = GetVertex(new Vector3(rPos.X + glyph.BoundsInTexture.Width, rPos.Y, 0), color, TexCoord(glyph.BoundsInTexture.Right, glyph.BoundsInTexture.Top));
                    var bl = GetVertex(new Vector3(rPos.X, rPos.Y + glyph.BoundsInTexture.Height, 0), color, TexCoord(glyph.BoundsInTexture.Left, glyph.BoundsInTexture.Bottom));
                    var br = GetVertex(new Vector3(rPos.X + glyph.BoundsInTexture.Width, rPos.Y + glyph.BoundsInTexture.Height, 0), color, TexCoord(glyph.BoundsInTexture.Right, glyph.BoundsInTexture.Bottom));

                    _indexBuffer.Add(tl);
                    _indexBuffer.Add(tr);
                    _indexBuffer.Add(bl);

                    _indexBuffer.Add(tr);
                    _indexBuffer.Add(bl);
                    _indexBuffer.Add(br);

                    offset.X += glyph.Width + glyph.RightSideBearing;
                }



            }
        }

        private Vector2 TexCoord(int left, int top)
        {
            var tex = _texture ?? _white;

            float u = (float)left / (float)tex.Width;
            float v = (float)top / (float)tex.Height;

            return new Vector2(u, v);
        }

        public void DrawRoundedRectangle(Rectangle rect, Color color, float roundingPercentage)
        {
            int trueRadius = (int)(((float)Math.Min(rect.Width, rect.Height) / 2) * MathHelper.Clamp(roundingPercentage, 0, 1));
            if(trueRadius <= 0)
            {
                FillRectangle(rect, color);
            }

            var innerRect = new Rectangle(
                    rect.Left + trueRadius,
                    rect.Top + trueRadius,
                    rect.Width - (trueRadius * 2),
                    rect.Height - (trueRadius * 2)
                );

            int segments = CIRCLE_SEGMENTS / 4;

            // Fill the inner areas of the rectangle.
            FillRectangle(new Rectangle(rect.Left + trueRadius, rect.Top, rect.Width - (trueRadius * 2), trueRadius), color);
            FillRectangle(new Rectangle(rect.Left, rect.Top + trueRadius, rect.Width, rect.Height - (trueRadius * 2)), color);
            FillRectangle(new Rectangle(rect.Left + trueRadius, rect.Bottom - trueRadius, rect.Width - (trueRadius * 2), trueRadius), color);

            FillCircleSegment(new Vector2(innerRect.Left, innerRect.Top), trueRadius, LeftAngle, TopAngle, segments, color, null, new Rectangle(0, 0, 1, 1));
            FillCircleSegment(new Vector2(innerRect.Right, innerRect.Top), trueRadius, TopAngle, RightEndAngle, segments, color, null, new Rectangle(0, 0, 1, 1));
            FillCircleSegment(new Vector2(innerRect.Right, innerRect.Bottom), trueRadius, RightStartAngle, BotAngle, segments, color, null, new Rectangle(0, 0, 1, 1));
            FillCircleSegment(new Vector2(innerRect.Left, innerRect.Bottom), trueRadius, LeftAngle, BotAngle, segments, color, null, new Rectangle(0, 0, 1, 1));
        }

        private void FillCircleSegment(Vector2 center, float radius, float start, float end, int sides, Color color, Texture2D texture, Rectangle uvRect)
        {
            SetTexture(texture);

            var rect = RectFromRadius(center, radius);

            var segs = GetCircleSegment(center, radius, start, end, sides);

            var vs = segs.Select(x => GetVertex(new Vector3(x.X, x.Y, 0), color, LinearMap(x, rect, uvRect))).ToArray();
            var vCenter = GetVertex(new Vector3(center.X, center.Y, 0), color, LinearMap(center, rect, uvRect));

            for(int i = 0; i < vs.Length - 1; i++)
            {
                int v0 = vs[i];
                int v1 = vs[i + 1];

                _indexBuffer.Add(vCenter);
                _indexBuffer.Add(v0);
                _indexBuffer.Add(v1);
            }
        }

        public const float LeftAngle = (float)Math.PI;
        public const float TopAngle = (float)(1.5 * Math.PI);
        public const float RightStartAngle = 0;
        public const float RightEndAngle = (float)(2 * Math.PI);
        public const float BotAngle = (float)(.5 * Math.PI);

        private Vector2 LinearMap(Vector2 pos, Rectangle src, Rectangle dest)
        {
            float x = (pos.X - src.Left) / src.Width;
            float y = (pos.Y - src.Top) / src.Height;

            return new Vector2(
                    dest.Left + (dest.Width * x),
                    dest.Top + (dest.Height * y)
                );
        }

        private Rectangle RectFromRadius(Vector2 center, float radius)
        {
            int trueRadius = (int)radius;

            return new Rectangle(
                    (int)center.X - trueRadius,
                    (int)center.Y - trueRadius,
                    trueRadius * 2,
                    trueRadius * 2
                );
        }

        private static IEnumerable<Vector2> GetCircleSegment(Vector2 center, float radius, float start, float end, int sides)
        {
            float step = (end - start) / sides;
            float theta = start;

            for(int i = 0; i < sides; i++)
            {
                // thank fuck for OpenWheels being open-source... I ain't figuring trig out on my own.  FUCK.  THAT.
                yield return center + new Vector2((float)(radius * Math.Cos(theta)), (float)(radius * Math.Sin(theta)));
                theta += step;
            }
            yield return center + new Vector2((float)(radius * Math.Cos(end)), (float)(radius * Math.Sin(end)));
        }
    }
}
