using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    public int Health = 100;
    public bool Invincible { get; set; }
    // Start is called before the first frame update
    private void Awake()
    { 
            instance = this;
        
    }

    public void ChangeHealth(int val) 
    {
        if (val < 0 && Invincible)
            return;
            
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
        
        UserProfileManager.instance.SetSlider(Health);
    }


    public void Die() 
    {
        GameManager.instance.LoadLevel(GameManager.LoadLevelOptions.CurrentLevel);
    }
}
