using SurvivalArena.GameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EE.ScoreSystem {
    public class ScoreComponent : IComponent {
        public int scoreToGive = 10;
        public int scoreModifier = 1;
        public float subtractDelay = 1;
        public float subtractDelayTimer = 0;
        public ScoreComponent(int scoreToGive, int scoreModifier) {
            this.scoreToGive = scoreToGive;
            this.scoreModifier = scoreModifier;
        }

        public void Update(float gameTime) {
            if (subtractDelayTimer > 0) {
                subtractDelayTimer -= gameTime;

            }
            if (subtractDelayTimer <= 0) {
                scoreToGive -= scoreModifier;
                subtractDelayTimer = subtractDelay;
            }
        }
        public void AddScore() {
            ScoreManager.IncreaseScore(scoreToGive);
        }
    }
}
