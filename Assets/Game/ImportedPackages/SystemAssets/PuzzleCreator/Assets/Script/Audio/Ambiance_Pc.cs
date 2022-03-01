// Description: Ambiance_Pc: do not destroy adio ambiance when a new scene is oaded
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ambiance_Pc : MonoBehaviour
{
   public static Ambiance_Pc instance = null;              //Static instance of GameManager which allows it to be accessed by any other script.


    void Awake()
    {
        if (instance == null)           //Check if instance already exists
            instance = this;            //if not, set instance to this

        else if (instance != this)      //If instance already exists and it's not this:
            Destroy(gameObject);        //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
    }


    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
}
