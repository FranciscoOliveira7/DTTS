using DTTS.GameObjects;
using DTTS.GameObjects.Collectables;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection.Emit;
using System.Reflection.Metadata;

namespace DTTS
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        DrawingUtil draw; // Simple drawing utiliy

        private SpriteFont mainFont;
        private SpriteFont scoreFont;
        private Texture2D scoreCircle;

        const int gameHeight = 850, gameWidth = 700;

        private Player player;
        bool wasFacingRight;

        PlayerStats highScore = new PlayerStats();

        private Wall wallTop, wallBottom, wallLeft, wallRight;

        private const int numOfSpikes = 7;
        private readonly Spike[,] spikes = new Spike[numOfSpikes,numOfSpikes];

        private readonly List<Collectable> powerUps = new List<Collectable>();
        private Invincibility invincibility;
        private SlowMotion slowmotion;
        ProgressionBar powerUpProgressBar;

        // All gameObjects list for the player's collision check
        private readonly List<GameObject> colliders = new List<GameObject>();

        // flags
        bool hasGameStarted, hasPressedSpace;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferHeight = gameHeight; //definição da altura
            _graphics.PreferredBackBufferWidth = gameWidth; //definição da largura
            _graphics.ApplyChanges();
            hasGameStarted = hasPressedSpace = hasGameStarted = false;
            wasFacingRight = true;

            highScore = FileUtil.LoadScore();
            highScore ??= new PlayerStats(0);
            // ??= operator means it will give a value if highscore is null

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            draw = new DrawingUtil(GraphicsDevice, _spriteBatch);

            Sounds.LoadSounds(Content);

            mainFont = Content.Load<SpriteFont>("MainFont");
            scoreFont = Content.Load<SpriteFont>("ScoreFont");
            scoreCircle = Content.Load<Texture2D>("Circle");

            player = new Player(Content.Load<Texture2D>("Bird"), new Vector2(gameWidth / 2 - 35, gameHeight / 2 - 35))
            {
                powerup = null
            };

            // Powerup Progress Bar
            powerUpProgressBar = new ProgressionBar(new Rectangle(gameWidth / 2 - 50, 75, 100, 6), draw);

            // Power Ups
            invincibility = new Invincibility(Content.Load<Texture2D>("star"), new Vector2(gameWidth / 2 + 200, gameHeight / 2), powerUpProgressBar);
            powerUps.Add(invincibility);
            slowmotion = new SlowMotion(Content.Load<Texture2D>("slowmotion"), new Vector2(gameWidth / 2 + 200, gameHeight / 2), powerUpProgressBar);
            powerUps.Add(slowmotion);
            slowmotion.Despawn();

            #region walls
            wallTop = new Wall(Content.Load<Texture2D>("Square"), new Vector2(0, 0), gameWidth, 50);
            wallBottom = new Wall(Content.Load<Texture2D>("Square"), new Vector2(0, gameHeight - 50), gameWidth, 50);
            wallLeft = new Wall(Content.Load<Texture2D>("Square"), new Vector2(0, 50), 50, gameHeight - 100);
            wallRight = new Wall(Content.Load<Texture2D>("Square"), new Vector2(gameWidth - 50, 50), 50, gameHeight);
            #endregion

            #region spikes
            for (int i = 0; i < numOfSpikes; i++)
            {
                int posY = (i + 1) * 90 + 30;
                spikes[i,0] = new Spike(Content.Load<Texture2D>("Spike"), new Vector2(-12, posY), Facing.right);
            }
            for (int i = 0; i < numOfSpikes; i++)
            {
                int posY = (i + 1) * 90 + 30;
                spikes[i,1] = new Spike(Content.Load<Texture2D>("Spike"), new Vector2(gameWidth - 58, posY), Facing.left);
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

        protected override void Update(GameTime gameTime)
        {
            double deltaTime = gameTime.ElapsedGameTime.TotalSeconds;

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                FileUtil.SaveScore(highScore);
                Exit();
            }
            else
            {
                if (hasGameStarted) MainGame(deltaTime);

                if (Keyboard.GetState().IsKeyDown(Keys.Space) && !hasPressedSpace)
                {
                    if (player.isDead) Restart();
                    else hasGameStarted = true;
                    hasPressedSpace = true;
                }
                if (Keyboard.GetState().IsKeyUp(Keys.Space)) hasPressedSpace = false;

                base.Update(gameTime);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.R))
            {
                Restart();
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(GameColors.backGround);

            _spriteBatch.Begin();

            if (player.isDead)
                _spriteBatch.DrawString(mainFont, "Press space to Restart", new Vector2(106, gameHeight / 2 + 150), Color.White);

            if (hasGameStarted)
            {
                _spriteBatch.Draw(scoreCircle, new((int)gameWidth / 2 - 125, (int)gameHeight / 2 - 125, 250, 250), null, Color.White, 0, new Vector2(0,0), SpriteEffects.None, 0f);
                _spriteBatch.DrawString(scoreFont, player.score.ToString("00"), new Vector2(gameWidth / 2 - 68, gameHeight / 2 - 73), GameColors.backGround);
            }
            else
            {
                _spriteBatch.DrawString(mainFont, "High Score: " + highScore.score, new Vector2(205, 100), Color.White);
                _spriteBatch.DrawString(mainFont, "Press space to Start", new Vector2(135, gameHeight / 2 + 150), Color.White);
            }

            player.Draw(_spriteBatch);

            foreach (var gameObject in colliders)
            {
                gameObject.Draw(_spriteBatch);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        protected void MainGame(double deltaTime)
        {
            player.Update(deltaTime, colliders);
            HandlePlayerScore();
            if (Keyboard.GetState().IsKeyDown(Keys.F))
            {
                powerUps[0].Spawn(player.isFacingRight);
            }
        }

        protected void Restart()
        {
            if (highScore.score < player.score) highScore.score = player.score;
            hasGameStarted = false;
            wasFacingRight = true;
            player.Restart();
            for (int i = 0; i < numOfSpikes; i++)
                for (int j = 0; j < 2; j++)
                    spikes[i, j].Deactivate();

            foreach (var powerup in powerUps)
                powerup.Despawn();

            GameColors.foreGround = Color.Gray;
            GameColors.backGround = Color.LightGray;
        }

        protected void HandlePlayerScore()
        {
            //If the player has turned, activate 3 random spikes on the other side
            if (player.HasTurned(wasFacingRight))
            {
                Random rnd = new Random();

                if (!HasPowerUpOnScreen() && player.powerup == null)
                {
                    int poweUpNumber = rnd.Next(powerUps.Count);
                    powerUps[poweUpNumber].Spawn(player.isFacingRight);
                }

                GenerateSpikes(rnd);

                GameColors.UpdateColor(player.score);
                
                wasFacingRight = player.isFacingRight;
            }
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
                spikes[i, (player.isFacingRight ? 0 : 1)].Deactivate();
            }

            int spikeNumber = rnd.Next(numOfSpikes);

            for (int j = 0; j < 3; j++)
            {
                //Find a deactive spike to activate
                while (spikes[spikeNumber, (player.isFacingRight ? 1 : 0)].isActive)
                    spikeNumber = rnd.Next(numOfSpikes);

                spikes[spikeNumber, (player.isFacingRight ? 1 : 0)].Activate();
            }
        }
    }
}