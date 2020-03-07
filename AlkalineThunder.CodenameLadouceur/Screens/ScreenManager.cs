using AlkalineThunder.CodenameLadouceur.Rendering;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlkalineThunder.CodenameLadouceur.Screens
{
    public sealed class ScreenManager : GameComponent
    {
        private List<Screen> _screens = new List<Screen>();

        public ScreenManager(GameLoop game) : base(game)
        {
        }

        public override void Initialize()
        {
            _screens.Add(new AboutScreen());
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var screen in _screens) screen.Update(gameTime);
            base.Update(gameTime);
        }

        public void Draw(GameTime gameTime, Renderer renderer)
        {
            foreach (var screen in _screens) screen.Draw(gameTime, renderer);
        }
    }
}
