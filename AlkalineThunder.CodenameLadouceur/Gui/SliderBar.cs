using System;
using System.Collections.Generic;
using System.Text;
using AlkalineThunder.Nucleus.Input;
using Microsoft.Xna.Framework;

namespace AlkalineThunder.Nucleus.Gui
{
    public sealed class SliderBar : Control
    {
        private float _sliderValue = 0;
        private bool _dragging = false;

        public event EventHandler SliderValueChanged;

        public SliderBar() : base()
        {
            GameLoop.Instance.Input.MouseMove += HandleMouseMove;
            GameLoop.Instance.Input.MouseUp += HandleMouseUp;
        }

        public float Value
        {
            get => _sliderValue;
            set
            {
                float clampedValue = MathHelper.Clamp(value, 0, 1);

                if(clampedValue != _sliderValue)
                {
                    _sliderValue = clampedValue;
                    SliderValueChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        protected override bool OnKeyDown(InputKeyEventArgs e)
        {
            if(IsFocused)
            {
                if(e.Key == Microsoft.Xna.Framework.Input.Keys.Left)
                {
                    Value -= 0.01f;
                    return true;
                }
                else if(e.Key == Microsoft.Xna.Framework.Input.Keys.Right)
                {
                    Value += 0.01f;
                    return true;
                }
                
            }
            return false;
        }

        protected override bool OnClick(MouseButtonEventArgs e)
        {
            return true;
        }

        private float GetSliderValueFromXPOsition(int x)
        {
            var nubStart = ContentBounds.Left + ActiveTheme.SliderRadius;
            var nubEnd = ContentBounds.Right - ActiveTheme.SliderRadius;

            if (x >= nubEnd) return 1;
            if (x <= nubStart) return 0;

            int xRel = x - nubStart;
            int width = nubEnd - nubStart;

            return (float)xRel / (float)width;
        }

        protected override Vector2 MeasureOverride()
        {
            return new Vector2(
                    0,
                    ActiveTheme.SliderRadius * 2
                );
        }

        protected override bool OnMouseDown(MouseButtonEventArgs e)
        {
            if(e.Button == MouseButton.Left)
            {
                Value = GetSliderValueFromXPOsition(e.X);
                _dragging = true;
                return true;
            }
            return false;
        }

        protected void HandleMouseMove(object sender, MouseMoveEventArgs e)
        {
            if(_dragging)
            {
                Value = GetSliderValueFromXPOsition(e.X);
            }
        }

        protected void HandleMouseUp(object sender, MouseButtonEventArgs e)
        {
            if(e.Button == MouseButton.Left && _dragging)
            {
                _dragging = false;
            }
        }

        protected override void OnDraw(GameTime gameTime)
        {
            var sliderColor = ActiveTheme.SliderBarColor;

            var sliderAxelHeight = ActiveTheme.SliderAxelHeight;

            var sliderAxelY = ContentBounds.Top + ((ContentBounds.Height - sliderAxelHeight) / 2);

            FillRectangle(new Rectangle(
                    ContentBounds.Left,
                    sliderAxelY,
                    ContentBounds.Width,
                    sliderAxelHeight
                ), sliderColor);

            var nubStartPos = ContentBounds.Left + ActiveTheme.SliderRadius;
            var nubEndPosition = ContentBounds.Right - ActiveTheme.SliderRadius;

            var nubX = MathHelper.Lerp(nubStartPos, nubEndPosition, _sliderValue);

            var nubY = ContentBounds.Top + (ContentBounds.Height / 2);

            FillCircle(new Vector2(nubX, nubY), ActiveTheme.SliderRadius, null, sliderColor);
        }
    }
}
