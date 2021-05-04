using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraRaycaster : MonoBehaviour {
    private const float maxDistance = 10;
    private GameObject gazedAtObject = null;

    public float gazeTimerDuration = 2f;
    private float elapsedTime;
    private bool gazeTimerActive = false;
    private Coroutine gazeTimerCoroutine;
    public Image gazeTimerImage;

    public Image raycastActiveImage;
    public Image raycastIdleImage;

    public void Update () {
        // Update Gaze Timer
        if (gazeTimerActive) {
            elapsedTime += Time.deltaTime;
            gazeTimerImage.fillAmount = elapsedTime / gazeTimerDuration;
        }

        // Check for new Raycast Hits
        RaycastHit hit;
        if (Physics.Raycast (transform.position, transform.forward, out hit, maxDistance)) {
            // GameObject detected in front of the camera.
            raycastActiveImage.gameObject.SetActive (true);
            raycastIdleImage.gameObject.SetActive (false);

            if (gazedAtObject != hit.transform.gameObject) {
                // New GameObject.
                gazedAtObject?.SendMessage ("RaycastExit");
                gazedAtObject = hit.transform.gameObject;
                gazedAtObject.SendMessage ("RaycastEnter");

                if (gazedAtObject.GetComponent<CameraRaycastReceiver> ().GazeEventPresent ())
                    StartGazeTimer ();
            }
        } else {
            // No GameObject detected in front of the camera.
            raycastActiveImage.gameObject.SetActive (false);
            raycastIdleImage.gameObject.SetActive (true);

            gazedAtObject?.SendMessage ("RaycastExit");
            gazedAtObject = null;
            StopGazeTimer ();
        }

        // Checks for screen touches.
        if (Google.XR.Cardboard.Api.IsTriggerPressed) {
            gazedAtObject?.SendMessage ("RaycastClick");
        }
    }

    void StartGazeTimer () {
        StopGazeTimer ();
        gazeTimerActive = true;
        gazeTimerCoroutine = StartCoroutine (WaitForActivation ());
    }

    IEnumerator WaitForActivation () {
        yield return new WaitForSeconds (gazeTimerDuration);
        gazedAtObject?.SendMessage ("RaycastGazedEnter");
        StopGazeTimer ();
    }

    void StopGazeTimer () {
        gazeTimerActive = false;
        if (gazeTimerCoroutine != null) {
            StopCoroutine (gazeTimerCoroutine);
            gazeTimerCoroutine = null;
        }
        elapsedTime = 0;
        gazeTimerImage.fillAmount = 0;
    }
}
