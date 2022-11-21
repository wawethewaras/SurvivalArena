using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SurvivalArena {
    internal class BaseGame : Game {

        private GraphicsDeviceManager graphicsDeviceManager;

        private SpriteBatch? spriteBatch = null;
        private ISurvivalArenaGame Game = null;
        private RenderTarget2D? screen;

        public BaseGame() : base() {
            graphicsDeviceManager = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsFixedTimeStep = true;
            IsMouseVisible = true;

            Game = new SurvivalArenaGame();
        }

        protected override void Initialize() {
            screen = new RenderTarget2D(graphicsDeviceManager.GraphicsDevice, 420, 168);
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Game.Initialize();
            base.Initialize();
        }
        protected override void LoadContent() {

            Game.LoadContent();
        }
        protected override void UnloadContent() {
            Game.UnloadContent();
            base.UnloadContent();
        }
        protected override void Update(GameTime gameTime) {
            Game.Update(gameTime);
            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime) {
            graphicsDeviceManager.GraphicsDevice.SetRenderTarget(screen);
            graphicsDeviceManager.GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();

            Game.Draw(gameTime, spriteBatch);
            spriteBatch.End();

            spriteBatch.Begin();
            graphicsDeviceManager.GraphicsDevice.SetRenderTarget(null);
            spriteBatch.Draw(screen, new Rectangle(0, 0, 840, 336), Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
