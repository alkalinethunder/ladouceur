using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using AlkalineThunder.Nucleus.Input;
using AlkalineThunder.Nucleus.Screens;
using AlkalineThunder.DevConsole;
using AlkalineThunder.Nucleus.Rendering;
using AlkalineThunder.Nucleus.Gui;
using Microsoft.Xna.Framework.Content;

namespace AlkalineThunder.Nucleus
{
    public sealed class GameLoop : Game
    {
        private ContentManager _engineContent = null;
        private GraphicsDeviceManager _graphics = null;
        private Renderer _renderer = null;
        
        public static GameLoop Instance { get; private set; }
        public ScreenManager ScreenManager { get; private set; }
        public InputManager Input { get; private set; }
        public DevConsole.DevConsole DevConsole { get; private set; }

        internal ContentManager EngineContent => _engineContent;

        public static event EventHandler GameReady;

        public GameLoop()
        {
            _engineContent = new ContentManager(this.Services);
            _engineContent.RootDirectory = "EngineContent";

            Instance = this;

            _graphics = new GraphicsDeviceManager(this);
            _graphics.IsFullScreen = false;

            var nativeResolution = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode;

            _graphics.PreferredBackBufferWidth = nativeResolution.Width;
            _graphics.PreferredBackBufferHeight = nativeResolution.Height;

            IsFixedTimeStep = true;
            IsMouseVisible = true;

            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            Control.LoadTheme<DefaultTheme>(EngineContent);

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
            this.DevConsole.ConsoleFont = EngineContent.Load<SpriteFont>("ConsoleFont");

            base.LoadContent();

            GameReady?.Invoke(this, EventArgs.Empty);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            ScreenManager.Draw(gameTime, _renderer);

            base.Draw(gameTime);
        }
    }
}
