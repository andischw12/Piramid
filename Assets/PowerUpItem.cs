using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class PickUpItem : MonoBehaviour 
{
    
    [SerializeField] ParticleSystem PickupParticleEffect;
    protected virtual void PickUpEffect()
    {

        ParticleSystem tmp = Instantiate(PickupParticleEffect, transform.position, transform.rotation);
        tmp.Play();
        tmp.loop = false;
        Destroy(gameObject);
    }
   public abstract void PickUp();

}

public abstract class FloorPickUpItem : PickUpItem 
{
    [SerializeField] protected int value;
    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            PickUp();
            PickUpEffect();
        }
    }
  
}


public class PowerUpItem : FloorPickUpItem
{
    public PowerUpType Type;
    public override void PickUp()
    {
        PowerUpManager.instance.TurnOnPowerUp(Type, value);
        PickUpEffect();
    }
}
