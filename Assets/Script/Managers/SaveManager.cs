using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : Singleton<SaveManager> {
    public void SaveHighscore(int highscore) {
        PlayerPrefs.SetInt("hsvalue", highscore);
        PlayerPrefs.Save();
    }

    public int LoadHighscore() {
        return PlayerPrefs.GetInt("hsvalue");
    }

    public void SaveVolume(float volume) {
        PlayerPrefs.SetFloat("vol", volume);
        PlayerPrefs.Save();
    }

    public float LoadVolume() {
        return PlayerPrefs.GetFloat("vol", 0.5f);
    }
}
