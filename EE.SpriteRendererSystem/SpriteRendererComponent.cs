using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SurvivalArena.GameObjects;

namespace EE.SpriteRendererSystem {
    public class SpriteRendererComponent : IComponent {
        public static List<SpriteRendererComponent> spriteRendererComponents = new List<SpriteRendererComponent>();

        public Texture2D _texture;
        public IHasPosition hasPosition;
        private bool isActive = true;
        public SpriteRendererComponent(Texture2D texture, IHasPosition hasPosition) {
            _texture = texture;
            this.hasPosition = hasPosition;
            spriteRendererComponents.Add(this);
        }

        public void Update(float gameTime) {

        }
        public void Draw(SpriteBatch spriteBatch) {
            if (!isActive) {
                return;
            }
            spriteBatch.Draw(_texture, hasPosition.Position, Color.White);
        }

        public void OnDestroy() {
            spriteRendererComponents.Remove(this);
        }

        public void SetActive() {
            isActive = true;
        }
        public void DeActive() {
            isActive = false;
        }
    }

}