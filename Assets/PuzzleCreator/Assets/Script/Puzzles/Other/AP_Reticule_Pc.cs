//Description: AP_Reticule_Pc: Various method to manage the reticule states (Canvas_UIPuzzle -> Reticule)
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AP_Reticule_Pc : MonoBehaviour
{
    public bool SeeInspector = false;
    public Color color_01 = new Color(1, .8f, 0.2F, .4f);
    public Color color_02 = new Color(.3F, .9f, 1, .5f);
    public Color color_03 = new Color(1, .5f, 0.3F, .4f);

    public List<EditorMethodsList_Pc.MethodsList> methodsListCanGrabReticule      // Create a list of Custom Methods that could be edit in the Inspector
   = new List<EditorMethodsList_Pc.MethodsList>();
    public List<EditorMethodsList_Pc.MethodsList> methodsListReticuleSelected      // Create a list of Custom Methods that could be edit in the Inspector
   = new List<EditorMethodsList_Pc.MethodsList>();
    public List<EditorMethodsList_Pc.MethodsList> methodsListReticuleNoSelection      // Create a list of Custom Methods that could be edit in the Inspector
   = new List<EditorMethodsList_Pc.MethodsList>();

    public CallMethods_Pc callMethods;                        // Access script taht allow to call public function in this script.

    private Image _image;

    public bool b_CanGrab = false;
    public bool b_Selected = false;

    private void Start()
    {
        _image = GetComponent<Image>();
    }


    public void callMethodsListCanGrabReticule(){
        callMethods.Call_A_Method(methodsListCanGrabReticule); 
    }
    public void callMethodsListReticuleSelected()
    {
        callMethods.Call_A_Method(methodsListReticuleSelected);
    }
    public void callMethodsReticuleNoSelection()
    {
        callMethods.Call_A_Method(methodsListReticuleNoSelection);
    }

    public void AP_CanGrabReticule(){
        _image.color = Color.red;
        b_CanGrab = true;
    }

    public void AP_ReticuleSelected()
    {
        _image.color = Color.white;
        b_Selected = true;
    }

    public void AP_ReticuleNoSelection()
    {
        _image.color = Color.white;
        b_CanGrab = false;
        b_Selected = false;
    }

}
