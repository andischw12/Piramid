using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteInEditMode]
public class ChromaLookAtCamera : MonoBehaviour
{

    public bool realtimeInEditor = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (realtimeInEditor || Application.isPlaying )
        {
            if (Camera.main != null) { 

            gameObject.transform.LookAt(new Vector3(Camera.main.transform.position.x, gameObject.transform.position.y, Camera.main.transform.position.z), new Vector3(0, -1, 0));
                gameObject.transform.Rotate(new Vector3(180, 0, 0));
            }

        }
        }


}
