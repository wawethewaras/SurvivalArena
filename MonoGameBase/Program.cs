
namespace SurvivalArena {

    public class Program {
        public static void Main(string[] args) {
            using (BaseGame game = new BaseGame()) {
                game.Run();
            }
        }
    }
}