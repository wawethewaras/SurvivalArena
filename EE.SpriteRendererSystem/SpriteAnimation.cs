using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EE.SpriteRendererSystem {
    public class SpriteAnimation {
        public List<Sprite> sprites = new List<Sprite>();

        private int currentIndex = 0;

        public float frameTimer = 0;
        public float frameDuration = 0.2f;
        public bool loop = true;

        public SpriteAnimation(Texture2D texture2D, int frameSizeX, int frameSizeY) {
            var width = texture2D.Width;
            var height = texture2D.Height;

            for (int y = 0; y < height; y += frameSizeY) {
                for (int x = 0; x < width; x += frameSizeX) {
                    var sprite = new Sprite();
                    sprite.sourceRectangle = new Rectangle(x,y, frameSizeX, frameSizeY);
                    sprite.texture2D = texture2D;
                    sprites.Add(sprite);
                }
            }

        }

        public void Update(float gameTime) {
            frameTimer += gameTime;
            if (frameTimer >= frameDuration) {
                frameTimer = 0;
                currentIndex++;
                if (currentIndex >= sprites.Count) {
                    currentIndex = loop ? 0 : sprites.Count - 1;
                }
            }
        }

        public Sprite GetSprite() => sprites[currentIndex];
    }
    public class Sprite {
        public Rectangle sourceRectangle;
        public Texture2D texture2D;

    }
}
