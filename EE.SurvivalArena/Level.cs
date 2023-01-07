
using EE.FileManagement;
using EE.InputSystem;
using EE.PoolingSystem;
using EE.SpriteRendererSystem;
using EE.SurvivalArena;
using EE.SurvivalArena.Units;
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
            var background = contentManager.Load<Texture2D>("Background");
            var bg = new BGDrawer(background);
            bg.drawOrder = -12;
            var treeBackground = contentManager.Load<Texture2D>("Tree_BG");
            var bg2 = new BGDrawer(treeBackground);
            bg2.drawOrder = -11;

            var filepath = string.Format("Content/level.txt"); ;

            FileReader.ReadFile(filepath, (string line, int currentLine) => 
                HandleLevelFile(contentManager, currentLine, line));
        }

        private void HandleLevelFile(ContentManager contentManager, int currentLine, string line) {
            for (int i = 0; i < line.Length; i++) {
                var tile = new Tile();
                var position = new Vector2(i * tileSize, currentLine * tileSize);

                if (line[i] == 'P') {
                    var player = new Player(contentManager, position);
                    player.healthComponent.DeathEvent += SurvivalArenaGame.GameOver;
                }
                else if (line[i] == 'E') {
                    var spawner = new GameObjectSpawner(contentManager, position);
                    PoolManager.gameObjects.Add(spawner);
                }
                else if (line[i] == 'R') {
                    UnitCreatorManager.CreateRockSpawner(contentManager, position);
                }
                else if (line[i] == '#') {
                    UnitCreatorManager.CreateTile(contentManager, "Tile", position);
                }
                else if (line[i] == '_') {
                    UnitCreatorManager.CreateTile(contentManager, "Tile_Grass", position);
                }
            }
        }

        public void Update(float gameTime) {
            for (int i = PoolManager.gameObjects.Count - 1; i >= 0; i--) {
                PoolManager.gameObjects[i].Update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch) {
            DrawManager.DrawAll(spriteBatch);
        }
    }

}
