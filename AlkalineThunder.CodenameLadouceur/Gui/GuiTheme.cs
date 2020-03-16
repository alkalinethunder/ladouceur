using System;
using System.Collections.Generic;
using System.Text;
using AlkalineThunder.CodenameLadouceur.Rendering;
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
        public abstract Brush SelectionBrush { get; }
        public abstract Brush ButtonBrush { get; }
        public abstract Brush ButtonHoveredBrush { get; }
        public abstract Brush ButtonPressedBrush { get; }
        public abstract SpriteFont TextEntryFont { get; }
        public abstract Color TextEntryHintColor { get; }
        public abstract Color TextEntryTextColor { get; }
        public abstract Color ListBoxItemColor { get; }
        public abstract Color ListBoxSelectedItemColor { get; }
        public abstract Brush ListBoxBrush { get; }
        public abstract int CheckSize { get; }
        public abstract int CheckBorderThickness { get; }

        public abstract int ScrollBarWidth { get; }
        public abstract Brush ScrollBarBrush { get; }

        public abstract void LoadContent(ContentManager content);

        public abstract Brush TextEntryBrush { get; }
        public abstract Brush TextEntryHoveredBrush { get; }
        public abstract Brush TextEntryFocusedBrush { get; }
    }
}
