
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace SurvivalArena {
    public class Tile {
        public Texture2D texture;
        public Vector2 position;

    }
    public class Level {
        private const int tileSize = 16;
        private Tile[,] tiles;
        public Level(ContentManager contentManager) {
            var tileTexture = contentManager.Load<Texture2D>("Tile"); ;
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
                            if (tileIds[i] != "-1") {
                                tile.texture = tileTexture;
                            }
                            tile.position = new Vector2(i, currentLine);
                            tiles[i, currentLine] = tile;

                        }
                        line = reader.ReadLine();
                        tileIds = line?.Split(',');
                        currentLine++;
                    }
                }
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
            for (int x = 0; x < tiles.GetLength(0); x++) {
                for (int y = 0; y < tiles.GetLength(1); y++) {
                    var tile = tiles[x, y];
                    if (tile.texture == null) {
                        continue;
                    }
                    spriteBatch.Draw(tile.texture, tile.position*tileSize, Color.White);
                }
            }
        }
    }
}
