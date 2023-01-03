using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MainMenuSystem {
    public class MainMenuManager {

        public Texture2D mainmenuBG;

        public EEButton start;
        public EEButton quit;

        public static float screenScaleWitdh = 1;
        public static float screenScaleHeight = 1;

        public event Action GameStarted;
        public MainMenuManager(ContentManager contentManager, GraphicsDeviceManager graphicsDeviceManager, RenderTarget2D screen) {
            screenScaleWitdh = graphicsDeviceManager.PreferredBackBufferWidth / screen.Width;
            screenScaleHeight = graphicsDeviceManager.PreferredBackBufferHeight / screen.Height;
            mainmenuBG = contentManager.Load<Texture2D>("MainMenu_BG");
            var startTexture = contentManager.Load<Texture2D>("start_button");

            var buttonXPosition = screen.Width / 2 * screenScaleWitdh - startTexture.Width /2;
            var targetPosition = new Vector2(buttonXPosition, 100);
            start = new EEButton(startTexture, targetPosition);
            targetPosition = new Vector2(buttonXPosition, 210);
            var quitTexture = contentManager.Load<Texture2D>("Quit_Button");

            quit = new EEButton(quitTexture, targetPosition);
        }


        public void Initialize() {

        }

        public void LoadContent(IServiceProvider serviceProvider, GraphicsDeviceManager graphicsDeviceManager) {

        }

        public void UnloadContent() {
        }

        public void Update(float gameTime) {
            GamePadCapabilities capabilities = GamePad.GetCapabilities(PlayerIndex.One);
            if (capabilities.IsConnected) {
                GamePadState state = GamePad.GetState(PlayerIndex.One);
                if (state.IsButtonDown(Buttons.Start)) {
                    GameStarted?.Invoke();
                }
            }

            start.Update(gameTime);
            quit.Update(gameTime);

        }
        public void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(mainmenuBG, Vector2.Zero, Color.White);
            start.Draw(spriteBatch);
            quit.Draw(spriteBatch);
        }
    }
}