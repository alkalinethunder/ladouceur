using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlkalineThunder.CodenameLadouceur.Gui
{
    public sealed class CheckBox : ContentControl
    {
        private const int _contentSpacing = 3;

        protected override Vector2 MeasureOverride()
        {
            var contentSize = (Content != null) ? Content.CalculateSize() : Vector2.Zero;

            var checkSize = new Vector2(ActiveTheme.CheckSize, ActiveTheme.CheckSize);

            if(Content != null)
            {
                return new Vector2(checkSize.X + _contentSpacing + contentSize.X, Math.Max(checkSize.Y, contentSize.Y));
            }
            else
            {
                return checkSize;
            }
        }

        protected override void ArrangeOverride()
        {
            if(Content != null)
            {
                PlaceControl(Content, new Rectangle(
                        ContentBounds.Left + ActiveTheme.CheckSize + _contentSpacing,
                        ContentBounds.Top,
                        ContentBounds.Width - ActiveTheme.CheckSize - _contentSpacing,
                        ContentBounds.Height
                    ));
            }
        }

        protected override void OnDraw(GameTime gameTime)
        {
            DrawRectangle(new Rectangle(
                    Bounds.Left,
                    Bounds.Top + ((Bounds.Height - ActiveTheme.CheckSize) / 2),
                    ActiveTheme.CheckSize,
                    ActiveTheme.CheckSize
                ), ActiveTheme.DefaultForeground, ActiveTheme.CheckBorderThickness);
        }
    }
}
