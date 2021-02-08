using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputGroupComponent : MonoBehaviour {
    public Sprite inputSpriteNormal;
    public Sprite inputSpriteError;
    public Image inputFieldImage;
    public TMPro.TextMeshProUGUI labelReference;
    public TMPro.TMP_InputField inputFieldReference;
    public TMPro.TextMeshProUGUI placeholderReference;
    public TMPro.TextMeshProUGUI errorReference;
    public TMPro.TextMeshProUGUI smallReference;

    [Header("Custom Input Settings")]
    public string labelLocaleName;
    public string placeholderLocaleName;
    public string smallLocaleName;
    public TMPro.TMP_InputField.ContentType contentType;

    void Start () {
        DisplayNormal ();
        SetLocalOrHide (labelLocaleName, labelReference);
        SetLocalOrHide (placeholderLocaleName, placeholderReference);
        SetLocalOrHide (smallLocaleName, smallReference);
        inputFieldReference.contentType = contentType;
        // DisplayError("Test Error");
    }

    void DisplayNormal () {
        inputFieldImage.sprite = inputSpriteNormal;
        labelReference.color = Color.black;
        errorReference.gameObject.SetActive (false);
    }

    public void DisplayError (string errorLocaleName) {
        errorReference.text = LocaleHelper.GetContent (errorLocaleName);
        errorReference.gameObject.SetActive (true);
        inputFieldImage.sprite = inputSpriteError;
        labelReference.color = Color.red;
    }

    void SetLocalOrHide (string localeName, TMPro.TextMeshProUGUI obj) {
        if (localeName == null || localeName == "") {
            obj.gameObject.SetActive (false);
        } else {
            obj.text = LocaleHelper.GetContent (localeName);
        }
    }
}
