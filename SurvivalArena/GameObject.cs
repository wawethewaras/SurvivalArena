using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SurvivalArena {
    public class GameObject : IHasPosition {
        protected Texture2D _texture;
        public Vector2 position;
        public ColliderComponent colliderComponent;

        private List<IComponent> components = new List<IComponent>();

        public Vector2 Position { get => position; set => position = value; }
        public GameObject(Texture2D texture, Vector2 position) {
            _texture = texture;
            this.position = position;

        }
        public virtual void Update(float gameTime) {
            foreach (var component in components) {
                component.Update(gameTime);
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(_texture, position, Color.White);
        }

        public void AddComponent(IComponent component) {
            components.Add(component);
        }
    }

    public interface IComponent {
        void Update(float gameTime);
    }
    public class PhysicsComponent : IComponent {
        private float gravity = 20;
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
    public class ColliderComponent {
  
        IHasPosition positionComponent;
        public int height;
        public int width;
        public Level level;

        public Rectangle Rectangle {
            get {
                return new Rectangle((int)positionComponent.Position.X, (int)positionComponent.Position.Y, width, height);
            }
        }

        public ColliderComponent(Level level, IHasPosition positionComponent, int width, int height) {
            this.positionComponent = positionComponent;
            this.width = width;
            this.height = height;
            this.level = level;
        }

        public Vector2 CheckCollision(Vector2 velocity) {
            foreach (var gameObject in level.gameObjects) {
                if (velocity.X > 0 && IsTouchingLeft(gameObject.colliderComponent, velocity)) {
                    velocity.X = 0;
                }
                if (velocity.X < 0 && IsTouchingRight(gameObject.colliderComponent, velocity)) {
                    velocity.X = 0;
                }
                if (velocity.Y < 0 && IsTouchingTop(gameObject.colliderComponent, velocity)) {
                    velocity.Y = 0;
                }
                if (velocity.Y < 0 && IsTouchingBottom(gameObject.colliderComponent, velocity)) {
                    velocity.Y = 0;
                }
            }
            foreach (var gameObject in level.tiles) {
                if (gameObject.colliderComponent == null) {
                    continue;
                }
                if (velocity.X > 0 && IsTouchingLeft(gameObject.colliderComponent, velocity)) {
                    velocity.X = 0;
                }
                if (velocity.X < 0 && IsTouchingRight(gameObject.colliderComponent, velocity)) {
                    velocity.X = 0;
                }
                if (velocity.Y > 0 && IsTouchingTop(gameObject.colliderComponent, velocity)) {
                    velocity.Y = 0;
                }
                if (velocity.Y < 0 && IsTouchingBottom(gameObject.colliderComponent, velocity)) {
                    velocity.Y = 0;
                }
            }
            return velocity;
        }
        protected bool IsTouchingLeft(ColliderComponent sprite, Vector2 velocity) {
            return Rectangle.Right + velocity.X > sprite.Rectangle.Left &&
              Rectangle.Left < sprite.Rectangle.Left &&
              Rectangle.Bottom > sprite.Rectangle.Top &&
              Rectangle.Top < sprite.Rectangle.Bottom;
        }

        protected bool IsTouchingRight(ColliderComponent sprite, Vector2 velocity) {
            return Rectangle.Left + velocity.X < sprite.Rectangle.Right &&
              Rectangle.Right > sprite.Rectangle.Right &&
              Rectangle.Bottom > sprite.Rectangle.Top &&
              Rectangle.Top < sprite.Rectangle.Bottom;
        }

        protected bool IsTouchingTop(ColliderComponent sprite, Vector2 velocity) {
            return Rectangle.Bottom + velocity.Y > sprite.Rectangle.Top &&
              Rectangle.Top < sprite.Rectangle.Top &&
              Rectangle.Right > sprite.Rectangle.Left &&
              Rectangle.Left < sprite.Rectangle.Right;
        }

        protected bool IsTouchingBottom(ColliderComponent sprite, Vector2 velocity) {
            return Rectangle.Top + velocity.Y < sprite.Rectangle.Bottom &&
              Rectangle.Bottom > sprite.Rectangle.Bottom &&
              Rectangle.Right > sprite.Rectangle.Left &&
              Rectangle.Left < sprite.Rectangle.Right;
        }

    }
    public interface IHasPosition {
        public Vector2 Position { get; set; }

    }
}