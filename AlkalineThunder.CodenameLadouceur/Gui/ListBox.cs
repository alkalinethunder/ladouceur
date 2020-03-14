using AlkalineThunder.CodenameLadouceur.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace AlkalineThunder.CodenameLadouceur.Gui
{
    public sealed class ListBox : Control
    {
        private int _selectedIndex = -1;

        public class ListBoxCollection : ICollection<string>
        {
            private List<string> _items = new List<string>();
            private ListBox _owner = null;

            public ListBoxCollection(ListBox owner)
            {
                _owner = owner ?? throw new ArgumentNullException(nameof(owner));
            }

            public int Count => _items.Count;

            public bool IsReadOnly => false;

            public string this[int index]
            {
                get => _items[index];
            }

            public void Add(string item)
            {
                _items.Add(item);
                if (_owner.SelectedIndex == -1) _owner.SelectedIndex = 0;
            }

            public void Clear()
            {
                _items.Clear();
                _owner.SelectedIndex = -1;
            }

            public bool Contains(string item)
            {
                return _items.Contains(item);
            }

            public void CopyTo(string[] array, int arrayIndex)
            {
                _items.CopyTo(array, arrayIndex);
            }

            public IEnumerator<string> GetEnumerator()
            {
                return _items.GetEnumerator();
            }

            public bool Remove(string item)
            {
                if(_items.Remove(item))
                {
                    if(_owner.SelectedIndex >= Count)
                    {
                        _owner.SelectedIndex = Count - 1;
                    }

                    return true;
                }
                else
                {
                    return false;
                }
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return _items.GetEnumerator();
            }
        }

        public ListBox()
        {
            Items = new ListBoxCollection(this);
        }

        public ListBoxCollection Items { get; private set; }
        public int SelectedIndex
        {
            get => _selectedIndex;
            set
            {
                if(_selectedIndex != value)
                {
                    if(value < -1)
                    {
                        _selectedIndex = -1;
                    }
                    else if(value > Items.Count - 1)
                    {
                        _selectedIndex = Items.Count - 1;
                    }
                    else
                    {
                        _selectedIndex = value;
                    }

                    SelectedIndexChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }
        public SpriteFont Font { get; set; }
        public string SelectedItem => SelectedIndex == 1 ? null : Items[SelectedIndex];


        public event EventHandler SelectedIndexChanged;

        private string StripNewLines(string text)
        {
            return text.Replace("\r", "").Replace("\n", "");
        }

        protected override Vector2 MeasureOverride()
        {
            float x = 0;
            float y = 0;

            var font = Font ?? ActiveTheme.DefaultFont;

            foreach(var item in Items)
            {
                var text = StripNewLines(item);
                var measure = font.MeasureString(text);

                y += measure.Y;
                x = Math.Max(x, measure.X);
            }

            return new Vector2(x + (ActiveTheme.ListBoxBorderThickness * 2), y + (ActiveTheme.ListBoxBorderThickness * 2));
        }

        protected override void OnDraw(GameTime gameTime)
        {
            var font = Font ?? ActiveTheme.DefaultFont;

            float x = Bounds.Left + ActiveTheme.ListBoxBorderThickness;
            float y = Bounds.Top + ActiveTheme.ListBoxBorderThickness;

            DrawRectangle(Bounds, ActiveTheme.ListBoxBorderColor, ActiveTheme.ListBoxBorderThickness);

            for(int i = 0; i < Items.Count; i++)
            {
                var text = StripNewLines(Items[i]);

                var textColor = (i == SelectedIndex) ? ActiveTheme.ListBoxSelectedItemColor : ActiveTheme.ListBoxItemColor;

                var measure = font.MeasureString(text);

                if(i == SelectedIndex)
                {
                    FillRectangle(new Rectangle((int)x, (int)y, Bounds.Width - (ActiveTheme.ListBoxBorderThickness * 2), (int)measure.Y), ActiveTheme.ListBoxSelectedHighlightColor);
                }

                DrawString(font, text, new Vector2(x, y), textColor);

                y += measure.Y;
            }

            base.OnDraw(gameTime);
        }

        protected override bool OnMouseUp(MouseButtonEventArgs e)
        {
            if(e.Button == MouseButton.Left)
            {
                var font = Font ?? ActiveTheme.DefaultFont;

                for(int i = 0; i < Items.Count; i++)
                {
                    var text = StripNewLines(Items[i]);

                    var height = font.MeasureString(text).Y;

                    var rect = new Rectangle(
                        Bounds.Left + ActiveTheme.ListBoxBorderThickness,
                        Bounds.Top + ActiveTheme.ListBoxBorderThickness + ((int)height * i),
                        Bounds.Width - (ActiveTheme.ListBoxBorderThickness * 2),
                        (int)height
                        );

                    if(e.X >= rect.Left && e.X <= rect.Right && e.Y >= rect.Top && e.Y <= rect.Bottom)
                    {
                        SelectedIndex = i;
                        return true;
                    }
                }
            }
            return base.OnMouseUp(e);
        }

        protected override bool OnKeyDown(InputKeyEventArgs e)
        {
            if(Items.Count > 0)
            {
                switch(e.Key)
                {
                    case Microsoft.Xna.Framework.Input.Keys.PageUp:
                        SelectedIndex = 0;
                        break;
                    case Microsoft.Xna.Framework.Input.Keys.PageDown:
                        SelectedIndex = Items.Count - 1;
                        break;
                    case Microsoft.Xna.Framework.Input.Keys.Up:
                        if(SelectedIndex > 0)
                        {
                            SelectedIndex--;
                        }
                        break;
                    case Microsoft.Xna.Framework.Input.Keys.Down:
                        if(SelectedIndex < Items.Count - 1)
                        {
                            SelectedIndex++;
                        }
                        break;
                }
            }
            return base.OnKeyDown(e);
        }
    }
}
