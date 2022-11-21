
namespace MonoGamePractice {

    public class Program {
        public static void Main(string[] args) {
            using (Game1 game = new Game1()) {
                game.Run();
            }
        }
    }
}