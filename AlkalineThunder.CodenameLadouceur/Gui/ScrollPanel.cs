﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlkalineThunder.CodenameLadouceur.Gui
{
    public sealed class ScrollPanel : Control, IContentControl
    {
        private Control _content = null;
        private int _scrollOffset = 0;

        protected override bool SupportsChildren => true;

        public Control Content
        {
            get => _content;
            set
            {
                if(_content != value)
                {
                    if(_content != null)
                    {
                        InternalChildren.Remove(_content);
                    }

                    _content = value;

                    if(_content != null)
                    {
                        InternalChildren.Add(_content);
                        _content.HorizontalAlignment = HorizontalAlignment.Stretch;
                        _content.VerticalAlignment = VerticalAlignment.Top;
                    }
                }
            }
        }

        protected override Vector2 MeasureOverride()
        {
            return Vector2.Zero;
        }

        public void ScrollToTop()
        {
            _scrollOffset = 0;
        }

        public void ScrollToBottom()
        {
            if(_content != null)
            {
                if(_content.DesiredSize.Y >= ContentBounds.Height)
                {
                    _scrollOffset = (int)_content.DesiredSize.Y - ContentBounds.Height;
                }
                else
                {
                    _scrollOffset = 0;
                }
            }
            else
            {
                _scrollOffset = 0;
            }
        }

        protected override void ArrangeOverride()
        {
            if(_content != null)
            {
                var contentSize = _content.CalculateSize();

                if(contentSize.Y > Content.Bounds.Height)
                {
                    PlaceControl(_content, new Rectangle(
                            ContentBounds.Left,
                            ContentBounds.Top - _scrollOffset,
                            ContentBounds.Width - ActiveTheme.ScrollBarWidth,
                            (int)contentSize.Y
                        ));
                }
                else
                {
                    _scrollOffset = 0;
                    PlaceControl(_content, new Rectangle(
                            ContentBounds.Left,
                            ContentBounds.Top,
                            ContentBounds.Width,
                            (int)contentSize.Y
                        ));
                }
            }
        }

        protected override void OnDraw(GameTime gameTime)
        {
            if(_content != null)
            {
                if (_content.DesiredSize.Y > ContentBounds.Height)
                {
                    var scrollTotalHeight = _content.DesiredSize.Y;
                    var viewHeight = ContentBounds.Height;

                    var viewHeightPercentage = viewHeight / scrollTotalHeight;
                    var offsetPercentage = _scrollOffset / scrollTotalHeight;

                    var offsetDrawLoc = MathHelper.Lerp(Bounds.Top, Bounds.Bottom, offsetPercentage);
                    var nubDrawHeight = MathHelper.Lerp(0, Bounds.Height, viewHeightPercentage);

                    FillRectangle(new Rectangle(
                            Bounds.Right - ActiveTheme.ScrollBarWidth,
                            (int)offsetDrawLoc,
                            ActiveTheme.ScrollBarWidth,
                            (int)nubDrawHeight
                        ), ActiveTheme.ScrollBarColor);
                }
            }
        }
    }
}
