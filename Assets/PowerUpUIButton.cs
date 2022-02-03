using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public abstract class PowerUpUIButton : MonoBehaviour
{
    [SerializeField] Button MyButton;
    int PowerUpVal;
    public void Start()
    {
        if (GetComponentInChildren<Button>())
        {
            MyButton = GetComponentInChildren<Button>();
            MyButton.onClick.AddListener(() => Use(PowerUpVal));
        }
    }
    public void Disable()
    {
        MyButton.interactable = false;
    }
    public void Enable(int val)
    {
        PowerUpVal = val;
        MyButton.interactable = true;
    }
    public virtual void Use(float PowerUpVal)
    {
        StartCoroutine(UseProcess(PowerUpVal));
        Disable();
    }
    public abstract IEnumerator UseProcess(float PowerUpVal);
}
 
