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

        private const int numOfSpikes = 7;
        private readonly Spike[,] spikes = new Spike[numOfSpikes, numOfSpikes];

        private readonly List<Collectable> powerUps = new List<Collectable>();
        private Invincibility invincibility;
        private SlowMotion slowmotion;
        private Thicc thicc;
        ProgressionBar powerUpProgressBar;

        // All gameObjects list for the player's collision check
        private readonly List<GameObject> colliders = new List<GameObject>();

        private Camera camera;

        // flags
        bool hasGameStarted, hasPressedSpace;

        public override void LoadContent()
        {
            game.player = new Player(Content.Load<Texture2D>("Bird"), new Vector2(screenWidth / 2 - 35, screenHeight / 2 - 35))
            {
                powerup = null
            };

            // Powerup Progress Bar
            powerUpProgressBar = new ProgressionBar(new Rectangle(screenWidth / 2 - 50, 75, 100, 6), game.draw);

            // Power Ups
            invincibility = new Invincibility(Content.Load<Texture2D>("star"), new(0, 0), powerUpProgressBar);
            slowmotion = new SlowMotion(Content.Load<Texture2D>("slowmotion"), new(0, 0), powerUpProgressBar);
            thicc = new Thicc(Content.Load<Texture2D>("skull"), new(0, 0), powerUpProgressBar);
            powerUps.Add(invincibility);
            powerUps.Add(slowmotion);
            powerUps.Add(thicc);

            #region walls
            wallTop = new Wall(Content.Load<Texture2D>("Square"), new Vector2(0, 0), screenWidth, 50);
            wallBottom = new Wall(Content.Load<Texture2D>("Square"), new Vector2(0, screenHeight - 50), screenWidth, 50);
            wallLeft = new Wall(Content.Load<Texture2D>("Square"), new Vector2(0, 50), 50, screenHeight - 100);
            wallRight = new Wall(Content.Load<Texture2D>("Square"), new Vector2(screenWidth - 50, 50), 50, screenHeight - 100);
            #endregion

            #region adding to colliders list
            colliders.Add(wallTop);
            colliders.Add(wallBottom);
            colliders.Add(wallLeft);
            colliders.Add(wallRight);
            colliders.AddRange(powerUps);
            #endregion
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(GameColors.backGround);

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
