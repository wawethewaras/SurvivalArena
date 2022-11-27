using EE.PoolingSystem;
using EE.SurvivalArena;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SurvivalArena.AISystem;
using SurvivalArena.ColliderSystem;
using SurvivalArena.HealthSystem;
using SurvivalArena.Physics;
using SurvivalArena.TileSystem;

namespace SurvivalArena.GameObjects {
    public class GameObjectSpawner : IUpdater {

        public Texture2D texture2D;
        public int invurnableDurationMax = 8;
        public int invurnableDurationMin = 4;
        protected Vector2 spawnPosition;

        public float invurnableDurationTimer = 0;
        public GameObjectSpawner(Texture2D texture2D, Vector2 spawnPosition) {
            this.texture2D = texture2D;
            this.spawnPosition = spawnPosition;
        }

        public void Update(float gameTime) {
            invurnableDurationTimer -= gameTime;
            if (invurnableDurationTimer <= 0) {
                UnitCreatorManager.SpawnADEnemy(texture2D, spawnPosition);
                Random random = new Random();
                invurnableDurationTimer = random.Next(invurnableDurationMin, invurnableDurationMax);
            }
        }



        public void Draw(SpriteBatch spriteBatch) {
        }
    }
}
