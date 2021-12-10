using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    [SerializeField]
    private SpawnableObject spawnPrefab;

    [SerializeField]
    private float maxSpawnTime;
    [SerializeField]
    private float minSpawnTime;

    private Timer timer;

    private void Start() {
        float threshold;
        int startingSpawnOrNot = Random.Range(0, 100);
        if (startingSpawnOrNot > 35) {
            threshold = 3f;
        } else {
            threshold = Random.Range(minSpawnTime, maxSpawnTime);
        }
        
        timer = new Timer(threshold, delegate {
            Spawn(); ResetTimer();
        });

        timer.Start();
    }

    private void Update() {
        if (GameManager.Instance.GameOver) {
            return;
        }

        timer.Update(Time.deltaTime);
    }

    private void ResetTimer() {
        float threshold = Random.Range(minSpawnTime, maxSpawnTime);
        timer.Reset(threshold);
    }

    private void Spawn() {
        SpawnableObject spawnedObj = Instantiate(spawnPrefab);
        spawnedObj.transform.position = transform.position;
        spawnedObj.Initalize();
    }

    public void AddTimerTicks(float ticks) {
        timer.Update(ticks);
    }

    public void ReduceMaxThreshold(float reduction) {
        if ((maxSpawnTime - reduction - 5f) >= minSpawnTime) {
            maxSpawnTime -= reduction;
        }
    }
}
