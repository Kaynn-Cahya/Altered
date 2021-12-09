using UnityEngine;

public enum ColourEnum {
    PINK,
    BLUE,
    GREEN
}

public class Colour {
    public const string PINK_HEX = "#F9B9C3";
    public const string BLUE_HEX = "#304C89";
    public const string GREEN_HEX = "#7DDE92";

    public ColourEnum CurrentColor {
        get; set;
    }

    public Colour(ColourEnum colorEnum) {
        CurrentColor = colorEnum;
    }

    public static Colour RandColour() {
        int selected = Random.Range(0, 3);
        return new Colour((ColourEnum) selected);
    }

    public Color GetUnityColor() {
        ColorUtility.TryParseHtmlString(GetHexColor(), out Color color);
        return color;
    }

    public string GetHexColor() {
        if (CurrentColor == ColourEnum.PINK) {
            return PINK_HEX;
        } else if (CurrentColor == ColourEnum.BLUE) {
            return BLUE_HEX;
        } else {
            return GREEN_HEX;
        }
    }

    public bool ColourMatch(Colour colour) {
        return colour.CurrentColor == CurrentColor;
    }
}
