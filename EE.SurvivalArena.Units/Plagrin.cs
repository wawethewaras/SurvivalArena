using EE.PoolingSystem;
using EE.ScoreSystem;
using EE.SoundSystem;
using EE.SpriteRendererSystem;
using EE.StateSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using SurvivalArena.GameObjects;
using SurvivalArena.HealthSystem;
using SurvivalArena.Physics;
using EE.CollisionSystem;

namespace EE.SurvivalArena.Units {
    public class Plagrin {
        public Plagrin(ContentManager contentManager, Vector2 spawnPosition) {
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
            delayComponent.SetRange(2f, 2f);

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
            spawner2.AddComponent(collider);

            collider.CollisionEvents += (ColliderComponent colliderComponent) => health.DealDamage(colliderComponent.tag);
            collider.CollisionEventFromOther += (ColliderComponent colliderComponent) => health.DealDamage(colliderComponent.tag);
            health.DeathEvent += collider.RemoveCollider;
            health.DeathEvent += poolableComponent.ReleaseSelf;
            health.DeathEvent += spriteRendererComponent.OnDestroy;
            health.DeathEvent += score.AddScore;
            health.DeathEvent += () => new ParticleEffect(contentManager, spawner2.position);
            health.DeathEvent += () => new ManaGem(contentManager, spawner2.position);

            health.HitEvent += () => new PlaySoundAction(contentManager, "Hit_Hurt_Enemy").Invoke();

            PoolManager.gameObjects.Add(spawner2);
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
            state.OnEnterEvent += () => physicsComponent.ADMovement(new Vector2(direction, 0));
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

    }
}
