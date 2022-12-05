using EE.PoolingSystem;
using EE.ScoreSystem;
using EE.SpriteRendererSystem;
using MainMenuSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using SurvivalArena.ColliderSystem;
using SurvivalArena.GameObjects;
using SurvivalArena.TileSystem;

namespace SurvivalArena {
    public class SurvivalArenaGame : IGame {

        public enum GameState { 
            Running,
            GameOver,
            Win
        }
        Level level;
        SpriteFont font;
        Song music;

        Vector2 scorePosition;
        ContentManager contentManager;
        public static GameState gameState = GameState.Running;

        public static TextInputComponent textInputComponent;

        EEButton eEButton;
        UICanvas uICanvas;
        RenderTarget2D screen;

        public SurvivalArenaGame(RenderTarget2D screen) : base() {
            this.screen = screen;

        }

        public void Initialize() {
        }
        public void LoadContent(IServiceProvider serviceProvider, GraphicsDeviceManager graphicsDeviceManager, GameWindow gameWindow) {
            contentManager = new ContentManager(serviceProvider, "Content");
            ColliderComponent.rectangeTexture = new Texture2D(graphicsDeviceManager.GraphicsDevice, 1, 1);
            ColliderComponent.rectangeTexture.SetData(new[] { Color.White });
            level = new Level(contentManager);

            font = contentManager.Load<SpriteFont>("FontTest");
            scorePosition = new Vector2(0, 0);

            music = contentManager.Load<Song>("BGMusic");
            MediaPlayer.Play(music);
            MediaPlayer.Volume = 0.00f;
            MediaPlayer.IsRepeating = true;

            ScoreManager.LoadHighScore();

            textInputComponent = new TextInputComponent(font, gameWindow);
            eEButton = new EEButton(graphicsDeviceManager);
            uICanvas = new UICanvas(screen.Width, screen.Height, font);

        }
        public void UnloadContent() {
        }
        public void Update(GameTime gameTime) {
            var time = (float)gameTime.ElapsedGameTime.TotalSeconds;

            switch (gameState) {
                case GameState.Running:
                    RunGame(time);
                    eEButton.Update(time);

                    break;
                case GameState.GameOver:
                case GameState.Win:
                    textInputComponent.Update(time);

                    if (Keyboard.GetState().IsKeyDown(Keys.R)) {
                        gameState = GameState.Running;
                        SpriteRendererComponent.spriteRendererComponents = new List<IEEDrawable>();
                        ColliderComponent.ColliderComponents = new List<ColliderComponent>();
                        PoolManager.gameObjects = new List<IUpdater>();
                        GameObjectSpawner.currentWaves = 0;
                        level = new Level(contentManager);
                        MediaPlayer.Play(music);
                        if (!ScoreManager.ScoreSaved && ScoreManager.Score > 0) {
                            ScoreManager.Name = textInputComponent.GetText();
                            ScoreManager.SaveCurrentScore();
                            ScoreManager.StoreScore();
                        }

                        ScoreManager.Score = 0;
                        ScoreManager.ScoreSaved = false;
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.Enter) && !ScoreManager.ScoreSaved && ScoreManager.Score > 0) {
                        ScoreManager.Name = textInputComponent.GetText();

                        ScoreManager.SaveCurrentScore();
                        ScoreManager.StoreScore();
                        ScoreManager.ScoreSaved = true;

                    }
                    break;
                default:
                    break;
            }

        }

        private void RunGame(float gameTime) {

            level.Update(gameTime);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
            int startY = 150;
            int offset = 15;

            switch (gameState) {
                case GameState.Running:
                    level.Draw(spriteBatch);
                    spriteBatch.DrawString(font, $"Score: {ScoreManager.Score}", scorePosition, Color.White);
                    eEButton.Draw(spriteBatch);

                    break;
                case GameState.GameOver:
                    uICanvas.DrawToCenter(spriteBatch, new Vector2(0, startY), "Game Over!");
                    uICanvas.DrawToCenter(spriteBatch, new Vector2(0, startY + offset), $"Score: {ScoreManager.Score}");
                    uICanvas.DrawToCenter(spriteBatch, new Vector2(0, startY + (offset * 2)), "Press R to Restart.");

                    startY += (offset * 4);
                    textInputComponent.Position = new Vector2(screen.Width, startY);
                    textInputComponent.Draw(spriteBatch);
                    DrawHighScores(spriteBatch, startY, offset);
                    break;
                case GameState.Win:
                    uICanvas.DrawToCenter(spriteBatch, new Vector2(0, startY), "Win!");
                    uICanvas.DrawToCenter(spriteBatch, new Vector2(0, startY + offset),  $"Score: {ScoreManager.Score}");
                    uICanvas.DrawToCenter(spriteBatch, new Vector2(0, startY + (offset * 2)), "Press R to Restart.");
                    startY += (offset * 4);
                    textInputComponent.Position = new Vector2(0, startY);
                    textInputComponent.Draw(spriteBatch);

                    DrawHighScores(spriteBatch, startY, offset);
                    break;
                default:
                    break;
            }


            //for (int i = ColliderComponent.ColliderComponents.Count - 1; i >= 0; i--) {
            //    if (!ColliderComponent.ColliderComponents[i].IsActive) {
            //        continue;
            //    }
            //    spriteBatch.Draw(ColliderComponent.rectangeTexture, ColliderComponent.ColliderComponents[i].Rectangle,
            //Color.Chocolate);
            //}
        }

        private int DrawHighScores(SpriteBatch spriteBatch, int startY, int offset) {
            startY += (offset * 4);
            uICanvas.DrawToCenter(spriteBatch, new Vector2(0, startY), $"High Scores");
            startY += offset;
            foreach (var score in ScoreManager.Highscores) {
                uICanvas.DrawToCenter(spriteBatch, new Vector2(0, startY + offset), score.name + " " + score.number);
                startY += offset;
            }

            return startY;
        }

        public static void GameOver() {
            textInputComponent.ResetText();
            gameState = GameState.GameOver;
        }
        public static void Win() {
            textInputComponent.ResetText();
            gameState = GameState.Win;
        }



    }
}
