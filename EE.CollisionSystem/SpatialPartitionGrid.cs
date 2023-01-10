
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

        public void Add(ColliderComponent colliderComponent) {
            var xPosition = (int)(colliderComponent.Position.X / xNodeSize);
            var yPosition = (int)(colliderComponent.Position.Y / yNodeSize);

            if (gridNodes[xPosition, yPosition] == null) {
                gridNodes[xPosition, yPosition] = colliderComponent;
            }
            else {
                var nextCollider = gridNodes[xPosition, yPosition];
                while (nextCollider.nextCollider != null) {
                    nextCollider = nextCollider.nextCollider;
                }
                nextCollider.nextCollider = colliderComponent;
                colliderComponent.previousCollider = nextCollider.nextCollider;
            }
        }
    }
}
