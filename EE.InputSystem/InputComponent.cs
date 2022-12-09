using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SurvivalArena.GameObjects;

namespace EE.InputSystem {
    public class InputComponent : IComponent {
        bool mReleased = true;


        public bool spacePressed;
        public bool attackPressed;

        public bool APressed;
        public bool DPressed;
        public bool SpacePressed {
            get {
                if (spacePressed) {
                    spacePressed = false;
                    return true;
                }
                return false;
            }
        }
        public bool spaceReleased = true;

        public bool AttackPressed {
            get {
                if (attackPressed) {
                    attackPressed = false;
                    return true;
                }
                return false;
            }
        }
        public bool attackReleased = true;

        public InputComponent() {
        }

        public void Update(float gameTime) {
            GamePadCapabilities capabilities = GamePad.GetCapabilities(PlayerIndex.One);
            if (capabilities.IsConnected) {
                ControllerInput(capabilities);
            }
            else {
                KeyboardInput();
            }
        }

        public void KeyboardInput() {
            if (Keyboard.GetState().IsKeyDown(Keys.A)) {
                APressed = true;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D)) {
                DPressed = true;
            }
            if (Keyboard.GetState().IsKeyUp(Keys.A)) {
                APressed = false;
            }
            if (Keyboard.GetState().IsKeyUp(Keys.D)) {
                DPressed = false;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Space) && spaceReleased) {
                spacePressed = true; ;
                spaceReleased = false;
            }
            if (Keyboard.GetState().IsKeyUp(Keys.Space)) {
                spacePressed = false;
                spaceReleased = true;
            }
            var mState = Mouse.GetState();

            if (mState.LeftButton == ButtonState.Pressed && attackReleased) {
                attackPressed = true;
                attackReleased = false;
            }
            if (mState.LeftButton == ButtonState.Released) {
                attackPressed = false;
                attackReleased = true;
            }
        }
        public void ControllerInput(GamePadCapabilities capabilities) {
            GamePadState state = GamePad.GetState(PlayerIndex.One);

            // You can check explicitly if a gamepad has support 
            if (capabilities.HasLeftXThumbStick) {
                APressed = state.ThumbSticks.Left.X < -0.5f;
                DPressed = state.ThumbSticks.Left.X > 0.5f;

                if (state.IsButtonDown(Buttons.A) && spaceReleased) {
                    spacePressed = true;
                    spaceReleased = false;
                }
                if (state.IsButtonUp(Buttons.A)) {
                    spacePressed = false;
                    spaceReleased = true;
                }
                if (state.IsButtonDown(Buttons.X) && attackReleased) {
                    attackPressed = true;
                    attackReleased = false;
                }
                if (state.IsButtonUp(Buttons.X)) {
                    attackPressed = false;
                    attackReleased = true;
                }
            }

        }
    }
}
