using AlkalineThunder.MultiColorText;
using AlkalineThunder.Nucleus.Input;
using AlkalineThunder.Nucleus.Pty;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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

        private PseudoTerminal _master;
        private PseudoTerminal _slave;

        private StreamReader _stdin = null;
        private StreamWriter _stdout = null;

        public StreamReader StandardIn
        {
            get
            {
                if (_stdin == null)
                    _stdin = new StreamReader(_master);

                return _stdin;
            }
        }

        public StreamWriter StandardOut
        {
            get
            {
                if (_stdout == null)
                {
                    _stdout = new StreamWriter(_master);
                    _stdout.AutoFlush = true;
                }
                return _stdout;
            }
        }

        public TerminalEmulator()
        {
            PseudoTerminal.CreatePair(out _master, out _slave);
        }   

        protected override Vector2 MeasureOverride()
        {
            var text = Text;
            var maxCharsPerLine = 80;
            var minLines = 24;

            var sb = new StringBuilder();

            int ptr = 0;
            int lineWidth = 0;

            while (ptr < text.Length)
            {
                string word = "";

                for (int i = ptr; i < text.Length; i++)
                {
                    char c = text[i];
                    word += c;
                    if (char.IsWhiteSpace(c)) break;
                }

                ptr += word.Length;

                if (lineWidth + word.Length > maxCharsPerLine)
                {
                    lineWidth = 0;
                    sb.Append(Environment.NewLine);
                }

                while (word.Length > maxCharsPerLine)
                {
                    var wordLine = word.Substring(0, maxCharsPerLine);
                    sb.Append(Environment.NewLine + wordLine);
                    word = word.Remove(0, wordLine.Length);
                }

                lineWidth += word.Length;
                sb.Append(word);
            }

            var defaultChar = ActiveTheme.ConsoleFont.MeasureString("?");
            var minSize = new Vector2(defaultChar.X * maxCharsPerLine, defaultChar.Y * minLines);

            var actualSize = ActiveTheme.ConsoleFont.MeasureString(sb);

            MinHeight = (int)minSize.Y;

            return new Vector2(minSize.X, Math.Max(minSize.Y, actualSize.Y));
        }

        protected override bool OnClick(MouseButtonEventArgs e)
        {
            return true;
        }

        protected override bool OnKeyDown(InputKeyEventArgs e)
        {
            switch(e.Key)
            {
                case Microsoft.Xna.Framework.Input.Keys.Left:
                    if (_inputPos > 0) _inputPos--;
                    break;
                case Microsoft.Xna.Framework.Input.Keys.Right:
                    if (_inputPos < _input.Length) _inputPos++;
                    break;
                case Microsoft.Xna.Framework.Input.Keys.Home:
                    _inputPos = 0;
                    break;
                case Microsoft.Xna.Framework.Input.Keys.End:
                    _inputPos = _input.Length;
                    break;
                case Microsoft.Xna.Framework.Input.Keys.Back:
                    if(_inputPos > 0)
                    {
                        _inputPos--;
                        _input = _input.Remove(_inputPos, 1);
                    }
                    break;
                case Microsoft.Xna.Framework.Input.Keys.Delete:
                    if(_inputPos < _input.Length)
                    {
                        _input = _input.Remove(_inputPos, 1);
                    }
                    break;
                case Microsoft.Xna.Framework.Input.Keys.Enter:
                    SubmitInput();
                    break;
            }
            return true;
        }

        private void SubmitInput()
        {
            // Submit the input text to the slave stream.
            for(int i = 0; i < _input.Length; i++)
            {
                _slave.WriteByte((byte)_input[i]);
            }

            // Submit a newline.
            _slave.WriteByte((byte)'\r');
            _slave.WriteByte((byte)'\n');

            // clear input, move cursor to start.
            _inputPos = 0;
            _input = "";
        }

        protected override bool OnTextInput(TextInputEventArgs e)
        {
            if(ActiveTheme.ConsoleFont.Characters.Contains(e.Character))
            {
                _input = _input.Insert(_inputPos, e.Character.ToString());
                _inputPos++;
            }
            return true;
        }

        private string WordWrap(string text, float maxWidth)
        {
            if (maxWidth <= 0) return text;

            // Dear reader,
            //
            // If you've ever played Hacknet and been curious about how Matt did his
            // word-wrapping - instead of just enjoying the gameplay, you're weird.  So am
            // I.  I asked Matt how he did it and he explained the algorithm he settled
            // on.  This is that algorithm, and I'll level with you.  It's a lot better than
            // any word-wrapping code I've written on my own.
            //
            // Matt didn't give me any code, just an idea of what to do, so this is my implementation
            // of the Hacknet word wrapping code.
            //
            // - Michael, or as my English teacher calls me, "Lyfox."

            // Resulting wrapped string...
            StringBuilder sb = new StringBuilder();

            // Font to use
            var font = ActiveTheme.ConsoleFont;

            // Current line width.
            float lineWidth = 0;

            // Where are we in the text?
            int textPtr = 0;

            var glyphs = font.GetGlyphs();

            // Keep going till we're out of text.
            while (textPtr < text.Length)
            {
                // Current word...
                string word = "";

                // Scan from the beginning of the src string until we
                // hit a space or newline.
                for (int i = textPtr; i < text.Length; i++)
                {
                    char c = text[i];

                    // Append the char to the string.
                    word += c;

                    // If the char is a space or newline, end.
                    if (char.IsWhiteSpace(c)) break;
                }

                // Measure the word to get the width.
                float wordWidth = font.MeasureString(word.Replace(CURSOR_CHAR.ToString(), "")).X;

                // If the word can't fit on the current line then this is where we wrap.
                if (lineWidth + wordWidth > maxWidth && lineWidth > 0)
                {
                    sb.Append("\r\n");
                    lineWidth = 0;
                }

                // Now, while the word CAN'T fit on its own line, then we'll find a substring that can.
                int wordPtr = 0;
                while (wordWidth > maxWidth)
                {
                    int i = 0;
                    float lw = 0;
                    int p = 0;
                    for (i = wordPtr; i < word.Length; i++)
                    {
                        char c = word[wordPtr + p];
                        if(c != CURSOR_CHAR) 
                        {
                            float w = font.MeasureString(c.ToString()).X;
                            if (lw + w > maxWidth)
                            {
                                wordPtr += p;
                                wordWidth -= lw;

                                sb.Append("\r\n");
                                break;
                            }
                            lw += w;
                        }
                        sb.Append(c);
                        p++;
                    }
                }

                // Append the word and increment the line width.
                sb.Append(word.Substring(wordPtr));
                lineWidth += wordWidth;

                // If the word ends with a newline then line width is zero.
                if (word.EndsWith("\n"))
                    lineWidth = 0;

                // Advance the text pointer by the length of the word.
                textPtr += word.Length;
            }

            return sb.ToString();
        }

        protected override void OnDraw(GameTime gameTime)
        {
            FillRectangle(ContentBounds, ActiveTheme.MapConsoleColor(ConsoleColor.Black));

            // We're gonna do some custom rendering for the text. It's going to be fucking difficult.
            // But we'll do it.
            // Because we can.

            // Text to be drawn.  A marker character is inserted so we know where the cursor
            // should be.
            var text = _text + _input.Insert(_inputPos, CURSOR_CHAR.ToString());

            // Use our own word-wrapping so the spritefont doesn't yell at us trying to
            // measure our marker
            var wrapped = WordWrap(text, ContentBounds.Width);

            // Find, then remove, the cursor marker. But keep the index of it.
            int cursorIndex = wrapped.IndexOf(CURSOR_CHAR);
            wrapped = wrapped.Remove(cursorIndex, 1);

            // Render location.
            float x = ContentBounds.Left;
            float y = ContentBounds.Top;

            // Text colors.
            var fg = ConsoleColor.White;
            var bg = ConsoleColor.Black;

            // Font to use.
            var font = ActiveTheme.ConsoleFont;

            // Loop through every character of the string INCLUDING the null terminator that
            // would be there if C# strings worked that way.
            for(int i = 0; i <= wrapped.Length; i++)
            {
                char c = (i < wrapped.Length) ? wrapped[i] : '\0';

                var bgToUse = (i == cursorIndex && IsFocused) ? fg : bg;
                var fgToUse = (i == cursorIndex && IsFocused) ? bg : fg;

                // Handle newlines and other shit.
                switch(c)
                {
                    case '\r':
                        x = ContentBounds.Left;
                        break;
                    case '\n':
                        x = ContentBounds.Left;
                        y += font.LineSpacing;
                        break;
                }

                // Measure the character
                var measure = (c == '\0') ? font.MeasureString("?") : font.MeasureString(c.ToString());

                // Draw the background.
                FillRectangle(new Rectangle(
                        (int)x,
                        (int)y,
                        (int)measure.X,
                        (int)measure.Y
                    ), ActiveTheme.MapConsoleColor(bgToUse));

                // Should we try to draw a character...?
                if(font.Characters.Contains(c))
                {
                    // Draw it.
                    DrawString(font, c.ToString(), new Vector2(x, y), ActiveTheme.MapConsoleColor(fgToUse));

                    // Move the render pos forward.
                    x += measure.X;
                }
            }
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            int read = 0;
            while ((read = _slave.ReadByte()) > 0)
            {
                _text += (char)read;
                if(Parent is ScrollPanel sp)
                {
                    sp.ScrollToBottom();
                }
            }

            base.OnUpdate(gameTime);
        }
    }
}
