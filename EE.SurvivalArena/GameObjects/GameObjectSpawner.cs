using EE.SurvivalArena;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SurvivalArena.GameObjects {
    public class GameObjectSpawner : IUpdater {
        public int maxWaves = 2;
        public static int currentWaves = 0;
        public static bool bossSpawned = false;


        public ContentManager contentManager;
        public int invurnableDurationMax = 6;
        public int invurnableDurationMin = 3;
        protected Vector2 spawnPosition;

        public float invurnableDurationTimer = 0;
        //List<int> numbers = new List<int>() { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 3, 3, 3 };
        List<int> numbers = new List<int>() { 1, 1 };

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
                    invurnableDurationTimer = random.Next(invurnableDurationMin, invurnableDurationMax);
                    currentList.Clear();
                    foreach (var item in numbers) {
                        currentList.Add(item);
                    }
                    currentWaves = 0;
                    return;
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
            }
        }

    }
}
