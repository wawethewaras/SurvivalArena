using EE.InputSystem;
using MainMenuSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SurvivalArena {

    internal class BaseGame : Game {

        private const int SCREENWIDTH = 1920;
        private const int SCREENHEIGHT = 1080;
        private const int GAMEWIDTH = 640;
        private const int GAMEHEIGHT= 480;

        private GraphicsDeviceManager graphicsDeviceManager;

        private SpriteBatch? spriteBatch = null;
        private IGame Game = null;
        private RenderTarget2D? screen;
        GameSettings gameSettings;

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
            gameSettings = new GameSettings(GAMEWIDTH, GAMEHEIGHT, SCREENWIDTH, SCREENHEIGHT);
            gameSettings.NewScrren += NewScreen;
            gameSettings.FullScreenEvent += FullScreen;
            Game = new SurvivalArenaGame(screen, gameSettings);
            InputComponent.XScale = (float)SCREENWIDTH / GAMEWIDTH;
            InputComponent.YScale = (float)SCREENHEIGHT /GAMEHEIGHT;

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
            graphicsDeviceManager.GraphicsDevice.Clear(Color.DimGray);

            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);

            Game.Draw(gameTime, spriteBatch);
            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);
            graphicsDeviceManager.GraphicsDevice.SetRenderTarget(null);
            spriteBatch.Draw(screen, new Rectangle(0, 0, gameSettings.SCREENWIDTH, gameSettings.SCREENHEIGHT), Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }
        public void NewScreen(int gAMEWIDTH, int gAMEHEIGHT, int sCREENWIDTH, int sCREENHEIGHT) {
            graphicsDeviceManager.PreferredBackBufferWidth = sCREENWIDTH;
            graphicsDeviceManager.PreferredBackBufferHeight = sCREENHEIGHT;
            graphicsDeviceManager.ApplyChanges();
            screen = new RenderTarget2D(graphicsDeviceManager.GraphicsDevice, gAMEWIDTH, gAMEHEIGHT);
            spriteBatch = new SpriteBatch(GraphicsDevice);
            InputComponent.XScale = (float)sCREENWIDTH / gAMEWIDTH;
            InputComponent.YScale = (float)sCREENHEIGHT / gAMEHEIGHT;
            MainMenuManager.screenScaleWitdh = graphicsDeviceManager.PreferredBackBufferWidth / screen.Width;
            MainMenuManager.screenScaleHeight = graphicsDeviceManager.PreferredBackBufferHeight / screen.Height;
        }
        public void FullScreen() {
            graphicsDeviceManager.IsFullScreen = !graphicsDeviceManager.IsFullScreen;
            graphicsDeviceManager.ApplyChanges();
        }
    }
}
