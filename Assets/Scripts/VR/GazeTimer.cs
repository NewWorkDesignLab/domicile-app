using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class GazeTimer : MonoBehaviour
{
  public static GazeTimer instance;

  public Image gazeTimerImage;
  public float timerDurarion = 2f;

  private float gazeTime;
  private bool gazeIsActive = false;
  private Coroutine timerCoroutine;

  void Awake()
  {
    instance = this;
  }

  void Update()
  {
    if (gazeIsActive)
    {
      gazeTime += Time.deltaTime;
      gazeTimerImage.fillAmount = gazeTime / timerDurarion;
    }
  }

  public void StartGazeTimer(Action action)
  {
    gazeIsActive = true;
    gazeTime = 0;
    if (timerCoroutine != null)
    {
      StopCoroutine(timerCoroutine);
      timerCoroutine = null;
    }
    timerCoroutine = StartCoroutine(WaitForActivation(action));
  }

  private IEnumerator WaitForActivation(Action action)
  {
    yield return new WaitForSeconds(timerDurarion);
    action.Invoke();
    StopGazeTimer();
  }

  public void StopGazeTimer()
  {
    gazeIsActive = false;
    if (timerCoroutine != null)
    {
      StopCoroutine(timerCoroutine);
      timerCoroutine = null;
    }
    gazeTime = 0;
    gazeTimerImage.fillAmount = 0;
  }
}
