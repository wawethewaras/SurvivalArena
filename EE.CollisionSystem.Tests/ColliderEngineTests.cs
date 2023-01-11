using EE.Test.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;
using SurvivalArena.GameObjects;

namespace EE.CollisionSystem.Tests {
    [TestClass]
    public class ColliderEngineTests {
        [TestMethod]
        public void Grid_Should_Init_Correctly() {
            var colliderEngine = new ColliderEngine(880, 587);

            colliderEngine.xSize.Should().Be(55);
            colliderEngine.ySize.Should().Be(37);
            colliderEngine.gridNodes.Should().BeNotNull();
            colliderEngine.gridNodes.GetLength(0).Should().Be(55);
            colliderEngine.gridNodes.GetLength(1).Should().Be(37);
        }

        [TestMethod]
        public void ResetColliders_Should_Create_New_Empty_Grid() {
            var colliderEngine = new ColliderEngine(64, 64);

            colliderEngine.gridNodes[1,1] = new ColliderComponent(new TestIHasPosition(), 0,0);
            colliderEngine.gridNodes[1, 1].Should().BeNotNull();

            colliderEngine.ResetColliders();

            colliderEngine.gridNodes[1, 1].Should().BeNull();

        }
    }

    public class TestIHasPosition : IHasPosition {
        public Vector2 Position { get; set; }
    }

}