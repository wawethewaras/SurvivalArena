using EE.SurvivalArena;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SurvivalArena.GameObjects {
    public class GameObjectSpawner : IUpdater {
        public static int WaveCounter = 0;

        public int maxWaves = 20;
        public static int currentWaves = 0;


        public ContentManager contentManager;
        public int invurnableDurationMax = 6;
        public int invurnableDurationMin = 3;
        public int delayAfterBoss = 9;

        protected Vector2 spawnPosition;

        public float invurnableDurationTimer = 0;
        List<int> numbers = new List<int>() { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 3, 3, 3 };

        List<int> currentList = new List<int>();

        public GameObjectSpawner(ContentManager contentManager, Vector2 spawnPosition) {
            this.contentManager = contentManager;
            this.spawnPosition = spawnPosition;
            foreach (var item in numbers) {
                currentList.Add(item);
            }
        }

        public void Update(float gameTime) {
            invurnableDurationTimer -= gameTime;
            if (invurnableDurationTimer <= 0) {
                Random random = new Random();
                if (currentWaves >= maxWaves) {
                    UnitCreatorManager.SpawnSlugrinEnemy(contentManager, spawnPosition);
                    invurnableDurationTimer = delayAfterBoss;
                    currentWaves = 0;
                    return;
                }
                if (currentList.Count <= 0) {
                    currentList.Clear();
                    foreach (var item in numbers) {
                        currentList.Add(item);
                    }
                }
                if (currentWaves == 3) {
                    UnitCreatorManager.SpawnFallingRock(contentManager, new Vector2(spawnPosition.X, 0));
                }

                var randomIndex = random.Next(0, currentList.Count);
                var randomNumber = currentList[randomIndex];
                if (randomNumber == 3) {
                    UnitCreatorManager.SpawnSlugHound(contentManager, spawnPosition);
                }
                else if (randomNumber == 2) {
                    UnitCreatorManager.SpawnShootingEnemy(contentManager, spawnPosition);
                }
                else if (randomNumber == 1) {
                    UnitCreatorManager.SpawnADEnemy(contentManager, spawnPosition);
                }

                currentList.RemoveAt(randomIndex);
                invurnableDurationTimer = random.Next(invurnableDurationMin, invurnableDurationMax);
                currentWaves++;
                WaveCounter++;
            }
        }

    }
}
