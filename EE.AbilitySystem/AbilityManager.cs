using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using SurvivalArena.GameObjects;

namespace EE.AbilitySystem {
    public class AbilityComponent : IComponent, IClampedValue {
        public int currentExp;
        public int maxExp = 10;

        public int projectileMovespeed = 350;
        public float projectileLifeTime = 0.15f;

        public SoundEffect powerUp;

        public AbilityComponent(ContentManager contentManager) {
            powerUp = contentManager.Load<SoundEffect>("Powerup");

        }

        public int Max => maxExp;

        public int Value => currentExp;

        public void Update(float gameTime) {
            if (currentExp >= maxExp) {
                projectileLifeTime += 0.15f;
                currentExp = 0;
                powerUp.Play();
            }
        }
    }

}