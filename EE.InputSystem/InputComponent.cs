using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SurvivalArena.GameObjects;
using System.Diagnostics;

namespace EE.InputSystem {
    public class InputComponent : IComponent {
        public static float YScale;
        public static float XScale;

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

        public Vector2 rightJoystickDirection;


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

                if (state.IsButtonDown(Buttons.LeftTrigger) && spaceReleased) {
                    spacePressed = true;
                    spaceReleased = false;
                }
                if (state.IsButtonUp(Buttons.LeftTrigger)) {
                    spacePressed = false;
                    spaceReleased = true;
                }
                if (state.IsButtonDown(Buttons.RightTrigger) && attackReleased) {
                    attackPressed = true;
                    attackReleased = false;
                }
                if (state.IsButtonUp(Buttons.RightTrigger)) {
                    attackPressed = false;
                    attackReleased = true;
                }
            }

        }
        public Vector2 ShootDirection(Vector2 vector2) {
            GamePadCapabilities capabilities = GamePad.GetCapabilities(PlayerIndex.One);
            GamePadState state = GamePad.GetState(PlayerIndex.One);
            var mouseinpout = Vector2.Zero;
            if (capabilities.IsConnected) {
                mouseinpout = new Vector2(state.ThumbSticks.Right.X, -state.ThumbSticks.Right.Y);
                mouseinpout.Normalize();
            }
            return capabilities.IsConnected ? mouseinpout : MouseDirection(vector2);
        } 
        public Vector2 MouseDirection(Vector2 vector2) { 
            var mouse = Mouse.GetState();
            var mousepos = new Vector2(mouse.X/ XScale, mouse.Y / YScale) - vector2;
            mousepos.Normalize();
            return mousepos;
        } 

    }

}
