using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MainMenuSystem {
    public class MainMenuManager {
        SpriteFont font;
        Texture2D rectange;
        Vector2 targetPosition;
        GraphicsDeviceManager graphicsDeviceManager;

        public EEButton start;
        public EEButton quit;

        public MainMenuManager(GraphicsDeviceManager graphicsDeviceManager, RenderTarget2D screen) {
            this.graphicsDeviceManager = graphicsDeviceManager;
        }



        public void Initialize() {

        }

        public void LoadContent(IServiceProvider serviceProvider, GraphicsDeviceManager graphicsDeviceManager) {
            var Content = new ContentManager(serviceProvider, "Content");

            font = Content.Load<SpriteFont>("FontTest");
            targetPosition = new Vector2(graphicsDeviceManager.PreferredBackBufferWidth/2, 0);

        }

        public void UnloadContent() {
        }

        public void Update(float gameTime) {
            start.Update(gameTime);
            quit.Update(gameTime);

        }
        public void Draw(SpriteBatch spriteBatch) {
            start.Draw(spriteBatch);
            quit.Draw(spriteBatch);
        }
    }
}