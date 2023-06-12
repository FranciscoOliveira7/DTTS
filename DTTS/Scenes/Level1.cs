using DTTS.GameObjects.Collectables;
using DTTS.GameObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.Metadata;
using Microsoft.Xna.Framework.Input;

namespace DTTS.Scenes
{
    public class Level1 : Scene
    {
        private Wall wallTop, wallBottom, wallLeft, wallRight;

        private const int numOfSpikes = 7;
        private readonly Spike[,] spikes = new Spike[numOfSpikes, 2];

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
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            game.draw = new DrawingUtil(_spriteBatch);
            Sounds.LoadSounds(Content);

            camera = new Camera();

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
            wallTop = new Wall(DTTSGame.instance.squareTexture, new Vector2(0, 0), screenWidth, 50);
            wallBottom = new Wall(DTTSGame.instance.squareTexture, new Vector2(0, screenHeight - 50), screenWidth, 50);
            wallLeft = new Wall(DTTSGame.instance.squareTexture, new Vector2(0, 50), 50, screenHeight - 100);
            wallRight = new Wall(DTTSGame.instance.squareTexture, new Vector2(screenWidth - 50, 50), 50, screenHeight - 100);
            #endregion

            #region spikes
            for (int i = 0; i < numOfSpikes; i++)
            {
                int posY = (i + 1) * 90 + 30;
                spikes[i, 0] = new Spike(DTTSGame.instance.spikeTexture, new Vector2(-12, posY), Facing.right);
            }
            for (int i = 0; i < numOfSpikes; i++)
            {
                int posY = (i + 1) * 90 + 30;
                spikes[i, 1] = new Spike(DTTSGame.instance.spikeTexture, new Vector2(screenWidth - 58, posY), Facing.left);
            }
            #endregion

            #region adding to colliders list
            colliders.Add(wallTop);
            colliders.Add(wallBottom);
            colliders.Add(wallLeft);
            colliders.Add(wallRight);
            colliders.AddRange(powerUps);

            for (int i = 0; i < numOfSpikes; i++)
                for (int j = 0; j < 2; j++)
                    colliders.Add(spikes[i, j]);

            for (int i = 0; i < numOfSpikes; i++)
                for (int j = 0; j < 2; j++)
                    spikes[i, j].Deactivate();
            #endregion
        }

        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin(transformMatrix: camera.Transform);

            if (game.player.isDead)
                _spriteBatch.DrawString(game.mainFont, "Press space to Restart", new Vector2(106, screenHeight / 2 + 150), Color.White);

            if (hasGameStarted)
            {
                _spriteBatch.Draw(game.scoreCircle, new((int)screenWidth / 2 - 125, (int)screenHeight / 2 - 125, 250, 250), null, Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 0f);
                _spriteBatch.DrawString(game.scoreFont, game.player.score.ToString("00"), new Vector2(screenWidth / 2 - 68, screenHeight / 2 - 73), GameColors.backGround);
            }
            else
            {
                _spriteBatch.DrawString(game.mainFont, "High Score: " + game.highScore.highscore, new Vector2(205, 100), Color.White);
                _spriteBatch.DrawString(game.mainFont, "Press space to Start", new Vector2(135, screenHeight / 2 + 150), Color.White);
                _spriteBatch.DrawString(game.mainFont, "Press esc to go back", new Vector2(250, screenHeight / 2 + 200), Color.White, 0, Vector2.Zero, 0.5f, SpriteEffects.None, 0);
            }

            game.player.Draw(_spriteBatch);

            foreach (var gameObject in colliders)
            {
                gameObject.Draw(_spriteBatch);
            }

            if (game.player.powerup != null && !game.player.powerup.isActive)
            {
                _spriteBatch.DrawString(game.mainFont, "Press E to use " + game.player.powerup.ToString(), new Vector2(205, 100), Color.White, 0, new(0, 0), .5f, SpriteEffects.None, 0);
            }

            _spriteBatch.End();
        }

        public override void PostUpdate(GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        public override void Update(GameTime gameTime)
        {
            double deltaTime = gameTime.ElapsedGameTime.TotalSeconds;

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                FileUtil.SaveScore(game.highScore);
                //game.Exit();
                DTTSGame.instance.ChangeScene(DTTSGame.instance.menu);
            }

            if (hasGameStarted) MainGame(deltaTime);

            if (Keyboard.GetState().IsKeyDown(Keys.Space) && !hasPressedSpace)
            {
                if (game.player.isDead) Restart();
                else hasGameStarted = true;
                hasPressedSpace = true;
            }
            if (Keyboard.GetState().IsKeyUp(Keys.Space)) hasPressedSpace = false;

            if (Keyboard.GetState().IsKeyDown(Keys.R))
            {
                Restart();
            }
        }

        protected void MainGame(double deltaTime)
        {
            game.player.Update(deltaTime, colliders);
            //camera.Follow(player);
        }

        public void Restart()
        {
            camera.Reset();
            if (game.highScore.highscore < game.player.score) game.highScore.highscore = game.player.score;
            hasGameStarted = false;
            game.player.Restart();
            for (int i = 0; i < numOfSpikes; i++)
                for (int j = 0; j < 2; j++)
                    spikes[i, j].Deactivate();

            foreach (var powerup in powerUps)
                powerup.Despawn();

            GameColors.foreGround = Color.Gray;
            GameColors.backGround = Color.LightGray;
        }

        public override void HandlePlayerScore()
        {
            Random rnd = new Random();

            if (!HasPowerUpOnScreen() && game.player.powerup == null)
            {
                int poweUpNumber = rnd.Next(powerUps.Count);
                powerUps[poweUpNumber].Spawn(game.player.isFacingRight);
            }

            GenerateSpikes(rnd);

            GameColors.UpdateColor(game.player.score);
        }

        protected bool HasPowerUpOnScreen()
        {
            foreach (Collectable powerup in powerUps)
            {
                if (powerup.isOnScreen) return true;
            }

            return false;
        }

        protected void GenerateSpikes(Random rnd)
        {
            for (int i = 0; i < numOfSpikes; i++)
            {
                spikes[i, (game.player.isFacingRight ? 0 : 1)].Deactivate();
            }

            int spikeNumber = rnd.Next(numOfSpikes);

            for (int j = 0; j < 3; j++)
            {
                //Find a deactive spike to activate
                while (spikes[spikeNumber, (game.player.isFacingRight ? 1 : 0)].isActive)
                    spikeNumber = rnd.Next(numOfSpikes);

                spikes[spikeNumber, (game.player.isFacingRight ? 1 : 0)].Activate();
            }
        }
    }
}
