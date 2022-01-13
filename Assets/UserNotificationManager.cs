using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserNotificationManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static UserNotificationManager instance;
    public  NotificationItem[] Notifications;
    public GameObject Background;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
        HideNotification();
    }


    private void Start()
    {
        
    }
    public void HideNotification()
    {
        Background.SetActive(false);
        foreach (NotificationItem gm in Notifications)
        {
            gm.gameObject.SetActive(false);
        }
    }

    public void ShowNotification(int num) 
    {
        HideNotification();
        Background.SetActive(true);
        Notifications[num].gameObject.SetActive(true);
        Notifications[num].counter++;
        Invoke("HideNotification",3f);
    }



}
