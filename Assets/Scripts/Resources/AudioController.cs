using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Resources {
    public class AudioController : MonoBehaviour {

        [SerializeField] private AudioClip[] dribbleSounds;
        [SerializeField] private AudioClip[] netSounds;
        [SerializeField] private AudioClip[] grabSounds;

        private AudioSource[] sources;

        private void Start() {
            sources = GetComponents<AudioSource>();
        }

        private void OnEnable() {
            sources ??= GetComponents<AudioSource>();
        }

        public void playDribbleSound() {
            AudioSource source = sources[0];
            
            source.clip = dribbleSounds[Random.Range(0, dribbleSounds.Length)];
            source.Play();
        }
        
        public void playNetSound() {
            AudioSource source = sources[1];
            
            source.clip = netSounds[Random.Range(0, netSounds.Length)];
            source.Play();
        }

        public void playGrabSound() {
            AudioSource source = sources[2];
            
            source.clip = grabSounds[Random.Range(0, grabSounds.Length)];
            source.Play();
        }
    }
}
