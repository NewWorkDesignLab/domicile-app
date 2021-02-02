using UnityEngine;

public class LoadingCirceComponent : MonoBehaviour {
    public RectTransform rectComponent;
    private float rotateSpeed = -400f;

    private void Update () {
        rectComponent.Rotate (0f, 0f, rotateSpeed * Time.deltaTime);
    }
}
