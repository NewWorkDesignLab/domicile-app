using UnityEngine;

public class BootstrapColorHelper : Singleton<BootstrapColorHelper> {
    public static BootstrapColors getColor;
    public BootstrapColors bootstrapColorHelper;

    protected override void Awake () {
        base.Awake ();
        getColor = bootstrapColorHelper;
    }
}
