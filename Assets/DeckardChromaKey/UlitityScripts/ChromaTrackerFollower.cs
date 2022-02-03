using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ChromaTrackerFollower : MonoBehaviour
{


    public DeckardChromaKey chromaKeySource;
    public bool anchorToLeft = true;
    public bool anchorToRight = false;
    public bool anchorToTop = false;
    public bool anchorToBottom = false;
    public bool anchorToHeadTop = false;
    public bool anchorShoulders = false;
    public float smoothMovement = 0.5f;



    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (anchorToLeft)
            gameObject.transform.localPosition = Vector3.Lerp(gameObject.transform.localPosition, new Vector3((chromaKeySource.trackResult.x / chromaKeySource.trackerResolutiuon) - 0.5f, 0, 0), smoothMovement * Time.deltaTime);
        if (anchorToRight)
            gameObject.transform.localPosition = Vector3.Lerp(gameObject.transform.localPosition, new Vector3((chromaKeySource.trackResult.y / chromaKeySource.trackerResolutiuon) - 0.5f, 0, 0), smoothMovement * Time.deltaTime);
        if (anchorToTop)
            gameObject.transform.localPosition = Vector3.Lerp(gameObject.transform.localPosition, new Vector3(0, (chromaKeySource.trackResult.z / chromaKeySource.trackerResolutiuon) - 0.5f, 0), smoothMovement * Time.deltaTime);
        if (anchorToBottom)
            gameObject.transform.localPosition = Vector3.Lerp(gameObject.transform.localPosition, new Vector3(0, (chromaKeySource.trackResult.w / chromaKeySource.trackerResolutiuon) - 0.5f, 0), smoothMovement * Time.deltaTime);
        if (anchorToHeadTop)
        {
            if (chromaKeySource.portraitMode)
            {
                gameObject.transform.localPosition = Vector3.Lerp(gameObject.transform.localPosition, 
                new Vector3((chromaKeySource.trackResult.x / chromaKeySource.trackerResolutiuon) - 0.5f,
                (chromaKeySource.headCentroid.x / chromaKeySource.trackerResolutiuon) - 0.5f,
                0),
                smoothMovement * Time.deltaTime);
            }
            else gameObject.transform.localPosition = Vector3.Lerp(gameObject.transform.localPosition,

               new Vector3(((1f - (1f- chromaKeySource.headCentroid.x / chromaKeySource.trackerResolutiuon)) - 0.5f),
               (chromaKeySource.trackResult.w / chromaKeySource.trackerResolutiuon) - 0.5f,
               0),
               smoothMovement * Time.deltaTime);
        }

        if (anchorShoulders)
            gameObject.transform.localPosition = Vector3.Lerp(gameObject.transform.localPosition, new Vector3((chromaKeySource.shouldersTrack / chromaKeySource.trackerResolutiuon) - 0.5f, 0, 0), smoothMovement);


    }
}
