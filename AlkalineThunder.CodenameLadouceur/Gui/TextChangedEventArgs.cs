using System;

namespace AlkalineThunder.Nucleus.Gui
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
