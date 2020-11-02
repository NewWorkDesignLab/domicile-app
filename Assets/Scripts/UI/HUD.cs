using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class HUD : MonoBehaviour
{
    public static HUD instance;
    public Text notificationText;

    void Awake()
    {
        instance = this;
    }


    public void ShowNotification(string text)
    {
        ShowNotification(text, 0, () => { });
    }
    public void ShowNotification(string text, float duration)
    {
        ShowNotification(text, duration, () => { });
    }
    public void ShowNotification(string text, float duration, Action callback)
    {
        var _text = new List<string>();
        _text.Add(text);
        StartCoroutine(NotificationHelper(_text, duration, callback));
    }
    public void ShowNotification(List<string> text, float duration)
    {
        ShowNotification(text, duration, () => { });
    }
    public void ShowNotification(List<string> text, float duration, Action callback)
    {
        StartCoroutine(NotificationHelper(text, duration, callback));
    }

    public IEnumerator NotificationHelper(List<string> text, float duration, Action callback)
    {
        foreach (string _text in text)
        {
            notificationText.text = _text;
            yield return new WaitForSecondsRealtime(duration);
        }
        if (duration != 0)
        {
            HideAllNotifications();
        }
        callback();
    }
    public void HideAllNotifications()
    {
        notificationText.text = "";
    }
}
