using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public abstract class PowerUpUIButton : MonoBehaviour
{
    [SerializeField] Button MyButton;
    int PowerUpVal;
    Image[] arr;
    public void Start()
    {
        if (GetComponentInChildren<Button>())
        {
            MyButton = GetComponentInChildren<Button>();
            MyButton.onClick.AddListener(() => Use(PowerUpVal));
            arr = GetComponentsInChildren<Image>();
        }
    }
    public void Disable()
    {
        MyButton.interactable = false;
        
        arr[0].color = new Color32(123, 123, 123, 255);
        arr[1].gameObject.SetActive(false);
        arr[2].gameObject.SetActive(false);
        arr[3].gameObject.SetActive(true);
    }
    public void Enable(int val)
    {
        PowerUpVal = val;
        MyButton.interactable = true;
       
        arr[0].color = new Color32(0, 96, 149, 222);
        arr[1].gameObject.SetActive(true);
        arr[2].gameObject.SetActive(true);
         arr[3].gameObject.SetActive(false);
        
    }
    public virtual void Use(float PowerUpVal)
    {
        StartCoroutine(UseProcess(PowerUpVal));
        Disable();
    }
    public abstract IEnumerator UseProcess(float PowerUpVal);
}
 
