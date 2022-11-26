using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SurvivalArena;

namespace MainMenuSystem {
    public class MainMenuManager : IGame {
        SpriteFont font;
        Texture2D rectange;
        Vector2 targetPosition;

        public MainMenuManager() {

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

        public void Update(GameTime gameTime) {
        }
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
            spriteBatch.DrawString(font, "Menu", targetPosition, Color.White);

        }
    }
}