using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static GameManager instance;
    public bool AcceptPlayerInput = true;
    public Level currentLevel;
    public Level[] levelList;
    public CustomTimer timer;
     

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }
    private void Start()
    {
        currentLevel = levelList[SceneManager.GetActiveScene().buildIndex];
        StartCoroutine(StartTimer());
    }

    //=======Level Control ========
    public enum LoadLevelOptions {CurrentLevel,NextLevel};
    public void LoadLevel(LoadLevelOptions l)
    {
        string SceneName;
        int SceneIndex = currentLevel.levelIndex + l.GetHashCode();
        SceneName = System.IO.Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(SceneIndex));
        Initiate.Fade(SceneName, Color.black, 1f);
    }
    //=========================

    //========Timer Control====
    IEnumerator StartTimer()
    {
        timer.duration = currentLevel.levelTime;
        timer.StartTimer();
        yield return new WaitUntil(()=>timer.GetTimerValue()==1);
        timer.PauseTimer();
        LoadLevel(LoadLevelOptions.CurrentLevel);
    }
     
}
