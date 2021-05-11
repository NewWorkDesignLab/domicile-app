using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetersHelper : MonoBehaviour {
    public GameObject targetMeter;
    public GameObject openButton;
    public GameObject closeButton;

    void Start () {
        CloseMeter ();
    }

    public void OpenMeter () {
        targetMeter.SetActive (true);
        openButton.SetActive (false);
        closeButton.SetActive (true);
    }
    public void CloseMeter () {
        targetMeter.SetActive (false);
        openButton.SetActive (true);
        closeButton.SetActive (false);
    }
}
