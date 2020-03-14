using AlkalineThunder.CodenameLadouceur.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlkalineThunder.CodenameLadouceur.Gui
{
    public sealed class Button : ContentControl
    {
        public Button()
        {
            Content = new Label("Button Text");
            Margin = new Padding(7, 4);
        }

        public int BorderThickness { get; set; } = 2;
        public bool IsPressed { get; private set; } = false;
        public bool IsHovered { get; private set; } = false;

        protected override bool OnMouseEnter(MouseMoveEventArgs e)
        {
            IsHovered = true;
            return base.OnMouseEnter(e);
        }

        protected override bool OnMouseLeave(MouseMoveEventArgs e)
        {
            IsPressed = false;
            IsHovered = false;
            return base.OnMouseLeave(e);
        }

        protected override bool OnMouseUp(MouseButtonEventArgs e)
        {
            if (e.Button == MouseButton.Left) IsPressed = false;
            return base.OnMouseUp(e);
        }

        protected override bool OnMouseDown(MouseButtonEventArgs e)
        {
            if (e.Button == MouseButton.Left) IsPressed = true;
            return base.OnMouseDown(e);
        }

        protected override Vector2 MeasureOverride()
        {
            if(Content != null)
            {
                var measure = Content.CalculateSize();
                return new Vector2(measure.X + (ActiveTheme.ButtonBorderThickness * 2), measure.Y + (ActiveTheme.ButtonBorderThickness * 2));
            }
            else
            {
                return new Vector2(ActiveTheme.ButtonBorderThickness * 2, ActiveTheme.ButtonBorderThickness * 2);
            }
        }

        protected override void OnDraw(GameTime gameTime)
        {
            var bgColor = ActiveTheme.ButtonBackgroundColor;

            if (IsHovered) bgColor = ActiveTheme.ButtonHoveredColor;
            if (IsPressed) bgColor = ActiveTheme.ButtonPressedColor;

            FillRectangle(Bounds, bgColor);
            DrawRectangle(Bounds, ActiveTheme.ButtonBorderColor, ActiveTheme.ButtonBorderThickness);
        }
    }
}
