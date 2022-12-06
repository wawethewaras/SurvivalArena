using EE.HealthSystem;
using EE.InputSystem;
using EE.PoolingSystem;
using EE.ScoreSystem;
using EE.SpriteRendererSystem;
using EE.StateSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SurvivalArena;
using SurvivalArena.ColliderSystem;
using SurvivalArena.GameObjects;
using SurvivalArena.HealthSystem;
using SurvivalArena.Physics;
using SurvivalArena.Sword;
using SurvivalArena.TileSystem;

namespace EE.SurvivalArena {
    public static class UnitCreatorManager {
        public static void CreateEnemySpawner(ContentManager contentManager, Vector2 position) {
            var enemyTexture = contentManager.Load<Texture2D>("Enemy"); 
            var spawner = new GameObjectSpawner(contentManager, position);

            PoolManager.gameObjects.Add(spawner);
        }
        public static void SpawnBossEnemy(ContentManager contentManager, Vector2 spawnPosition) {
            var texture2D = contentManager.Load<Texture2D>("Boss");
            var enemyTexture = new Texture2D[] {
                texture2D
            };
            var enemyAnimation = new SpriteAnimation(enemyTexture);


            GameObject spawner2 = new GameObject(spawnPosition);
            var collider = new ColliderComponent(spawner2, texture2D.Width , texture2D.Height );
            collider.tag = "Enemy";
            collider.LookingRight = Level.Player != null && Level.Player.Position.X > spawner2.position.X;
            var physicsComponent = new PhysicsComponent(spawner2, collider);
            physicsComponent.moveSpeed = 500;
            var stateComponent = new StateComponent();
            stateComponent.OnAct += physicsComponent.ADMovement;

            var health = new HealthComponent(5, spawner2);
            var poolableComponent = new PoolableComponent(spawner2);
            var spriteRendererComponent = new SpriteRendererComponent(enemyAnimation, spawner2, collider);
            var score = new ScoreComponent(100, 1);
            health.hurtTag = "Sword";
            spawner2.AddComponent(physicsComponent);
            spawner2.AddComponent(stateComponent);
            spawner2.AddComponent(health);
            spawner2.AddComponent(spriteRendererComponent);
            spawner2.AddComponent(score);

            collider.CollisionEvents += (ColliderComponent colliderComponent) => health.DealDamage(colliderComponent.tag);
            health.DeathEvent += collider.RemoveCollider;
            health.DeathEvent += poolableComponent.ReleaseSelf;
            health.DeathEvent += spriteRendererComponent.OnDestroy;
            health.DeathEvent += score.AddScore;
            health.DeathEvent += SurvivalArenaGame.Win;


            PoolManager.gameObjects.Add(spawner2);
        }

        public static void SpawnADEnemy(ContentManager contentManager, Vector2 spawnPosition) {
            var texture2D = contentManager.Load<Texture2D>("Enemy");
            var enemyTexture = new Texture2D[] {
                texture2D
            };
            var enemyAnimation = new SpriteAnimation(enemyTexture);

            var hitSound = contentManager.Load<SoundEffect>("Hit_Hurt_Enemy");

            GameObject spawner2 = new GameObject(spawnPosition);
            var collider = new ColliderComponent(spawner2, texture2D.Width , texture2D.Height );
            collider.tag = "Enemy";
            collider.LookingRight = Level.Player != null && Level.Player.Position.X > spawner2.position.X;
            var physicsComponent = new PhysicsComponent(spawner2, collider);
            var stateComponent = new StateComponent();
            stateComponent.OnAct += physicsComponent.ADMovement;

            var health = new HealthComponent(1, spawner2);
            var poolableComponent = new PoolableComponent(spawner2);
            var spriteRendererComponent = new SpriteRendererComponent(enemyAnimation, spawner2, collider);
            var score = new ScoreComponent(100, 1);
            health.hurtTag = "Sword";
            spawner2.AddComponent(physicsComponent);
            spawner2.AddComponent(stateComponent);
            spawner2.AddComponent(health);
            spawner2.AddComponent(spriteRendererComponent);
            spawner2.AddComponent(score);

            collider.CollisionEvents += (ColliderComponent colliderComponent) => health.DealDamage(colliderComponent.tag);
            health.DeathEvent += collider.RemoveCollider;
            health.DeathEvent += poolableComponent.ReleaseSelf;
            health.DeathEvent += spriteRendererComponent.OnDestroy;
            health.DeathEvent += score.AddScore;
            health.DeathEvent += () => {
                Random random = new Random();
                var num = random.Next(0, 10);
                if (num > 7) {
                    SpawnPotion(contentManager, spawner2.position);
                }
            };

            health.HitEvent += () => hitSound.Play();

            PoolManager.gameObjects.Add(spawner2);
        }


