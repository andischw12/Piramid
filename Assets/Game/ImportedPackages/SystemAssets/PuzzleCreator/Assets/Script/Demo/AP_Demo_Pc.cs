using UnityEngine;
using UnityEngine.SceneManagement;

public class AP_Demo_Pc : MonoBehaviour
{
    // Load the first in the ScenesInBuild 
    void Start()
    {
       SceneManager.LoadScene(0, LoadSceneMode.Single);   
    }


}
