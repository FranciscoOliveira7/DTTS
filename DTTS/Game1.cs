using DTTS.GameObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace DTTS
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        const int gameHeight = 850, gameWidth = 700;

        private Player player;

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

            player = new Player(Content.Load<Texture2D>("Bird"), new Vector2(gameWidth / 2 - 35, gameHeight / 2 - 35));

            wallTop = new Wall(Content.Load<Texture2D>("Square"), new Vector2(50, 0), gameWidth - 100, 50);
            wallBottom = new Wall(Content.Load<Texture2D>("Square"), new Vector2(50, gameHeight - 50), gameWidth - 100, 50);
            wallLeft = new Wall(Content.Load<Texture2D>("Square"), new Vector2(0, 0), 50, gameHeight);
            wallRight = new Wall(Content.Load<Texture2D>("Square"), new Vector2(gameWidth - 50, 0), 50, gameHeight);

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

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.LightGray);

            _spriteBatch.Begin();

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