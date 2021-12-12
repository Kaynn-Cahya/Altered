using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : Singleton<AudioManager> {

    [SerializeField]
    private AudioSource mainAudioSource;

    [System.Serializable]
    private struct AudioTypeClip {
        [SerializeField]
        private AudioType audioType;

        [SerializeField]
        private AudioClip audioClip;

        public AudioType AudioType {
            get => audioType;
        }
        public AudioClip AudioClip {
            get => audioClip;
        }
    }

    [SerializeField]
    private AudioTypeClip[] audioCollection;

    private void Start() {
        DontDestroyOnLoad(gameObject);
        SceneManager.LoadScene("MainMenu");
        mainAudioSource.volume = SaveManager.Instance.LoadVolume();
    }

    public void PlayAudio(AudioType audioType) {
        foreach (var audio in audioCollection) {
            if (audio.AudioType == audioType) {
                mainAudioSource.PlayOneShot(audio.AudioClip);
                return;
            }
        }
    }

    public void SetAudioVolume(float audioVolume) {
        mainAudioSource.volume = audioVolume;
    }

    public float GetAudioVolume() {
        return mainAudioSource.volume;
    }
}
