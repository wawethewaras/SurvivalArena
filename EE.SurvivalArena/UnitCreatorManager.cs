using EE.InputSystem;
using EE.PoolingSystem;
using EE.ScoreSystem;
using EE.SpriteRendererSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SurvivalArena.AISystem;
using SurvivalArena.ColliderSystem;
using SurvivalArena.GameObjects;
using SurvivalArena.HealthSystem;
using SurvivalArena.Physics;
using SurvivalArena.Sword;
using SurvivalArena.TileSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EE.SurvivalArena {
    public static class UnitCreatorManager {
        public static void CreateEnemySpawner(ContentManager contentManager, Vector2 position) {
            var enemyTexture = contentManager.Load<Texture2D>("Player"); 
            var spawner = new GameObjectSpawner(enemyTexture, position);

            PoolManager.gameObjects.Add(spawner);
        }

        public static void SpawnADEnemy(Texture2D texture2D, Vector2 spawnPosition) {
            GameObject spawner2 = new GameObject(spawnPosition);
            var collider = new ColliderComponent(spawner2, texture2D.Width, texture2D.Height);
            collider.tag = "Enemy";
            //spawner2.colliderComponent = collider;
            var physicsComponent = new PhysicsComponent(spawner2, collider);
            var aIComponent = new AIComponent(physicsComponent);
            var health = new HealthComponent(1, spawner2);
            var poolableComponent = new PoolableComponent(spawner2);
            var spriteRendererComponent = new SpriteRendererComponent(texture2D, spawner2, collider);
            var score = new ScoreComponent(100, 1);
            health.hurtTag = "Sword";
            spawner2.AddComponent(physicsComponent);
            spawner2.AddComponent(aIComponent);
            spawner2.AddComponent(health);
            spawner2.AddComponent(spriteRendererComponent);
            spawner2.AddComponent(score);

            collider.CollisionEvents += health.DealDamage;
            health.DeathEvent += collider.RemoveCollider;
            health.DeathEvent += poolableComponent.ReleaseSelf;
            health.DeathEvent += spriteRendererComponent.OnDestroy;
            health.DeathEvent += score.AddScore;

            PoolManager.gameObjects.Add(spawner2);
        }
        public static void CreatePlayer(ContentManager contentManager, Vector2 position) {
            var playerTexture = contentManager.Load<Texture2D>("Player");
            var swordTexture = contentManager.Load<Texture2D>("Player");

            var player = new GameObject(position);
            var collider = new ColliderComponent(player, playerTexture.Width, playerTexture.Height);
            var physicsComponent = new PhysicsComponent(player, collider);
            var inputComponent = new InputComponent();
            var swordComponent = new SwordComponent(swordTexture, player);
            var health = new HealthComponent(5, player);
            var spriteRendererComponent = new SpriteRendererComponent(playerTexture, player, collider);

            var hasOffSet = new HasPositionWithOfSet(player, collider, new Vector2(swordTexture.Width, 0));
            var swordRender = new SpriteRendererComponent(swordTexture, hasOffSet, collider);
            var swordCollider = new ColliderComponent(hasOffSet, playerTexture.Width, playerTexture.Height);
            swordCollider.tag = "Sword";
            swordCollider.DeActive();

            health.hurtTag = "Enemy";
            collider.CollisionEvents += health.DealDamage;
            health.DeathEvent += collider.RemoveCollider;
            health.DeathEvent += spriteRendererComponent.OnDestroy;

            inputComponent.DPressed += physicsComponent.SetMovementSpeedPositive;
            inputComponent.APressed += physicsComponent.SetMovementSpeedNegative;
            inputComponent.SpacePressed += physicsComponent.Jump;


            player.AddComponent(physicsComponent);
            player.AddComponent(inputComponent);
            player.AddComponent(swordComponent);
            player.AddComponent(health);
            player.AddComponent(spriteRendererComponent);

            swordComponent.SwordAttack += swordCollider.SetActive;
            swordComponent.SwordAttack += swordRender.SetActive;

            swordComponent.SwordAttackCancel += swordCollider.DeActive;
            swordComponent.SwordAttackCancel += swordRender.DeActive;

            PoolManager.gameObjects.Add(player);
        }

        public static void CreateTile(ContentManager contentManager, Vector2 position) {
            var tileTexture = contentManager.Load<Texture2D>("Tile"); ;
            var tile = new Tile();
            tile.position = position;
            var colliderComponent = new ColliderComponent(tile, tileTexture.Width, tileTexture.Height);
            colliderComponent.tag = "Wall";

            var spriteRendererComponent = new SpriteRendererComponent(tileTexture, tile);

        }
    }
}
