using EE.SurvivalArena;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SurvivalArena.GameObjects {
    public class GameObjectSpawner : IUpdater {
        public static bool BossSpawned = false;
        public int maxWaves = 2;
        public int currentWaves = 0;


        public ContentManager contentManager;
        public int invurnableDurationMax = 8;
        public int invurnableDurationMin = 4;
        protected Vector2 spawnPosition;

        public float invurnableDurationTimer = 0;
        public GameObjectSpawner(ContentManager contentManager, Vector2 spawnPosition) {
            this.contentManager = contentManager;
            this.spawnPosition = spawnPosition;
        }

        public void Update(float gameTime) {
            if (BossSpawned) {
                return;
            }
            if (currentWaves >= maxWaves && !BossSpawned) {
                UnitCreatorManager.SpawnBossEnemy(contentManager, spawnPosition);
                BossSpawned = true;
            }
            invurnableDurationTimer -= gameTime;
            if (invurnableDurationTimer <= 0) {
                Random random = new Random();
                var enemyRandom = random.Next(0, 2);
                if (enemyRandom > 0) {
                    UnitCreatorManager.SpawnADEnemy(contentManager, spawnPosition);
                }
                else {
                    UnitCreatorManager.SpawnShootingEnemy(contentManager, spawnPosition);
                }
                invurnableDurationTimer = random.Next(invurnableDurationMin, invurnableDurationMax);
                currentWaves++;
            }
        }



        public void Draw(SpriteBatch spriteBatch) {
        }
    }
}
