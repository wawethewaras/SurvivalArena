using SurvivalArena.GameObjects;

namespace SurvivalArena.HealthSystem {
    public class HealthComponent : IComponent {
        public int maxHealth = 10;
        public int health = 10;
        public float invurnableDuration = 1;
        public float invurnableDurationTimer = 0;
        public GameObject gameObject;
        public string hurtTag = "None";

        public event Action HitEvent;
        public event Action DeathEvent;

        public HealthComponent(int health, GameObject gameObject) {
            this.maxHealth = health;
            this.health = health;
            this.gameObject = gameObject;
        }

        public void DealDamage(string tag) {
            if (invurnableDurationTimer <= 0 && tag == hurtTag) {
                health--;
                HitEvent?.Invoke();
                invurnableDurationTimer = invurnableDuration;
                if (health <= 0) {
                    DeathEvent?.Invoke();
                }
            }
        }

        public void Update(float gameTime) {
            if (invurnableDurationTimer > 0) {
                invurnableDurationTimer -= gameTime;
            }
        }

        public void Heal(int value) {
            health += value;
            if (health > maxHealth) {
                health = maxHealth;
            }
        }
    }
}
