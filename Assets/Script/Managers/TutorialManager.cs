using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : Singleton<TutorialManager>
{
    [SerializeField]
    private Text collectableAdvise;

    [SerializeField]
    private Text destroyAdvise;

    private Timer timer;

    private void Start() {
        if (GlobalVars.Instance.TutorialFinished && GlobalVars.Instance.IsRetrying) {
            return;
        }

        LeanTween.scale(collectableAdvise.gameObject, Vector3.one, 0.15f).setEaseSpring().setOnComplete(
            () => {
                timer = new Timer(5f, delegate {
                    LeanTween.scale(collectableAdvise.gameObject, Vector3.zero, 0.15f).setEaseLinear().setOnComplete(ShowDestroyAdvise);
                });
            }
            );
    }

    private void ShowDestroyAdvise() {
        LeanTween.scale(destroyAdvise.gameObject, Vector3.one, 0.15f).setEaseSpring().setOnComplete(
    () => {
        timer = new Timer(10f, delegate {
            LeanTween.scale(destroyAdvise.gameObject, Vector3.zero, 0.15f).setEaseLinear();
            GlobalVars.Instance.TutorialFinished = true;
        });
    }
    );
    }

    private void Update() {
        if (GameManager.Instance.GameOver || timer == null) {
            return;
        }

        timer.Update(Time.deltaTime);
    }

    private void TriggerGameOver() {
        collectableAdvise.gameObject.SetActive(false);
        destroyAdvise.gameObject.SetActive(false);
    }
}
