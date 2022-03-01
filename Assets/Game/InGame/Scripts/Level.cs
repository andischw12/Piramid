using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;




public class Level:MonoBehaviour
{
    public int levelIndex;
    public float levelTime;
    
    
       
    // Use this for initialization
    void Start()
    {
        levelIndex = SceneManager.GetActiveScene().buildIndex;
    }
}
 