using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EE.SoundSystem {
    public class PlaySoundAction {

        private SoundEffect soundEffect;

        public PlaySoundAction(ContentManager contentManager, string soundName) {
            soundEffect = contentManager.Load<SoundEffect>(soundName);
        }

        public void Invoke() => soundEffect.Play();
    }
}
