using DTTS.GameObjects.Collectables;
using DTTS.GameObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTTS.Scenes
{
    public class Menu : Scene
    {
        private Wall wallTop, wallBottom, wallLeft, wallRight;

        // All gameObjects list for the player's collision check
        private readonly List<GameObject> colliders = new List<GameObject>();

        private Camera camera;

        // flags
        bool hasGameStarted, hasPressedSpace;

        public override void LoadContent()
        {
            #region walls
            wallTop = new Wall(DTTSGame.instance.squareTexture, new Vector2(0, 0), screenWidth, 50);
            wallBottom = new Wall(DTTSGame.instance.squareTexture, new Vector2(0, screenHeight - 50), screenWidth, 50);
            wallLeft = new Wall(DTTSGame.instance.squareTexture, new Vector2(0, 50), 50, screenHeight - 100);
            wallRight = new Wall(DTTSGame.instance.squareTexture, new Vector2(screenWidth - 50, 50), 50, screenHeight - 100);
            #endregion

            #region adding to colliders list
            colliders.Add(wallTop);
            colliders.Add(wallBottom);
            colliders.Add(wallLeft);
            colliders.Add(wallRight);
            #endregion
        }

        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin();

            _spriteBatch.DrawString(game.mainFont, "High Score: " + game.highScore.highscore, new Vector2(205, 100), Color.White);
            _spriteBatch.DrawString(game.mainFont, "Press space to Start", new Vector2(135, screenHeight / 2 + 150), Color.White);

            foreach (var gameObject in colliders)
            {
                gameObject.Draw(_spriteBatch);
            }

            game.player.Draw(_spriteBatch);

            if (game.player.powerup != null && !game.player.powerup.isActive)
            {
                _spriteBatch.DrawString(game.mainFont, "Press E to use " + game.player.powerup.ToString(), new Vector2(205, 100), Color.White, 0, new(0, 0), .5f, SpriteEffects.None, 0);
            }

            _spriteBatch.End();
        }

        public override void HandlePlayerScore()
        {
            throw new NotImplementedException();
        }

        public override void PostUpdate(GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        public override void Update(GameTime gameTime)
        {

        }
    }
}
