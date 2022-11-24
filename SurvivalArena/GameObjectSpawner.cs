using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SurvivalArena {
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
            spawner2.colliderComponent = collider;
            var physicsComponent = new PhysicsComponent(spawner2, collider);
            var aIComponent = new AIComponent(physicsComponent);
            spawner2.AddComponent(physicsComponent);
            spawner2.AddComponent(aIComponent);

            Level.gameObjects.Add(spawner2);
        }

        public void Draw(SpriteBatch spriteBatch) {
        }
    }
    public interface IUpdater {
        void Update(float gameTime);
        void Draw(SpriteBatch spriteBatch);

    }
}
