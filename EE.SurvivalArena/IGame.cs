using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SurvivalArena {
    public interface IGame {
        void Draw(GameTime gameTime, SpriteBatch spriteBatch);
        void Initialize();
        void LoadContent(IServiceProvider serviceProvider, GraphicsDeviceManager graphicsDeviceManager, GameWindow gameWindow);
        void UnloadContent();
        void Update(GameTime gameTime);
        Action Quit { get; set; }
    }
}