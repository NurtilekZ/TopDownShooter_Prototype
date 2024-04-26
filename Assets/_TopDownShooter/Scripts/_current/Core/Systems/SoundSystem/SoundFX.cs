using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _current.Core.Systems.SoundSystem
{
    [CreateAssetMenu(menuName = "Sound", fileName = "SoundLibrary")]
    public class SoundFX : ScriptableObject
    {
        private static readonly List<SurfaceHitSound> _surfaceHitSounds = new();

        public static void PlaySoundAtPoint(AudioClip shootSound, Vector3 point)
        {
            AudioSource.PlayClipAtPoint(shootSound, point);
        }

        public static void PlayBulletHitSoundAtPoint(int surfaceLayer, Vector3 hitPosition)
        {
            var audioClip = _surfaceHitSounds.FirstOrDefault(x => x.Layer.value == surfaceLayer)?.Sound;
            if (audioClip != null) PlaySoundAtPoint(audioClip, hitPosition);
        }
    }
}