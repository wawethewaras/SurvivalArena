using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MainMenuSystem {

    public class PauseMenuManager {

        public EEButton start;
        public EEButton quit;
        public event Action ReturnEvent;

        public PauseMenuManager(ContentManager contentManager, GraphicsDeviceManager graphicsDeviceManager, RenderTarget2D screen) {
            var targetPosition = new Vector2(screen.Width / 2, 100);
            var returnTexture = contentManager.Load<Texture2D>("Return_button");
            var quitTexture = contentManager.Load<Texture2D>("Quit_Button");

            start = new EEButton(returnTexture, targetPosition);
            targetPosition = new Vector2(screen.Width / 2, 210);
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
                    ReturnEvent?.Invoke();
                }
            }
            start.Update(gameTime);
            quit.Update(gameTime);

        }
        public void Draw(SpriteBatch spriteBatch) {
            start.Draw(spriteBatch);
            quit.Draw(spriteBatch);
        }
    }
}