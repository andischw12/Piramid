//Desciption: AP_PadLock_Pc: Use to save and load data for the padlock
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AP_PadLock_Pc : MonoBehaviour
{
    public float startPosition = 0;
    public float endPosition = 0;
    public float speed = 2;
    public AudioClip a_Unlock;

    private AudioSource _Source;

    public bool b_Unlock = false;

    public void Start()
    {
        _Source = GetComponent<AudioSource>();
    }

    public void AP_OpenPadLock()
    {
        StartCoroutine(I_AP_OpenPadLock());
    }

    IEnumerator I_AP_OpenPadLock()
    {
        #region
        b_Unlock = true;

        if (a_Unlock && _Source)
        {
            _Source.clip = a_Unlock;
            _Source.Play();
        }
       

        while (endPosition != transform.localPosition.y)
        {
            float tmpFloat = Mathf.MoveTowards(transform.localPosition.y, endPosition, Time.deltaTime* speed);
            transform.localPosition = new Vector3(0, tmpFloat, 0);
            yield return null;
        }
        yield return null;
        #endregion
    }

    public void AP_InitPadlock(string s_ObjectDatas)
    {
        #region
        string[] codes = s_ObjectDatas.Split('_');              // Split data in an array.

        //--> Actions to do for this puzzle ----> BEGIN <----
        if (s_ObjectDatas == "")
        {                               // Save Doesn't exist

        }
        else
        {                                                   // Save exist
            int startValue = 1;

            //Debug.Log("startValue: " + startValue);

            if (codes[startValue] == "T")
            {
                b_Unlock = true;
                transform.localPosition = new Vector3(0, endPosition, 0);
            }

            else
            {
                b_Unlock = false;
                transform.localPosition = new Vector3(0, startPosition, 0);
            }
        }
        #endregion
    }

    public string AP_SaveData()
    {
        #region
        string valuesToSave = "";

        valuesToSave += r_TrueFalse(b_Unlock);
        valuesToSave += "_";


        return valuesToSave;
        #endregion
    }

    //--> Convert bool to T or F string
    private string r_TrueFalse(bool s_Ref)
    {
        if (s_Ref) return "T";
        else return "F";
    }
}
