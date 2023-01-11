
using Microsoft.Xna.Framework;

namespace EE.CollisionSystem {

    public class ColliderEngine {
        public static ColliderEngine? TheColliderEngine;
        public const int xNodeSize = 16;
        public const int yNodeSize = 16;
        public int xSize;
        public int ySize;

        public IColliderComponent[,] gridNodes;

        public ColliderEngine(int levelSizeX, int levelSizeY) {
            this.xSize = (int)Math.Ceiling((double)levelSizeX / xNodeSize);
            this.ySize = (int)Math.Ceiling((double)levelSizeY/ yNodeSize);

            gridNodes = new IColliderComponent[xSize, ySize];

            if (TheColliderEngine != null) {
                throw new Exception("Trying to create multiple Collider Engines! Currently there should be only one.");
            }
            if (TheColliderEngine == null) {
                TheColliderEngine = this;
            }

        }

        public void ResetColliders() {
            gridNodes = new IColliderComponent[xSize, ySize];
        }

        public void Add(IColliderComponent colliderComponent) {
            var xPosition = (int)(colliderComponent.Position.X / xNodeSize);
            var yPosition = (int)(colliderComponent.Position.Y / yNodeSize);
            colliderComponent.CurrentCell = new Vector2(xPosition, yPosition);
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
