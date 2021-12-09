using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableSpawner : MonoBehaviour {
    [SerializeField]
    private Collectable collectablePrefab;
    [SerializeField]
    private GameObject spawnWarningParticle;

    [SerializeField]
    private float maxSpawnTime;
    [SerializeField]
    private float minSpawnTime;

    [SerializeField]
    private Transform[] spawnPoints;

    private Timer timer;

    private void Start() {

        float threshold = Random.Range(minSpawnTime, maxSpawnTime);
        timer = new Timer(1, delegate {
            Spawn();
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
        List<Colour> avaibleColours = new List<Colour>() {
            new Colour(ColourEnum.BLUE),
            new Colour(ColourEnum.GREEN),
            new Colour(ColourEnum.PINK)
        };
        avaibleColours.RemoveAll(x => x.ColourMatch(Player.Instance.CurrentColour));

        int spawnPoint1 = Random.Range(0, spawnPoints.Length);
        int spawnPoint2;
        do {
            spawnPoint2 = Random.Range(0, spawnPoints.Length);
        } while (spawnPoint2 == spawnPoint1);

        StartCoroutine(startSpawning(avaibleColours[0], spawnPoints[spawnPoint1].position));
        StartCoroutine(startSpawning(avaibleColours[1], spawnPoints[spawnPoint2].position));
    }


    private IEnumerator startSpawning(Colour colour, Vector2 point) {
        var warningParticle = Instantiate(spawnWarningParticle);
        warningParticle.transform.position = point;

        Color color = colour.GetUnityColor();
        color.a = 0.7f;
        warningParticle.GetComponent<ParticleSystem>().startColor = color;

        yield return new WaitForSeconds(1);

        Collectable collectable = Instantiate(collectablePrefab);
        collectable.transform.position = point;
        collectable.Initalize(colour);
        ResetTimer();
    }

}
