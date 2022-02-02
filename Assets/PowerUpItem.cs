using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpItem : MonoBehaviour
{
    public PowerUpType Type;
    // Start is called before the first frame update

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player") 
        {
            print("hit");
            PowerUpManager.instance.TurnOnPowerUp(Type);
            Destroy(gameObject);
        }
    }

}
