﻿using AlkalineThunder.Nucleus.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlkalineThunder.Nucleus.Gui
{
    public sealed class TextEntry : Control
    {
        private string _text = string.Empty;
        private int _cursorPos = 0;
        private bool _caretVisible = false;
        private double _caretTime = 0;
        private float _offset = 0;

        public string HintText { get; set; } = "Enter text here...";
        public bool IsPassword { get; set; } = false;

        public event EventHandler<TextChangedEventArgs> TextChanged;

        private void FireTextChanged()
        {
            TextChanged?.Invoke(this, new TextChangedEventArgs(Text));
        }

        public string Text
        {
            get => _text;
            set
            {
                if(_text != value)
                {
                    if (string.IsNullOrEmpty(value)) 
                        _text = string.Empty;
                    else 
                        _text = value;

                    if (_cursorPos > _text.Length)
                        _cursorPos = _text.Length;

                    FireTextChanged();
                }
            }
        }

        public int CursorPos => _cursorPos;
        public SpriteFont Font { get; set; } = null;
        public double CaretBlinkSeconds { get; set; } = 0.25;

        private void UpdateTextOffset()
        {
            var toCaret = _text.Substring(0, _cursorPos);

            if (IsPassword)
            {
                int l = toCaret.Length;
                for (int i = 0; i < l; i++)
                {
                    toCaret = toCaret.Remove(0, 1) + "*";
                }
            }

            var font = Font ?? ActiveTheme.DefaultFont;

            var measure = font.MeasureString(toCaret);

            var realCaret = measure.X - _offset;

            if(realCaret > Bounds.Width)
            {
                _offset = measure.X - Bounds.Width;
            }
            else if(realCaret < 0)
            {
                _offset = 0;
            }

        }

        protected override void OnUpdate(GameTime gameTime)
        {
            if(IsFocused)
            {
                _caretTime += gameTime.ElapsedGameTime.TotalSeconds;
                if(_caretTime >= CaretBlinkSeconds)
                {
                    _caretTime = 0;
                    _caretVisible = !_caretVisible;
                }
            }

            UpdateTextOffset();

            base.OnUpdate(gameTime);
        }

        protected override Vector2 MeasureOverride()
        {
            var font = Font ?? ActiveTheme.DefaultFont;

            if(IsPassword)
            {
                if(!string.IsNullOrEmpty(Text))
                {
                    var charMeasure = font.MeasureString("*");
                    return new Vector2((charMeasure.X * Text.Length) + ActiveTheme.TextEntryBrush.Margin.Width, charMeasure.Y + ActiveTheme.TextEntryBrush.Margin.Height);
                }
            }

            var text = string.IsNullOrEmpty(Text) ? HintText : Text;

            return font.MeasureString(text) + new Vector2(ActiveTheme.TextEntryBrush.Margin.Width, ActiveTheme.TextEntryBrush.Margin.Height);
        }

        protected override bool OnTextInput(TextInputEventArgs e)
        {
            var font = ActiveTheme.TextEntryFont;

            if(font.Characters.Contains(e.Character))
            {
                _text = _text.Insert(_cursorPos, e.Character.ToString());
                _cursorPos++;
                FireTextChanged();
            }

            return true;
        }

        protected override bool OnKeyDown(InputKeyEventArgs e)
        {
            switch(e.Key)
            {
                case Microsoft.Xna.Framework.Input.Keys.Home:
                case Microsoft.Xna.Framework.Input.Keys.PageUp:
                    _cursorPos = 0;
                    break;
                case Microsoft.Xna.Framework.Input.Keys.End:
                case Microsoft.Xna.Framework.Input.Keys.PageDown:
                    _cursorPos = _text.Length;
                    break;
                case Microsoft.Xna.Framework.Input.Keys.Delete:
                    if(_cursorPos < _text.Length)
                    {
                        _text = _text.Remove(_cursorPos, 1);
                        FireTextChanged();
                    }
                    break;
                case Microsoft.Xna.Framework.Input.Keys.Back:
                    if(CursorPos > 0)
                    {
                        _cursorPos--;
                        _text = _text.Remove(_cursorPos, 1);
                        FireTextChanged();
                    }
                    break;
                case Microsoft.Xna.Framework.Input.Keys.Left:
                    if (_cursorPos > 0) _cursorPos--;
                    break;
                case Microsoft.Xna.Framework.Input.Keys.Right:
                    if (_cursorPos < _text.Length) _cursorPos++;
                    break;
            }

            return base.OnKeyDown(e);
        }

        protected override bool OnMouseEnter(MouseMoveEventArgs e)
        {
            IsHovered = true;
            return base.OnMouseEnter(e);
        }

        public bool IsHovered { get; private set; } = false;

        protected override bool OnMouseLeave(MouseMoveEventArgs e)
        {
            IsHovered = false;
            return base.OnMouseLeave(e);
        }

        protected override bool OnGainedFocus(FocusEventArgs e)
        {
            _caretVisible = true;
            _caretTime = 0;
            return base.OnGainedFocus(e);
        }

        protected override bool OnClick(MouseButtonEventArgs e)
        {
            if(e.Button == MouseButton.Left)
            {
                if(string.IsNullOrEmpty(_text))
                {
                    _cursorPos = 0;
                    _offset = 0;
                    return true;
                }

                var xPos = e.X;

                var font = Font ?? ActiveTheme.DefaultFont;

                float measure = 0;
                for(int i = 0; i < _text.Length; i++)
                {
                    if(IsPassword)
                    {
                        measure += font.MeasureString("*").X;
                    }
                    else
                    {
                        measure += font.MeasureString(_text[i].ToString()).X;
                    }

                    if (Bounds.Left + measure >= xPos)
                    {
                        _cursorPos = i;
                        return true;
                    }
                }

                _cursorPos = _text.Length;
            }
            return base.OnMouseUp(e);
        }

        protected override void OnDraw(GameTime gameTime)
        {
            var brush = ActiveTheme.TextEntryBrush;

            if (IsHovered) brush = ActiveTheme.TextEntryHoveredBrush;
            if (IsFocused) brush = ActiveTheme.TextEntryFocusedBrush;

            DrawBrush(Bounds, brush);

            var font = Font ?? ActiveTheme.DefaultFont;

            var color = string.IsNullOrEmpty(Text) ? ActiveTheme.TextEntryHintColor : ActiveTheme.TextEntryTextColor;

            var text = "";

            if(IsPassword && !string.IsNullOrEmpty(Text))
            {
                for(int i = 0; i < Text.Length; i++)
                {
                    text += "*";
                }
            }
            else
            {
                text = string.IsNullOrEmpty(Text) ? HintText : Text;
            }

            var measure = font.MeasureString(text);

            var y = Bounds.Top + brush.Margin.Top;

            DrawString(font, text, new Vector2(Bounds.Left - (int)_offset, y), color);

            if(IsFocused && _caretVisible)
            {
                var toCaret = text.Substring(0, _cursorPos);
                var toCaretMeasure = font.MeasureString(toCaret);

                FillRectangle(new Rectangle((Bounds.Left + (int)toCaretMeasure.X) - (int)_offset, (int)y, 1, (int)measure.Y), ActiveTheme.TextEntryTextColor);
            }

            base.OnDraw(gameTime);
        }
    }
}
