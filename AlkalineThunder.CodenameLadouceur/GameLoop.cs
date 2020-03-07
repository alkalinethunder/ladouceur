using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using AlkalineThunder.CodenameLadouceur.Input;
using AlkalineThunder.CodenameLadouceur.Screens;
using AlkalineThunder.CodenameLadouceur.Rendering;

namespace AlkalineThunder.CodenameLadouceur
{
    public sealed class GameLoop : Game
    {
        private GraphicsDeviceManager _graphics = null;
        private Renderer _renderer = null;
        private ScreenManager _screenManager = null;
        private InputManager _input = null;

        public GameLoop()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.IsFullScreen = true;

            var nativeResolution = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode;

            _graphics.PreferredBackBufferWidth = nativeResolution.Width;
            _graphics.PreferredBackBufferHeight = nativeResolution.Height;

            IsFixedTimeStep = true;
        }

        protected override void Initialize()
        {
            _input = new InputManager(this);
            _screenManager = new ScreenManager(this);
            Components.Add(_input);
            Components.Add(_screenManager);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _renderer = new Renderer(this.GraphicsDevice);
            base.LoadContent();
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _screenManager.Draw(gameTime, _renderer);

            base.Draw(gameTime);
        }
    }
}
