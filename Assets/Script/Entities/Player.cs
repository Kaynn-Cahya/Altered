using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController2D))]
public class Player : Singleton<Player> {

    [SerializeField]
    private bool immune;

    [SerializeField, Tooltip("Sprites for the hat on the player")]
    private SpriteRenderer[] hatsSprite;

    [SerializeField]
    private GameObject deathParticle;

    [SerializeField]
    private float moveSpeed;

    public ColorEnum CurrentColour {
        get; private set;
    }

    public Vector2 CurrentPosition {
        get {
            return transform.position;
        }
    }

    private CharacterController2D controller;

    private float xMovement;
    private bool jumping;
    private Vector3 size;

    private void Start() {
        controller = GetComponent<CharacterController2D>();

        CurrentColour = ColorEnum.BLUE;
        SetHatColour();
        size = transform.localScale;
    }

    private void Update() {
        float leftInput = (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) ? -1 : 0;
        float rightInput = (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) ? 1 : 0;

        xMovement = (leftInput + rightInput) * moveSpeed;

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow)) {
            jumping = true;
        }
    }

    private void FixedUpdate() {
        controller.Move(xMovement * Time.fixedDeltaTime, jumping);
        jumping = false;
    }

    private void SetHatColour() {
        UnityEngine.Color color = CurrentColour.GetColor();
        foreach (var sprite in hatsSprite) {
            sprite.color = color;
        }
    }


    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Collectable")) {
            GameManager.Instance.AddScore(5);
            CurrentColour = collision.GetComponent<Collectable>().Color;
            SetHatColour();
            Destroy(collision.gameObject);
            EnemyManager.Instance.TriggerPlayerColourChanged();
            AudioManager.Instance.PlayCollectAudio();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (immune) {
            return;
        }

        if (collision.gameObject.CompareTag("Enemy")) {
            if (collision.gameObject.GetComponent<Enemy>().Color != CurrentColour) {
                AudioManager.Instance.PlayDeathAudio();
                Die();
            }
        }
    }

    public void Squash() {
        LeanTween.scale(gameObject, new Vector2(size.y, size.x), 0.1f).setOnComplete(ScaleBack);
        #region Local_Function
        void ScaleBack() {
            LeanTween.scale(gameObject, new Vector2(size.x, size.y), 0.1f);
        }
        #endregion
    }

    private void Die() {
        NewgroundsManager.Instance.UnlockMedal(NewgroundsManager.MEDAL_TAG.DIE);
        GameManager.Instance.TriggerGameOver();
        SpawnDeathParticle();
        Destroy(gameObject);
    }

    private void SpawnDeathParticle() {
        GameObject particle = Instantiate(deathParticle);
        particle.transform.position = transform.position;

        var color = ColorEnum.ORANGE.GetColor();
        color.a = 0.6f;
        particle.GetComponent<ParticleSystem>().startColor = color;
    }
}
