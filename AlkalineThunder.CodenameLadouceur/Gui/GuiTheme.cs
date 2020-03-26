using System;
using System.Collections.Generic;
using System.Text;
using AlkalineThunder.Nucleus.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace AlkalineThunder.Nucleus.Gui
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

        public abstract Color ToggleSwitchBackgroundColor { get; }
        public abstract Color ToggleSwitchForegroundColor { get; }
        public abstract Color ToggleSwitchActiveBackgroundColor { get; }

        public virtual int ToggleSwitchNubRadius { get; } = 12;
        public virtual int ToggleSwitchNubPadding { get; } = 2;

        public virtual int ProgressBarHeight => 4;

        public abstract Brush ProgressBarBrush { get; }
        public abstract Brush ProgressBrush { get; }

        public abstract Color SliderBarColor { get; }
        public virtual int SliderRadius { get; } = 10;
        public virtual int SliderAxelHeight { get; } = 3;

        public virtual Color ConsoleBlack => DefaultBackground;
        public virtual Color ConsoleWhite => DefaultForeground;
        public virtual Color ConsoleGray => Color.Gray;
        public virtual Color ConsoleDarkGray => Color.DarkGray;
        public virtual Color ConsoleDarkRed => Color.DarkRed;
        public virtual Color ConsoleDarkGreen => Color.DarkGreen;
        public virtual Color ConsoleDarkBlue => Color.DarkBlue;
        public virtual Color ConsoleDarkCyan => Color.DarkCyan;
        public virtual Color ConsoleDarkYellow => Color.Orange;
        public virtual Color ConsoleDarkMagenta => Color.DarkMagenta;
        public virtual Color ConsoleRed => Color.Red;
        public virtual Color ConsoleGreen => Color.Green;
        public virtual Color ConsoleBlue => Color.Blue;
        public virtual Color ConsoleYellow => Color.Yellow;
        public virtual Color ConsoleMagenta => Color.Magenta;
        public virtual Color ConsoleCyan => Color.Cyan;

        public virtual Color WindowInactiveColor => DefaultBackground;
        public virtual Color WindowActiveColor => SelectionBrush.BrushColor;
        public virtual Color WindowBackground => DefaultBackground;

        public Color MapConsoleColor(ConsoleColor consoleColor)
        {
            switch (consoleColor)
            {
                case ConsoleColor.Black:
                    return ConsoleBlack;
                case ConsoleColor.White:
                    return ConsoleWhite;
                case ConsoleColor.Gray:
                    return ConsoleGray;
                case ConsoleColor.Red:
                    return ConsoleRed;
                case ConsoleColor.Green:
                    return ConsoleGreen;
                case ConsoleColor.Blue:
                    return ConsoleBlue;
                case ConsoleColor.Cyan:
                    return ConsoleCyan;
                case ConsoleColor.DarkGray:
                    return ConsoleDarkGray;
                case ConsoleColor.Magenta:
                    return ConsoleMagenta;
                case ConsoleColor.Yellow:
                    return ConsoleYellow;
                case ConsoleColor.DarkRed:
                    return ConsoleDarkRed;
                case ConsoleColor.DarkGreen:
                    return ConsoleDarkGreen;
                case ConsoleColor.DarkBlue:
                    return ConsoleDarkBlue;
                case ConsoleColor.DarkCyan:
                    return ConsoleDarkCyan;
                case ConsoleColor.DarkMagenta:
                    return ConsoleDarkMagenta;
                default:
                    return ConsoleWhite;

            }
        }

    }
}
