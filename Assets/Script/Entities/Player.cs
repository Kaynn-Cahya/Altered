using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(CharacterController2D))]
public class Player : Singleton<Player> {
    // TODO: Refactor Dash controllers into it's own script

    [SerializeField]
    private int PLAYER_LAYER = 7;

    [SerializeField]
    private int ENEMY_LAYER = 8;

    [SerializeField]
    private bool immune;

    [SerializeField, Tooltip("Sprites for the hat on the player")]
    private SpriteRenderer[] hatsSprite;

    [SerializeField]
    private GameObject deathParticle;

    [SerializeField]
    private float moveSpeed;

    [SerializeField]
    private float dashingSpeed;

    [SerializeField]
    private float dashDuration;

    [SerializeField]
    private float dashCooldown;

    [SerializeField]
    private TextMeshPro dashReadyText;

    [SerializeField]
    private Transform groundChecker;

    public ColorEnum CurrentColour {
        get; private set;
    }

    public Vector2 CurrentPosition {
        get {
            return transform.position;
        }
    }

    private CharacterController2D controller;
    private SpriteRenderer playerSprite;
    private Rigidbody2D playerRigidbody;

    private float xMovement;
    private bool jumping;
    private Vector3 size;

    private bool isDashing;
    private int lastMoveDirection;
    private Timer dashTimer;
    private float gravityScale;
    private Timer dashCooldownTimer;
    private bool canDash;

    private void Start() {
        controller = GetComponent<CharacterController2D>();
        playerSprite = GetComponent<SpriteRenderer>();
        playerRigidbody = GetComponent<Rigidbody2D>();

        gravityScale = playerRigidbody.gravityScale;
        CurrentColour = ColorEnum.BLUE;
        SetHatColour();
        size = transform.localScale;
        isDashing = false;
        canDash = true;

        dashTimer = new Timer(dashDuration, delegate {
            ToggleDashing(false);
        });

        dashCooldownTimer = new Timer(dashCooldown, delegate {
            canDash = true;
            FlashDashReady();
            AudioManager.Instance.PlayAudio(AudioType.DASH_READY);
        });
    }

    private void Update() {

        if ((Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.L)) && canDash) {
            ToggleDashing(true);
        } else if (!canDash) {
            dashCooldownTimer.Update(Time.deltaTime);
        }

        if (isDashing) {
            xMovement = lastMoveDirection * dashingSpeed;
            dashTimer.Update(Time.deltaTime);
        } else {

            int xInput = GetXInput();

            if (xInput != 0) {
                lastMoveDirection = xInput;
            }

            xMovement = xInput * moveSpeed;
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow)) {
                jumping = true;
            }
        }
    }

    private int GetXInput() {
        int leftInput = (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) ? -1 : 0;
        int rightInput = (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) ? 1 : 0;

        return (leftInput + rightInput);
    }

    private void FixedUpdate() {
        controller.Move(xMovement * Time.fixedDeltaTime, jumping, isDashing);
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
            AudioManager.Instance.PlayAudio(AudioType.COLLECT);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {

        if (isDashing) {
            bool stopDashing = collision.gameObject.CompareTag("PlayerBorder");
            if (collision.gameObject.CompareTag("World")) {
                if (collision.gameObject.transform.position.y > groundChecker.position.y) {
                    stopDashing = true;
                }
            }

            if (stopDashing) {
                ToggleDashing(false);
            }
        }

        if (immune) {
            return;
        }

        if (collision.gameObject.CompareTag("Enemy")) {
            if (collision.gameObject.GetComponent<Enemy>().Color != CurrentColour) {
                AudioManager.Instance.PlayAudio(AudioType.DEATH);
                Die();
            }
        }
    }

    private void ToggleDashing(bool dashing) {
        isDashing = dashing;
        Physics2D.IgnoreLayerCollision(PLAYER_LAYER, ENEMY_LAYER, dashing);

        if (dashing) {
            canDash = false;
            jumping = false;
            SetPlayerAlpha(0.6f);
            playerRigidbody.gravityScale = 0f;

            dashCooldownTimer.Reset(dashCooldown);
            AudioManager.Instance.PlayAudio(AudioType.DASH);
        } else {
            SetPlayerAlpha(1f);
            playerRigidbody.gravityScale = gravityScale;
        }

        dashTimer.Reset(dashDuration);

        #region Local_Function
        void SetPlayerAlpha(float alpha) {
            var color = playerSprite.color;
            color.a = alpha;
            playerSprite.color = color;
        }
        #endregion
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

    private void FlashDashReady() {
        dashReadyText.LeanAlphaText(1f, 0.1f).setOnComplete(FadeBack);
        #region Local_Function
        void FadeBack() {
            dashReadyText.LeanAlphaText(0f, 1f).setEaseLinear();
        }
        #endregion
    }
}
