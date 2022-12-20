using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SurvivalArena.ColliderSystem;
using SurvivalArena.GameObjects;

namespace SurvivalArena.Physics {
    public class PhysicsComponent : IComponent {
        public float gravity = 500;
        public float currentGravity = 200;
        public float gravityGainSpeed = 1000;

        public float moveSpeed = 200;
        public float jumpHeight = 1000;
        public float jumpTime = 0.1f;
        public float jumpTimeCounter = 0;

        public Vector2 velocity = Vector2.Zero;
        IHasPosition positionComponent;
        public ColliderComponent colliderComponent;

        private bool isGrounded = true;
        public PhysicsComponent(IHasPosition positionComponent, ColliderComponent colliderComponent) {
            this.positionComponent = positionComponent;
            this.colliderComponent = colliderComponent;
        }

        public void Update(float gameTime) {
            if (jumpTimeCounter > 0) {
                velocity.Y -= jumpHeight;
                jumpTimeCounter -= gameTime;
                currentGravity = 0;
            }
            else {
                if (currentGravity < gravity) {
                    currentGravity += gravityGainSpeed * gameTime;
                }
                else if (currentGravity > gravity) {
                    currentGravity = gravity;
                }
            }
            velocity.Y += currentGravity;

            velocity.Y *= gameTime;
            velocity.X *= gameTime;
            velocity = colliderComponent.CheckCollision(velocity);

            if (velocity.X != 0) {
                colliderComponent.LookingRight = velocity.X > 0 ? true : false;
            }
            isGrounded = velocity.Y == 0 ? true : false;

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
            if (jumpTimeCounter <= 0 && isGrounded) {
                jumpTimeCounter = jumpTime;
            }
        }

        public void ADMovement(float gameTime) {
            if (colliderComponent.collidedWithWall) {
                colliderComponent.LookingRight = !colliderComponent.LookingRight;
                colliderComponent.collidedWithWall = false;
            }
            int direction = colliderComponent.LookingRight ? 1 : -1;

            velocity.X += moveSpeed * direction;

        }
        public void ADMovement(Vector2 direction) {
            velocity = direction * moveSpeed;
        }
    }
}