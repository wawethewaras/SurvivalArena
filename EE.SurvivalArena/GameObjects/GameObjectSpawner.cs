using EE.SurvivalArena;
using EE.SurvivalArena.Units;
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
        List<int> numbers2 = new List<int>() { 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 3, 3, 3, 4,4,4,4 };

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
                    new Slugrin(contentManager, spawnPosition);
                    invurnableDurationTimer = delayAfterBoss;
                    currentWaves = 0;
                    numbers = numbers2;
                    return;
                }
                if (currentList.Count <= 0) {
                    currentList.Clear();
                    foreach (var item in numbers) {
                        currentList.Add(item);
                    }
                }

                var randomIndex = random.Next(0, currentList.Count);
                var randomNumber = currentList[randomIndex];
                if (randomNumber == 4) {
                    new Woodling(contentManager, spawnPosition);
                }
                else if(randomNumber == 3) {
                    new SlugHound(contentManager, spawnPosition);
                }
                else if (randomNumber == 2) {
                    new Plagrin(contentManager, spawnPosition);
                }
                else if (randomNumber == 1) {
                    new Slime(contentManager, spawnPosition);
                }

                currentList.RemoveAt(randomIndex);
                invurnableDurationTimer = random.Next(invurnableDurationMin, invurnableDurationMax);
                currentWaves++;
                WaveCounter++;
            }
        }

    }
}
