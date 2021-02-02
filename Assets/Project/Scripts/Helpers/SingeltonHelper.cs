using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingeltonHelper : MonoBehaviour {
    public static List<SingeltonHelper> instances;
    public string type;

    void Awake () {
        if (type == null) {
            Debug.LogWarning ("[Singleton Awake] Singleton has no Type defined. Singleton Setup will be skipped.");
            return;
        }
        if (instances == null)
            instances = new List<SingeltonHelper> ();

        foreach (SingeltonHelper singleton in instances) {
            if (singleton.type == type) {
                Destroy (this.gameObject);
                Debug.Log ("[Singleton Awake] Another Singelton-GameObject of Type " + type + " already existed. Going to destroy this Instance.");
                return;
            }
        }
        Debug.Log ("[Singleton Awake] No Singelton-GameObject of Type " + type + " existed. Going to add this Instance to the Instances-List.");
        instances.Add (this);
        DontDestroyOnLoad (this.gameObject);
    }
}
