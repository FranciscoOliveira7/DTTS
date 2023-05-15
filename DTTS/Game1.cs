using DTTS.GameObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Reflection.Metadata;

namespace DTTS
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private SpriteFont scoreFont;
        private Texture2D scoreCircle;

        const int gameHeight = 850, gameWidth = 700;

        private Player player;
        bool wasFacingRight;

        private Wall wallTop, wallBottom, wallLeft, wallRight;

        private const int numOfSpikes = 7;
        private Spike[,] spikes = new Spike[numOfSpikes,numOfSpikes];

        // All gameObjects list for the player's collision check
        private List<GameObject> gameObjects = new List<GameObject>();

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

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            scoreFont = Content.Load<SpriteFont>("ScoreFont");
            scoreCircle = Content.Load<Texture2D>("Circle");

            player = new Player(Content.Load<Texture2D>("Bird"), new Vector2(gameWidth / 2 - 35, gameHeight / 2 - 35));

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

            gameObjects.Add(wallTop);
            gameObjects.Add(wallBottom);
            gameObjects.Add(wallLeft);
            gameObjects.Add(wallRight);

            for (int i = 0; i < numOfSpikes; i++)
                for (int j = 0; j < 2; j++)
                    gameObjects.Add(spikes[i, j]);

            for (int i = 0; i < numOfSpikes; i++)
                for (int j = 0; j < 2; j++)
                    spikes[i, j].Deactivate();

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            double deltaTime = gameTime.ElapsedGameTime.TotalSeconds;

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            player.Update(deltaTime, gameObjects);

            for (int i = 0; i < numOfSpikes; i++)
                for (int j = 0; j < 2; j++)
                    spikes[i,j].Update(deltaTime);

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
                    while (spikes[spikeNumber, (player.isFacingRight ? 1 : 0)].isActive)
                    {
                        spikeNumber = rnd.Next(numOfSpikes);
                    }
                    spikes[spikeNumber, (player.isFacingRight ? 1 : 0)].Activate();
                }
                wasFacingRight = player.isFacingRight;
            }


            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.LightGray);

            _spriteBatch.Begin();

            _spriteBatch.Draw(scoreCircle, new((int)gameWidth / 2 - 125, (int)gameHeight / 2 - 125, 250, 250), null, Color.White, 0, new Vector2(0,0), SpriteEffects.None, 0f);
            _spriteBatch.DrawString(scoreFont, player.score.ToString("00"), new Vector2(gameWidth / 2 - 68, gameHeight / 2 - 73), Color.LightGray);

            player.Draw(_spriteBatch);

            foreach (var gameObject in gameObjects)
            {
                gameObject.Draw(_spriteBatch);
            }

            _spriteBatch.End();

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}