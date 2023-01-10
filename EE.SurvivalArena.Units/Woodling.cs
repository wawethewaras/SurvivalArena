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
    public class Woodling {
        public Woodling(ContentManager contentManager, Vector2 spawnPosition) {
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
                    new HealthPotion(contentManager, spawner2.position);
                }
            };
            health.DeathEvent += () => new ParticleEffect(contentManager, spawner2.position);
            health.DeathEvent += () => new ManaGem(contentManager, spawner2.position);

            health.HitEvent += () => new PlaySoundAction(contentManager, "Hit_Hurt_Enemy").Invoke();

            PoolManager.gameObjects.Add(spawner2);
        }
    }
}
