using Mirror;
using UnityEngine;

public class MetersHelper : NetworkBehaviour {
    public GameObject targetMeter;
    public GameObject openButton;
    public GameObject closeButton;

    [SyncVar (hook = nameof (SetCurrentState))]
    public bool currentState;

    public override void OnStartServer () {
        base.OnStartServer ();
        currentState = false;
    }

    public override void OnStartClient () {
        SetCurrentState (true, currentState);
    }

    [Command (requiresAuthority = false)]
    public void OpenMeter () {
        currentState = true;
    }

    [Command (requiresAuthority = false)]
    public void CloseMeter () {
        currentState = false;
    }

    private void SetCurrentState (bool oldState, bool newState) {
        targetMeter.SetActive (newState);
        openButton.SetActive (!newState);
        closeButton.SetActive (newState);
    }
}
