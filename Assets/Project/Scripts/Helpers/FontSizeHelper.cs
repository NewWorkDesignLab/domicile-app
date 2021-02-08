using UnityEngine;

public class FontSizeHelper : Singleton<FontSizeHelper> {
    public static FontSizeSet getFontSize;
    public FontSizeSet fontSizeSet;

    protected override void Awake () {
        base.Awake ();
        getFontSize = fontSizeSet;
    }
}
