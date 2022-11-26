using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SurvivalArena.ColliderSystem;
using SurvivalArena.GameObjects;

namespace SurvivalArena.Physics {
    public class PhysicsComponent : IComponent {
        public float gravity = 200;
        public float moveSpeed = 200;
        public float jumpHeight = 300;
        public float jumpTime = 0.1f;
        public float jumpTimeCounter = 0;

        public Vector2 velocity = Vector2.Zero;
        IHasPosition positionComponent;
        public ColliderComponent colliderComponent;
        public PhysicsComponent(IHasPosition positionComponent, ColliderComponent colliderComponent) {
            this.positionComponent = positionComponent;
            this.colliderComponent = colliderComponent;
        }

        public void Update(float gameTime) {
            if (jumpTimeCounter <= 0) {
                velocity.Y += gravity;
            }
            else {
                velocity.Y -= jumpHeight;
                jumpTimeCounter -= gameTime;
            }

            velocity.Y *= gameTime;
            velocity.X *= gameTime;
            velocity = colliderComponent.CheckCollision(velocity);

            if (velocity.X != 0) {
                colliderComponent.LookingRight = velocity.X > 0 ? true : false;
            }

            positionComponent.Position += velocity;
            velocity = Vector2.Zero;
        }

        public void SetMovementSpeedNegative() {
            velocity.X -= moveSpeed;
        }
        public void SetMovementSpeedPositive() {
            velocity.X += moveSpeed;
        }
        public void Jump() {
            if (jumpTimeCounter <= 0) {
                jumpTimeCounter = jumpTime;
            }
        }
    }
}