
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
        public static IHasPosition Player;
        private const int tileSize = 16;

        Texture2D background;
        Texture2D treeBackground;

        public Level(ContentManager contentManager) {
            background = contentManager.Load<Texture2D>("Background");
            var bg = new BGDrawer(background);
            bg.drawOrder = -12;
            treeBackground = contentManager.Load<Texture2D>("Tree_BG");
            var bg2 = new BGDrawer(treeBackground);
            bg2.drawOrder = -11;

            var filepath = string.Format("Content/level.txt"); ;

            using (Stream fileStream = TitleContainer.OpenStream(filepath)) {
                int currentLine = 0;
                using (StreamReader reader = new StreamReader(fileStream)) {
                    string line = reader.ReadLine();
                    var tileIds = line;
                    while (line != null) {
                        for (int i = 0; i < tileIds.Length; i++) {
                            var tile = new Tile();
                            var position = new Vector2(i * tileSize, currentLine * tileSize);

                            if (tileIds[i] == 'P') {
                                UnitCreatorManager.CreatePlayer(contentManager, position);
                            }
                            else if (tileIds[i] == 'E') {
                                UnitCreatorManager.CreateEnemySpawner(contentManager, position);
                            }
                            else if (tileIds[i] == '#') {
                                UnitCreatorManager.CreateTileGround(contentManager, position);
                            }
                            else if (tileIds[i] == '_') {
                                UnitCreatorManager.CreateTileGrass(contentManager, position);
                            }
                        }
                        line = reader.ReadLine();
                        tileIds = line;
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
            DrawManager.DrawAll(spriteBatch);
        }
    }
    public class BGDrawer : IEEDrawable {
        private Texture2D texture2D;


        public int drawOrder = -1;

        public BGDrawer(Texture2D texture2D) {
            this.texture2D = texture2D;
            SpriteRendererComponent.spriteRendererComponents.Add(this);
        }

        public int DrawOrder => drawOrder;

        public void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(texture2D, Vector2.Zero, Color.White);
        }
    }

}
