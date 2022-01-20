using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevelCollider : MonoBehaviour
{
    string nextSceneName;
    // Start is called before the first frame update

    private void Start()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex+1;
        nextSceneName = System.IO.Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(nextSceneIndex));
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
            GameManager.instance.LoadLevel(GameManager.LoadLevelOptions.NextLevel);
    }
}
