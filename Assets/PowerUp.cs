using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public abstract class PowerUp : MonoBehaviour
{
    public PowerUpType type;
    [SerializeField] Button MyButton;
    public int PowerUpVal;
    Text Counter;

    public void Start()
    {
        if (GetComponentInChildren<Button>())
        {
            MyButton = GetComponentInChildren<Button>();
            MyButton.onClick.AddListener(() => Use(PowerUpVal));

        }

        if (GetComponentInChildren<Text>())
            Counter = GetComponentInChildren<Text>();

    }
    public void Disable()
    {
        MyButton.interactable = false;
    }

    public void Enable()
    {
        MyButton.interactable = true;
    }

    public virtual void Use(float PowerUpVal)
    {

        StartCoroutine(UseProcess(PowerUpVal));
        Disable();

    }

    public abstract IEnumerator UseProcess(float PowerUpVal);

}
 
