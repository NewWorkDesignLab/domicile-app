using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisuals : MonoBehaviour {
    public Player player;
    public float headNormalHeight;
    public float headCrawlHeight;
    public MeshRenderer[] visualObjects;

    public void Hide () {
        foreach (MeshRenderer obj in visualObjects) {
            obj.enabled = false;
        }
    }

    public void Show () {
        foreach (MeshRenderer obj in visualObjects) {
            obj.enabled = true;
        }
    }

    public void Stand () {
        SetHeadPosition (headNormalHeight);
    }

    public void Crawl () {
        SetHeadPosition (headCrawlHeight);
    }

    private void SetHeadPosition (float value) {
        Vector3 newPos = player.cameraHolderIfLocalPlayer.transform.localPosition;
        newPos.y = value;
        player.cameraHolderIfLocalPlayer.transform.localPosition = newPos;
    }
}
