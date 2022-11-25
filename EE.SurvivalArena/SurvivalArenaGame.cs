using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SurvivalArena.TileSystem;

namespace SurvivalArena {
    public class SurvivalArenaGame : IGame {

        Level level;
        public SurvivalArenaGame() : base() {
        }

        public void Initialize() {
        }
        public void LoadContent(IServiceProvider serviceProvider, GraphicsDeviceManager graphicsDeviceManager) {
            var contentManager = new ContentManager(serviceProvider, "Content");
            level = new Level(contentManager);
        }
        public void UnloadContent() {
        }
        public void Update(GameTime gameTime) {
            var time = (float)gameTime.ElapsedGameTime.TotalSeconds;
            level.Update(time);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
            level.Draw(spriteBatch);
        }
    }
}
