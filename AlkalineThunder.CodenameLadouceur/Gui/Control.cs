using AlkalineThunder.Nucleus.Input;
using AlkalineThunder.Nucleus.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace AlkalineThunder.Nucleus.Gui
{
    public abstract class Control : IRenderer
    {
        private List<IAttachedProperty> _props = new List<IAttachedProperty>();

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
        public Rectangle ContentBounds => Margin.Affect(Bounds);
        
        public Control()
        {
            if (ActiveTheme == null) throw new InvalidOperationException("You must load a theme before you can use the GUI.  Michael, you're retarded.  Your game loop should've never let this happen.  God damnit.");
            InternalChildren = new ControlCollection(this);
        }

        public bool GetProperty<T>(string name, out T prop)
        {
            var attachedProperty = _props.FirstOrDefault(x => x.Name == name && x.Value is T);
            if(attachedProperty != null)
            {
                prop = (T)attachedProperty.Value;
                return true;
            }
            else
            {
                prop = default(T);
                return false;
            }
        }

        public bool HasProperty<T>(string name)
        {
            return _props.Any(x => x.Name == name && x.Value is T);
        }

        public void SetAttachedProperty<T>(string name, T value)
        {
            if (HasProperty<T>(name))
            {
                _props.RemoveAll(x => x.Name == name);
            }
            _props.Add(new AttachedProperty<T>(name, value));

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

        public bool FireMouseEnter(MouseMoveEventArgs e)
        {
            return OnMouseEnter(e);
        }

        public bool FireLostFocus(FocusEventArgs e)
        {
            return OnLostFocus(e);
        }

        public bool FireKeyUp(InputKeyEventArgs e)
        {
            return OnKeyUp(e);
        }

        public bool FireKeyDown(InputKeyEventArgs e)
        {
            return OnKeyDown(e);
        }

        public bool FireTextInput(TextInputEventArgs e)
        {
            return OnTextInput(e);
        }

        public bool FireGainedFocus(FocusEventArgs e)
        {
            return OnGainedFocus(e);
        }

        public bool FireMouseDown(MouseButtonEventArgs e)
        {
            return OnMouseDown(e);
        }

        public bool FireMouseUp(MouseButtonEventArgs e)
        {
            return OnMouseUp(e);
        }

        public bool FireClick(MouseButtonEventArgs e)
        {
            return OnClick(e);
        }

        public bool FireMouseLeave(MouseMoveEventArgs e)
        {
            return OnMouseLeave(e);
        }

        public bool FireMouseMove(MouseMoveEventArgs e)
        {
            return OnMouseMove(e);
        }

        public bool FireMouseScroll(MouseScrollEventArgs e)
        {
            return OnMouseScroll(e);
        }

        protected virtual bool OnMouseScroll(MouseScrollEventArgs e)
        {
            if(MouseScroll != null)
            {
                MouseScroll(this, e);
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool Collapsed { get; set; } = false;
        public bool Visible { get; set; } = true;
        public bool Enabled { get; set; } = true;

        public event EventHandler<MouseMoveEventArgs> MouseEnter;
        public event EventHandler<MouseMoveEventArgs> MouseMove;
        public event EventHandler<MouseMoveEventArgs> MouseLeave;
        public event EventHandler<MouseButtonEventArgs> MouseDown;
        public event EventHandler<MouseButtonEventArgs> Click;
        public event EventHandler<MouseButtonEventArgs> MouseUp;
        public event EventHandler<InputKeyEventArgs> KeyDown;
        public event EventHandler<InputKeyEventArgs> KeyUp;
        public event EventHandler<TextInputEventArgs> TextInput;
        public event EventHandler<FocusEventArgs> GainedFocus;
        public event EventHandler<FocusEventArgs> LostFocus;
        public event EventHandler<MouseScrollEventArgs> MouseScroll;

        public IEnumerable<Control> Parents
        {
            get
            {
                var parent = Parent;
                while(parent != null)
                {
                    yield return parent;
                    parent = parent.Parent;
                }
            }
        }

        public bool ParentsEnabled => Enabled && !Parents.Any(x => x.Enabled == false);

        protected virtual bool OnMouseEnter(MouseMoveEventArgs e)
        {
            if(MouseEnter != null)
            {
                MouseEnter(this, e);
                return true;
            }
            else
            {
                return false;
            }
        }

        protected virtual bool OnMouseLeave(MouseMoveEventArgs e)
        {
            if (MouseLeave != null)
            {
                MouseLeave(this, e);
                return true;
            }
            else
            {
                return false;
            }
        }
        protected virtual bool OnMouseMove(MouseMoveEventArgs e)
        {
            if (MouseMove != null)
            {
                MouseMove(this, e);
                return true;
            }
            else
            {
                return false;
            }
        }
        protected virtual bool OnKeyDown(InputKeyEventArgs e)
        {
            if (KeyDown != null)
            {
                KeyDown(this, e);
                return true;
            }
            else
            {
                return false;
            }
        }
        protected virtual bool OnKeyUp(InputKeyEventArgs e)
        {
            if (KeyUp != null)
            {
                KeyUp(this, e);
                return true;
            }
            else
            {
                return false;
            }
        }
        protected virtual bool OnTextInput(TextInputEventArgs e)
        {
            if (TextInput != null)
            {
                TextInput(this, e);
                return true;
            }
            else
            {
                return false;
            }
        }
        protected virtual bool OnClick(MouseButtonEventArgs e)
        {
            if (Click != null)
            {
                Click(this, e);
                return true;
            }
            else
            {
                return false;
            }
        }
        protected virtual bool OnMouseDown(MouseButtonEventArgs e)
        {
            if (MouseDown != null)
            {
                MouseDown(this, e);
                return true;
            }
            else
            {
                return false;
            }
        }
        protected virtual bool OnMouseUp(MouseButtonEventArgs e)
        {
            if (MouseUp != null)
            {
                MouseUp(this, e);
                return true;
            }
            else
            {
                return false;
            }
        }
        protected virtual bool OnGainedFocus(FocusEventArgs e)
        {
            if (GainedFocus != null)
            {
                GainedFocus(this, e);
                return true;
            }
            else
            {
                return false;
            }
        }
        protected virtual bool OnLostFocus(FocusEventArgs e)
        {
            if (LostFocus != null)
            {
                LostFocus(this, e);
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsFocused => Screens.ScreenManager.FocusedControl == this;

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
            if (Enabled && Visible)
            {
                if (x >= this.Bounds.Left && x <= this.Bounds.Right && y >= this.Bounds.Top && y <= this.Bounds.Bottom)
                {
                    for (int i = this.InternalChildren.Count - 1; i >= 0; i--)
                    {
                        var child = this.InternalChildren[i];
                        var foundInChild = child.FindControl(x, y);
                        if (foundInChild != null) return foundInChild;
                    }

                    return this;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        protected virtual void OnDraw(GameTime gameTime)
        {

        }

        private Rectangle GetScissorRect()
        {
            var bounds = Bounds;

            var parent = Parent;
            while(parent != null)
            {
                bounds = Rectangle.Intersect(bounds, parent.Bounds);
                parent = parent.Parent;
            }

            return bounds;
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
            if (Visible)
            {
                if (_renderer != null) throw new InvalidOperationException("Control is already drawing.");

                var rect = GetScissorRect();

                if (rect.IsEmpty) return;

                _renderer = renderer;
                _renderer.SetScissorRect(rect);

                if (!ParentsEnabled)
                {
                    _renderer.Tint = Color.Gray;
                }

                _renderer.Begin();
                OnDraw(gameTime);
                _renderer.End();
                _renderer.SetScissorRect(Rectangle.Empty);

                _renderer.Tint = Color.White;

                foreach (var child in InternalChildren) child.Draw(gameTime, _renderer);

                _renderer = null;
            }
        }

        public Vector2 CalculateSize()
        {
            if (Collapsed)
            {
                return Vector2.Zero;
            }
            else
            {
                var desiredSize = MeasureOverride() + Padding + Margin;

                if (desiredSize.X < MinWidth) desiredSize.X = MinWidth;
                if (desiredSize.Y < MinHeight) desiredSize.Y = MinHeight;
                if (MaxWidth > 0 && desiredSize.X > MaxWidth) desiredSize.X = MaxWidth;
                if (MaxHeight > 0 && desiredSize.Y > MaxHeight) desiredSize.Y = MaxHeight;

                DesiredSize = desiredSize;

                return new Vector2(Width == 0 ? DesiredSize.X : Width, Height == 0 ? DesiredSize.Y : Height);
            }
        }

        public static void PlaceControl(Control control, Rectangle bounds)
        {
            var actualSize = control.CalculateSize();

            var finalBounds = new Rectangle();

            finalBounds.Width = (control.HorizontalAlignment == HorizontalAlignment.Stretch && !control.Collapsed) ? bounds.Width : (int)actualSize.X;
            finalBounds.Height = (control.VerticalAlignment == VerticalAlignment.Stretch && !control.Collapsed) ? bounds.Height : (int)actualSize.Y;

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

            control.Bounds = finalBounds;

            control.ArrangeOverride();
        }

        public void DrawBrush(Rectangle rect, Brush brush)
        {
            _renderer.DrawBrush(rect, brush);
        }

        public void DrawRectangle(Rectangle rect, Color color, Padding edges)
        {
            _renderer.DrawRectangle(rect, color, edges);
        }

        public void Clear(Color color)
        {
            _renderer.Clear(color);
        }

        public void DrawRectangle(Rectangle rect, Texture2D texture, Color color, Padding edges)
        {
            _renderer.DrawRectangle(rect, texture, color, edges);
        }

        public void FillRectangle(Rectangle rect, Texture2D texture, Color color, Padding edges)
        {
            _renderer.FillRectangle(rect, texture, color, edges);
        }

        public void DrawRoundedRectangle(Rectangle rect, Color color, float roundingPercentage)
        {
            _renderer.DrawRoundedRectangle(rect, color, roundingPercentage);
        }

        public void FillCircle(Vector2 pos, float radius, Texture2D texture, Color color)
        {
            _renderer.FillCircle(pos, radius, texture, color);
        }
    }
}
