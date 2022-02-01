using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpFreeze : PowerUp
{
    public float FreezeTime;
     

    public override IEnumerator  UseProcess() 
    {
        FindObjectOfType<CustomTimer>().PauseTimer();
        yield return new WaitForSeconds(FreezeTime);
        FindObjectOfType<CustomTimer>().StartTimer();
    }
     
}
