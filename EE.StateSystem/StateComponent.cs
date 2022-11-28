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
}