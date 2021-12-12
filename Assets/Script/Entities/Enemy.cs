using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController2D))]
public class Enemy : SpawnableObject {

    [SerializeField]
    private float speed;

    [SerializeField]
    private GameObject deathParticle;

    private Player player;

    public bool FollowPlayer {
        get; private set;
    }

    public ColorEnum Color {
        get; private set;
    }

    private SpriteRenderer spriteRenderer;
    private CharacterController2D controller;

    private float xMovement;
    private bool jumping;
    private Vector3 size;

    private void Update() {
        if (FollowPlayer && !GameManager.Instance.GameOver) {
            xMovement = (transform.position.x > player.CurrentPosition.x ? -1 : 1) * speed;
        }
    }

    private void FixedUpdate() {
        controller.Move(xMovement * Time.fixedDeltaTime, jumping);
        jumping = false;
    }

    public override void Initalize(object[] args) {
        controller = GetComponent<CharacterController2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        jumping = false;

        this.player = Player.Instance;
        this.Color = GenerateColour();
        SetColor();
        ToggleFollow();
        size = transform.localScale;

        EnemyManager.Instance.AddEnemy(this);

        #region Local_Function

        ColorEnum GenerateColour() {
            ColorEnum color;
            do {
                color = Utils.RandColorEnum();
            } while (color == player.CurrentColour);
            return color;
        }

        #endregion
    }

    private void SetColor() {

        spriteRenderer.color = Color.GetColor();

    }

    public void ToggleFollow() {
        if (player.CurrentColour == Color) {
            FollowPlayer = false;
            xMovement = 0;
            jumping = false;
        } else {
            FollowPlayer = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (!FollowPlayer) {
            return;
        }

        if (collision.gameObject.CompareTag("JumpTrigger")) {
            if ((transform.position.y - 0.05f) < player.CurrentPosition.y && Random.Range(0, 100) >= 20) {
                jumping = true;
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision) {
        CollisionTriggered(collision);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        CollisionTriggered(collision);
    }

    private void CollisionTriggered(Collision2D collision) {
        var playerColour = player.CurrentColour;

        if (collision.gameObject.CompareTag("Player")) {
            if (playerColour == Color) {
                AudioManager.Instance.PlayAudio(AudioType.DEATH);
                Die();
            }
        } else if (collision.gameObject.CompareTag("Enemy")) {
            if (playerColour == Color) {
                return;
            }
            var otherEnemy = collision.gameObject.GetComponent<Enemy>();
            // Other enemy is same as player, but this is different from player.
            if (otherEnemy.Color == playerColour) {
                AudioManager.Instance.PlayAudio(AudioType.DEATH);
                otherEnemy.Die();
                Die();
            }
        }
    }

    public void Stop() {
        xMovement = 0;
        FollowPlayer = false;
    }

    private void Die() {
        GameManager.Instance.AddScore(1);
        SpawnDeathParticle();
        EnemyManager.Instance.RemoveEnemy(this);
        Destroy(gameObject);
    }

    public void Squash() {
        LeanTween.scale(gameObject, new Vector2(size.y, size.x), 0.1f).setOnComplete(ScaleBack);
        #region Local_Function
        void ScaleBack() {
            LeanTween.scale(gameObject, new Vector2(size.x, size.y), 0.1f);
        }
        #endregion
    }

    private void SpawnDeathParticle() {
        GameObject particle = Instantiate(deathParticle);
        particle.transform.position = transform.position;

        var color = Color.GetColor();
        color.a = 0.6f;
        particle.GetComponent<ParticleSystem>().startColor = color;
    }
}
