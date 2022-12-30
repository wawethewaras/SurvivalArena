using Microsoft.Xna.Framework;

namespace EE.FileManagement {
    public static class FileReader {

        public static void ReadFile(string filepath, Action<string, int> action) {
            using Stream fileStream = TitleContainer.OpenStream(filepath);
            using StreamReader reader = new StreamReader(fileStream);

            string line = reader.ReadLine();
            int currentLine = 0;
            while (line != null) {
                action.Invoke(line, currentLine);
                line = reader.ReadLine();
                currentLine++;
            }
        }
    }
}