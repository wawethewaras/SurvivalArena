using EE.PoolingSystem;
using EE.ScoreSystem;
using EE.SpriteRendererSystem;
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
        public SurvivalArenaGame() : base() {
        }

        public void Initialize() {
        }
        public void LoadContent(IServiceProvider serviceProvider, GraphicsDeviceManager graphicsDeviceManager) {
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

        }
        public void UnloadContent() {
        }
        public void Update(GameTime gameTime) {
            if (Keyboard.GetState().IsKeyDown(Keys.R) && gameState != GameState.Running) {
                gameState = GameState.Running;
                SpriteRendererComponent.spriteRendererComponents = new List<IEEDrawable>();
                ColliderComponent.ColliderComponents = new List<ColliderComponent>();
                PoolManager.gameObjects = new List<IUpdater>();
                GameObjectSpawner.currentWaves = 0;
                level = new Level(contentManager);
                MediaPlayer.Play(music);

            }
            var time = (float)gameTime.ElapsedGameTime.TotalSeconds;
            level.Update(time);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
            switch (gameState) {
                case GameState.Running:
                    level.Draw(spriteBatch);
                    spriteBatch.DrawString(font, $"Score: {ScoreManager.Score}", scorePosition, Color.White);
                    break;
                case GameState.GameOver:
                    spriteBatch.DrawString(font, "Game Over!", new Vector2(600, 350), Color.White);
                    spriteBatch.DrawString(font, $"Score: {ScoreManager.Score}", new Vector2(600, 365), Color.White);
                    spriteBatch.DrawString(font, "Press R to Restart.", new Vector2(600, 380), Color.White);
                    break;
                case GameState.Win:
                    spriteBatch.DrawString(font, "Win!", new Vector2(600, 350), Color.White);
                    spriteBatch.DrawString(font, $"Score: {ScoreManager.Score}", new Vector2(600, 365), Color.White);
                    spriteBatch.DrawString(font, "Press R to Restart.", new Vector2(600, 380), Color.White);
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
        public static void GameOver() {
            gameState = GameState.GameOver;
        }
        public static void Win() {
            gameState = GameState.Win;
        }
    }
}
