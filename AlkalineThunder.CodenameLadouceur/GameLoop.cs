using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using AlkalineThunder.CodenameLadouceur.Input;
using AlkalineThunder.CodenameLadouceur.Screens;
using AlkalineThunder.DevConsole;
using AlkalineThunder.CodenameLadouceur.Rendering;

namespace AlkalineThunder.CodenameLadouceur
{
    public sealed class GameLoop : Game
    {
        private GraphicsDeviceManager _graphics = null;
        private Renderer _renderer = null;
        
        public ScreenManager ScreenManager { get; private set; }
        public InputManager Input { get; private set; }
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
            _renderer = new Renderer(this.GraphicsDevice);

            Input = new InputManager(this);
            ScreenManager = new ScreenManager(this);
            Components.Add(Input);
            Components.Add(ScreenManager);

            DevConsole = new DevConsole.DevConsole(this, _renderer);

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

            ScreenManager.Draw(gameTime, _renderer);

            base.Draw(gameTime);
        }
    }
}
