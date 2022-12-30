using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using SurvivalArena.GameObjects;

namespace EE.AbilitySystem {
    public class AbilityComponent : IComponent{
        public int currentExp;

        public int projectileMovespeed = 350;
        public float projectileLifeTime = 0.15f;

        public SoundEffect powerUp;

        public AbilityComponent(ContentManager contentManager) {
            powerUp = contentManager.Load<SoundEffect>("Powerup");

        }
        public void Update(float gameTime) {
            if (currentExp >= 10) {
                projectileLifeTime += 0.5f;
                currentExp = 0;
                powerUp.Play();
            }
        }
    }
}