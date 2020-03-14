using AlkalineThunder.CodenameLadouceur.Gui;
using AlkalineThunder.CodenameLadouceur.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace AlkalineThunder.CodenameLadouceur.Screens
{
    public abstract class Screen : ContentControl
    {
        private ScreenManager ScreenManager { get; set; }
        public ContentManager ContentManager { get; private set; }

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
