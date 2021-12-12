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

    public Colour Colour {
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

        if (TryGetColourFromArgs(out Colour argColour)) {
            Colour = argColour;
        } else {
            Colour = new Colour(ColourEnum.BLUE);
            Debug.LogError("Collectable.cs :: Initalize() :: Colour is not passed in as Arg.");
        }
        SetColor();

        #region Local_Function

        bool TryGetColourFromArgs(out Colour colour) {
            colour = null;

            foreach (var arg in args) {
                if (arg as Colour != null) {
                    colour = arg as Colour;
                    return true;
                }
            }

            return colour != null;
        }

        #endregion
    }

    private void SetColor() {
        if (ColorUtility.TryParseHtmlString(Colour.GetHexColor(), out Color color)) {
            spriteRenderer.color = color;
            particleEmitter.startColor = color;
        } else {
            Debug.LogError("Collectable.cs :: 'SetColour()' color is invalid");
        }
    }
}
