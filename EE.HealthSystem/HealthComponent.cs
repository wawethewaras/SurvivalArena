﻿using SurvivalArena.GameObjects;

namespace SurvivalArena.HealthSystem {
    public class HealthComponent : IComponent {

        public int health = 10;
        public float invurnableDuration = 1;
        public float invurnableDurationTimer = 0;
        public GameObject gameObject;
        public string hurtTag = "None";

        public event Action DeathEvent;

        public HealthComponent(int health, GameObject gameObject) {
            this.health = health;
            this.gameObject = gameObject;
        }

        public void DealDamage(string tag) {
            if (invurnableDurationTimer <= 0 && tag == hurtTag) {
                health--;
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
    }
}