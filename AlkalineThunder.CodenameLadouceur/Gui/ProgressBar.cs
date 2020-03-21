using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace AlkalineThunder.Nucleus.Gui
{
    public sealed class ProgressBar : Control
    {
        private float _progressPercentage = 0;

        public event EventHandler ProgressChanged;

        public float Progress
        {
            get => _progressPercentage;
            set
            {
                float clampedValue = MathHelper.Clamp(value, 0, 1);

                if(clampedValue != _progressPercentage)
                {
                    _progressPercentage = clampedValue;
                    ProgressChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        protected override Vector2 MeasureOverride()
        {
            return new Vector2(
                    0,
                    ActiveTheme.ProgressBarHeight
                );
        }

        protected override void OnDraw(GameTime gameTime)
        {
            var bgBrush = ActiveTheme.ProgressBarBrush;
            var fgBrush = ActiveTheme.ProgressBrush;

            DrawBrush(ContentBounds, bgBrush);

            var progressWidth = (int)(ContentBounds.Width * _progressPercentage);

            DrawBrush(new Rectangle(
                    ContentBounds.Left,
                    ContentBounds.Top,
                    progressWidth,
                    ContentBounds.Height
                ), fgBrush);
        }
    }
}
