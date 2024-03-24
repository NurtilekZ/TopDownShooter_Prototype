using UnityEngine;

namespace _current.Systems.WeaponSystem.Configs
{
    [CreateAssetMenu(menuName = "Guns/Audio Config", fileName = "Audio Config", order = 5)]
    public class AudioConfiguration : ScriptableObject
    {
        [Range(0, 1f)] 
        public float Volume = 1f;
        public AudioClip[] FireClips;
        public AudioClip EmptyClip;
        public AudioClip ReloadClip;
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
    }
}