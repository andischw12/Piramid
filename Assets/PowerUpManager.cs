using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public enum PowerUpType {Freeze,Invisible,MagicAttack}
public class PowerUpManager : MonoBehaviour
{
    public static PowerUpManager instance;
    
    public PowerUp[] PowerUpArr;
    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;

    }
    void Start()
    {
        
        PowerUpArr = GetComponentsInChildren<PowerUp>();
        ResetPowerUps();
    }

    public void TurnOnPowerUp(PowerUpType power) 
    {
        print(power);
        if (power ==PowerUpType.Freeze)
            PowerUpArr[0].Enable();
        else if (power == PowerUpType.Invisible)
            PowerUpArr[PowerUpType.Invisible.GetHashCode()].Enable();
        else
            PowerUpArr[PowerUpType.MagicAttack.GetHashCode()].Enable();
    } 

    public void ResetPowerUps() 
    {

        
        for (int i = 0; i <3; i++)
        {
            print(PowerUpArr.Length);
            PowerUpArr[i].Disable();
        }
    }

    // Update is called once per frame
     
}
