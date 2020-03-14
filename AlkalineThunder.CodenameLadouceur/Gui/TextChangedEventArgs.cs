using System;

namespace AlkalineThunder.CodenameLadouceur.Gui
{
    public sealed class TextChangedEventArgs : EventArgs
    {
        public string Text { get; private set; }

        public TextChangedEventArgs(string text)
        {
            Text = text;
        }
    }
}
