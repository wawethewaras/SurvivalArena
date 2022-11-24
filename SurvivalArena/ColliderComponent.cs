﻿using Microsoft.Xna.Framework;

namespace SurvivalArena {
    public class ColliderComponent {
  
        public static List<ColliderComponent> ColliderComponents = new List<ColliderComponent>();
        IHasPosition positionComponent;
        public int height;
        public int width;
        public bool collidedWithWall;
        public event Action<ColliderComponent> CollisionEvents;
        public string tag;
        public bool LookingRight = true;

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
                var colliderComponent = ColliderComponents[i];
                if (velocity.X > 0 && IsTouchingLeft(colliderComponent, velocity)) {
                    CollisionEvents?.Invoke(colliderComponent);
                }
                else if (velocity.X < 0 && IsTouchingRight(colliderComponent, velocity)) {
                    CollisionEvents?.Invoke(colliderComponent);
                }
                else if (velocity.Y > 0 && IsTouchingTop(colliderComponent, velocity)) {
                    CollisionEvents?.Invoke(colliderComponent);
                }
                else if (velocity.Y < 0 && IsTouchingBottom(colliderComponent, velocity)) {
                    CollisionEvents?.Invoke(colliderComponent);
                }
            }
            var xStart = (int)Math.Floor((positionComponent.Position.X) / 16);
            var YStart = (int)Math.Floor((positionComponent.Position.Y)/ 16);
            var xEnd = (int)Math.Round((positionComponent.Position.X + velocity.X + width) / 16);
            var YEnd = (int)Math.Round((positionComponent.Position.Y + velocity.Y + height) / 16);

            for (int x = xStart; x <= xEnd; x++) {
                for (int y = YStart; y <= YEnd; y++) {
                    if (x >= Level.tiles.GetLength(0) || x < 0) {
                        velocity.X = 0;
                    }
                    if (y >= Level.tiles.GetLength(1) || y < 0) {
                        velocity.Y = 0;
                    }

                    if (x >= Level.tiles.GetLength(0) || x < 0|| y >= Level.tiles.GetLength(1) || y < 0) {
                        continue;
                    }
                    var tile = Level.tiles[x, y];
                    if (tile.colliderComponent == null) {
                        continue;
                    }
                    if (velocity.X > 0 && IsTouchingLeft(tile.colliderComponent, velocity)) {
                        velocity.X = 0;
                        collidedWithWall = true;
                    }
                    if (velocity.X < 0 && IsTouchingRight(tile.colliderComponent, velocity)) {
                        velocity.X = 0;
                        collidedWithWall = true;
                    }
                    if (velocity.Y > 0 && IsTouchingTop(tile.colliderComponent, velocity)) {
                        velocity.Y = 0;
                    }
                    if (velocity.Y < 0 && IsTouchingBottom(tile.colliderComponent, velocity)) {
                        velocity.Y = 0;
                    }
                    
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
}