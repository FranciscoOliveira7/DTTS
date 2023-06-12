using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTTS.Scenes
{
    public abstract class Scene
    {
        protected ContentManager Content;
        protected GraphicsDevice GraphicsDevice;
        protected SpriteBatch _spriteBatch;

        protected DTTSGame game = DTTSGame.instance;

        public const int screenHeight = DTTSGame.screenHeight;
        public const int screenWidth = DTTSGame.screenWidth;

        public Scene()
        {
            GraphicsDevice = DTTSGame.instance.GraphicsDevice;
            Content = DTTSGame.instance.Content;
            _spriteBatch = DTTSGame.instance._spriteBatch;
        }

        public abstract void Draw(GameTime gameTime);

        public abstract void PostUpdate(GameTime gameTime);

        public abstract void Update(GameTime gameTime);

        public abstract void LoadContent();

        public abstract void HandlePlayerScore();
    }
}
