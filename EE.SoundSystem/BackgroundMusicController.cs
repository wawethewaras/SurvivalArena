using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;

namespace EE.SoundSystem {
    public class BackgroundMusicController {
        private Song music;

        public BackgroundMusicController(ContentManager contentManager) {
            music = contentManager.Load<Song>("BGMusic");
        }

        public void Play() {
            MediaPlayer.Play(music);
            MediaPlayer.Volume = 0.00f;
            MediaPlayer.IsRepeating = true;
        }
    }
}