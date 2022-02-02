using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    public int Health =100;
    // Start is called before the first frame update
    private void Awake()
    { 
            instance = this;
        
    }

    public void ChangeHealth(int val) 
    {
       
            
        Health += val;
        if (Health > 100)
            Health = 100;
        if (Health < 1)
        {
            Health = 0;
            Die();
        }
        if(val<0)
            UserProfileManager.instance.ColorEffect(Color.red);
        else if(val>0)
            UserProfileManager.instance.ColorEffect(Color.green);
        UserProfileManager.instance.SetSlider(Health);
    }


    public void Die() 
    {
        GameManager.instance.LoadLevel(GameManager.LoadLevelOptions.CurrentLevel);
    }
}
