using DTTS.GameObjects;
using DTTS.GameObjects.Collectables;
using DTTS.Scenes;
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
    public class DTTSGame : Game
    {
        private GraphicsDeviceManager _graphics;
        public SpriteBatch _spriteBatch;

        public static DTTSGame instance;

        public DrawingUtil draw; // Simple drawing utiliy

        public SpriteFont mainFont;
        public SpriteFont scoreFont;
        public Texture2D scoreCircle;

        // Textures
        public Texture2D spikeTexture;
        public Texture2D squareTexture;

        public const int screenHeight = 850, screenWidth = 700;

        public Player player;

        public PlayerStats highScore = new PlayerStats();

        private Scene currentScene;
        public Scene menu;
        public Scene level1;
        public Scene level2;
        public Scene info;

        public DTTSGame()
        {
            // First time testing Singleton concept
            instance = this;

            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferHeight = screenHeight; //definição da altura
            _graphics.PreferredBackBufferWidth = screenWidth; //definição da largura
            _graphics.ApplyChanges();

            highScore = FileUtil.LoadScore() ?? new PlayerStats();
            // ?? operator means if the FileUtil.LoadScore() return null
            // it will assign the new PlayerStats(0) instead

            _spriteBatch = new SpriteBatch(GraphicsDevice);

            menu = new Menu();
            level1 = new Level1();
            level2 = new Level2();
            info = new InfoLevel();
            currentScene = menu;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            player = new Player(Content.Load<Texture2D>("Bird"), new(screenWidth / 2 - 35, screenHeight / 2 - 35));

            draw = new DrawingUtil(_spriteBatch);
            Sounds.LoadSounds(Content);

            spikeTexture = Content.Load<Texture2D>("Spike");
            squareTexture = Content.Load<Texture2D>("Square");

            mainFont = Content.Load<SpriteFont>("MainFont");
            scoreFont = Content.Load<SpriteFont>("ScoreFont");
            scoreCircle = Content.Load<Texture2D>("Circle");

            menu.LoadContent();
            level1.LoadContent();
            level2.LoadContent();
            info.LoadContent();
            currentScene.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            currentScene.Update(gameTime);

            base.Update(gameTime);
        }

        public void ChangeScene(Scene newScene)
        {
            currentScene.Restart();
            currentScene = newScene;
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(GameColors.backGround);
            currentScene.Draw(gameTime);

            base.Draw(gameTime);
        }

        public void HandlePlayerScore() => currentScene.HandlePlayerScore();
    }
}