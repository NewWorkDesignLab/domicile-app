using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class FileRequests : MonoBehaviour {

  // https://answers.unity.com/questions/1632065/how-to-upload-multiple-files-to-a-server-using-uni.html
  public void UploadImages (string path, string[] images, Action<string> onSuccess, Action<string> onFailure) {
    if (images.Length > 0) {
      WWWForm form = new WWWForm ();
      for (int i = 0; i < images.Length; i++) {
        byte[] bytes = File.ReadAllBytes (images[i]);
        form.AddBinaryData ("images[]", bytes, Path.GetFileName (images[i]), "image/png");
      }
      UnityWebRequest request = UnityWebRequest.Post (String.Format ("{0}{1}", ServerManager.instance.host, path), form);
      StartCoroutine (ServerManager.instance.RequestHelper (request, onSuccess, onFailure));
    } else {
      Debug.Log("[FileRequests UploadImages] No Images to upload.");
    }
  }
}
