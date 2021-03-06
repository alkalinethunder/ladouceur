﻿using AlkalineThunder.MultiColorText;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AlkalineThunder.Nucleus.Gui
{
    public sealed class Label : Control
    {
        private TextRenderer _textRenderer;

        public Label()
        {
            _textRenderer = new TextRenderer();
        }

        public Label(string text) : this()
        {
            this.Text = text;
        }

        public Label(string text, SpriteFont font) : this(text)
        {
            Font = font;
        }

        public string Text { get; set; } = string.Empty;
        public Color? TextColor { get; set; } = null;
        public SpriteFont Font { get; set; }
        public float WrapWidth { get; set; } = 0;

        protected override Vector2 MeasureOverride()
        {
            if (!string.IsNullOrEmpty(Text))
            {
                var realFont = Font ?? ActiveTheme.DefaultFont;
                return _textRenderer.MeasureString(realFont, Text, WrapWidth);
            }
            else
            {
                return Vector2.Zero;
            }
        }

        protected override void OnDraw(GameTime gameTime)
        {
            if(!string.IsNullOrEmpty(Text))
            {
                _textRenderer.DefaultColor = (TextColor == null) ? ActiveTheme.DefaultForeground : (Color)TextColor;
                var realFont = Font ?? ActiveTheme.DefaultFont;
                _textRenderer.DrawString(this, Text, realFont, new Vector2(Bounds.Left, Bounds.Top), WrapWidth);
            }
        }

        public static implicit operator Label(string text)
        {
            return new Label(text);
        }
    }
}
