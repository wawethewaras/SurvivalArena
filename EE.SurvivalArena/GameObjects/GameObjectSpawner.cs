using EE.SurvivalArena;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SurvivalArena.GameObjects {
    public class GameObjectSpawner : IUpdater {

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
            invurnableDurationTimer -= gameTime;
            if (invurnableDurationTimer <= 0) {
                UnitCreatorManager.SpawnShootingEnemy(contentManager, spawnPosition);
                Random random = new Random();
                invurnableDurationTimer = random.Next(invurnableDurationMin, invurnableDurationMax);
            }
        }



        public void Draw(SpriteBatch spriteBatch) {
        }
    }
}
