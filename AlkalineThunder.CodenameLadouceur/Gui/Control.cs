using AlkalineThunder.CodenameLadouceur.Input;
using AlkalineThunder.CodenameLadouceur.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace AlkalineThunder.CodenameLadouceur.Gui
{
    public abstract class Control : IRenderer
    {
        public sealed class ControlCollection : ICollection<Control>
        {
            private List<Control> _children = new List<Control>();
            private Control _owner = null;

            public ControlCollection(Control owner)
            {
                _owner = owner ?? throw new InvalidOperationException("Owner control is null.");
            }

            public int Count => _children.Count;

            public bool IsReadOnly => !_owner.SupportsChildren;

            public Control this[int index]
            {
                get => this._children[index];
            }

            public void Add(Control item)
            {
                if (item == null) throw new ArgumentNullException(nameof(item));
                if (IsReadOnly) throw new InvalidOperationException("Control doesn't support children.");
                if (item.Parent != null) throw new InvalidOperationException("Control already has a parent.");

                item.Parent = _owner;
                _children.Add(item);
            }

            public void Clear()
            {
                while(_children.Count > 0)
                {
                    Remove(_children[0]);
                }
            }

            public bool Contains(Control item)
            {
                return item != null && item.Parent == _owner;
            }

            public void CopyTo(Control[] array, int arrayIndex)
            {
                _children.CopyTo(array, arrayIndex);
            }

            public IEnumerator<Control> GetEnumerator()
            {
                return _children.GetEnumerator();
            }

            public bool Remove(Control item)
            {
                if (item == null) throw new ArgumentNullException(nameof(item));
                if (item.Parent != _owner) throw new InvalidOperationException("Control isn't parented to this control.");

                item.Parent = null;
                _children.Remove(item);

                return true;
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return _children.GetEnumerator();
            }
        }

        private Renderer _renderer = null;

        public static GuiTheme ActiveTheme { get; private set; }
        public Padding Padding { get; set; } = 0;
        public Padding Margin { get; set; } = 0;
        public int MinWidth { get; set; }
        public int MinHeight { get; set; }
        public int MaxWidth { get; set; }
        public int MaxHeight { get; set; }

        public int Width { get; set; }
        public int Height { get; set; }

        protected virtual bool SupportsChildren => false;
        protected ControlCollection InternalChildren { get; } = null;
        public Control Parent { get; private set; }
        public HorizontalAlignment HorizontalAlignment { get; set; }
        public VerticalAlignment VerticalAlignment { get; set; }
        public Rectangle Bounds { get; private set; }
        public Vector2 DesiredSize { get; private set; }
        
        public Control()
        {
            if (ActiveTheme == null) throw new InvalidOperationException("You must load a theme before you can use the GUI.  Michael, you're retarded.  Your game loop should've never let this happen.  God damnit.");
            InternalChildren = new ControlCollection(this);
        }

        public static void LoadTheme<T>(ContentManager content) where T : GuiTheme, new()
        {
            if (content == null) throw new ArgumentNullException(nameof(content));

            Control.ActiveTheme = new T();
            Control.ActiveTheme.LoadContent(content);
        }

        public void DrawString(SpriteFont font, string text, Vector2 pos, Color color)
        {
            _renderer.DrawString(font, text, pos, color);
        }

        public void FillRectangle(Rectangle rect, Texture2D texture, Color color)
        {
            _renderer.FillRectangle(rect, texture, color);
        }

        public void FillRectangle(Rectangle rect, Color color)
        {
            _renderer.FillRectangle(rect, color);
        }

        public bool MouseEnter(MouseMoveEventArgs e)
        {
            return false;
        }

        public bool LostFocus(FocusEventArgs e)
        {
            return true;
        }

        public bool KeyUp(InputKeyEventArgs e)
        {
            return true;
        }

        public bool KeyDown(InputKeyEventArgs e)
        {
            return true;
        }

        public bool TextInput(TextInputEventArgs e)
        {
            return true;
        }

        public bool GainedFocus(FocusEventArgs e)
        {
            return true;
        }

        public bool MouseDown(MouseButtonEventArgs e)
        {
            return true;
        }

        public bool MouseUp(MouseButtonEventArgs e)
        {
            return true;
        }

        public bool Click(MouseButtonEventArgs e)
        {
            return true; ;
        }

        public bool MouseLeave(MouseMoveEventArgs e)
        {
            return false;
        }

        public bool MouseMove(MouseMoveEventArgs e)
        {
            return false;
        }

        public bool HasParent(Control control)
        {
            var parent = this.Parent;
            while(parent != null)
            {
                if(parent == control)
                {
                    return true;
                }
                else
                {
                    parent = parent.Parent;
                }
            }

            return false;
        }

        protected virtual void OnUpdate(GameTime gameTime)
        {

        }

        public Control FindControl(int x, int y)
        {
            for(int i = this.InternalChildren.Count - 1; i >= 0; i--)
            {
                var child = this.InternalChildren[i];
                var foundInChild = child.FindControl(x, y);
                if (foundInChild != null) return foundInChild;
            }

            if(x >= this.Bounds.Left && x <= this.Bounds.Right && y >= this.Bounds.Top && y <= this.Bounds.Bottom)
            {
                return this;
            }
            else
            {
                return null;
            }
        }

        protected virtual void OnDraw(GameTime gameTime)
        {

        }

        public void DrawRectangle(Rectangle bounds, Color color, int thickness)
        {
            _renderer.DrawRectangle(bounds, color, thickness);
        }

        public void Update(GameTime gameTime)
        {
            OnUpdate(gameTime);
            foreach (var child in InternalChildren) child.Update(gameTime);
        }

        protected virtual Vector2 MeasureOverride()
        {
            return Vector2.Zero;
        }

        protected virtual void ArrangeOverride()
        {

        }

        public void Draw(GameTime gameTime, Renderer renderer)
        {
            if (_renderer != null) throw new InvalidOperationException("Control is already drawing.");
            _renderer = renderer;
            _renderer.Begin();
            OnDraw(gameTime);
            _renderer.End();

            foreach (var child in InternalChildren) child.Draw(gameTime, _renderer);

            _renderer = null;
        }

        public Vector2 CalculateSize()
        {
            var desiredSize = MeasureOverride() + Padding + Margin;

            if (desiredSize.X < MinWidth) desiredSize.X = MinWidth;
            if (desiredSize.Y < MinHeight) desiredSize.Y = MinHeight;
            if (MaxWidth > 0 && desiredSize.X > MaxWidth) desiredSize.X = MaxWidth;
            if (MaxHeight > 0 && desiredSize.Y > MaxHeight) desiredSize.Y = MaxHeight;

            DesiredSize = desiredSize;

            return new Vector2(Width == 0 ? DesiredSize.X : Width, Height == 0 ? DesiredSize.Y : Height);
        }

        public static void PlaceControl(Control control, Rectangle bounds)
        {
            var actualSize = control.CalculateSize();

            var finalBounds = new Rectangle();

            finalBounds.Width = (control.HorizontalAlignment == HorizontalAlignment.Stretch) ? bounds.Width : (int)actualSize.X;
            finalBounds.Height = (control.VerticalAlignment == VerticalAlignment.Stretch) ? bounds.Height : (int)actualSize.Y;

            switch(control.HorizontalAlignment)
            {
                case HorizontalAlignment.Left:
                case HorizontalAlignment.Stretch:
                    finalBounds.X = bounds.X;
                    break;
                case HorizontalAlignment.Center:
                    finalBounds.X = bounds.Left + ((bounds.Width - finalBounds.Width) / 2);
                    break;
                case HorizontalAlignment.Right:
                    finalBounds.X = bounds.Right - finalBounds.Width;
                    break;
            }
            
            switch(control.VerticalAlignment)
            {
                case VerticalAlignment.Stretch:
                case VerticalAlignment.Top:
                    finalBounds.Y = bounds.Y;
                    break;
                case VerticalAlignment.Middle:
                    finalBounds.Y = bounds.Y + ((bounds.Height - finalBounds.Height)) / 2;
                    break;
                case VerticalAlignment.Bottom:
                    finalBounds.Y = bounds.Bottom - finalBounds.Height;
                    break;
            }

            finalBounds.X += control.Margin.Left;
            finalBounds.Y += control.Margin.Top;
            finalBounds.Width -= control.Margin.Width;
            finalBounds.Height -= control.Margin.Height;

            control.Bounds = finalBounds;

            control.ArrangeOverride();
        }
    }
}
