
using EE.InputSystem;
using EE.PoolingSystem;
using EE.SpriteRendererSystem;
using EE.SurvivalArena;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SurvivalArena.ColliderSystem;
using SurvivalArena.GameObjects;
using SurvivalArena.HealthSystem;
using SurvivalArena.Physics;
using SurvivalArena.Sword;

namespace SurvivalArena.TileSystem {
    public class Level {
        private const int tileSize = 16;
        public Level(ContentManager contentManager) {
            var filepath = string.Format("Content/SurvivalArena.csv"); ;

            using (Stream fileStream = TitleContainer.OpenStream(filepath)) {
                int currentLine = 0;
                using (StreamReader reader = new StreamReader(fileStream)) {
                    string line = reader.ReadLine();
                    var tileIds = line.Split(',');
                    while (line != null) {
                        for (int i = 0; i < tileIds.Length; i++) {
                            var tile = new Tile();
                            var position = new Vector2(i * tileSize, currentLine * tileSize);

                            if (tileIds[i] == "0") {
                                UnitCreatorManager.CreatePlayer(contentManager, position);
                            }
                            else if (tileIds[i] == "E") {
                                UnitCreatorManager.CreateEnemySpawner(contentManager, position);

                            }
                            else if (tileIds[i] != "-1") {
                                UnitCreatorManager.CreateTile(contentManager, position);
                            }
                        }
                        line = reader.ReadLine();
                        tileIds = line?.Split(',');
                        currentLine++;
                    }
                }
            }
        }



        public void Update(float gameTime) {
            for (int i = PoolManager.gameObjects.Count - 1; i >= 0; i--) {
                PoolManager.gameObjects[i].Update(gameTime);
            }
        }
        public void Draw(SpriteBatch spriteBatch) {
            for (int i = SpriteRendererComponent.spriteRendererComponents.Count - 1; i >= 0; i--) {
                SpriteRendererComponent.spriteRendererComponents[i].Draw(spriteBatch);
            }
        }
    }
}