        public static void SpawnShootingEnemy(ContentManager contentManager, Vector2 spawnPosition) {
            var texture2D = contentManager.Load<Texture2D>("Enemy");
            var enemyTexture = new Texture2D[] {
                texture2D
            };
            var enemyAnimation = new SpriteAnimation(enemyTexture);

            var hitSound = contentManager.Load<SoundEffect>("Hit_Hurt_Enemy");

            

            GameObject spawner2 = new GameObject(spawnPosition);
            var collider = new ColliderComponent(spawner2, texture2D.Width , texture2D.Height );
            collider.tag = "Enemy";
            collider.LookingRight = Level.Player != null && Level.Player.Position.X > spawner2.position.X;
            var physicsComponent = new PhysicsComponent(spawner2, collider);
            var health = new HealthComponent(1, spawner2);
            var poolableComponent = new PoolableComponent(spawner2);
            var spriteRendererComponent = new SpriteRendererComponent(enemyAnimation, spawner2, collider);
            var score = new ScoreComponent(100, 1);
            var stateComponent = new StateComponent();
            var shootAction = new ShootAction();
            shootAction.ShootEvent += () => SpawnProjectile(contentManager, spawner2);
            stateComponent.OnAct += shootAction.Shoot;

            health.hurtTag = "Sword";
            spawner2.AddComponent(physicsComponent);
            spawner2.AddComponent(health);
            spawner2.AddComponent(spriteRendererComponent);
            spawner2.AddComponent(score);
            spawner2.AddComponent(stateComponent);

            collider.CollisionEvents += (ColliderComponent colliderComponent) => health.DealDamage(colliderComponent.tag);
            health.DeathEvent += collider.RemoveCollider;
            health.DeathEvent += poolableComponent.ReleaseSelf;
            health.DeathEvent += spriteRendererComponent.OnDestroy;
            health.DeathEvent += score.AddScore;
            health.HitEvent += () => hitSound.Play();

            PoolManager.gameObjects.Add(spawner2);
        }


        public static void CreatePlayer(ContentManager contentManager, Vector2 position) {
            var playerTexture = contentManager.Load<Texture2D>("Player");
            var playerTexture2 = contentManager.Load<Texture2D>("Player2");

            var swordTexture = contentManager.Load<Texture2D>("Player");

            var playerTextures = new Texture2D[] {
                playerTexture,
                playerTexture2
            };
            var swordTextures = new Texture2D[] {
                swordTexture
            };
            var playerAnimation = new SpriteAnimation(playerTextures);
            var swordAnimation = new SpriteAnimation(swordTextures);

            var hitSound = contentManager.Load<SoundEffect>("Hit_Hurt");

            var gameOverSound = contentManager.Load<SoundEffect>("GameOver");
            var jumpSound = contentManager.Load<SoundEffect>("Jump");
            var hammerSound = contentManager.Load<SoundEffect>("Hammer");


            var player = new GameObject(position);
            Level.Player = player;
            var collider = new ColliderComponent(player, playerTexture.Width , playerTexture.Height );
            collider.tag = "Player";

            var physicsComponent = new PhysicsComponent(player, collider);
            var inputComponent = new InputComponent();
            var swordComponent = new SwordComponent(swordTexture, player);
            var health = new HealthComponent(5, player);
            var spriteRendererComponent = new SpriteRendererComponent(playerAnimation, player, collider);

            var hasOffSet = new HasPositionWithOfSet(player, collider, new Vector2(swordTexture.Width, 0));
            var swordRender = new SpriteRendererComponent(swordAnimation, hasOffSet, collider);
            var swordCollider = new ColliderComponent(hasOffSet, playerTexture.Width, playerTexture.Height);
            swordCollider.tag = "Sword";
            swordCollider.DeActive();

            health.hurtTag = "Enemy";
            collider.CollisionEvents += (ColliderComponent colliderComponent) => health.DealDamage(colliderComponent.tag);
            collider.CollisionEventFromOther += (ColliderComponent colliderComponent) => health.DealDamage(colliderComponent.tag);
            collider.CollisionEventFromOther += (ColliderComponent colliderComponent) => {
                if (colliderComponent.tag == "Heal") {
                    health.health++;
                }
                colliderComponent.CollisionFromOther(colliderComponent);
            };

            health.DeathEvent += collider.RemoveCollider;
            health.DeathEvent += spriteRendererComponent.OnDestroy;
            health.DeathEvent += SurvivalArenaGame.GameOver;
            health.DeathEvent += () => gameOverSound.Play();
            health.HitEvent += () => hitSound.Play();

            inputComponent.DPressed += physicsComponent.SetMovementSpeedPositive;
            inputComponent.APressed += physicsComponent.SetMovementSpeedNegative;
            inputComponent.SpacePressed += physicsComponent.Jump;
            inputComponent.SpacePressed += () => jumpSound.Play();

            player.AddComponent(physicsComponent);
            player.AddComponent(inputComponent);
            player.AddComponent(swordComponent);
            player.AddComponent(health);
            player.AddComponent(spriteRendererComponent);

            swordComponent.SwordAttack += swordCollider.SetActive;
            swordComponent.SwordAttack += swordRender.SetActive;
            swordComponent.SwordAttack += () => hammerSound.Play();

            swordComponent.SwordAttackCancel += swordCollider.DeActive;
            swordComponent.SwordAttackCancel += swordRender.DeActive;
            HealthUIManager healthUIManager = new HealthUIManager(contentManager, health);
            SpriteRendererComponent.spriteRendererComponents.Add(healthUIManager);
            PoolManager.gameObjects.Add(player);
        }

