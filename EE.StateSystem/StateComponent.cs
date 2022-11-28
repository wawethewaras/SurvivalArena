using SurvivalArena.GameObjects;

namespace EE.StateSystem {
    public class StateComponent : IComponent {
        public event Action OnStart;
        public event Action<float> OnAct;
        public event Action OnStop;

        public void Update(float gameTime) {
            OnAct?.Invoke(gameTime);
        }
    }

    public class ShootAction {
        public float shootDelay = 3;
        public float shootDelayTimer = 3;
        public event Action ShootEvent;

        public void Shoot(float gameTime) {
            if (shootDelayTimer > 0) {
                shootDelayTimer -= gameTime;
            }
            if (shootDelayTimer <= 0) {
                ShootEvent?.Invoke();
                shootDelayTimer = shootDelay;
            }
        }
    }
}