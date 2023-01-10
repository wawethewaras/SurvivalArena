
using Microsoft.Xna.Framework;

namespace EE.CollisionSystem {

    public class ColliderEngine {
        public static ColliderEngine TheColliderEngine;
        public const int xNodeSize = 16;
        public const int yNodeSize = 16;
        public int xSize;
        public int ySize;

        public ColliderComponent[,] gridNodes;

        public ColliderEngine(int levelSizeX, int levelSizeY) {
            this.xSize = levelSizeX/ xNodeSize;
            this.ySize = levelSizeY/ yNodeSize;

            gridNodes = new ColliderComponent[xSize, ySize];
        }

        public void ResetColliders() {
            gridNodes = new ColliderComponent[xSize, ySize];
        }

        public void Add(ColliderComponent colliderComponent) {
            var xPosition = (int)(colliderComponent.Position.X / xNodeSize);
            var yPosition = (int)(colliderComponent.Position.Y / yNodeSize);
            colliderComponent.currentCell = new Vector2(xPosition, yPosition);
            if (gridNodes[xPosition, yPosition] == null) {
                gridNodes[xPosition, yPosition] = colliderComponent;
            }
            else {
                var nextCollider = gridNodes[xPosition, yPosition];
                while (nextCollider.NextCollider != null) {
                    nextCollider = nextCollider.NextCollider;
                }
                nextCollider.NextCollider = colliderComponent;
                colliderComponent.PreviousCollider = nextCollider;
            }
        }

        public void RemoveNode(ColliderComponent colliderComponent) {
            if (gridNodes[(int)colliderComponent.currentCell.X, (int)colliderComponent.currentCell.Y] == colliderComponent) {
                gridNodes[(int)colliderComponent.currentCell.X, (int)colliderComponent.currentCell.Y] = colliderComponent.NextCollider;
            }

            if (colliderComponent.PreviousCollider != null && colliderComponent.NextCollider != null) {
                colliderComponent.PreviousCollider.NextCollider = colliderComponent.NextCollider;
                colliderComponent.NextCollider.PreviousCollider = colliderComponent.PreviousCollider;
            }
            else if (colliderComponent.PreviousCollider != null) {
                colliderComponent.PreviousCollider.NextCollider = null;
            }
            else if(colliderComponent.NextCollider != null) {
                colliderComponent.NextCollider.PreviousCollider = null;
            }

            colliderComponent.PreviousCollider = null;
            colliderComponent.NextCollider = null;
        }
    }
}
