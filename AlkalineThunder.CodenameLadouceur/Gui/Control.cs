using AlkalineThunder.CodenameLadouceur.Rendering;
using Microsoft.Xna.Framework;
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

            public bool IsReadOnly => _owner.SupportsChildren;

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

        protected virtual bool SupportsChildren => false;
        protected ControlCollection InternalChildren { get; } = null;
        public Control Parent { get; private set; }

        public Control()
        {
            InternalChildren = new ControlCollection(this);
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

        protected virtual void OnUpdate(GameTime gameTime)
        {

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
    }
}
