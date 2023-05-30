using DTTS.GameObjects;
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

        private SpriteFont mainFont;
        private SpriteFont scoreFont;
        private Texture2D scoreCircle;

        const int gameHeight = 850, gameWidth = 700;

        private Player player;
        bool wasFacingRight;

        PlayerStats highScore = new PlayerStats();

        private Wall wallTop, wallBottom, wallLeft, wallRight;

        private const int numOfSpikes = 7;
        private Spike[,] spikes = new Spike[numOfSpikes,numOfSpikes];

        private Collectable powerUp;

        // All gameObjects list for the player's collision check
        private List<GameObject> collisionObjects = new List<GameObject>();

        bool hasGameStarted, hasPressedSpace;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            _graphics.PreferredBackBufferHeight = gameHeight; //definição da altura
            _graphics.PreferredBackBufferWidth = gameWidth; //definição da largura
            _graphics.ApplyChanges();
            hasGameStarted = hasPressedSpace = false;
            wasFacingRight = true;
            //if ((highScore = FileUtil.LoadScore()) == null) { }

            highScore = FileUtil.LoadScore();
            if (highScore == null) highScore.score = 0;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            Sounds.death = Content.Load<SoundEffect>("death");
            Sounds.jump = Content.Load<SoundEffect>("jump");
            Sounds.score = Content.Load<SoundEffect>("score");

            mainFont = Content.Load<SpriteFont>("MainFont");
            scoreFont = Content.Load<SpriteFont>("ScoreFont");
            scoreCircle = Content.Load<Texture2D>("Circle");

            player = new Player(Content.Load<Texture2D>("Bird"), new Vector2(gameWidth / 2 - 35, gameHeight / 2 - 35));

            powerUp = new Invincibility(Content.Load<Texture2D>("Square"), new Vector2(gameWidth / 2 + 200, gameHeight / 2));

            wallTop = new Wall(Content.Load<Texture2D>("Square"), new Vector2(0, 0), gameWidth, 50);
            wallBottom = new Wall(Content.Load<Texture2D>("Square"), new Vector2(0, gameHeight - 50), gameWidth, 50);
            wallLeft = new Wall(Content.Load<Texture2D>("Square"), new Vector2(0, 50), 50, gameHeight - 100);
            wallRight = new Wall(Content.Load<Texture2D>("Square"), new Vector2(gameWidth - 50, 50), 50, gameHeight);

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

            collisionObjects.Add(wallTop);
            collisionObjects.Add(wallBottom);
            collisionObjects.Add(wallLeft);
            collisionObjects.Add(wallRight);
            collisionObjects.Add(powerUp);

            for (int i = 0; i < numOfSpikes; i++)
                for (int j = 0; j < 2; j++)
                    collisionObjects.Add(spikes[i, j]);

            for (int i = 0; i < numOfSpikes; i++)
                for (int j = 0; j < 2; j++)
                    spikes[i, j].Deactivate();

            // TODO: use this.Content to load your game content here
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

            foreach (var gameObject in collisionObjects)
            {
                gameObject.Draw(_spriteBatch);
            }

            powerUp.Draw(_spriteBatch);

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        protected void MainGame(double deltaTime)
        {
            player.Update(deltaTime, collisionObjects);
            HandlePlayerScore();
            if (Keyboard.GetState().IsKeyDown(Keys.F))
            { 
                powerUp.Spawn();
            }
        }

        protected void Restart()
        {
            if (highScore.score < player.score) highScore.score = player.score;
            hasGameStarted = false;
            player.Restart();
            for (int i = 0; i < numOfSpikes; i++)
                for (int j = 0; j < 2; j++)
                    spikes[i, j].Deactivate();
            GameColors.foreGround = Color.Gray;
            GameColors.backGround = Color.LightGray;
        }

        protected void HandlePlayerScore()
        {
            //If the player has turned, activate 3 random spikes on the other side
            if (player.HasTurned(wasFacingRight))
            {
                for (int i = 0; i < numOfSpikes; i++)
                {
                    spikes[i, (player.isFacingRight ? 0 : 1)].Deactivate();
                }

                Random rnd = new Random();
                int spikeNumber = rnd.Next(numOfSpikes);

                for (int j = 0; j < 3; j++)
                {
                    //Find a deactive spike to activate
                    while (spikes[spikeNumber, (player.isFacingRight ? 1 : 0)].isActive)
                        spikeNumber = rnd.Next(numOfSpikes);

                    spikes[spikeNumber, (player.isFacingRight ? 1 : 0)].Activate();
                }
                GameColors.UpdateColor(player.score);
                
                wasFacingRight = player.isFacingRight;
            }
        }
    }
}