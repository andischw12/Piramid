using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthItem : MonoBehaviour
{
     
    [SerializeField] int value;
    // Start is called before the first frame update




    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") 
        {
            PlayerManager.instance.ChangeHealth(value);
            Destroy(this.gameObject);
        }
    }

}
