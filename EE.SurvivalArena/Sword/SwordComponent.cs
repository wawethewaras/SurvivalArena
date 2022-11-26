using EE.PoolingSystem;
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

        public event Action SwordAttack;

        public event Action SwordAttackCancel;

        public SwordComponent(Texture2D swordTexture, GameObject parent) {
            this.swordTexture = swordTexture;
            this.parent = parent;
            Offset = new Vector2(swordTexture.Width, 0);
        }

        public void Update(float gameTime) {
            var mState = Mouse.GetState();

            if (mState.LeftButton == ButtonState.Pressed && mReleased) {
                mReleased = false;
                SwordAttack?.Invoke();
            }
            if (mState.LeftButton == ButtonState.Released) {
                SwordAttackCancel?.Invoke();

                mReleased = true;
            }

        }
    }
}
