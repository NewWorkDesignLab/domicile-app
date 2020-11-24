using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GestureManager : MonoBehaviour {
    public static GestureManager instance;
    public Queue<PlayerPose> PlayerPoseHistory { get; } = new Queue<PlayerPose> ();
    float prevGestureTime;
    float recognitionInterval = 0.5f;

    void Awake () {
        instance = this;
    }

    void Update () {
        UpdateHistory ();
        DetectNod ();
    }

    void UpdateHistory () {
        var orientation = Camera.main.gameObject.transform.rotation;
        PlayerPoseHistory.Enqueue (new PlayerPose (Time.time, orientation));
        if (PlayerPoseHistory.Count >= 60) {
            PlayerPoseHistory.Dequeue ();
        }
    }
    void DetectNod () {
        try {
            var averagePitch = PlayerPoseWithin (0.2f, 0.4f).Average (sample => sample.eulerAngles.x);
            var maxPitch = PlayerPoseWithin (0.01f, 0.2f).Max (sample => sample.eulerAngles.x);
            var pitch = PlayerPoseHistory.First ().eulerAngles.x;

            if (maxPitch - averagePitch > 10f && Mathf.Abs (pitch - averagePitch) < 5f) {
                if (prevGestureTime < Time.time - recognitionInterval) {
                    prevGestureTime = Time.time;
                    Debug.Log ("NOOOOODDDDEED");
                    // IngameMenuScript.instance.ToggleMenu();
                }
            }
        } catch (InvalidOperationException) {
            // PoseSamplesWithin contains no entry
        }
    }

    IEnumerable<PlayerPose> PlayerPoseWithin (float startTime, float endTime) {
        return PlayerPoseHistory.Where (sample =>
            sample.timestamp < Time.time - startTime &&
            sample.timestamp >= Time.time - endTime);
    }

    public class PlayerPose {
        public PlayerPose (float timestamp, Quaternion orientation) {
            this.timestamp = timestamp;
            this.orientation = orientation;

            eulerAngles = orientation.eulerAngles;
            eulerAngles.x = WrapDegree (eulerAngles.x);
            eulerAngles.y = WrapDegree (eulerAngles.y);
        }
        public float timestamp;
        public Quaternion orientation;
        public Vector3 eulerAngles;

        private float WrapDegree (float degree) {
            if (degree > 180f) {
                return degree - 360f;
            }
            return degree;
        }
    }
}
