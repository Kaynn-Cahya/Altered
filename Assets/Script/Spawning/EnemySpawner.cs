using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : Spawner {

    [SerializeField, Range(0, 100)]
    private float startSpawnChance;

    protected override bool BeforeStart() {
        float threshold;
        if (Random.Range(0, 100) <= startSpawnChance) {
            threshold = 2;
        } else {
            threshold = Random.Range(minSpawnTime, maxSpawnTime);
        }

        timer = new Timer(threshold, delegate {
            Spawn();
            ResetTimer();
        });
        timer.Start();

        return true;
    }
}
