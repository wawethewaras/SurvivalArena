using MainMenuSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SurvivalArena {
    internal class BaseGame : Game {

        private const int SCREENWIDTH = 1280;
        private const int SCREENHEIGHT = 720;
        private const int GAMEWIDTH = 640;
        private const int GAMEHEIGHT= 480;

        private GraphicsDeviceManager graphicsDeviceManager;

        private SpriteBatch? spriteBatch = null;
        private IGame Game = null;
        private RenderTarget2D? screen;

        public BaseGame() : base() {
            graphicsDeviceManager = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

        }

        protected override void Initialize() {
            graphicsDeviceManager.IsFullScreen = false;
            graphicsDeviceManager.PreferredBackBufferWidth = SCREENWIDTH;
            graphicsDeviceManager.PreferredBackBufferHeight = SCREENHEIGHT;
            graphicsDeviceManager.ApplyChanges();
            screen = new RenderTarget2D(graphicsDeviceManager.GraphicsDevice, GAMEWIDTH, GAMEHEIGHT);
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Game = new SurvivalArenaGame(screen);
            Game.Quit += Exit;
            Game.Initialize();
            base.Initialize();
        }
        protected override void LoadContent() {           
            Game.LoadContent(Services, graphicsDeviceManager, Window);
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

            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);

            Game.Draw(gameTime, spriteBatch);
            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);
            graphicsDeviceManager.GraphicsDevice.SetRenderTarget(null);
            spriteBatch.Draw(screen, new Rectangle(0, 0, SCREENWIDTH, SCREENHEIGHT), Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
