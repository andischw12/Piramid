using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UserNotificationManager : MonoBehaviour
{
    public static UserNotificationManager instance;
    Animator NotificationAnimator;
    Transform[] Components;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        NotificationAnimator =GetComponentInChildren<Animator>();
        Components = GetComponentsInChildren<Transform>();
        TurnComponentsOff();
    }
    private void HideNotification()
    {
        NotificationAnimator.SetTrigger("HideNotification");
    }
    public void ShowNotification(string text,float timeToShow) 
    {
        TurnComponentsOn();
        StartCoroutine(ShowNotificationProcess(text,timeToShow));
    }
    IEnumerator ShowNotificationProcess(string text, float timeToShow) 
    {
        GetComponentInChildren<TextMeshProUGUI>().text = text;
        NotificationAnimator.SetTrigger("ShowNotification");
        yield return new WaitForSeconds(timeToShow);
        HideNotification();
    }
    void TurnComponentsOn() 
    {
        foreach (Transform t in Components)
            t.gameObject.SetActive(true);
    }
    void TurnComponentsOff()
    {
        foreach (Transform t in Components) 
            t.gameObject.SetActive(false);
    }
}
