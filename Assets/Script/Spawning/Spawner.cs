using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Spawns on self by default, overridable functions
/// </summary>
public abstract class Spawner : MonoBehaviour {

    [SerializeField]
    protected SpawnableObject spawnPrefab;

    [SerializeField]
    protected float maxSpawnTime;
    [SerializeField]
    protected float minSpawnTime;

    protected Timer timer;

    private void Start() {
        if (!BeforeStart()) {
            float threshold = Random.Range(minSpawnTime, maxSpawnTime);
            timer = new Timer(threshold, delegate {
                Spawn();
                ResetTimer();
            });

            timer.Start();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns>True to override this with the default Start()</returns>
    protected virtual bool BeforeStart() {
        return false;
    }

    private void Update() {
        if (GameManager.Instance.GameOver) {
            return;
        }

        timer.Update(Time.deltaTime);
    }

    protected virtual void ResetTimer() {
        float threshold = Random.Range(minSpawnTime, maxSpawnTime);
        timer.Reset(threshold);
    }

    protected virtual void Spawn() {
        SpawnableObject spawnedObj = Instantiate(spawnPrefab);
        spawnedObj.transform.position = transform.position;
        spawnedObj.Initalize();
    }

    public virtual void AddTimerTicks(float ticks) {
        timer.Update(ticks);
    }

    public virtual void ReduceMaxThreshold(float reduction) {
        if ((maxSpawnTime - reduction - 5f) >= minSpawnTime) {
            maxSpawnTime -= reduction;
        }
    }
}
