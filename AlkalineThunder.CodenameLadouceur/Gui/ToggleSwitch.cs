using AlkalineThunder.CodenameLadouceur.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlkalineThunder.CodenameLadouceur.Gui
{
    public sealed class ToggleSwitch : ContentControl
    {
        private bool _toggleState = false;

        public event EventHandler ToggleStatechanged;

        public bool ToggleState
        {
            get => _toggleState;
            set
            {
                if(_toggleState != value)
                {
                    _toggleState = value;
                    ToggleStatechanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public int ContentSpacing { get; set; } = 8;

        protected override bool OnClick(MouseButtonEventArgs e)
        {
            ToggleState = !ToggleState;
            return base.OnClick(e);
        }

        protected override Vector2 MeasureOverride()
        {
            // Nub is a circle, so the width and height are both uniform.
            var nubSize = ActiveTheme.ToggleSwitchNubRadius * 2;

            // Height of the rounded switch.
            var switchHeight = nubSize + (ActiveTheme.ToggleSwitchNubPadding * 2);

            // Width of the rounded switch.
            var switchWidth = (nubSize * 2) + (ActiveTheme.ToggleSwitchNubPadding * 2);

            if(Content != null)
            {
                var contentMeasure = Content.CalculateSize();

                return new Vector2(
                        switchWidth + ContentSpacing + contentMeasure.X,
                        Math.Max(switchHeight, contentMeasure.Y)
                    ); ;
            }
            else
            {
                return new Vector2(switchWidth, switchHeight);
            }
        }

        protected override void OnContentChanged(Control content)
        {
            base.OnContentChanged(content);

            if(Content != null)
            {
                Content.VerticalAlignment = VerticalAlignment.Middle;
            }
        }

        protected override void ArrangeOverride()
        {
            if(Content != null)
            {
                var switchWidthAndSpace = ContentSpacing + (ActiveTheme.ToggleSwitchNubRadius * 2) + (ActiveTheme.ToggleSwitchNubRadius * 2);

                PlaceControl(Content, new Rectangle(
                        ContentBounds.Left + switchWidthAndSpace,
                        ContentBounds.Top,
                        ContentBounds.Width - switchWidthAndSpace,
                        ContentBounds.Height
                    ));
            }
        }

        protected override void OnDraw(GameTime gameTime)
        {
            var nubRadius = ActiveTheme.ToggleSwitchNubRadius;
            var nubPadding = ActiveTheme.ToggleSwitchNubPadding;

            var nubSize = nubRadius * 2;
            var switchHeight = nubSize + (nubPadding * 2);
            var switchWidth = (nubSize * 2) + (nubPadding * 2);

            var switchTop = Bounds.Top + ((Bounds.Height - switchHeight) / 2);
            var switchLeft = Bounds.Left;

            var switchBackground = ToggleState ? ActiveTheme.ToggleSwitchActiveBackgroundColor : ActiveTheme.ToggleSwitchBackgroundColor;

            DrawRoundedRectangle(new Rectangle(
                    switchLeft,
                    switchTop,
                    switchWidth,
                    switchHeight
                ), switchBackground, 1);

            var nubTop = switchTop + nubPadding + nubRadius;
            var nubLeft = !ToggleState
                ? switchLeft + nubPadding + nubRadius
                : (switchLeft + switchWidth) - nubPadding - nubRadius;

            FillCircle(new Vector2(nubLeft, nubTop), nubRadius, null, ActiveTheme.ToggleSwitchForegroundColor);
            
        }
    }
}
