using EE.SurvivalArena;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SurvivalArena.GameObjects {
    public class RockSpawner : IUpdater {

        public ContentManager contentManager;
        public int invurnableDurationMax = 8;
        public int invurnableDurationMin = 5;

        protected Vector2 spawnPosition;

        public float invurnableDurationTimer = 0;

        public RockSpawner(ContentManager contentManager, Vector2 spawnPosition) {
            this.contentManager = contentManager;
            this.spawnPosition = spawnPosition;
        }

        public void Update(float gameTime) {
            //Start falling rocks after first boss is spawned.
            if (GameObjectSpawner.WaveCounter < 23) {
                return;
            }
            invurnableDurationTimer -= gameTime;
            if (invurnableDurationTimer <= 0) {
                Random random = new Random();
                UnitCreatorManager.SpawnFallingRock(contentManager, spawnPosition);

                invurnableDurationTimer = random.Next(invurnableDurationMin, invurnableDurationMax);
            }
        }

    }
}
