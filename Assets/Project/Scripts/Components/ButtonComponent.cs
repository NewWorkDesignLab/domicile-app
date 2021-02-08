using UnityEngine;
using UnityEngine.UI;

public class ButtonComponent : MonoBehaviour {
    public Button buttonReference;
    public Image buttonImageReference;
    public VerticalLayoutGroup buttonLayoutGroup;
    public RectTransform buttonRectTransform;
    public TMPro.TextMeshProUGUI textReference;

    [Header ("Custom Button Settings")]
    public ButtonType buttonType;
    public string buttonTextLocaleName;
    [Space (20)]
    public Button.ButtonClickedEvent onClick;

    void Start () {
        textReference.text = LocaleHelper.GetContent (buttonTextLocaleName);
        buttonReference.onClick = onClick;
        switch (buttonType) {
            case ButtonType.Primary:
                buttonImageReference.color = BootstrapColorHelper.getColor.primary;
                textReference.color = Color.white;
                break;
            case ButtonType.Secondary:
                buttonImageReference.color = BootstrapColorHelper.getColor.secondary;
                textReference.color = Color.white;
                break;
            case ButtonType.Success:
                buttonImageReference.color = BootstrapColorHelper.getColor.success;
                textReference.color = Color.white;
                break;
            case ButtonType.Danger:
                buttonImageReference.color = BootstrapColorHelper.getColor.danger;
                textReference.color = Color.white;
                break;
            case ButtonType.Warning:
                buttonImageReference.color = BootstrapColorHelper.getColor.warning;
                textReference.color = Color.black;
                break;
            case ButtonType.Info:
                buttonImageReference.color = BootstrapColorHelper.getColor.info;
                textReference.color = Color.white;
                break;
            case ButtonType.Light:
                buttonImageReference.color = BootstrapColorHelper.getColor.light;
                textReference.color = Color.black;
                break;
            case ButtonType.Dark:
                buttonImageReference.color = BootstrapColorHelper.getColor.dark;
                textReference.color = Color.white;
                break;
            case ButtonType.Link:
                // adjust styling
                buttonImageReference.enabled = false;
                textReference.color = Color.blue;
                textReference.fontStyle = TMPro.FontStyles.Underline;
                textReference.alignment = TMPro.TextAlignmentOptions.Left;
                // adjust paddings
                RectOffset tmpPadding = new RectOffset (0, 0, 0, 0);
                buttonLayoutGroup.padding = tmpPadding;
                LayoutRebuilder.MarkLayoutForRebuild (buttonRectTransform);
                break;
            default:
                buttonImageReference.color = BootstrapColorHelper.getColor.primary;
                textReference.color = Color.white;
                break;
        }
    }
}
public enum ButtonType { Primary, Secondary, Success, Danger, Warning, Info, Light, Dark, Link }
