using EE.PoolingSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SurvivalArena.AISystem;
using SurvivalArena.ColliderSystem;
using SurvivalArena.HealthSystem;
using SurvivalArena.Physics;
using SurvivalArena.TileSystem;

namespace SurvivalArena.GameObjects {
    public class GameObjectSpawner : IUpdater {

        public GameObject spawner;
        public int invurnableDurationMax = 8;
        public int invurnableDurationMin = 4;
        protected Vector2 spawnPosition;

        public float invurnableDurationTimer = 0;
        public GameObjectSpawner(GameObject spawner, Vector2 spawnPosition) {
            this.spawner = spawner;
            this.spawnPosition = spawnPosition;
        }

        public void Update(float gameTime) {
            invurnableDurationTimer -= gameTime;
            if (invurnableDurationTimer <= 0) {
                SpawnEnemy();
                Random random = new Random();
                invurnableDurationTimer = random.Next(invurnableDurationMin, invurnableDurationMax);
            }
        }

        public void SpawnEnemy() {
            GameObject spawner2 = new GameObject(spawner._texture, spawnPosition);
            var collider = new ColliderComponent(spawner2, spawner._texture.Width, spawner._texture.Height);
            collider.tag = "Enemy";
            //spawner2.colliderComponent = collider;
            var physicsComponent = new PhysicsComponent(spawner2, collider);
            var aIComponent = new AIComponent(physicsComponent);
            var health = new HealthComponent(1, spawner2);
            var poolableComponent = new PoolableComponent(spawner2);
            health.hurtTag = "Sword";
            spawner2.AddComponent(physicsComponent);
            spawner2.AddComponent(aIComponent);
            spawner2.AddComponent(health);

            collider.CollisionEvents += health.DealDamage;
            health.DeathEvent += collider.RemoveCollider;
            health.DeathEvent += poolableComponent.ReleaseSelf;

            PoolManager.gameObjects.Add(spawner2);
        }

        public void Draw(SpriteBatch spriteBatch) {
        }
    }
}
