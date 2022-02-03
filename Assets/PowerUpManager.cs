using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum PowerUpType {Freeze,Invicible,Attack}
public class PowerUpManager : MonoBehaviour
{
    public static PowerUpManager instance;
    PowerUpUIButton[] PowerUpArr;
    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        PowerUpArr = GetComponentsInChildren<PowerUpUIButton>();
        ResetPowerUps();
    }
    public void TurnOnPowerUp(PowerUpType power,int value) 
    {
        print(power);
        if (power ==PowerUpType.Freeze)
            PowerUpArr[0].Enable(value);
        else if (power == PowerUpType.Invicible)
            PowerUpArr[PowerUpType.Invicible.GetHashCode()].Enable(value);
        else
            PowerUpArr[PowerUpType.Attack.GetHashCode()].Enable(value);
    } 
    public void ResetPowerUps() 
    {
        for (int i = 0; i <3; i++)
        {
            print(PowerUpArr.Length);
            PowerUpArr[i].Disable();
        }
    }
}