        public static void CreateTile(ContentManager contentManager, Vector2 position) {
            var tileTexture = contentManager.Load<Texture2D>("Tile");
            var tileTextures = new Texture2D[] {
                tileTexture
            };
            var tileAnimation = new SpriteAnimation(tileTextures);

            var tile = new Tile();
            tile.position = position;
            var colliderComponent = new ColliderComponent(tile, tileTexture.Width, tileTexture.Height);
            colliderComponent.tag = "Wall";

            var spriteRendererComponent = new SpriteRendererComponent(tileAnimation, tile);

        }

        public static void SpawnProjectile(ContentManager contentManager, IHasPosition spawnPosition) {
            var texture2D = contentManager.Load<Texture2D>("Bomb");
            var bombTexture = new Texture2D[] {
                texture2D
            };

            var bombAnimation = new SpriteAnimation(bombTexture);
            var explosionSound = contentManager.Load<SoundEffect>("Explosion");


            var position = spawnPosition.Position;
            GameObject spawner2 = new GameObject(position);
            var collider = new ColliderComponent(spawner2, texture2D.Width , texture2D.Height );
            collider.tag = "Enemy";
            collider.LookingRight = Level.Player != null && Level.Player.Position.X > spawner2.position.X;
            var physicsComponent = new PhysicsComponent(spawner2, collider);
            physicsComponent.gravity = 0;
            var stateComponent = new StateComponent();
            stateComponent.OnAct += physicsComponent.ADMovement;
            var poolableComponent = new PoolableComponent(spawner2);
            var spriteRendererComponent = new SpriteRendererComponent(bombAnimation, spawner2, collider);
            spawner2.AddComponent(physicsComponent);
            spawner2.AddComponent(stateComponent);
            spawner2.AddComponent(spriteRendererComponent);

            collider.CollisionEvents += (ColliderComponent x) => {
                if (x.tag == "Wall" || x.tag == "Player" || x.tag == "Sword") {
                    collider.RemoveCollider();
                    poolableComponent.ReleaseSelf();
                    spriteRendererComponent.OnDestroy();
                    x.CollisionFromOther(collider);

                }
            };

            PoolManager.gameObjects.Add(spawner2);
        }
        public static void SpawnPotion(ContentManager contentManager, Vector2 spawnPosition) {
            var texture2D = contentManager.Load<Texture2D>("Heart");
            var texture2Ds = new Texture2D[] {
                texture2D
            };


            var potionTexture = new SpriteAnimation(texture2Ds);
            var healSound = contentManager.Load<SoundEffect>("Heal");


            GameObject spawner2 = new GameObject(spawnPosition);
            var collider = new ColliderComponent(spawner2, texture2D.Width, texture2D.Height);
            collider.tag = "Heal";
            var poolableComponent = new PoolableComponent(spawner2);
            var spriteRendererComponent = new SpriteRendererComponent(potionTexture, spawner2, collider);
            var physicsComponent = new PhysicsComponent(spawner2, collider);
            physicsComponent.gravity = 0.1f;

            spawner2.AddComponent(spriteRendererComponent);
            spawner2.AddComponent(physicsComponent);

            collider.CollisionEvents += (ColliderComponent x) => {
                if (x.tag == "Player") {
                    collider.RemoveCollider();
                    poolableComponent.ReleaseSelf();
                    spriteRendererComponent.OnDestroy();
                    healSound.Play();
                    x.CollisionFromOther(collider);
                }
            };
            collider.CollisionEventFromOther += (ColliderComponent x) => {
                if (x.tag == "Player") {
                    collider.RemoveCollider();
                    poolableComponent.ReleaseSelf();
                    spriteRendererComponent.OnDestroy();
                    healSound.Play();
                    x.CollisionFromOther(collider);
                }
            };
            PoolManager.gameObjects.Add(spawner2);
        }
    }
}
