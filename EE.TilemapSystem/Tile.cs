
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SurvivalArena.GameObjects;

namespace SurvivalArena.TileSystem {
    public class Tile : IHasPosition {
        public Texture2D texture;
        public Vector2 position;

        public Vector2 Position { get => position; set => position = value; }
    }
}
