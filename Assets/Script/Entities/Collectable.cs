using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : SpawnableObject {

    [SerializeField]
    private float lifetime;

    [SerializeField]
    private float floatingOffset;

    [SerializeField]
    private ParticleSystem particleEmitter;

    private float currLifetime;

    SpriteRenderer spriteRenderer;

    public ColorEnum Color {
        get; private set;
    }

    public void Start() {
        currLifetime = 0;

        LeanTween.moveY(gameObject, transform.position.y + floatingOffset, 1f).setEaseInOutSine().setLoopPingPong();
    }

    public void Update() {
        currLifetime += Time.deltaTime;

        UpdateFade();
        if (lifetime <= currLifetime) {
            Destroy(gameObject);
        }

        #region Local_Function
        void UpdateFade() {
            var currColor = spriteRenderer.color;
            currColor.a = Mathf.Lerp(1f, 0.15f, currLifetime / lifetime);
            spriteRenderer.color = currColor;
            particleEmitter.startColor = currColor;
        }
        #endregion
    }

    public override void Initalize(object[] args) {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (TryGetColourFromArgs(out ColorEnum argColour)) {
            Color = argColour;
        } else {
            Color = ColorEnum.BLUE;
            Debug.LogError("Collectable.cs :: Initalize() :: Color is not passed in as Arg.");
        }
        SetColor();

        #region Local_Function

        bool TryGetColourFromArgs(out ColorEnum colorEnum) {
            colorEnum = ColorEnum.ORANGE;

            foreach (var arg in args) {
                if (typeof(ColorEnum).IsAssignableFrom(arg.GetType())) {
                    colorEnum = (ColorEnum)arg;
                    return true;
                }
            }

            return colorEnum != ColorEnum.ORANGE;
        }

        #endregion
    }

    private void SetColor() {
        var color = Color.GetColor();
        spriteRenderer.color = color;
        particleEmitter.startColor = color;
    }
}
