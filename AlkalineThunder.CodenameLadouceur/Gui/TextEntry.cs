using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlkalineThunder.CodenameLadouceur.Gui
{
    public sealed class TextEntry : Control
    {
        private string _text = string.Empty;
        private int _cursorPos = 0;

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
                }
            }
        }

        public int CursorPos => _cursorPos;

        protected override bool OnTextInput(TextInputEventArgs e)
        {
            var font = ActiveTheme.TextEntryFont;

            if(font.Characters.Contains(e.Character))
            {
                _text = _text.Insert(_cursorPos, e.Character.ToString());
                _cursorPos++;
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
                    }
                    break;
                case Microsoft.Xna.Framework.Input.Keys.Back:
                    if(CursorPos > 0)
                    {
                        _text = _text.Remove(_cursorPos, 1);
                        _cursorPos--;
                    }
                    break;
            }

            return base.OnKeyDown(e);
        }
    }
}
