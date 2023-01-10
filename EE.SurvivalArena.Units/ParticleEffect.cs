using EE.PoolingSystem;
using EE.SpriteRendererSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using SurvivalArena.GameObjects;

namespace EE.SurvivalArena.Units {
    internal class ParticleEffect {
        public ParticleEffect(ContentManager contentManager, Vector2 spawnPosition) {
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
