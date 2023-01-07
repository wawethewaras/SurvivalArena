using EE.AbilitySystem;
using EE.HealthSystem;
using EE.InputSystem;
using EE.PoolingSystem;
using EE.ScoreSystem;
using EE.SoundSystem;
using EE.SpriteRendererSystem;
using EE.StateSystem;
using EE.SurvivalArena.Units;
using MainMenuSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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
            var spawner = new GameObjectSpawner(contentManager, position);

            PoolManager.gameObjects.Add(spawner);
        }
        public static void CreateRockSpawner(ContentManager contentManager, Vector2 position) {
            var spawner = new RockSpawner(contentManager, position);

            PoolManager.gameObjects.Add(spawner);
        }
        public static void SpawnSlugrinEnemy(ContentManager contentManager, Vector2 spawnPosition) {
            var enemyAnimation = new SpriteAnimation(contentManager, "Slugrin", 32, 45);
            var hitSound = contentManager.Load<SoundEffect>("Hit_Hurt_Enemy");


            GameObject spawner2 = new GameObject(spawnPosition);
            var collider = new ColliderComponent(spawner2, 32, 45);
            collider.tag = "Enemy";
            collider.LookingRight = Player.PlayerReference != null && Player.PlayerReference.Position.X > spawner2.position.X;
            var physicsComponent = new PhysicsComponent(spawner2, collider);
            physicsComponent.moveSpeed = 300;
            var state = new State();
            state.OnActEvent += physicsComponent.ADMovement;
            var stateComponent = new StateComponent();
            stateComponent.TransitionToState(state);



            var health = new HealthComponent(5, spawner2);
            var poolableComponent = new PoolableComponent(spawner2);
            var spriteRendererComponent = new SpriteRendererComponent(enemyAnimation, spawner2, collider);
            var score = new ScoreComponent(500, 1);
            health.hurtTag = "Sword";

            HealthUIManager healthUIManager = new HealthUIManager(contentManager, health, spawner2);
            SpriteRendererComponent.spriteRendererComponents.Add(healthUIManager);

            spawner2.AddComponent(physicsComponent);
            spawner2.AddComponent(stateComponent);
            spawner2.AddComponent(health);
            spawner2.AddComponent(spriteRendererComponent);
            spawner2.AddComponent(score);

            collider.CollisionEvents += (ColliderComponent colliderComponent) => health.DealDamage(colliderComponent.tag);
            collider.CollisionEventFromOther += (ColliderComponent colliderComponent) => health.DealDamage(colliderComponent.tag);
            health.DeathEvent += collider.RemoveCollider;
            health.DeathEvent += poolableComponent.ReleaseSelf;
            health.DeathEvent += spriteRendererComponent.OnDestroy;
            health.DeathEvent += score.AddScore;
            //health.DeathEvent += SurvivalArenaGame.Win;
            health.DeathEvent += () => SpawnExplosion(contentManager, spawner2.position);
            health.DeathEvent += () => SpawnMana(contentManager, spawner2.position);
         
            health.DeathEvent += healthUIManager.DeActive;

            health.HitEvent += () => hitSound.Play();

            PoolManager.gameObjects.Add(spawner2);

        }
        public static void SpawnSlugHound(ContentManager contentManager, Vector2 spawnPosition) {
            var enemyAnimation = new SpriteAnimation(contentManager, "Slughound", 38, 32);

            var spawner2 = new GameObject(spawnPosition);
            var collider = new ColliderComponent(spawner2, 38, 32);
            collider.tag = "Enemy";
            collider.LookingRight = Player.PlayerReference != null && Player.PlayerReference.Position.X > spawner2.position.X;
            var physicsComponent = new PhysicsComponent(spawner2, collider);
            physicsComponent.moveSpeed = 250;

            var delayComponent = new DelayComponent();
            delayComponent.SetRange(0.25f,1.5f);
            delayComponent.resetOnDefault = false;


            var stateComponent = new StateComponent();

            var walkState = new TransitionState(stateComponent);
            walkState.OnActEvent += physicsComponent.ADMovement;
            var jumpState = new TransitionState(stateComponent);
            jumpState.OnEnterEvent += physicsComponent.Jump;
            jumpState.OnEnterEvent += delayComponent.Reset;

            var jumpRequirement = new RequirementDelegate();
            jumpRequirement.Add(() => delayComponent.swordTimeCounter < 0);
            var jumpTransition = new Transition(jumpRequirement, jumpState);
            walkState.transitions.Add(jumpTransition);

            var walkRequirement = new RequirementDelegate();
            walkRequirement.Add(() => true);
            var walkTransition = new Transition(walkRequirement, walkState);
            jumpState.transitions.Add(walkTransition);

            stateComponent.TransitionToState(walkState);


            var health = new HealthComponent(1, spawner2);
            health.hurtTag = "Sword";

            var poolableComponent = new PoolableComponent(spawner2);
            var spriteRendererComponent = new SpriteRendererComponent(enemyAnimation, spawner2, collider);
            var score = new ScoreComponent(150, 1);
            spawner2.AddComponent(physicsComponent);
            spawner2.AddComponent(stateComponent);
            spawner2.AddComponent(health);
            spawner2.AddComponent(spriteRendererComponent);
            spawner2.AddComponent(score);
            spawner2.AddComponent(delayComponent);

            collider.CollisionEvents += (ColliderComponent colliderComponent) => health.DealDamage(colliderComponent.tag);
            collider.CollisionEventFromOther += (ColliderComponent colliderComponent) => health.DealDamage(colliderComponent.tag);
            health.DeathEvent += collider.RemoveCollider;
            health.DeathEvent += poolableComponent.ReleaseSelf;
            health.DeathEvent += spriteRendererComponent.OnDestroy;
            health.DeathEvent += score.AddScore;
            health.DeathEvent += () => SpawnExplosion(contentManager, spawner2.position);
            health.DeathEvent += () => SpawnMana(contentManager, spawner2.position);
            health.HitEvent += () => new PlaySoundAction(contentManager, "Hit_Hurt_Enemy").Invoke();

            PoolManager.gameObjects.Add(spawner2);
        }

        public static void SpawnADEnemy(ContentManager contentManager, Vector2 spawnPosition) {
            var enemyAnimation = new SpriteAnimation(contentManager, "Slime", 32, 32);

            GameObject spawner2 = new GameObject(spawnPosition);
            var collider = new ColliderComponent(spawner2, 32, 32);
            collider.tag = "Enemy";
            collider.LookingRight = Player.PlayerReference != null && Player.PlayerReference.Position.X > spawner2.position.X;
            var physicsComponent = new PhysicsComponent(spawner2, collider);
            physicsComponent.moveSpeed = 75;
            var state = new State();
            state.OnActEvent += physicsComponent.ADMovement;
            var stateComponent = new StateComponent();
            stateComponent.TransitionToState(state);

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
            collider.CollisionEventFromOther += (ColliderComponent colliderComponent) => health.DealDamage(colliderComponent.tag);
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
            health.DeathEvent += () => SpawnExplosion(contentManager, spawner2.position);
            health.DeathEvent += () => SpawnMana(contentManager, spawner2.position);

            health.HitEvent += () => new PlaySoundAction(contentManager, "Hit_Hurt_Enemy").Invoke();

            PoolManager.gameObjects.Add(spawner2);
        }

        public static void SpawnShieldEnemy(ContentManager contentManager, Vector2 spawnPosition) {
            var enemyAnimation = new SpriteAnimation(contentManager, "Sapling", 16, 16);

            GameObject spawner2 = new GameObject(spawnPosition);
            var collider = new ColliderComponent(spawner2, 16, 16);
            collider.tag = "Enemy";
            collider.LookingRight = Player.PlayerReference != null && Player.PlayerReference.Position.X > spawner2.position.X;
            var physicsComponent = new PhysicsComponent(spawner2, collider);
            physicsComponent.moveSpeed = 75;
            var state = new State();
            state.OnActEvent += physicsComponent.ADMovement;
            var stateComponent = new StateComponent();
            stateComponent.TransitionToState(state);

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

            collider.CollisionEvents += (ColliderComponent colliderComponent) => {
                if (colliderComponent.tag != health.hurtTag) {
                    return;
                }
                var lookingRightAndTargetLeft = collider.LookingRight && colliderComponent.Position.X < collider.Position.X;
                var lookinLeftAndTargetRight = !collider.LookingRight && colliderComponent.Position.X > collider.Position.X;

                if (lookingRightAndTargetLeft || lookinLeftAndTargetRight) {
                    health.DealDamage(colliderComponent.tag);
                }
            };
            collider.CollisionEventFromOther += (ColliderComponent colliderComponent) => {
                if (colliderComponent.tag != health.hurtTag) {
                    return;
                }
                var lookingRightAndTargetLeft = collider.LookingRight && colliderComponent.Position.X < collider.Position.X;
                var lookinLeftAndTargetRight = !collider.LookingRight && colliderComponent.Position.X > collider.Position.X;

                if (lookingRightAndTargetLeft || lookinLeftAndTargetRight) {
                    health.DealDamage(colliderComponent.tag);
                }
                else {
                    new PlaySoundAction(contentManager, "Blocked").Invoke();
                }
            };
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
            health.DeathEvent += () => SpawnExplosion(contentManager, spawner2.position);
            health.DeathEvent += () => SpawnMana(contentManager, spawner2.position);

            health.HitEvent += () => new PlaySoundAction(contentManager, "Hit_Hurt_Enemy").Invoke();

            PoolManager.gameObjects.Add(spawner2);
        }

        public static void SpawnShootingEnemy(ContentManager contentManager, Vector2 spawnPosition) {         
            GameObject spawner2 = new GameObject(spawnPosition);
            var collider = new ColliderComponent(spawner2, 32, 32);
            collider.tag = "Enemy";
            collider.LookingRight = Player.PlayerReference != null && Player.PlayerReference.Position.X > spawner2.position.X;
            var physicsComponent = new PhysicsComponent(spawner2, collider);
            var health = new HealthComponent(1, spawner2);
            health.hurtTag = "Sword";
            var poolableComponent = new PoolableComponent(spawner2);
            var enemyAnimation = new SpriteAnimation(contentManager, "Plant", 32, 32);
            var spriteRendererComponent = new SpriteRendererComponent(enemyAnimation, spawner2, collider);
            var score = new ScoreComponent(100, 1);
            var shootAction = new ShootAction();
            shootAction.ShootEvent += () => SpawnEnemyProjectile(contentManager, spawner2);

            var state = new State();
            state.OnActEvent += shootAction.Shoot;
            var stateComponent = new StateComponent();
            stateComponent.TransitionToState(state);


            var delayComponent = new DelayComponent();
            delayComponent.SetRange(3f, 4f);

            delayComponent.Reset();
            delayComponent.SwordAttack += () => {
                poolableComponent.ReleaseSelf();
                spriteRendererComponent.OnDestroy();
                collider.RemoveCollider();
            };

            spawner2.AddComponent(physicsComponent);
            spawner2.AddComponent(health);
            spawner2.AddComponent(spriteRendererComponent);
            spawner2.AddComponent(score);
            spawner2.AddComponent(stateComponent);
            spawner2.AddComponent(delayComponent);

            collider.CollisionEvents += (ColliderComponent colliderComponent) => health.DealDamage(colliderComponent.tag);
            collider.CollisionEventFromOther += (ColliderComponent colliderComponent) => health.DealDamage(colliderComponent.tag);
            health.DeathEvent += collider.RemoveCollider;
            health.DeathEvent += poolableComponent.ReleaseSelf;
            health.DeathEvent += spriteRendererComponent.OnDestroy;
            health.DeathEvent += score.AddScore;
            health.DeathEvent += () => SpawnExplosion(contentManager, spawner2.position);
            health.DeathEvent += () => SpawnMana(contentManager, spawner2.position);

            health.HitEvent += () => new PlaySoundAction(contentManager, "Hit_Hurt_Enemy").Invoke();

            PoolManager.gameObjects.Add(spawner2);
        }
        public static SwordComponent SpawnSword(ContentManager contentManager, IHasPosition hasPosition, IHasFacingDirection hasFacingDirection) {
            var hammerSound = contentManager.Load<SoundEffect>("Hammer");

            var swordAnimation = new SpriteAnimation(contentManager, "SporeBolt", 32, 32);
            var swordComponent = new SwordComponent();

            var swordRender = new SpriteRendererComponent(swordAnimation, hasPosition, hasFacingDirection);
            var swordCollider = new ColliderComponent(hasPosition, 32, 32);
            swordCollider.tag = "Sword";
            swordCollider.DeActive();

            swordComponent.SwordAttack += swordCollider.SetActive;
            swordComponent.SwordAttack += swordRender.SetActive;
            swordComponent.SwordAttack += () => hammerSound.Play();

            swordComponent.SwordAttackCancel += swordCollider.DeActive;
            swordComponent.SwordAttackCancel += swordRender.DeActive;

            return swordComponent;
        }
        public static void CreateTileGround(ContentManager contentManager, Vector2 position) {
            CreateTile(contentManager, "Tile", position);
        }
        public static void CreateTileGrass(ContentManager contentManager, Vector2 position) {
            CreateTile(contentManager, "Tile_Grass", position);
        }
        public static void CreateTile(ContentManager contentManager, string tileName,Vector2 position) {
            var tileAnimation = new SpriteAnimation(contentManager, tileName, 16, 16);

            var tile = new Tile();
            tile.position = position;
            var colliderComponent = new ColliderComponent(tile, 16, 16);
            colliderComponent.tag = "Wall";

            var spriteRendererComponent = new SpriteRendererComponent(tileAnimation, tile);

        }

        public static void SpawnEnemyProjectile(ContentManager contentManager, IHasPosition spawnPosition) {
            var bombAnimation = new SpriteAnimation(contentManager, "PoisonBolt", 16, 16);

            var position = spawnPosition.Position;
            GameObject spawner2 = new GameObject(position);
            var collider = new ColliderComponent(spawner2, 16, 16);
            collider.tag = "Enemy";
            var direction = Player.PlayerReference != null && Player.PlayerReference.Position.X > spawner2.position.X ? 1 : -1;
            var physicsComponent = new PhysicsComponent(spawner2, collider);
            physicsComponent.gravity = 0;
            var state = new State();
            state.OnEnterEvent += () => physicsComponent.ADMovement(new Vector2(direction,0));
            var stateComponent = new StateComponent();
            stateComponent.TransitionToState(state);
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
                }
            };
            PoolManager.gameObjects.Add(spawner2);
        }
        public static void SpawnFallingRock(ContentManager contentManager, Vector2 position) {
            var bombAnimation = new SpriteAnimation(contentManager, "Rock", 32, 32);

            GameObject spawner2 = new GameObject(position);
            var collider = new ColliderComponent(spawner2, 32, 32);
            collider.tag = "Enemy";
            var physicsComponent = new PhysicsComponent(spawner2, collider);
            var state = new State();
            var stateComponent = new StateComponent();
            stateComponent.TransitionToState(state);
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
                }
            };

            PoolManager.gameObjects.Add(spawner2);
        }

        public static void SpawnPotion(ContentManager contentManager, Vector2 spawnPosition) {
            var potionTexture = new SpriteAnimation(contentManager, "Heart", 16, 16);
            var healSound = contentManager.Load<SoundEffect>("Heal");


            GameObject spawner2 = new GameObject(spawnPosition);
            var collider = new ColliderComponent(spawner2, 16, 16);

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
                }
            };
            collider.CollisionEventFromOther += (ColliderComponent x) => {
                if (x.tag == "Player") {
                    collider.RemoveCollider();
                    poolableComponent.ReleaseSelf();
                    spriteRendererComponent.OnDestroy();
                    healSound.Play();
                }
            };
            PoolManager.gameObjects.Add(spawner2);
        }
        public static void SpawnMana(ContentManager contentManager, Vector2 spawnPosition) {
            var potionTexture = new SpriteAnimation(contentManager, "Mana", 10, 10);
            var healSound = contentManager.Load<SoundEffect>("Mana_Collect");


            GameObject spawner2 = new GameObject(spawnPosition);
            var collider = new ColliderComponent(spawner2, 10, 10);

            collider.tag = "Mana";
            var poolableComponent = new PoolableComponent(spawner2);
            var spriteRendererComponent = new SpriteRendererComponent(potionTexture, spawner2, collider);
            var physicsComponent = new PhysicsComponent(spawner2, collider);
            physicsComponent.gravity = 0.1f;

            var delayComponent = new DelayComponent();
            delayComponent.SetRange(2,2);
            delayComponent.Reset();
            delayComponent.SwordAttack += () => {
                poolableComponent.ReleaseSelf();
                spriteRendererComponent.OnDestroy();

            };

            spawner2.AddComponent(spriteRendererComponent);
            spawner2.AddComponent(physicsComponent);
            spawner2.AddComponent(delayComponent);

            collider.CollisionEvents += (ColliderComponent x) => {
                if (x.tag == "Player") {
                    collider.RemoveCollider();
                    poolableComponent.ReleaseSelf();
                    spriteRendererComponent.OnDestroy();
                    healSound.Play();
                }
            };
            collider.CollisionEventFromOther += (ColliderComponent x) => {
                if (x.tag == "Player") {
                    collider.RemoveCollider();
                    poolableComponent.ReleaseSelf();
                    spriteRendererComponent.OnDestroy();
                    healSound.Play();
                }
            };
            PoolManager.gameObjects.Add(spawner2);
        }

        public static void SpawnExplosion(ContentManager contentManager, Vector2 spawnPosition) {
            var potionTexture = new SpriteAnimation(contentManager, "ParticleExplosion", 16, 16);

            GameObject spawner2 = new GameObject(spawnPosition);
            var poolableComponent = new PoolableComponent(spawner2);
            var spriteRendererComponent = new SpriteRendererComponent(potionTexture, spawner2, null);
            var delayComponent = new DelayComponent();
            delayComponent.Reset();
            delayComponent.SwordAttack += () => {
                poolableComponent.ReleaseSelf();
                spriteRendererComponent.OnDestroy();

            };
            spawner2.AddComponent(spriteRendererComponent);
            spawner2.AddComponent(delayComponent);


            PoolManager.gameObjects.Add(spawner2);
        }
    }
}
