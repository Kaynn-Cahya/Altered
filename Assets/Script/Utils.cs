using UnityEngine;

/// <summary>
/// Also, ExtensionMethods
/// </summary>
public static class Utils {
    public static Color GetColor(this ColorEnum colorEnum) {

        if (colorEnum == ColorEnum.BLUE) {
            return new Color(0.18823529411764706f, 0.2980392156862745f, 0.5372549019607843f);
        }
        if (colorEnum == ColorEnum.GREEN) {
            return new Color(0.49019607843137253f, 0.8705882352941177f, 0.5725490196078431f);
        }
        if (colorEnum == ColorEnum.PINK) {
            return new Color(0.9764705882352941f, 0.7254901960784313f, 0.7647058823529411f);
        }
        if (colorEnum == ColorEnum.ORANGE) {
            return new Color(0.9176470588235294f, 0.7294117647058823f, 0.4196078431372549f);
        }

        Debug.LogWarning("Util.cs :: GetColor() :: No match for color found, " + colorEnum.ToString());
        return new Color(1, 1, 1);
    }

    public static ColorEnum RandColorEnum() {

        int selection = Random.Range(0, 3);

        return (ColorEnum)selection;
    }
}

