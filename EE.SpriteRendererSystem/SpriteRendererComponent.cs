using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SurvivalArena.GameObjects;

namespace EE.SpriteRendererSystem {
    public class SpriteRendererComponent : IComponent, IEEDrawable {
        public static List<IEEDrawable> spriteRendererComponents = new List<IEEDrawable>();

        public SpriteAnimation spriteAnimation;
        public IHasPosition hasPosition;
        public IHasFacingDirection hasFacingDirection;

        private bool isActive = true;
        public SpriteRendererComponent(SpriteAnimation spriteAnimation, IHasPosition hasPosition, IHasFacingDirection hasFacingDirection = null) {
            this.spriteAnimation = spriteAnimation;
            this.hasPosition = hasPosition;
            spriteRendererComponents.Add(this);
            this.hasFacingDirection = hasFacingDirection;
        }

        public void Update(float gameTime) {
            spriteAnimation.Update(gameTime);
        }
        public void Draw(SpriteBatch spriteBatch) {
            if (!isActive) {
                return;
            }
            var sprite = spriteAnimation.GetSprite();
            if (hasFacingDirection != null && !hasFacingDirection.LookingRight) {
                SpriteEffects flip = SpriteEffects.FlipHorizontally;
                spriteBatch.Draw(sprite.texture2D, hasPosition.Position, sprite.sourceRectangle, Color.White, 0, Vector2.Zero, 1, flip, 1);
            }
            else {
                spriteBatch.Draw(sprite.texture2D, hasPosition.Position, sprite.sourceRectangle, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 1);
            }

        }
        public void ChangeAnimation(SpriteAnimation newSpriteAnimation) {
            spriteAnimation = newSpriteAnimation;
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