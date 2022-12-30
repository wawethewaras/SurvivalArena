using EE.SpriteRendererSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SurvivalArena.GameObjects;

namespace SurvivalArena.TileSystem {
    public class BGDrawer : IEEDrawable {
        private Texture2D texture2D;


        public int drawOrder = -1;

        public BGDrawer(Texture2D texture2D) {
            this.texture2D = texture2D;
            SpriteRendererComponent.spriteRendererComponents.Add(this);
        }

        public int DrawOrder => drawOrder;

        public void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(texture2D, Vector2.Zero, Color.White);
        }
    }

}
