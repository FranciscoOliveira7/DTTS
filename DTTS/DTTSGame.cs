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

        public const int screenHeight = 850, screenWidth = 700;

        public Player player;

        public PlayerStats highScore = new PlayerStats();

        private Scene currentScene;
        public Scene menu;
        public Scene level1;

        private bool hasGameStarted;

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

            hasGameStarted = false;

            highScore = FileUtil.LoadScore() ?? new PlayerStats(0);
            // ?? operator means if the FileUtil.LoadScore() return null
            // it will assign the new PlayerStats(0) instead

            _spriteBatch = new SpriteBatch(GraphicsDevice);

            menu = new Menu();
            level1 = new Level1();
            currentScene = menu;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            draw = new DrawingUtil(_spriteBatch);
            Sounds.LoadSounds(Content);

            mainFont = Content.Load<SpriteFont>("MainFont");
            scoreFont = Content.Load<SpriteFont>("ScoreFont");
            scoreCircle = Content.Load<Texture2D>("Circle");

            menu.LoadContent();
            level1.LoadContent();
            currentScene.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            currentScene.Update(gameTime);

            if (Keyboard.GetState().IsKeyDown(Keys.Space) && !hasGameStarted)
            {
                ChangeScene(level1);
                hasGameStarted = true;
            }

            base.Update(gameTime);
        }

        public void ChangeScene(Scene newScene)
        {
            if (currentScene is Level1) ((Level1)currentScene).Restart();
            currentScene = newScene;
            hasGameStarted = false;
        }

        protected override void Draw(GameTime gameTime) => currentScene.Draw(gameTime);

        public void HandlePlayerScore() => currentScene.HandlePlayerScore();
    }
}