using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : Singleton<EnemyManager> {

    private HashSet<Enemy> existingEnemies;

    [SerializeField]
    private Spawner[] enemySpawners;

    private void Start() {
        existingEnemies = new HashSet<Enemy>();
    }

    public void AddEnemy(Enemy enemy) {
        existingEnemies.Add(enemy);
    }

    public void RemoveEnemy(Enemy enemy) {
        existingEnemies.Remove(enemy);

        if (existingEnemies.Count == 0) {
            GameManager.Instance.TriggerClearScreen();


            foreach (var spawner in enemySpawners) {
                spawner.AddTimerTicks(Random.Range(1f, 5f));
                spawner.ReduceMaxThreshold(0.1f);
            }
        }
    }

    public void TriggerPlayerColourChanged() {
        foreach (var enemy in existingEnemies) {
            enemy.ToggleFollow();
        }
    }

    public void TriggerGameOver() {
        foreach (var enemy in existingEnemies) {
            enemy.Stop();
        }
    }
}
