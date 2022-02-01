using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public abstract class PowerUp : MonoBehaviour
{
    public PowerUpType type;
    Button MyButton;
    Text Counter;
    public int CountInt { get; set; }
    private void Start()
    {
        if (GetComponentInChildren<Button>()) 
        {
            MyButton = GetComponentInChildren<Button>();
            MyButton.onClick.AddListener(() => Use());
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

    public virtual void Use() 
    {
        CountInt--;
        if (CountInt == 0) 
        {
            Disable();
            return;
        }
        StartCoroutine(UseProcess());   

    }

    public abstract IEnumerator UseProcess();
     
}
public enum PowerUpType {Freeze,Invisible,MagicAttack }
public class PowerUpManager : MonoBehaviour
{
    public static PowerUpManager instance;
    
    public PowerUp[] PowerUpArr; 
    // Start is called before the first frame update
    void Start()
    {
        PowerUpArr = FindObjectsOfType<PowerUp>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
