using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SurvivalArena.GameObjects;
using SurvivalArena.HealthSystem;

namespace EE.HealthSystem {
    public class HealthUIManager : IEEDrawable {
        SpriteFont font;
        HealthComponent healthComponent;
        Vector2 textPosition;
        private int drawOrder = 1;
        public int DrawOrder => drawOrder;

        public List<Image> spriteRendererComponents = new List<Image>();
        private bool isActive = true;

        public HealthUIManager(ContentManager contentManager, HealthComponent healthComponent, IHasPosition hasPosition) {
            var playerHealth = contentManager.Load<Texture2D>("Health");

            this.healthComponent = healthComponent;

            for (int i = 0; i < healthComponent.health; i++) {
                var offSet = new HasPositionWithOfSet(hasPosition, null, new Vector2(i *10 -10, -10));
                var swordRender = new Image(playerHealth, offSet);
                spriteRendererComponents.Add(swordRender);
            }
        }

        public void Draw(SpriteBatch spriteBatch) {
            if (!isActive) {
                return;
            }
            for (int i = 0; i < spriteRendererComponents.Count; i++) {
                if (i < healthComponent.health) {
                    spriteRendererComponents[i].color = Color.Red;
                    spriteRendererComponents[i].Draw(spriteBatch);
                }
                else {
                    spriteRendererComponents[i].color = Color.Gray;
                    spriteRendererComponents[i].Draw(spriteBatch);

                }
            }
        }
        public void SetActive() {
            isActive = true;
        }
        public void DeActive() {
            isActive = false;
        }
        public class Image : IEEDrawable {
            public int DrawOrder => throw new NotImplementedException();
            Texture2D texture2D;
            public IHasPosition hasPosition;
            private bool isActive = true;
            public Color color;

            public Image(Texture2D texture2D, IHasPosition hasPosition) {
                this.texture2D = texture2D;
                this.hasPosition = hasPosition;
            }

            public void Draw(SpriteBatch spriteBatch) {
                if (!isActive) {
                    return;
                }
                spriteBatch.Draw(texture2D, hasPosition.Position, color);

            }
        }
    }
}
