using Microsoft.Xna.Framework.Input;
using SurvivalArena.GameObjects;

namespace EE.InputSystem {
    public class InputComponent : IComponent {
        bool mReleased = true;

        public event Action APressed;
        public event Action DPressed;
        public event Action SpacePressed;

        public bool spaceReleased = true;
        public InputComponent() {
        }

        public void Update(float gameTime) {
            if (Keyboard.GetState().IsKeyDown(Keys.A)) {
                APressed?.Invoke();
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.D)) {
                DPressed?.Invoke();

            }

            if (Keyboard.GetState().IsKeyDown(Keys.Space) && spaceReleased) {
                SpacePressed?.Invoke();
                spaceReleased = false; ;
            }
            if (Keyboard.GetState().IsKeyUp(Keys.Space)) {
                spaceReleased = true;
            }

        }
    }
}
