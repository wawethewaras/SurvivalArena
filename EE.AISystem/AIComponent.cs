﻿using SurvivalArena.GameObjects;
using SurvivalArena.Physics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurvivalArena.AISystem {
    public class AIComponent : IComponent {
        PhysicsComponent physicsComponent;
        public bool lookingAtRight;

        public AIComponent(PhysicsComponent physicsComponent) {
            this.physicsComponent = physicsComponent;
            lookingAtRight = physicsComponent.colliderComponent.LookingRight;

        }

        public void Update(float gameTime) {
            if (physicsComponent.colliderComponent.collidedWithWall) {
                lookingAtRight = !lookingAtRight;
                physicsComponent.colliderComponent.collidedWithWall = false;
            }
            int direction = lookingAtRight ? 1 : -1;

            physicsComponent.velocity.X += physicsComponent.moveSpeed * direction;

        }
    }
}
