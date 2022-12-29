using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MainMenuSystem {

    public class PauseMenuManager {

        public EEButton start;
        public EEButton quit;
        public event Action ReturnEvent;

        public PauseMenuManager() {
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