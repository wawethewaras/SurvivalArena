using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SurvivalArena.GameObjects;

namespace EE.CollisionSystem {
    public interface IColliderComponent { 
        Vector2 Position { get; }
        IColliderComponent? NextCollider { get; set; }
        IColliderComponent? PreviousCollider { get; set; }
        bool IsActive { get; }
        Rectangle Rectangle { get; }
        string Tag { get; }
        Vector2 CurrentCell { get; set; }

        void CollisionFromOther(ColliderComponent colliderComponent);
    }
    public class ColliderComponent : IHasFacingDirection, IComponent, IColliderComponent {
        public IColliderComponent? nextCollider;
        public IColliderComponent? NextCollider { 
            get {
                return nextCollider;
            }
            set {
                nextCollider = value;
                if (nextCollider == this) {
                    throw new Exception("This should never happen. Will cause an infinite loop.");
                }
            }
        }
        public IColliderComponent? previousCollider;
        public IColliderComponent? PreviousCollider {
            get {
                return previousCollider;
            }
            set {
                previousCollider = value;
                if (previousCollider == this) {
                    throw new Exception("This should never happen. Will cause an infinite loop.");
                }
            }
        }
        public Vector2 currentCell;
        public Vector2 CurrentCell { get => currentCell; set => currentCell = value; }

        public static Texture2D? rectangeTexture = null;

        IHasPosition positionComponent;
        public Vector2 Position => positionComponent.Position;
        public int height;
        public int width;
        public bool collidedWithWall;
        public event Action<IColliderComponent> CollisionEvents;
        public event Action<IColliderComponent> CollisionEventFromOther;

        public void CollisionFromOther(ColliderComponent collider) {
            CollisionEventFromOther?.Invoke(collider);
        }
        public string Tag => tag;
        public string tag;

        public bool LookingRight { get; set; } = true;
        public string tagThatStopsMovement = "Wall";
        private bool isActive = true;
        public bool IsActive => isActive;

        public Rectangle Rectangle {
            get {
                return new Rectangle((int)positionComponent.Position.X, (int)positionComponent.Position.Y, width, height);
            }
        }

        public ColliderComponent(IHasPosition positionComponent, int width, int height) {
            if (ColliderEngine.TheColliderEngine == null) {
                throw new Exception("Collider engine needs to be initilized!");
            }

            this.positionComponent = positionComponent;
            this.width = width;
            this.height = height;
            ColliderEngine.TheColliderEngine.Add(this);
        }
        public void Update(float gameTime) {
            var xPosition = (int)(Position.X / ColliderEngine.xNodeSize);
            var yPosition = (int)(Position.Y / ColliderEngine.yNodeSize);

            if (xPosition != currentCell.X || yPosition != currentCell.Y) {

                ColliderEngine.TheColliderEngine.RemoveNode(this);
                ColliderEngine.TheColliderEngine.Add(this);
            }
        }

        public Vector2 CheckCollision(Vector2 velocity) {
            //Quick hack since zero velocity doesn't currently cause collision
            if (velocity.X == 0 && velocity.Y == 0) {
                velocity = new Vector2(0.1f,0.1f);
            }

            var xStart = (int)Math.Floor(positionComponent.Position.X / 16);
            var YStart = (int)Math.Floor(positionComponent.Position.Y / 16);
            var xEnd = (int)Math.Round((positionComponent.Position.X + velocity.X + width) / 16);
            var YEnd = (int)Math.Round((positionComponent.Position.Y + velocity.Y + height) / 16);

            var gridNodes = ColliderEngine.TheColliderEngine.gridNodes;
            for (int x = xStart; x <= xEnd; x++) {
                for (int y = YStart; y <= YEnd; y++) {
                    if (x >= gridNodes.GetLength(0) || x < 0) {
                        velocity.X = 0;
                    }
                    if (y >= gridNodes.GetLength(1) || y < 0) {
                        velocity.Y = 0;
                    }

                    if (x >= gridNodes.GetLength(0) || x < 0 || y >= gridNodes.GetLength(1) || y < 0) {
                        continue;
                    }
                    var colliderComponent = gridNodes[x, y];
                    while (colliderComponent != null) {
                        var collidedWithSomething = false;
                        if (!colliderComponent.IsActive) {
                            continue;
                        }
                        if (IsTouchingLeft(colliderComponent, velocity)) {
                            velocity.X = colliderComponent.Tag == tagThatStopsMovement ? 0 : velocity.X;
                            collidedWithWall = colliderComponent.Tag == tagThatStopsMovement ? true : collidedWithWall;
                            collidedWithSomething = true;
                        }
                        else if (IsTouchingRight(colliderComponent, velocity)) {
                            velocity.X = colliderComponent.Tag == tagThatStopsMovement ? 0 : velocity.X;
                            collidedWithWall = colliderComponent.Tag == tagThatStopsMovement ? true : collidedWithWall;
                            collidedWithSomething = true;
                        }
                        else if (IsTouchingTop(colliderComponent, velocity)) {
                            velocity.Y = colliderComponent.Tag == tagThatStopsMovement ? 0 : velocity.Y;
                            collidedWithSomething = true;
                        }
                        else if (IsTouchingBottom(colliderComponent, velocity)) {
                            velocity.Y = colliderComponent.Tag == tagThatStopsMovement ? 0 : velocity.Y;
                            collidedWithSomething = true;
                        }
                        if (collidedWithSomething) {
                            CollisionEvents?.Invoke(colliderComponent);
                            colliderComponent.CollisionFromOther(this);
                        }
                        colliderComponent = colliderComponent.NextCollider;
                    }



                }
            }
            return velocity;
        }
        protected bool IsTouchingLeft(IColliderComponent sprite, Vector2 velocity) {
            return Rectangle.Right + velocity.X > sprite.Rectangle.Left &&
              Rectangle.Left < sprite.Rectangle.Left &&
              Rectangle.Bottom > sprite.Rectangle.Top &&
              Rectangle.Top < sprite.Rectangle.Bottom;
        }

        protected bool IsTouchingRight(IColliderComponent sprite, Vector2 velocity) {
            return Rectangle.Left + velocity.X < sprite.Rectangle.Right &&
              Rectangle.Right > sprite.Rectangle.Right &&
              Rectangle.Bottom > sprite.Rectangle.Top &&
              Rectangle.Top < sprite.Rectangle.Bottom;
        }

        protected bool IsTouchingTop(IColliderComponent sprite, Vector2 velocity) {
            return Rectangle.Bottom + velocity.Y > sprite.Rectangle.Top &&
              Rectangle.Top < sprite.Rectangle.Top &&
              Rectangle.Right > sprite.Rectangle.Left &&
              Rectangle.Left < sprite.Rectangle.Right;
        }

        protected bool IsTouchingBottom(IColliderComponent sprite, Vector2 velocity) {
            return Rectangle.Top + velocity.Y < sprite.Rectangle.Bottom &&
              Rectangle.Bottom > sprite.Rectangle.Bottom &&
              Rectangle.Right > sprite.Rectangle.Left &&
              Rectangle.Left < sprite.Rectangle.Right;
        }
        public void RemoveCollider() {
            ColliderEngine.TheColliderEngine.RemoveNode(this);
        }

        public void SetActive() {
            isActive = true;
        }
        public void DeActive() {
            isActive = false;
        }


    }



}