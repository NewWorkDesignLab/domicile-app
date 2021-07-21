using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraRaycaster : MonoBehaviour {
    private const float maxDistance = 6;
    private GameObject gazedAtAnyObject = null;
    private GameObject gazedAtRaycastReceiver = null;

    public float gazeTimerDuration = 1f;
    private float elapsedTime;
    private bool gazeTimerActive = false;
    private Coroutine gazeTimerCoroutine;
    public Image gazeTimerImage;
    public Image raycastActiveImage;
    public Image raycastIdleImage;

    public LayerMask layersToRaycast;

    public void Update () {
        // Update Gaze Timer
        if (gazeTimerActive) {
            elapsedTime += Time.deltaTime;
            gazeTimerImage.fillAmount = elapsedTime / gazeTimerDuration;
        }

        // Check for new Raycast Hits
        RaycastHit hit;
        if (Physics.Raycast (transform.position, transform.forward, out hit, maxDistance, layersToRaycast)) {
            // GameObject detected in front of the camera.
            if (gazedAtAnyObject != hit.transform.gameObject) {
                // New GameObject.
                gazedAtAnyObject = hit.transform.gameObject;

                CameraRaycastReceiver receiver = gazedAtAnyObject.GetComponent<CameraRaycastReceiver> ();
                if (receiver != null) {
                    // Is RaycastReceiver
                    raycastActiveImage.gameObject.SetActive (true);
                    raycastIdleImage.gameObject.SetActive (false);

                    if (gazedAtRaycastReceiver != gazedAtAnyObject) {
                        // New RaycastReceiver GameObject.
                        gazedAtRaycastReceiver?.SendMessage ("RaycastExit", null, SendMessageOptions.DontRequireReceiver);
                        gazedAtRaycastReceiver = gazedAtAnyObject;
                        gazedAtRaycastReceiver.SendMessage ("RaycastEnter", null, SendMessageOptions.DontRequireReceiver);

                        if (receiver.GazeEventPresent ())
                            StartGazeTimer ();
                    }
                } else {
                    // New GameObject but not a RaycastReceiver
                    raycastActiveImage.gameObject.SetActive (false);
                    raycastIdleImage.gameObject.SetActive (true);

                    gazedAtRaycastReceiver?.SendMessage ("RaycastExit", null, SendMessageOptions.DontRequireReceiver);
                    gazedAtRaycastReceiver = null;
                    StopGazeTimer ();
                }
            } else {
                // No New GameObject.
            }
        } else {
            // No GameObject detected in front of the camera.
            raycastActiveImage.gameObject.SetActive (false);
            raycastIdleImage.gameObject.SetActive (true);

            gazedAtRaycastReceiver?.SendMessage ("RaycastExit", null, SendMessageOptions.DontRequireReceiver);
            gazedAtRaycastReceiver = null;
            gazedAtAnyObject = null;
            StopGazeTimer ();
        }

        // Checks for screen touches.
        if (Google.XR.Cardboard.Api.IsTriggerPressed) {
            gazedAtAnyObject?.SendMessage ("RaycastClick", null, SendMessageOptions.DontRequireReceiver);
        }
    }

    void StartGazeTimer () {
        StopGazeTimer ();
        gazeTimerActive = true;
        gazeTimerCoroutine = StartCoroutine (WaitForActivation ());
    }

    IEnumerator WaitForActivation () {
        yield return new WaitForSeconds (gazeTimerDuration);
        gazedAtAnyObject?.SendMessage ("RaycastGazedEnter", null, SendMessageOptions.DontRequireReceiver);
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
