using EE.PoolingSystem;
using EE.ScoreSystem;
using EE.SoundSystem;
using EE.SpriteRendererSystem;
using EE.StateSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using EE.CollisionSystem;
using SurvivalArena.GameObjects;
using SurvivalArena.HealthSystem;
using SurvivalArena.Physics;

namespace EE.SurvivalArena.Units {
    public class SlugHound {
        public SlugHound(ContentManager contentManager, Vector2 spawnPosition) {
            var enemyAnimation = new SpriteAnimation(contentManager, "Slughound", 38, 32);

            var spawner2 = new GameObject(spawnPosition);
            var collider = new ColliderComponent(spawner2, 38, 32);
            collider.tag = "Enemy";
            collider.LookingRight = Player.PlayerReference != null && Player.PlayerReference.Position.X > spawner2.position.X;
            var physicsComponent = new PhysicsComponent(spawner2, collider);
            physicsComponent.moveSpeed = 250;

            var delayComponent = new DelayComponent();
            delayComponent.SetRange(0.25f, 1.5f);
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
            spawner2.AddComponent(collider);

            collider.CollisionEvents += (IColliderComponent colliderComponent) => health.DealDamage(colliderComponent.Tag);
            collider.CollisionEventFromOther += (IColliderComponent colliderComponent) => health.DealDamage(colliderComponent.Tag);
            health.DeathEvent += collider.RemoveCollider;
            health.DeathEvent += poolableComponent.ReleaseSelf;
            health.DeathEvent += spriteRendererComponent.OnDestroy;
            health.DeathEvent += score.AddScore;
            health.DeathEvent += () => new ParticleEffect(contentManager, spawner2.position);
            health.DeathEvent += () => new ManaGem(contentManager, spawner2.position);
            health.HitEvent += () => new PlaySoundAction(contentManager, "Hit_Hurt_Enemy").Invoke();

            PoolManager.gameObjects.Add(spawner2);
        }

    }
}
