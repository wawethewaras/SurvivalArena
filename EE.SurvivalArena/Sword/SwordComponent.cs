using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SurvivalArena.ColliderSystem;
using SurvivalArena.GameObjects;

namespace SurvivalArena.Sword {
    public class SwordComponent : IComponent {
        public Texture2D swordTexture;
        public GameObject parent;
        public Vector2 Offset;
        bool mReleased = true;
        public GameObject sword;

        public SwordComponent(Texture2D swordTexture, GameObject parent) {
            this.swordTexture = swordTexture;
            this.parent = parent;
            Offset = new Vector2(swordTexture.Width, 0);
        }

        public void Update(float gameTime) {
            var mState = Mouse.GetState();

            if (mState.LeftButton == ButtonState.Pressed && mReleased) {
                mReleased = false;
                //var direction = parent.colliderComponent.LookingRight ? Offset : -Offset;
                //sword = new GameObject(swordTexture, parent, direction);
                //var collider = new ColliderComponent(sword, swordTexture.Width, swordTexture.Height);
                //collider.tag = "Sword";
                //sword.colliderComponent = collider;
                //Level.gameObjects.Add(sword);

            }
            if (mState.LeftButton == ButtonState.Released) {
                if (sword != null) {
                    //Level.gameObjects.Remove(sword);
                    //ColliderComponent.ColliderComponents.Remove(sword.colliderComponent);
                    //sword = null;
                }

                mReleased = true;
            }

        }
    }
}
