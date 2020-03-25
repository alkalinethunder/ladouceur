using AlkalineThunder.MultiColorText;
using AlkalineThunder.Nucleus.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlkalineThunder.Nucleus.Gui
{
    public sealed class TerminalEmulator : Control
    {
        private string _text = string.Empty;
        private string _input = string.Empty;
        private int _inputPos = 0;
        private const char CURSOR_CHAR = '█';
        private TextRenderer _myRenderer = new TextRenderer();

        public string Text => _text + _input;

        protected override Vector2 MeasureOverride()
        {
            var minWidth = _myRenderer.MeasureString(ActiveTheme.ConsoleFont, "#").X * 80;
            var minHeight = _myRenderer.MeasureString(ActiveTheme.ConsoleFont, Environment.NewLine).Y * 25;

            var actualMeasure = _myRenderer.MeasureString(ActiveTheme.ConsoleFont, Text + Environment.NewLine);

            return new Vector2(
                    Math.Max(minWidth, actualMeasure.X),
                    Math.Max(minHeight, actualMeasure.Y)
                );
        }

        protected override bool OnClick(MouseButtonEventArgs e)
        {
            return true;
        }

        protected override void OnDraw(GameTime gameTime)
        {
        }
    }
}
