using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableSpawner : Spawner {
    [SerializeField, Tooltip("Particle to warn spawning")]
    private GameObject spawnWarningParticle;

    [SerializeField]
    private Transform[] spawnPoints;

    protected override bool BeforeStart() {
        timer = new Timer(1, delegate {
            Spawn();
        });
        timer.Start();

        return true;
    }

    protected override void Spawn() {
        List<Colour> avaibleColours = getNonPlayerColours();

        // Spawnpoint 1 shouldn't be same as spawnpoint 2
        int spawnPoint1 = Random.Range(0, spawnPoints.Length);
        int spawnPoint2;
        do {
            spawnPoint2 = Random.Range(0, spawnPoints.Length);
        } while (spawnPoint2 == spawnPoint1);

        StartCoroutine(StartSpawning(avaibleColours[0], spawnPoints[spawnPoint1].position));
        StartCoroutine(StartSpawning(avaibleColours[1], spawnPoints[spawnPoint2].position));

        #region Local_Function

        static List<Colour> getNonPlayerColours() {
            List<Colour> nonPlayerColours = new List<Colour>() {
                new Colour(ColourEnum.BLUE),
                new Colour(ColourEnum.GREEN),
                new Colour(ColourEnum.PINK)
            };
            nonPlayerColours.RemoveAll(x => x.ColourMatch(Player.Instance.CurrentColour));
            return nonPlayerColours;
        }

        #endregion
    }

    private IEnumerator StartSpawning(Colour colour, Vector2 point) {
        var warningParticle = Instantiate(spawnWarningParticle);
        warningParticle.transform.position = point;

        Color color = colour.GetUnityColor();
        color.a = 0.7f;
        warningParticle.GetComponent<ParticleSystem>().startColor = color;

        yield return new WaitForSeconds(1);

        SpawnableObject collectable = Instantiate(spawnPrefab);
        collectable.transform.position = point;
        collectable.Initalize(new object[] { colour });
        ResetTimer();
    }

}
