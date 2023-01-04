using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;

namespace EE.SoundSystem {
    public class BackgroundMusicController {
        private Song music;

        float[] volumeSettings = new float[] { 0,0.1f, 0.15f,0.2f,0.25f};
        int currentSetting = 0;
        public BackgroundMusicController(ContentManager contentManager) {
            music = contentManager.Load<Song>("BGMusic");
        }
        public void Play() {
            if (currentSetting >= volumeSettings.Length) {
                currentSetting = 0;
            }
            var volume = volumeSettings[currentSetting];
            MediaPlayer.Play(music);
            MediaPlayer.Volume = volume;
            MediaPlayer.IsRepeating = true;
            currentSetting++;
        }
        public void ChangeVolume() {
            if (currentSetting >= volumeSettings.Length) {
                currentSetting = 0;
            }
            var volume = volumeSettings[currentSetting];
            MediaPlayer.Volume = volume;
            currentSetting++;
        }
    }
}