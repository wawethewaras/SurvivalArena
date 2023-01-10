using EE.AbilitySystem;
using EE.HealthSystem;
using EE.InputSystem;
using EE.PoolingSystem;
using EE.ScoreSystem;
using EE.SoundSystem;
using EE.SpriteRendererSystem;
using EE.StateSystem;
using MainMenuSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using EE.CollisionSystem;
using SurvivalArena.GameObjects;
using SurvivalArena.HealthSystem;
using SurvivalArena.Physics;
using SurvivalArena.TileSystem;

namespace EE.SurvivalArena.Units {
    public class ManaGem {
        public ManaGem(ContentManager contentManager, Vector2 spawnPosition) {
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
            delayComponent.SetRange(2, 2);
            delayComponent.Reset();
            delayComponent.SwordAttack += () => {
                poolableComponent.ReleaseSelf();
                spriteRendererComponent.OnDestroy();

            };

            spawner2.AddComponent(spriteRendererComponent);
            spawner2.AddComponent(physicsComponent);
            spawner2.AddComponent(delayComponent);
            spawner2.AddComponent(collider);

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
    }
}
