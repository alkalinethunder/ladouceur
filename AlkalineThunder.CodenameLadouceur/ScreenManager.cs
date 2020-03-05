using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlkalineThunder.CodenameLadouceur
{
    public sealed class ScreenManager : GameComponent
    {
        private GameLoop _game = null;

        public ScreenManager(GameLoop game) : base(game)
        {
            _game = game;
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public void Draw(GameTime gameTime)
        {

        }
    }
}
