
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace SurvivalArena {
    public class Level {
        private const int tileSize = 16;
        public static Tile[,] tiles;
        public static List<IUpdater> gameObjects = new List<IUpdater>();
        public Level(ContentManager contentManager) {
            var tileTexture = contentManager.Load<Texture2D>("Tile"); ;
            var playerTexture = contentManager.Load<Texture2D>("Player"); ;
            var enemyTexture = contentManager.Load<Texture2D>("Player"); ;
            var swordTexture = contentManager.Load<Texture2D>("Player"); ;


            var filepath = string.Format("Content/SurvivalArena.csv"); ;
            tiles = new Tile[80, 45];

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
                                var player = new GameObject(playerTexture, position);
                                var collider = new ColliderComponent(player, playerTexture.Width, playerTexture.Height);
                                player.colliderComponent = collider;
                                var physicsComponent = new PhysicsComponent(player, collider);
                                var inputComponent = new InputComponent(physicsComponent);
                                var swordComponent = new SwordComponent(swordTexture, player);

                                player.AddComponent(physicsComponent);
                                player.AddComponent(inputComponent);
                                player.AddComponent(swordComponent);

                                gameObjects.Add(player);
                            }
                            else if (tileIds[i] == "E") {
                                var enemy = new GameObject(enemyTexture, position);
                                var spawner = new GameObjectSpawner(enemy, position);

                                gameObjects.Add(spawner);

                            }
                            else if (tileIds[i] != "-1") {
                                tile.texture = tileTexture;
                                tile.colliderComponent = new ColliderComponent(tile, tileTexture.Width, tileTexture.Height);
                            }

                            tile.position = position;
                            tiles[i, currentLine] = tile;

                        }
                        line = reader.ReadLine();
                        tileIds = line?.Split(',');
                        currentLine++;
                    }
                }
            }
        }
        public void Update(float gameTime) {
            for (int i = gameObjects.Count - 1; i >= 0; i--) {
                gameObjects[i].Update(gameTime);
            }
        }
        public void Draw(SpriteBatch spriteBatch) {
            for (int x = 0; x < tiles.GetLength(0); x++) {
                for (int y = 0; y < tiles.GetLength(1); y++) {
                    var tile = tiles[x, y];
                    if (tile.texture == null) {
                        continue;
                    }
                    spriteBatch.Draw(tile.texture, tile.position, Color.White);
                }
            }
            for (int i = gameObjects.Count - 1; i >= 0; i--) {
                gameObjects[i].Draw(spriteBatch);
            }
        }
    }
}
