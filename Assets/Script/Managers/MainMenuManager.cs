using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : Singleton<MainMenuManager> {
    [SerializeField]
    private Text highscoreText;

    [SerializeField]
    private Slider volumeSlider;

    // Start is called before the first frame update
    void Start() {
        var highscore = SaveManager.Instance.LoadHighscore();
        highscoreText.text = "Highscore: " + highscore;

        volumeSlider.value = AudioManager.Instance.GetAudioVolume();

        volumeSlider.onValueChanged.AddListener(newVol => {
            AudioManager.Instance.SetAudioVolume(newVol);
        });
    }

}
