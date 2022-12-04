using Microsoft.Xna.Framework.Graphics;

namespace EE.SpriteRendererSystem {
    public class SpriteAnimation {
        public Texture2D[] sprites;

        private int currentIndex = 0;

        public float frameTimer = 0;
        public float frameDuration = 0.2f; //ms
        public bool loop = true;

        public SpriteAnimation(Texture2D[] sprites) {
            this.sprites = sprites;
        }

        public void Update(float gameTime) {
            frameTimer += gameTime;
            if (frameTimer >= frameDuration) {
                frameTimer = 0;
                currentIndex++;
                if (currentIndex >= sprites.Length) {
                    currentIndex = loop ? 0 : sprites.Length - 1;
                }
            }
        }

        public Texture2D GetTexture() => sprites[currentIndex];
    }
}
