using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SurvivalArena {
    public interface ISurvivalArenaGame {
        void Draw(GameTime gameTime, SpriteBatch spriteBatch);
        void Initialize();
        void LoadContent(IServiceProvider serviceProvider, GraphicsDeviceManager graphicsDeviceManager);
        void UnloadContent();
        void Update(GameTime gameTime);
    }
}