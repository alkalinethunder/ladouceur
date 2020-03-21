using AlkalineThunder.Nucleus.Gui;
using AlkalineThunder.Nucleus.Rendering;
using AlkalineThunder.Nucleus.Windowing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace AlkalineThunder.Nucleus.Screens
{
    public abstract class Screen : ContentControl
    {
        private ScreenManager ScreenManager { get; set; }
        public ContentManager ContentManager { get; private set; }

        private List<Window> _windows = new List<Window>();

        public Window[] Windows => _windows.ToArray();

        public virtual Rectangle WindowBounds => Bounds;

        public Window OpenWindow(string titleText)
        {
            var win = new Window();
            win.Title = titleText;
            _windows.Add(win);
            return win;
        }

        public void Exit()
        {
            ScreenManager.Game.Exit();
        }

        protected T AddScreen<T>() where T : Screen, new()
        {
            return ScreenManager.AddScreen<T>();
        }

        protected void RemoveScreen()
        {
            this.ScreenManager.RemoveScreen(this);
        }

        protected virtual void OnInitialize()
        {

        }

        protected virtual void OnLoadContent()
        {

        }

        public void LoadContent(ContentManager content)
        {
            this.ContentManager = content;
            OnLoadContent();
        }

        public void Initialize(ScreenManager screenManager)
        {
            this.ScreenManager = screenManager;
            OnInitialize();
        }
    }
}
