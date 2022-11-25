using Microsoft.Xna.Framework.Input;
using SurvivalArena.GameObjects;
using SurvivalArena.Physics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurvivalArena.InputSystem {
    public class InputComponent : IComponent {
        PhysicsComponent physicsComponent;
        bool mReleased = true;

        public InputComponent(PhysicsComponent physicsComponent) {
            this.physicsComponent = physicsComponent;
        }

        public void Update(float gameTime) {
            if (Keyboard.GetState().IsKeyDown(Keys.A)) {
                physicsComponent.velocity.X -= physicsComponent.moveSpeed;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.D)) {
                physicsComponent.velocity.X += physicsComponent.moveSpeed;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Space) && physicsComponent.jumpTimeCounter <= 0) {
                physicsComponent.jumpTimeCounter = physicsComponent.jumpTime;
            }


        }
    }
}
