using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SurvivalArena {
    public class PhysicsComponent : IComponent {
        private float gravity = 200;
        private float moveSpeed = 200;
        private float jumpHeight = 300;
        private float jumpTime = 0.1f;
        private float jumpTimeCounter = 0;

        Vector2 velocity = Vector2.Zero;
        IHasPosition positionComponent;
        ColliderComponent colliderComponent;
        public PhysicsComponent(IHasPosition positionComponent, ColliderComponent colliderComponent) {
            this.positionComponent = positionComponent;
            this.colliderComponent = colliderComponent;
        }

        public void Update(float gameTime) {
            if (Keyboard.GetState().IsKeyDown(Keys.A)) {
                velocity.X -= moveSpeed;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.D)) {
                velocity.X += moveSpeed;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Space) && jumpTimeCounter <= 0) {
                jumpTimeCounter = jumpTime;
            }

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


            positionComponent.Position += velocity;
            velocity = Vector2.Zero;
        }
    }
}