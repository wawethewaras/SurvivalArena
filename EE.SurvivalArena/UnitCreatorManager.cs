using EE.PoolingSystem;
using EE.SpriteRendererSystem;
using EE.StateSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using EE.CollisionSystem;
using SurvivalArena.GameObjects;
using SurvivalArena.Physics;
using SurvivalArena.TileSystem;

namespace EE.SurvivalArena {
    public static class UnitCreatorManager {
        public static void CreateEnemySpawner(ContentManager contentManager, Vector2 position) {
            var spawner = new GameObjectSpawner(contentManager, position);

            PoolManager.gameObjects.Add(spawner);
        }
        public static void CreateRockSpawner(ContentManager contentManager, Vector2 position) {
            var spawner = new RockSpawner(contentManager, position);

            PoolManager.gameObjects.Add(spawner);
        }

        public static void CreateTile(ContentManager contentManager, string tileName,Vector2 position) {
            var tileAnimation = new SpriteAnimation(contentManager, tileName, 16, 16);

            var tile = new Tile();
            tile.position = position;
            var colliderComponent = new ColliderComponent(tile, 16, 16);
            colliderComponent.tag = "Wall";

            var spriteRendererComponent = new SpriteRendererComponent(tileAnimation, tile);

        }

        public static void SpawnFallingRock(ContentManager contentManager, Vector2 position) {
            var bombAnimation = new SpriteAnimation(contentManager, "Rock", 32, 32);

            GameObject spawner2 = new GameObject(position);
            var collider = new ColliderComponent(spawner2, 32, 32);
            collider.tag = "Enemy";
            var physicsComponent = new PhysicsComponent(spawner2, collider);
            var state = new State();
            var stateComponent = new StateComponent();
            stateComponent.TransitionToState(state);
            var poolableComponent = new PoolableComponent(spawner2);
            var spriteRendererComponent = new SpriteRendererComponent(bombAnimation, spawner2, collider);
            spawner2.AddComponent(physicsComponent);
            spawner2.AddComponent(stateComponent);
            spawner2.AddComponent(spriteRendererComponent);

            collider.CollisionEvents += (ColliderComponent x) => {
                if (x.tag == "Wall" || x.tag == "Player" || x.tag == "Sword") {
                    collider.RemoveCollider();
                    poolableComponent.ReleaseSelf();
                    spriteRendererComponent.OnDestroy();
                }
            };

            PoolManager.gameObjects.Add(spawner2);
        }


    }
}
