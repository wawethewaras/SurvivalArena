using Microsoft.Xna.Framework;
using SurvivalArena.GameObjects;

namespace SurvivalArena.ColliderSystem {
    public class ColliderComponent {

        public static List<ColliderComponent> ColliderComponents = new List<ColliderComponent>();
        IHasPosition positionComponent;
        public int height;
        public int width;
        public bool collidedWithWall;
        public event Action<string> CollisionEvents;
        public string tag;
        public bool LookingRight = true;
        public string tagThatStopsMovement = "Wall";

        public Vector2 OffSet;

        public Rectangle Rectangle {
            get {
                return new Rectangle((int)positionComponent.Position.X + (int)OffSet.X, (int)positionComponent.Position.Y + (int)OffSet.Y, width, height);
            }
        }

        public ColliderComponent(IHasPosition positionComponent, int width, int height, Vector2 vector2 = new Vector2()) {
            this.positionComponent = positionComponent;
            this.width = width;
            this.height = height;
            this.OffSet = vector2;

            ColliderComponents.Add(this);
        }

        public Vector2 CheckCollision(Vector2 velocity) {

            for (int i = ColliderComponents.Count - 1; i >= 0; i--) {
                var collidedWithSomething = false;
                var colliderComponent = ColliderComponents[i];
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
        ColliderComponent sword;
        public void SpawnSword() {
            var direction = LookingRight ? width : -width;
            var offSet = new Vector2(direction,0);
            sword = new ColliderComponent(positionComponent, width, height, offSet);
            sword.tag = "Sword";
        }
        public void RemoveSword() {
            if (sword == null) {
                return;
            }
            ColliderComponents.Remove(sword);
            sword = null;

        }
    }
}