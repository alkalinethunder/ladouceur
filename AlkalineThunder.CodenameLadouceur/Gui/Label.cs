using AlkalineThunder.MultiColorText;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AlkalineThunder.CodenameLadouceur.Gui
{
    public sealed class Label : Control
    {
        private TextRenderer _textRenderer;

        public Label()
        {
            _textRenderer = new TextRenderer();
        }

        public string Text { get; set; } = string.Empty;
        public Color TextColor { get => _textRenderer.DefaultColor; set => _textRenderer.DefaultColor = value; }
        public SpriteFont Font { get; set; }
        public float WrapWidth { get; set; } = 0;

        protected override Vector2 MeasureOverride()
        {
            if(Font != null && !string.IsNullOrEmpty(Text))
            {
                return _textRenderer.MeasureString(Font, Text, WrapWidth);
            }
            else
            {
                return Vector2.Zero;
            }
        }

        protected override void OnDraw(GameTime gameTime)
        {
            if(Font != null && !string.IsNullOrEmpty(Text))
            {
                _textRenderer.DrawString(this, Text, Font, new Vector2(Bounds.Left, Bounds.Top), WrapWidth);
            }
        }
    }
}
