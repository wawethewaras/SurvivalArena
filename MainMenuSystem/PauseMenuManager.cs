using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MainMenuSystem {

    public class PauseMenuManager {
        public Texture2D mainmenuBG;
        public EEButton start;
        public EEButton changeResolution;
        public EEButton changeVolume;
        public EEButton changeFullScreen;
        public EEButton quit;
        public event Action ReturnEvent;

        public PauseMenuManager(ContentManager contentManager, GraphicsDeviceManager graphicsDeviceManager, RenderTarget2D screen) {
            var screenScaleWitdh = graphicsDeviceManager.PreferredBackBufferWidth / screen.Width;
            mainmenuBG = contentManager.Load<Texture2D>("MainMenu_BG");
            var returnTexture = contentManager.Load<Texture2D>("Return_button");
            var resolution = contentManager.Load<Texture2D>("Resolution_button");
            var music = contentManager.Load<Texture2D>("Music_button");
            var fullscreen = contentManager.Load<Texture2D>("Fullscreen_button");
            var quitTexture = contentManager.Load<Texture2D>("Quit_Button");
            var buttonXPosition = screen.Width / 2 * screenScaleWitdh;
            var targetPosition = new Vector2(buttonXPosition - returnTexture.Width / 2, 100);

            start = new EEButton(returnTexture, targetPosition);
            targetPosition = new Vector2(buttonXPosition - resolution.Width / 2, 210);
            changeResolution = new EEButton(resolution, targetPosition);
            targetPosition = new Vector2(buttonXPosition - music.Width / 2, 320);
            changeVolume = new EEButton(music, targetPosition);
            targetPosition = new Vector2(buttonXPosition - fullscreen.Width / 2, 430);
            changeFullScreen = new EEButton(fullscreen, targetPosition);
            targetPosition = new Vector2(buttonXPosition - quitTexture.Width / 2, 530);
            quit = new EEButton(quitTexture, targetPosition);
        }

        public void Update(float gameTime) {
            GamePadCapabilities capabilities = GamePad.GetCapabilities(PlayerIndex.One);
            if (capabilities.IsConnected) {
                GamePadState state = GamePad.GetState(PlayerIndex.One);
                if (state.IsButtonDown(Buttons.Start)) {
                    ReturnEvent?.Invoke();
                }
            }
            start.Update(gameTime);
            changeResolution.Update(gameTime);
            changeVolume.Update(gameTime);
            changeFullScreen.Update(gameTime);
            quit.Update(gameTime);

        }
        public void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(mainmenuBG, Vector2.Zero, Color.White);
            start.Draw(spriteBatch);
            changeResolution.Draw(spriteBatch);
            changeVolume.Draw(spriteBatch);
            changeFullScreen.Draw(spriteBatch);
            quit.Draw(spriteBatch);
        }

    }
}