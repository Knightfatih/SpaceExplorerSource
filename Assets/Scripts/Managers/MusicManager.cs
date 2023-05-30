using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Audio
{
    public class MusicManager : MonoBehaviour
    {
        [SerializeField] public AudioClip[] musicTracks;

        private AudioSource audioSource;
        private int currentTrackIndex = 0;

        private static MusicManager instance;

        public static MusicManager Instance => instance;

        void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }

            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public void Start()
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.loop = false;

            PlayNextTrack();
        }

        public void PlayNextTrack()
        {
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }

            audioSource.clip = musicTracks[currentTrackIndex];
            audioSource.Play();

            currentTrackIndex = (currentTrackIndex + 1) % musicTracks.Length;
        }

        public void Update()
        {
            if (!audioSource.isPlaying)
            {
                PlayNextTrack();
            }
        }

        public void SetVolume(float volume)
        {
            audioSource.volume = Mathf.Clamp01(volume);
        }
    }
}
