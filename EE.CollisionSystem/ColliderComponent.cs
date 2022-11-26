using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SurvivalArena.GameObjects;

namespace SurvivalArena.ColliderSystem {
    public class ColliderComponent : IHasFacingDirection {

        public static Texture2D? rectangeTexture = null;


        public static List<ColliderComponent> ColliderComponents = new List<ColliderComponent>();
        IHasPosition positionComponent;
        public int height;
        public int width;
        public bool collidedWithWall;
        public event Action<string> CollisionEvents;
        public string tag;
        public bool LookingRight { get; set; } = true;
        public string tagThatStopsMovement = "Wall";
        private bool isActive = true;

        public Rectangle Rectangle {
            get {
                return new Rectangle((int)positionComponent.Position.X, (int)positionComponent.Position.Y, width, height);
            }
        }

        public ColliderComponent(IHasPosition positionComponent, int width, int height) {
            this.positionComponent = positionComponent;
            this.width = width;
            this.height = height;

            ColliderComponents.Add(this);
        }

        public Vector2 CheckCollision(Vector2 velocity) {

            for (int i = ColliderComponents.Count - 1; i >= 0; i--) {
                var collidedWithSomething = false;
                var colliderComponent = ColliderComponents[i];
                if (!colliderComponent.isActive) {
                    continue;
                }
                if (velocity.X > 0 && IsTouchingLeft(colliderComponent, velocity)) {
                    velocity.X = colliderComponent.tag == tagThatStopsMovement ? 0 : velocity.X;
                    collidedWithWall = colliderComponent.tag == tagThatStopsMovement ? true : collidedWithWall;
                    collidedWithSomething = true;
                }
                else if (velocity.X < 0 && IsTouchingRight(colliderComponent, velocity)) {
                    velocity.X = colliderComponent.tag == tagThatStopsMovement ? 0 : velocity.X;
                    collidedWithWall = colliderComponent.tag == tagThatStopsMovement ? true : collidedWithWall;
                    collidedWithSomething = true;
                }
                else if (velocity.Y > 0 && IsTouchingTop(colliderComponent, velocity)) {
                    velocity.Y = colliderComponent.tag == tagThatStopsMovement ? 0 : velocity.Y;
                    collidedWithSomething = true;
                }
                else if (velocity.Y < 0 && IsTouchingBottom(colliderComponent, velocity)) {
                    velocity.Y = colliderComponent.tag == tagThatStopsMovement ? 0 : velocity.Y;
                    collidedWithSomething = true;
                }
                if (collidedWithSomething) {
                    CollisionEvents?.Invoke(colliderComponent.tag);
                }
            }
            #region Dont check everything
            //var xStart = (int)Math.Floor(positionComponent.Position.X / 16);
            //var YStart = (int)Math.Floor(positionComponent.Position.Y / 16);
            //var xEnd = (int)Math.Round((positionComponent.Position.X + velocity.X + width) / 16);
            //var YEnd = (int)Math.Round((positionComponent.Position.Y + velocity.Y + height) / 16);

            //for (int x = xStart; x <= xEnd; x++) {
            //    for (int y = YStart; y <= YEnd; y++) {
            //        if (x >= Level.tiles.GetLength(0) || x < 0) {
            //            velocity.X = 0;
            //        }
            //        if (y >= Level.tiles.GetLength(1) || y < 0) {
            //            velocity.Y = 0;
            //        }

            //        if (x >= Level.tiles.GetLength(0) || x < 0 || y >= Level.tiles.GetLength(1) || y < 0) {
            //            continue;
            //        }
            //        var tile = Level.tiles[x, y];
            //        if (tile.colliderComponent == null) {
            //            continue;
            //        }
            //        if (velocity.X > 0 && IsTouchingLeft(tile.colliderComponent, velocity)) {
            //            velocity.X = 0;
            //            collidedWithWall = true;
            //        }
            //        if (velocity.X < 0 && IsTouchingRight(tile.colliderComponent, velocity)) {
            //            velocity.X = 0;
            //            collidedWithWall = true;
            //        }
            //        if (velocity.Y > 0 && IsTouchingTop(tile.colliderComponent, velocity)) {
            //            velocity.Y = 0;
            //        }
            //        if (velocity.Y < 0 && IsTouchingBottom(tile.colliderComponent, velocity)) {
            //            velocity.Y = 0;
            //        }

            //    }
            //}
            #endregion
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
        public void RemoveCollider() {
            ColliderComponents.Remove(this);
        }

        public void SetActive() {
            isActive = true;
        }
        public void DeActive() {
            isActive = false;
        }
    }
    public class HasPositionWithOfSet : IHasPosition {
        public IHasPosition hasPosition;
        public IHasFacingDirection hasFacingDirection;

        public Vector2 OffSet;

        public HasPositionWithOfSet(IHasPosition hasPosition, IHasFacingDirection hasFacingDirection, Vector2 offSet) {
            this.hasPosition = hasPosition;
            this.hasFacingDirection = hasFacingDirection;
            OffSet = offSet;
        }

        public Vector2 Position { get => hasPosition.Position + (hasFacingDirection.LookingRight ? OffSet : -OffSet); set => hasPosition.Position = value; }
    }

    public interface IHasFacingDirection {
        public bool LookingRight { get; set; }
    }
}