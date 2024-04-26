using UnityEngine;
using Random = UnityEngine.Random;

namespace _current.Core.Systems.WeaponSystem.Configs
{
    [CreateAssetMenu(menuName = "StaticData/WeaponConfig", fileName = "AudioConfig", order = 4)]
    public class WeaponAudioConfig : ScriptableObject
    {
        [Range(0, 1f)] 
        public float Volume = 1f;
        public AudioClip[] FireClips;
        public AudioClip EmptyClip;
        public AudioClip ReloadClip;
        public AudioClip EndReloadClip;
        public AudioClip LastBulletClip;

        public void PlayShootingClip(AudioSource audioSource, bool isLastBullet = false)
        {
            if (isLastBullet && LastBulletClip != null)
            {
                audioSource.PlayOneShot(LastBulletClip, Volume);
            }
            else
            {
                audioSource.PlayOneShot(FireClips[Random.Range(0, FireClips.Length)], Volume);
            }
        }

        public void PlayOutOfAmmoClip(AudioSource audioSource)
        {
            if (EmptyClip != null)
            {
                audioSource.PlayOneShot(EmptyClip, Volume);
            }
        }

        public void PlayReloadClip(AudioSource audioSource)
        {
            if (ReloadClip != null)
            {
                audioSource.PlayOneShot(ReloadClip, Volume);
            }
        }

        public void PlayEndReloadClip(AudioSource audioSource)
        {
            if (EndReloadClip != null)
            {
                audioSource.PlayOneShot(EndReloadClip, Volume);
            }
        }
    }
}