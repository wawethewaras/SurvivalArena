using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SurvivalArena.GameObjects;

namespace EE.SpriteRendererSystem {
    public class SpriteRendererComponent : IComponent {
        public static List<SpriteRendererComponent> spriteRendererComponents = new List<SpriteRendererComponent>();

        public Texture2D _texture;
        public IHasPosition hasPosition;
        public IHasFacingDirection hasFacingDirection;

        private bool isActive = true;
        public SpriteRendererComponent(Texture2D texture, IHasPosition hasPosition, IHasFacingDirection hasFacingDirection = null) {
            _texture = texture;
            this.hasPosition = hasPosition;
            spriteRendererComponents.Add(this);
            this.hasFacingDirection = hasFacingDirection;
        }

        public void Update(float gameTime) {

        }
        public void Draw(SpriteBatch spriteBatch) {
            if (!isActive) {
                return;
            }
            if (hasFacingDirection != null && !hasFacingDirection.LookingRight) {
                SpriteEffects flip = SpriteEffects.FlipHorizontally;
                spriteBatch.Draw(_texture, hasPosition.Position, null, Color.White, 0, Vector2.Zero, 1, flip, 1);
            }
            else {
                spriteBatch.Draw(_texture, hasPosition.Position, Color.White);
            }

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