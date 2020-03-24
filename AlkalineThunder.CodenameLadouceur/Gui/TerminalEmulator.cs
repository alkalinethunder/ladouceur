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
            FillRectangle(ContentBounds, ActiveTheme.MapConsoleColor(ConsoleColor.Black));

            _myRenderer.DefaultColor = ActiveTheme.MapConsoleColor(ConsoleColor.Gray);

            if(IsFocused)
            {
                // First we draw a background layer of the terminal as a hack.
                // In this background layer, everything but the current input pos
                // transparent.  We also insert a cursor char at that position.
                var hackInput = _input;
                if (_inputPos < hackInput.Length)
                {
                    hackInput = hackInput.Remove(_inputPos, 1);
                }
                
                hackInput = hackInput.Insert(_inputPos, $"{CURSOR_CHAR}\\{CURSOR_CHAR}{CURSOR_CHAR}");

                _myRenderer.DefaultColor = Color.Transparent;
                _myRenderer.SetColor(CURSOR_CHAR, ActiveTheme.MapConsoleColor(ConsoleColor.White));
                _myRenderer.DrawString(this, _text + hackInput, ActiveTheme.ConsoleFont, new Vector2(ContentBounds.Left, ContentBounds.Top), ContentBounds.Width);

            }
            else
            {
                _myRenderer.DrawString(this, Text, ActiveTheme.ConsoleFont, new Vector2(ContentBounds.Left, ContentBounds.Top), ContentBounds.Width);
            }
        }
    }
}
