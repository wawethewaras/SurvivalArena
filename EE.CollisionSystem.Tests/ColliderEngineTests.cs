using EE.Test.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;
using SurvivalArena.GameObjects;

namespace EE.CollisionSystem.Tests {
    [TestClass]
    public class ColliderEngineTests {
        [TestInitialize()]
        public void TestInit() {
            ColliderEngine.TheColliderEngine = null;
        }

        [TestMethod]
        public void ColliderEngine_Should_Init_Correctly() {
            var colliderEngine = new ColliderEngine(880, 587);

            colliderEngine.xSize.Should().Be(55);
            colliderEngine.ySize.Should().Be(37);
            colliderEngine.gridNodes.Should().BeNotNull();
            colliderEngine.gridNodes.GetLength(0).Should().Be(55);
            colliderEngine.gridNodes.GetLength(1).Should().Be(37);
            ColliderEngine.TheColliderEngine.Should().Be(colliderEngine);

            var positions = CheckThatAllTheCellsAreEmpty(colliderEngine.gridNodes);
            positions.Count.Should().Be(1);
        }
        [TestMethod]
        public void Creating_Two_ColliderEngines_Should_Create_Error() {
            var colliderEngine = new ColliderEngine(880, 587);

            try {
                colliderEngine = new ColliderEngine(880, 587);
                Assert.Fail();
            }
            catch { 
            
            }
        }

        [TestMethod]
        public void ResetColliders_Should_Create_New_Empty_Grid() {
            var colliderEngine = new ColliderEngine(64, 64);

            colliderEngine.gridNodes[1,1] = new ColliderComponent(new TestIHasPosition(), 0,0);
            colliderEngine.gridNodes[1, 1].Should().BeNotNull();
            var positions = CheckThatAllTheCellsAreEmpty(colliderEngine.gridNodes);
            positions.Count.Should().Be(1);

            colliderEngine.ResetColliders();

            positions = CheckThatAllTheCellsAreEmpty(colliderEngine.gridNodes);
            positions.Count.Should().Be(0);
            colliderEngine.gridNodes[1, 1].Should().BeNull();

        }

        [TestMethod]
        public void Add_Should_Add_New_Collider_To_Grid() {
            var colliderEngine = new ColliderEngine(64, 64);

            var colliderComponent = new TestColliderComponent();
            colliderComponent.Position = new Vector2(32,32);

            colliderEngine.Add(colliderComponent);

            var positions = CheckThatAllTheCellsAreEmpty(colliderEngine.gridNodes);
            positions.Count.Should().Be(1);
            colliderEngine.gridNodes[2, 2].Should().Be(colliderComponent);

            colliderComponent.CurrentCell.Should().Be(new Vector2(2,2));
            colliderComponent.PreviousCollider.Should().BeNull();
            colliderComponent.NextCollider.Should().BeNull();
        }
        [TestMethod]
        public void Adding_Multiple_Colliders_To_Different_Cells() {
            var colliderEngine = new ColliderEngine(64, 64);

            var colliderComponent1 = new TestColliderComponent();
            colliderComponent1.Position = new Vector2(32, 32);
            var colliderComponent2 = new TestColliderComponent();
            colliderComponent2.Position = new Vector2(16, 32);

            colliderEngine.Add(colliderComponent1);
            colliderEngine.Add(colliderComponent2);

            var positions = CheckThatAllTheCellsAreEmpty(colliderEngine.gridNodes);
            positions.Count.Should().Be(2);

            colliderEngine.gridNodes[2, 2].Should().Be(colliderComponent1);
            colliderEngine.gridNodes[1, 2].Should().Be(colliderComponent2);

            colliderComponent1.CurrentCell.Should().Be(new Vector2(2, 2));
            colliderComponent1.PreviousCollider.Should().BeNull();
            colliderComponent1.NextCollider.Should().BeNull();

            colliderComponent2.CurrentCell.Should().Be(new Vector2(1, 2));
            colliderComponent2.PreviousCollider.Should().BeNull();
            colliderComponent2.NextCollider.Should().BeNull();
        }

        [TestMethod]
        public void Adding_Multiple_Colliders_To_Same_Cell_Should_Link_Them() {
            var colliderEngine = new ColliderEngine(64, 64);

            var colliderComponent1 = new TestColliderComponent();
            colliderComponent1.Position = new Vector2(32, 32);
            var colliderComponent2 = new TestColliderComponent();
            colliderComponent2.Position = new Vector2(32, 32);

            colliderEngine.Add(colliderComponent1);
            colliderEngine.Add(colliderComponent2);

            var positions = CheckThatAllTheCellsAreEmpty(colliderEngine.gridNodes);
            positions.Count.Should().Be(1);

            colliderEngine.gridNodes[2, 2].Should().Be(colliderComponent1);

            colliderComponent1.CurrentCell.Should().Be(new Vector2(2, 2));
            colliderComponent1.PreviousCollider.Should().BeNull();
            colliderComponent1.NextCollider.Should().Be(colliderComponent2);

            colliderComponent2.CurrentCell.Should().Be(new Vector2(2, 2));
            colliderComponent2.PreviousCollider.Should().Be(colliderComponent1);
            colliderComponent2.NextCollider.Should().BeNull();
        }

        //Returns List of Positions that are not empty.
        public List<Vector2> CheckThatAllTheCellsAreEmpty(IColliderComponent[,] colliderComponents) {
            var positions = new List<Vector2>();
            for (int i = 0; i < colliderComponents.GetLength(0); i++) {
                for (int j = 0; j < colliderComponents.GetLength(1); j++) {
                    if (colliderComponents[i,j] != null) {
                        positions.Add(new Vector2(i, j));
                    }
                }
            }
            return positions;
        }
    }

    public class TestIHasPosition : IHasPosition {
        public Vector2 Position { get; set; }
    }
    public class TestColliderComponent : IColliderComponent {

        public Vector2 Position { get; set; }
        public IColliderComponent? NextCollider { get; set; }
        public IColliderComponent? PreviousCollider { get; set; }

        public bool IsActive { get; set; }

        public Rectangle Rectangle { get; set; }

        public string Tag { get; set; }

        public Vector2 CurrentCell { get; set; }


        public void CollisionFromOther(ColliderComponent colliderComponent) {
            throw new NotImplementedException();
        }
    }
}