
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SurvivalArena {
    public class Tile : IHasPosition{
        public Texture2D texture;
        public Vector2 position;
        public ColliderComponent colliderComponent;

        Vector2 IHasPosition.Position { get => position; set => position = value; }
    }
}
