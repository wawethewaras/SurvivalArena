using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SurvivalArena {
    internal class BaseGame : Game {

        private const int SCREENWIDTH = 1280;
        private const int SCREENHEIGHT = 720;

        private GraphicsDeviceManager graphicsDeviceManager;

        private SpriteBatch? spriteBatch = null;
        private ISurvivalArenaGame Game = null;
        private RenderTarget2D? screen;

        public BaseGame() : base() {
            graphicsDeviceManager = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            Game = new SurvivalArenaGame();
        }

        protected override void Initialize() {
            graphicsDeviceManager.IsFullScreen = false;
            graphicsDeviceManager.PreferredBackBufferWidth = SCREENWIDTH;
            graphicsDeviceManager.PreferredBackBufferHeight = SCREENHEIGHT;
            graphicsDeviceManager.ApplyChanges();
            screen = new RenderTarget2D(graphicsDeviceManager.GraphicsDevice, SCREENWIDTH, SCREENHEIGHT);
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Game.Initialize();
            base.Initialize();
        }
        protected override void LoadContent() {

            Game.LoadContent(Services, graphicsDeviceManager);
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
            spriteBatch.Draw(screen, new Rectangle(0, 0, SCREENWIDTH, SCREENHEIGHT), Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
