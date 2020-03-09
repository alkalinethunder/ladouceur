﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using AlkalineThunder.CodenameLadouceur.Input;
using AlkalineThunder.CodenameLadouceur.Screens;
using AlkalineThunder.DevConsole;

namespace AlkalineThunder.CodenameLadouceur
{
    public sealed class GameLoop : Game
    {
        private GraphicsDeviceManager _graphics = null;
        private SpriteBatch _batch;
        private ScreenManager _screenManager = null;
        private InputManager _input = null;
        
        public DevConsole.DevConsole DevConsole { get; private set; }

        public GameLoop()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.IsFullScreen = true;

            var nativeResolution = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode;

            _graphics.PreferredBackBufferWidth = nativeResolution.Width;
            _graphics.PreferredBackBufferHeight = nativeResolution.Height;

            IsFixedTimeStep = true;

            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            _batch = new SpriteBatch(GraphicsDevice);
            _input = new InputManager(this);
            _screenManager = new ScreenManager(this);
            Components.Add(_input);
            Components.Add(_screenManager);

            DevConsole = new DevConsole.DevConsole(this, new SpriteBatchConsoleRenderer(_batch));

            base.Initialize();
        }

        protected override void LoadContent()
        {
            this.DevConsole.ConsoleFont = Content.Load<SpriteFont>("ConsoleFont");

            base.LoadContent();
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _screenManager.Draw(gameTime);

            base.Draw(gameTime);
        }
    }
}
