using EE.AbilitySystem;
using EE.HealthSystem;
using EE.InputSystem;
using EE.PoolingSystem;
using EE.SpriteRendererSystem;
using EE.StateSystem;
using MainMenuSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SurvivalArena.ColliderSystem;
using SurvivalArena.GameObjects;
using SurvivalArena.HealthSystem;
using SurvivalArena.Physics;
using SurvivalArena.TileSystem;

namespace EE.SurvivalArena.Units {
    public class Player : GameObject {
        public static IHasPosition PlayerReference;
        public HealthComponent healthComponent;
        public Player(ContentManager contentManager, Vector2 position) : base(position) {
            var playerAnimation_Idle = new SpriteAnimation(contentManager, "Player", 32, 32);
            var playerAnimation_Walk = new SpriteAnimation(contentManager, "Player_Run", 32, 32);

            var hitSound = contentManager.Load<SoundEffect>("Hit_Hurt");

            var gameOverSound = contentManager.Load<SoundEffect>("GameOver");
            var jumpSound = contentManager.Load<SoundEffect>("Jump");


            var player = new GameObject(position);
            PlayerReference = player;
            var collider = new ColliderComponent(player, 32, 32);
            collider.tag = "Player";

            var physicsComponent = new PhysicsComponent(player, collider);
            var inputComponent = new InputComponent();
            healthComponent = new HealthComponent(5, player);
            var spriteRendererComponent = new SpriteRendererComponent(playerAnimation_Idle, player, collider);
            var abilityComponent = new AbilityComponent(contentManager);
            var hasOffSet = new HasPositionWithOfSet(player, collider, new Vector2(16, 0));
            var delayComponent = new DelayComponent();
            delayComponent.SetRange(0.4f, 0.4f);
            delayComponent.resetOnDefault = false;

            healthComponent.hurtTag = "Enemy";
            collider.CollisionEvents += (ColliderComponent colliderComponent) => healthComponent.DealDamage(colliderComponent.tag);
            collider.CollisionEventFromOther += (ColliderComponent colliderComponent) => healthComponent.DealDamage(colliderComponent.tag);
            collider.CollisionEventFromOther += (ColliderComponent colliderComponent) => {
                if (colliderComponent.tag == "Heal") {
                    healthComponent.Heal(1);
                }
            };
            collider.CollisionEventFromOther += (ColliderComponent colliderComponent) => {
                if (colliderComponent.tag == "Mana") {
                    abilityComponent.currentExp++;
                }
            };

            healthComponent.DeathEvent += collider.RemoveCollider;
            healthComponent.DeathEvent += spriteRendererComponent.OnDestroy;
            //health.DeathEvent += SurvivalArenaGame.GameOver;
            healthComponent.DeathEvent += () => gameOverSound.Play();
            healthComponent.HitEvent += () => hitSound.Play();


            var stateComponent = new StateComponent();
            var idleState = new TransitionState(stateComponent);
            var walkRightState = new TransitionState(stateComponent);
            var walkLeftState = new TransitionState(stateComponent);
            var attackState = new TransitionState(stateComponent);
            var jumpState = new TransitionState(stateComponent);

            var requirementLeft = new RequirementDelegate();
            requirementLeft.Add(() => inputComponent.APressed);
            var walkLeftTransition = new Transition(requirementLeft, walkLeftState);
            walkLeftState.OnActEvent += (float tick) => physicsComponent.SetMovementSpeedNegative();
            walkLeftState.OnEnterEvent += () => spriteRendererComponent.ChangeAnimation(playerAnimation_Walk);

            var requirementRight = new RequirementDelegate();
            requirementRight.Add(() => inputComponent.DPressed);
            var walkRightTransition = new Transition(requirementRight, walkRightState);
            walkRightState.OnActEvent += (float tick) => physicsComponent.SetMovementSpeedPositive();
            walkRightState.OnEnterEvent += () => spriteRendererComponent.ChangeAnimation(playerAnimation_Walk);

            var requirementJump = new RequirementDelegate();
            requirementJump.Add(() => inputComponent.SpacePressed);
            var JumpTransition = new Transition(requirementJump, jumpState);
            jumpState.OnEnterEvent += physicsComponent.Jump;
            jumpState.OnEnterEvent += () => jumpSound.Play();

            var requirement2 = new RequirementDelegate();
            requirement2.Add(() => !inputComponent.APressed && !inputComponent.DPressed);
            var idleTransition = new Transition(requirement2, idleState);
            idleState.OnEnterEvent += () => spriteRendererComponent.ChangeAnimation(playerAnimation_Idle);


            var requirementSword = new RequirementDelegate();
            requirementSword.Add(() => inputComponent.AttackPressed && delayComponent.swordTimeCounter <= 0);
            var swordTransition = new Transition(requirementSword, attackState);
            //attackState.OnEnterEvent += swordComponent.CreateSword;
            attackState.OnEnterEvent += () => SpawnPlayerProjectile(contentManager, hasOffSet, collider, abilityComponent);
            attackState.OnEnterEvent += () => delayComponent.Reset();
            idleState.transitions.Add(walkRightTransition);
            idleState.transitions.Add(walkLeftTransition);
            idleState.transitions.Add(JumpTransition);
            idleState.transitions.Add(swordTransition);

            walkRightState.transitions.Add(idleTransition);
            walkRightState.transitions.Add(walkLeftTransition);
            walkRightState.transitions.Add(JumpTransition);
            walkRightState.transitions.Add(swordTransition);

            walkLeftState.transitions.Add(idleTransition);
            walkLeftState.transitions.Add(walkRightTransition);
            walkLeftState.transitions.Add(JumpTransition);
            walkLeftState.transitions.Add(swordTransition);

            jumpState.transitions.Add(walkRightTransition);
            jumpState.transitions.Add(walkLeftTransition);
            jumpState.transitions.Add(idleTransition);
            jumpState.transitions.Add(swordTransition);

            attackState.transitions.Add(walkRightTransition);
            attackState.transitions.Add(walkLeftTransition);
            attackState.transitions.Add(JumpTransition);
            attackState.transitions.Add(idleTransition);

            stateComponent.TransitionToState(idleState);

            player.AddComponent(stateComponent);
            player.AddComponent(physicsComponent);
            player.AddComponent(inputComponent);
            //player.AddComponent(swordComponent);
            player.AddComponent(healthComponent);
            player.AddComponent(spriteRendererComponent);
            player.AddComponent(delayComponent);
            player.AddComponent(abilityComponent);

            HealthUIManager healthUIManager = new HealthUIManager(contentManager, healthComponent, player);


            SpriteRendererComponent.spriteRendererComponents.Add(healthUIManager);
            PoolManager.gameObjects.Add(player);

            var sliderBG = contentManager.Load<Texture2D>("LevelBar_BG");
            var sliderFG = contentManager.Load<Texture2D>("LevelBar_FG");


            var powerLevel = new EESlider(sliderFG, sliderBG, Vector2.Zero, abilityComponent);

            SpriteRendererComponent.spriteRendererComponents.Add(powerLevel);
            PoolManager.gameObjects.Add(powerLevel);
        }
        public static void SpawnPlayerProjectile(ContentManager contentManager, IHasPosition spawnPosition, IHasFacingDirection hasFacingDirection, AbilityComponent abilityComponent) {
            var bombAnimation = new SpriteAnimation(contentManager, "SporeBolt", 32, 32);

            var mpos = new InputComponent().ShootDirection(spawnPosition.Position);
            hasFacingDirection.LookingRight = mpos.X > 0;

            var position = spawnPosition.Position;
            GameObject spawner2 = new GameObject(position);
            var collider = new ColliderComponent(spawner2, 32, 32);
            collider.tag = "Sword";
            var physicsComponent = new PhysicsComponent(spawner2, collider);
            physicsComponent.gravity = 0;
            physicsComponent.moveSpeed = abilityComponent.projectileMovespeed;
            var state = new State();

            state.OnEnterEvent += () => physicsComponent.ADMovement(mpos);
            var stateComponent = new StateComponent();
            stateComponent.TransitionToState(state);
            var poolableComponent = new PoolableComponent(spawner2);
            var spriteRendererComponent = new SpriteRendererComponent(bombAnimation, spawner2, collider);

            var delayComponent = new DelayComponent();
            delayComponent.SetRange(abilityComponent.projectileLifeTime, abilityComponent.projectileLifeTime);
            delayComponent.Reset();
            delayComponent.SwordAttack += () => {
                poolableComponent.ReleaseSelf();
                spriteRendererComponent.OnDestroy();
                collider.RemoveCollider();
            };

            spawner2.AddComponent(physicsComponent);
            spawner2.AddComponent(stateComponent);
            spawner2.AddComponent(spriteRendererComponent);
            spawner2.AddComponent(delayComponent);

            collider.CollisionEvents += (ColliderComponent x) => {
                if (x.tag == "Wall" || x.tag == "Enemy") {
                    collider.RemoveCollider();
                    poolableComponent.ReleaseSelf();
                    spriteRendererComponent.OnDestroy();
                }
            };
            PoolManager.gameObjects.Add(spawner2);
        }
    }
}