using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class ReflectionProperties : MonoBehaviour
{
    public Component component;
    // Start is called before the first frame update
    void Start()
    {
        Component c = component;

        Debug.Log("name " + c.name + " type " + c.GetType() + " basetype " + c.GetType().BaseType);
        foreach (FieldInfo fi in c.GetType().GetFields())
        {
            System.Object obj = (System.Object)c;
            if (fi.GetType() == (typeof(float)))
            Debug.Log("fi name " + fi.Name + " val " + fi.GetValue(obj));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
