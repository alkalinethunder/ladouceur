using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace AlkalineThunder.CodenameLadouceur.Gui
{
    public abstract class GuiTheme{

        public abstract SpriteFont DefaultFont { get; }
        public abstract SpriteFont TitleFont { get; }
        public abstract SpriteFont BigFont { get; }
        public abstract SpriteFont SmallFont { get; }
        public abstract SpriteFont ConsoleFont { get; }

        public abstract Color DefaultBackground { get; }
        public abstract Color DefaultForeground { get; }
        public abstract int ButtonBorderThickness { get; }
        public abstract Color ButtonBorderColor { get; }
        public abstract Color ButtonBackgroundColor { get; }
        public abstract Color ButtonHoveredColor { get; }
        public abstract Color ButtonPressedColor { get; }
        public abstract SpriteFont TextEntryFont { get; }
        public abstract Color TextEntryHintColor { get; }
        public abstract Color TextEntryTextColor { get; }
        public abstract Color ListBoxItemColor { get; }
        public abstract Color ListBoxSelectedItemColor { get; }
        public abstract Color ListBoxSelectedHighlightColor { get; }
        public abstract Color ListBoxBorderColor { get; }
        public abstract int ListBoxBorderThickness { get; }

        public abstract int CheckSize { get; }
        public abstract int CheckBorderThickness { get; }

        public abstract int ScrollBarWidth { get; }
        public abstract Color ScrollBarColor { get; }

        public abstract void LoadContent(ContentManager content);


    }
}
