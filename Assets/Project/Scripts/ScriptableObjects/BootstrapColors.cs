using UnityEngine;

[CreateAssetMenu (fileName = "defaultBootstrapColors", menuName = "Domicile/BootstrapColors", order = 1)]
public class BootstrapColors : ScriptableObject {
    public Color primary;
    public Color secondary;
    public Color success;
    public Color danger;
    public Color warning;
    public Color info;
    public Color light;
    public Color dark;
}
