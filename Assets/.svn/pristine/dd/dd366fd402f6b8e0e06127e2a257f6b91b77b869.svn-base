using UnityEngine;
using System.Collections;
namespace engine {
    public class DefaultSoundManager : ISoundManager {

        public void playSound(string soundName, Vector3 position) {
            if (soundName != null) {
                AudioClip clip = Engine.res.loadSound(soundName);
                if (clip != null) {
                    AudioSource.PlayClipAtPoint(clip, position);
                }
            }
        }

    }

}
