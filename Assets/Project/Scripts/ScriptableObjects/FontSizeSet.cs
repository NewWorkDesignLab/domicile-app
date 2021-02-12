using UnityEngine;

[CreateAssetMenu (fileName = "defaultFontSizeSet", menuName = "Domicile/FontSizeSet", order = 1)]
public class FontSizeSet : ScriptableObject {
    public int bigHeaderFontSize;
    public int headerFontSize;
    public int subheadingFontSize;
    public int textFontSize;
    public int smallFontSize;
}
