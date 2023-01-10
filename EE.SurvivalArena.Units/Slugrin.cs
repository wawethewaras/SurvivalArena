using EE.HealthSystem;
using EE.PoolingSystem;
using EE.ScoreSystem;
using EE.SpriteRendererSystem;
using EE.StateSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using EE.CollisionSystem;
using SurvivalArena.GameObjects;
using SurvivalArena.HealthSystem;
using SurvivalArena.Physics;

namespace EE.SurvivalArena.Units {
    public class Slugrin {
        public Slugrin(ContentManager contentManager, Vector2 spawnPosition) {
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
            spawner2.AddComponent(collider);

            collider.CollisionEvents += (ColliderComponent colliderComponent) => health.DealDamage(colliderComponent.tag);
            collider.CollisionEventFromOther += (ColliderComponent colliderComponent) => health.DealDamage(colliderComponent.tag);
            health.DeathEvent += collider.RemoveCollider;
            health.DeathEvent += poolableComponent.ReleaseSelf;
            health.DeathEvent += spriteRendererComponent.OnDestroy;
            health.DeathEvent += score.AddScore;
            //health.DeathEvent += SurvivalArenaGame.Win;
            health.DeathEvent += () => new ParticleEffect(contentManager, spawner2.position);
            health.DeathEvent += () => new ManaGem(contentManager, spawner2.position);

            health.DeathEvent += healthUIManager.DeActive;

            health.HitEvent += () => hitSound.Play();

            PoolManager.gameObjects.Add(spawner2);

        }

    }
}
