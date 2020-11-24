using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineHelper : MonoBehaviour {
  public static CoroutineHelper instance;

  void Awake () {
    instance = this;
  }
}
