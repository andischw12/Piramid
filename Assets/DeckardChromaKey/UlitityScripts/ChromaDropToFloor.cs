using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ChromaDropToFloor : MonoBehaviour
{
    public DeckardChromaKey chromaKeyer;
    public Transform rightTracker;
    public Transform topTracker;
    public float smoothness = 0.1f;
    public bool placeOnRoot = true;

    void Start()
    {

    }

    void LateUpdate()
    {
        KeepOnFloor();
    }

    void KeepOnFloor()
    {

        if (chromaKeyer.portraitMode)
        {
            if (rightTracker.position.y < gameObject.transform.position.y)
            {
                chromaKeyer.projectionPlane.localPosition = new Vector3(0, chromaKeyer.projectionPlane.localPosition.y + (smoothness * Vector3.Distance(gameObject.transform.position, rightTracker.transform.position)), 0);
            }

            else if (rightTracker.position.y > gameObject.transform.position.y)
            {
                chromaKeyer.projectionPlane.localPosition = new Vector3(0, chromaKeyer.projectionPlane.localPosition.y - (smoothness * Vector3.Distance(gameObject.transform.position, rightTracker.transform.position)), 0);
            }

        }

        else
        {
            if (topTracker.position.y < gameObject.transform.position.y)
            {
                chromaKeyer.projectionPlane.localPosition = new Vector3(0, chromaKeyer.projectionPlane.localPosition.y + (smoothness * Vector3.Distance(gameObject.transform.position, topTracker.transform.position)), 0);
            }

            else if (topTracker.position.y > gameObject.transform.position.y)
            {
                chromaKeyer.projectionPlane.localPosition = new Vector3(0, chromaKeyer.projectionPlane.localPosition.y - (smoothness * Vector3.Distance(gameObject.transform.position, topTracker.transform.position)), 0);
            }

        }
    }
}
