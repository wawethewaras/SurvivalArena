namespace EE.ScoreSystem {
    public class ScoreManager {
        public static int Score = 0;

        public static void IncreaseScore(int scoreToAdd) {
            Score += scoreToAdd;
        }
    }
}