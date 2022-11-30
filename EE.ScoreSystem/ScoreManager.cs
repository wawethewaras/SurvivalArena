using System.Xml.Serialization;

namespace EE.ScoreSystem {
    public class ScoreManager {
        public static List<Score> Highscores = new List<Score>();
        private static string _fileName = "scores.xml";

        public static int Score = 0;

        public static void IncreaseScore(int scoreToAdd) {
            Score += scoreToAdd;
        }
        public static void SaveCurrentScore() {
            Highscores.Add(new Score() { number = Score, Name = "Player" });
            Highscores = Highscores.OrderByDescending(x => x.number).ToList();
        }

        public static void LoadHighScore() {
            if (File.Exists(_fileName)) {
                using (var reader = new StreamReader(new FileStream(_fileName, FileMode.Open))) {
                    var serilizer = new XmlSerializer(typeof(List<Score>));
                    Highscores = serilizer.Deserialize(reader) as List<Score>;
                }
            }
        }
        public static void StoreScore() {
            using (var writer = new StreamWriter(new FileStream(_fileName, FileMode.Create))) {
                var serilizer = new XmlSerializer(typeof(List<Score>));

                serilizer.Serialize(writer, Highscores);
            }
        }
    }
    public class Score {
        public string Name;
        public int number;
    }
}