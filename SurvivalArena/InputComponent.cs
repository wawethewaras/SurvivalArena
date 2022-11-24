using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurvivalArena {
    public class InputComponent :IComponent{
        PhysicsComponent physicsComponent;

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
