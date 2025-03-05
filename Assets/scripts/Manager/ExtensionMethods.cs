using UnityEngine;

namespace Assets.scripts.Manager
{
    public static class ExtensionMethods {

	
        public static void PlaySound(this AudioSource source,AudioClip clip)
        {
            if (source.isPlaying) source.Stop();
            source.clip = clip;
            source.Play();
        }
    }
}
