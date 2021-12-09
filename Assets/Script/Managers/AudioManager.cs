using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : Singleton<AudioManager> {
    [SerializeField]
    private AudioSource mainAudioSource;

    [SerializeField]
    private AudioClip collectAudio;

    [SerializeField]
    private AudioClip deathAudio;

    private void Start() {
        DontDestroyOnLoad(gameObject);
        SceneManager.LoadScene("MainMenu");
        mainAudioSource.volume = SaveManager.Instance.LoadVolume();
    }

    public void PlayCollectAudio() {
        mainAudioSource.PlayOneShot(collectAudio);
    }

    public void PlayDeathAudio() {
        mainAudioSource.PlayOneShot(deathAudio);
    }

    public void SetAudioVolume(float audioVolume) {
        mainAudioSource.volume = audioVolume;
    }

    public float GetAudioVolume() {
        return mainAudioSource.volume;
    }
}
