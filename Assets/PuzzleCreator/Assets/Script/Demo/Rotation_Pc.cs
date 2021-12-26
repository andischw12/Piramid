using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation_Pc : MonoBehaviour
{

    public List<RectTransform> listRecTransform = new List<RectTransform>();
    public List<int> listRotation = new List<int>();

    public float currentRotation = 0;
    public float rotSpeed = 10;

    // Update is called once per frame
    void Update()
    {
        currentRotation = Mathf.MoveTowards(currentRotation, 360, Time.deltaTime * rotSpeed);
        currentRotation %= 360;

        for (var i = 0;i< listRecTransform.Count; i++)
        {
            listRecTransform[i].localEulerAngles = new Vector3(0, 0, currentRotation * listRotation[i]);
        }
    }
}
