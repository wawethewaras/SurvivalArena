using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurvivalArena {
    public class AIComponent : IComponent {
        PhysicsComponent physicsComponent;

        public AIComponent(PhysicsComponent physicsComponent) {
            this.physicsComponent = physicsComponent;
        }

        public void Update(float gameTime) {
            physicsComponent.velocity.X += 100;
        }
    }
}
