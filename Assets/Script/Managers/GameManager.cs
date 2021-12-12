using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager> {

    [SerializeField]
    private CanvasGroup gameOverUI;

    [SerializeField]
    private Text gameScoreText;

    [SerializeField]
    private Text gameOverScore;

    [SerializeField]
    private Text clearScreenText;

    public int Score {
        get; private set;
    }

    public bool GameOver {
        get; private set;
    }

    private Vector3 gameScoreSize;

    private void Start() {
        gameScoreSize = gameScoreText.gameObject.transform.localScale;
        Score = 0;
        GameOver = false;
        gameOverUI.interactable = false;
        gameOverUI.blocksRaycasts = false;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.R) && GameOver) {
            Retry();
        }
    }

    public void TriggerGameOver() {
        NewgroundsManager.Instance.AddScoreToLeaderboard(Score, NewgroundsManager.BOARD_TYPE.HIGHEST_SCORE);
        NewgroundsManager.Instance.AddScoreToLeaderboard(Score, NewgroundsManager.BOARD_TYPE.TOTAL_SCORE);

        bool newHighscore = false;
        int highestScore = SaveManager.Instance.LoadHighscore();
        if (Score > highestScore) {
            newHighscore = true;
            SaveManager.Instance.SaveHighscore(Score);
        }

        gameOverScore.text = (newHighscore ? "New High" : "") + "Score: " + Score.ToString();
        GameOver = true;
        EnemyManager.Instance.TriggerGameOver();

        gameOverUI.gameObject.SetActive(true);
        LeanTween.alphaCanvas(gameOverUI, 1f, 1.5f).setEaseLinear().setOnComplete(EnableInteraction);

        #region Local_Function
        void EnableInteraction() {
            gameOverUI.interactable = true;
            gameOverUI.blocksRaycasts = true;
            gameOverFullyShown = true;
        }
        #endregion
    }

    public void AddScore(int amount) {
        TriggerScoreAchievement(Score, amount);
        Score += amount;
        gameScoreText.text = Score.ToString();
        LeanTween.scale(gameScoreText.gameObject, gameScoreSize + new Vector3(0.7f, 0.7f, 0.7f), 0.15f).setEasePunch().setOnComplete(ScaleBack);
        #region Local_Function
        void ScaleBack() {
            LeanTween.scale(gameScoreText.gameObject, gameScoreSize, 0.15f).setEasePunch();
        }
        #endregion
    }

    public void Retry() {
        GlobalVars.Instance.IsRetrying = true;
        SceneTransitionManager.Instance.ToGameScene();
    }

    public void TriggerClearScreen() {
        AddScore(10);
        NewgroundsManager.Instance.UnlockMedal(NewgroundsManager.MEDAL_TAG.CLEAR_SCREEN);
        StartCoroutine(ClearScreenCoroutine());

        IEnumerator ClearScreenCoroutine() {
            LeanTween.scale(clearScreenText.gameObject, Vector3.one, 0.1f).setEaseInBounce();

            yield return new WaitForSeconds(1.5f);

            LeanTween.scale(clearScreenText.gameObject, Vector3.zero, 0.1f).setEaseLinear();
        }
    }

    private void TriggerScoreAchievement(int originalScore, int addition) {
        int newScore = originalScore + addition;

        NewgroundsManager.MEDAL_TAG? medalTag = null;

        if (originalScore < 25 && newScore >= 25) {
            medalTag = NewgroundsManager.MEDAL_TAG.P_25;
        } else if (originalScore < 50 && newScore >= 50) {
            medalTag = NewgroundsManager.MEDAL_TAG.P_50;
        } else if (originalScore < 100 && newScore >= 100) {
            medalTag = NewgroundsManager.MEDAL_TAG.P_100;
        } else if (originalScore < 200 && newScore >= 200) {
            medalTag = NewgroundsManager.MEDAL_TAG.P_200;
        }

        if (medalTag != null) {
            NewgroundsManager.Instance.UnlockMedal((NewgroundsManager.MEDAL_TAG) medalTag);
        }
    }

}
