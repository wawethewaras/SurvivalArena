using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SurvivalArena.GameObjects;

namespace EE.SpriteRendererSystem {
    public static class DrawManager {
        public static void DrawAll(SpriteBatch spriteBatch) {
            SpriteRendererComponent.spriteRendererComponents = SpriteRendererComponent.spriteRendererComponents.OrderByDescending(x => x.DrawOrder).ToList();

            for (int i = SpriteRendererComponent.spriteRendererComponents.Count - 1; i >= 0; i--) {
                SpriteRendererComponent.spriteRendererComponents[i].Draw(spriteBatch);
            }
        }
    }
    public class SpriteRendererComponent : IComponent, IEEDrawable {
        public static List<IEEDrawable> spriteRendererComponents = new List<IEEDrawable>();

        public SpriteAnimation spriteAnimation;
        public IHasPosition hasPosition;
        public IHasFacingDirection hasFacingDirection;

        private bool isActive = true;

        private int drawOrder = 0;
        public int DrawOrder => drawOrder;

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