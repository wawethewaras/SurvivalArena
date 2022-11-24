using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurvivalArena {
    public class HealthComponent : IComponent {

        public int health = 10;
        public float invurnableDuration = 1;
        public float invurnableDurationTimer = 0;
        public GameObject gameObject;

        public HealthComponent(int health, GameObject gameObject) {
            this.health = health;
            this.gameObject = gameObject;
        }

        public void DealDamage(ColliderComponent colliderComponent) {
            if (invurnableDurationTimer <= 0 && colliderComponent.tag == "Sword") {
                health--;
                invurnableDurationTimer = invurnableDuration;
                if (health <= 0) {
                    Level.gameObjects.Remove(gameObject);
                    ColliderComponent.ColliderComponents.Remove(gameObject.colliderComponent);
                }
            }
        }

        public void Update(float gameTime) {

        }
    }
}
