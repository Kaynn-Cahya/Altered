public class Timer {

    public delegate void OnTimerRing();

    private OnTimerRing onTimerRingCallback;
    private float currentTimer;
    private float threshold;
    private bool rang;

    public Timer(float threshold, OnTimerRing onTimerRingCallback) {
        this.threshold = threshold;
        this.onTimerRingCallback = onTimerRingCallback;
    }

    public void Start() {
        currentTimer = 0;
        rang = false;
    }

    public void Update(float deltaTime) {
        if (rang) {
            return;
        }

        currentTimer += deltaTime;

        if (currentTimer >= threshold) {
            rang = true;
            onTimerRingCallback.Invoke();
        }
    }

    public void Reset(float newThreshold) {
        threshold = newThreshold;
        Reset();
    }

    public void Reset() {
        currentTimer = 0;
        rang = false;
    }
}
