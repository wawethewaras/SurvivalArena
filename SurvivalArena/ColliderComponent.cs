﻿using Microsoft.Xna.Framework;

namespace SurvivalArena {
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
}