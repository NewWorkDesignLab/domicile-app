using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class FileRequests : MonoBehaviour
{
  // https://answers.unity.com/questions/1632065/how-to-upload-multiple-files-to-a-server-using-uni.html
  public void UploadImages(string path, List<string> images, Action<string> onSuccess, Action<string> onFailure)
  {
    WWWForm form = new WWWForm();
    for (int i = 0; i < images.Count; i++)
    {
      byte[] bytes = File.ReadAllBytes(images[i]);
      form.AddBinaryData("images[]", bytes, Path.GetFileName(images[i]), "image/png");
    }
    UnityWebRequest request = UnityWebRequest.Post(String.Format("{0}{1}", API.instance.host, path), form);
    StartCoroutine(API.instance.RequestHelper(request, onSuccess, onFailure));
  }
}
