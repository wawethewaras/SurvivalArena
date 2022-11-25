using Microsoft.Xna.Framework.Graphics;

namespace SurvivalArena {
    public interface IUpdater {
        void Update(float gameTime);
        void Draw(SpriteBatch spriteBatch);

    }
}
