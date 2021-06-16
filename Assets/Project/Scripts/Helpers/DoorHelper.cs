using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class DoorHelper : NetworkBehaviour {
    public GameObject targetDoor;
    public GameObject openDoor;
    public GameObject closeDoor;
    public MeshCollider meshCollider;
    public float animationDuration = .5f;

    private Coroutine animationCoroutine;

    [SyncVar (hook = nameof (SetCurrentState))]
    // true = open; false = close
    private bool currentState;

    void Awake () {
        closeDoor.SetActive (false);
        openDoor.SetActive (false);
        targetDoor.SetActive (true);
    }

    public override void OnStartServer () {
        base.OnStartServer ();
        currentState = true;
    }

    public override void OnStartClient () {
        SetCurrentStateDuration (false, currentState, 0.001f);
    }

    [Command (requiresAuthority = false)]
    public void ToogleDoor () {
        currentState = !currentState;
    }

    private void SetCurrentState (bool oldState, bool newState) {
        SetCurrentStateDuration (oldState, newState, animationDuration);
    }
    private void SetCurrentStateDuration (bool oldState, bool newState, float duration) {
        if (animationCoroutine != null)
            StopCoroutine (animationCoroutine);
        animationCoroutine = StartCoroutine (DoorAnimation (newState, duration));
    }

    private IEnumerator DoorAnimation (bool _currentState, float duration) {
        meshCollider.enabled = false;

        Quaternion startRotation = targetDoor.transform.rotation;
        Quaternion endRotation = _currentState ? closeDoor.transform.rotation : openDoor.transform.rotation;
        Vector3 startPosition = targetDoor.transform.position;
        Vector3 endPosition = _currentState ? closeDoor.transform.position : openDoor.transform.position;
        Vector3 startScale = targetDoor.transform.localScale;
        Vector3 endScale = _currentState ? closeDoor.transform.localScale : openDoor.transform.localScale;

        for (float t = 0; t < duration; t += Time.deltaTime) {
            targetDoor.transform.rotation = Quaternion.Lerp (startRotation, endRotation, t / duration);
            targetDoor.transform.position = Vector3.Lerp (startPosition, endPosition, t / duration);
            targetDoor.transform.localScale = Vector3.Lerp (startScale, endScale, t / duration);
            yield return null;
        }
        targetDoor.transform.rotation = endRotation;
        targetDoor.transform.position = endPosition;
        targetDoor.transform.localScale = endScale;

        meshCollider.enabled = true;
    }
}
