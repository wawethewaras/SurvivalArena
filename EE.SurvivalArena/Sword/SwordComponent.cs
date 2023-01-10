using Microsoft.Xna.Framework.Input;
using SurvivalArena.GameObjects;

namespace SurvivalArena.Sword {
    public class SwordComponent : IComponent {
        public GameObject sword;
        public MouseState mState;

        public float swordTime = 0.15f;
        public float swordTimeCounter = 0;

        public event Action SwordAttack;
        public event Action SwordAttackCancel;

        public SwordComponent() {

        }

        public void Update(float gameTime) {
            if (swordTimeCounter > 0) {
                swordTimeCounter -= gameTime;
            }
            mState = Mouse.GetState();

            if (swordTimeCounter <= 0) {
                SwordAttackCancel?.Invoke();
            }
        }

        public void CreateSword() {
            swordTimeCounter = swordTime;
            SwordAttack?.Invoke();
        }
    }
}
