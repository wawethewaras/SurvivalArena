using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MainMenuSystem {
    public class MainMenuManager {


        public EEButton start;
        public EEButton quit;

        public static float screenScaleWitdh = 1;
        public static float screenScaleHeight = 1;

        public event Action GameStarted;
        public MainMenuManager() {
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
            start.Draw(spriteBatch);
            quit.Draw(spriteBatch);
        }
    }
}