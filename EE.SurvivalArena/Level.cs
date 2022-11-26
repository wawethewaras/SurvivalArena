
using EE.InputSystem;
using EE.PoolingSystem;
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
        public static Tile[,] tiles;
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
                                var physicsComponent = new PhysicsComponent(player, collider);
                                var inputComponent = new InputComponent();
                                var swordComponent = new SwordComponent(swordTexture, player);
                                var health = new HealthComponent(5, player);
                                health.hurtTag = "Enemy";
                                collider.CollisionEvents += health.DealDamage;
                                health.DeathEvent += collider.RemoveCollider;
                                inputComponent.DPressed += physicsComponent.SetMovementSpeedPositive;
                                inputComponent.APressed += physicsComponent.SetMovementSpeedNegative;
                                inputComponent.SpacePressed += physicsComponent.Jump;


                                player.AddComponent(physicsComponent);
                                player.AddComponent(inputComponent);
                                player.AddComponent(swordComponent);
                                player.AddComponent(health);

                                swordComponent.SwordAttack += collider.SpawnSword;
                                swordComponent.SwordAttackCancel += collider.RemoveSword;

                                PoolManager.gameObjects.Add(player);
                            }
                            else if (tileIds[i] == "E") {
                                var enemy = new GameObject(enemyTexture, position);
                                var spawner = new GameObjectSpawner(enemy, position);

                                PoolManager.gameObjects.Add(spawner);

                            }
                            else if (tileIds[i] != "-1") {
                                tile.texture = tileTexture;
                                var colliderComponent = new ColliderComponent(tile, tileTexture.Width, tileTexture.Height);
                                colliderComponent.tag = "Wall";
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
            for (int i = PoolManager.gameObjects.Count - 1; i >= 0; i--) {
                PoolManager.gameObjects[i].Update(gameTime);
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
            for (int i = PoolManager.gameObjects.Count - 1; i >= 0; i--) {
                PoolManager.gameObjects[i].Draw(spriteBatch);
            }
        }
    }
}
