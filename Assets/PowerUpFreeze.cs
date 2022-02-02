using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpFreeze : PowerUp
{
    
     

    public override IEnumerator  UseProcess(float powerUpVal) 
    {
        
        FindObjectOfType<CustomTimer>().PauseTimer();
        yield return new WaitForSeconds(powerUpVal);
        FindObjectOfType<CustomTimer>().StartTimer();
    }
     
}
