// Description : isObjectActivated_Pc : Use to save if an object is activated or not in the save system.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class isObjectActivated_Pc : MonoBehaviour {

    [HideInInspector]
    public bool SaveVoiceOverProperties = true;


    [Header("Choose the state of this object when the scene starts and save data doesn't exist.")]
    public bool  firstTimeEnabledObject = false;

    [HideInInspector]
    public List<EditorMethodsList_Pc.MethodsList> methodsList      // Create a list of Custom Methods that could be edit in the Inspector
    = new List<EditorMethodsList_Pc.MethodsList>();

    [HideInInspector]
    public List<EditorMethodsList_Pc.MethodsList> methodsListObjDeactivated      // Create a list of Custom Methods that could be edit in the Inspector
    = new List<EditorMethodsList_Pc.MethodsList>();
    
    [HideInInspector]
    public List<EditorMethodsList_Pc.MethodsList> methodsListSaveExtend      // Create a list of Custom Methods that could be edit in the Inspector
    = new List<EditorMethodsList_Pc.MethodsList>();

    [HideInInspector]
    public List<EditorMethodsList_Pc.MethodsList> methodsListSaveExtendLoadProcess      // Create a list of Custom Methods that could be edit in the Inspector
    = new List<EditorMethodsList_Pc.MethodsList>();

    [HideInInspector]
    public CallMethods_Pc callMethods;

//--> Use to load object state
	public void saveSystemInitGameObject(string s_ObjectDatas){
        #region
        string[] codes = s_ObjectDatas.Split ('_');
		// Load Parameters 
        if(s_ObjectDatas != ""){                                  // Save Exist
            //Debug.Log("Here : " + s_ObjectDatas);
            for (var i = 0; i < codes.Length; i++)
            {
                if (codes[0] == "T")
                {
                    if (gameObject.GetComponent<Renderer>())
                    {         // Items
                        StartCoroutine(CallAllTheMethodsOneByOne(methodsList,0,true));
                    }
                    else
                    {                                                   // Obj activate
                        StartCoroutine(CallAllTheMethodsOneByOne(methodsList,1,true));
                    }
                }
                else if (codes[0] == "F")
                {
                    if (gameObject.GetComponent<Renderer>())
                    {          // Items
                        StartCoroutine(CallAllTheMethodsOneByOne(methodsListObjDeactivated,0,false));
                    }
                    else
                    {                                               // Obj activate
                        StartCoroutine(CallAllTheMethodsOneByOne(methodsListObjDeactivated,1,false));                       
                    }
                }
            } 

           
        }
        else{                                                       // Save doesn't exist   (only for Obj Activate)
             if (!gameObject.GetComponent<Renderer>()){
                if (firstTimeEnabledObject)
                {
                    StartCoroutine(CallAllTheMethodsOneByOne(methodsList,1,true));
                }
                else
                {
                    StartCoroutine(CallAllTheMethodsOneByOne(methodsListObjDeactivated,1,false));
                }
            }
            else{
                if (firstTimeEnabledObject)
                {
                    StartCoroutine(CallAllTheMethodsOneByOne(methodsList,0,true));
                }
                else
                {

                    StartCoroutine(CallAllTheMethodsOneByOne(methodsListObjDeactivated,0,false));
                }
            }
            //Debug.Log(gameObject.name);
        }

        callMethods.Call_A_Method_WithSpecificStringArgument(methodsListSaveExtendLoadProcess, s_ObjectDatas);       // Extend Save Procees

        #endregion
    }

    //--> Enable or disable gameObject renderer
    private void InitRenderer(GameObject obj, bool b_Enabled){
        #region
        obj.GetComponent<Renderer> ().enabled = b_Enabled;
		Transform[] Children = obj.GetComponentsInChildren<Transform>(true);
		for (var j = 0; j < Children.Length; j++) {
			if(Children[j].GetComponent<Renderer>()){
				Children[j].GetComponent<Renderer>().enabled = b_Enabled;
			}
            else{
                Children[j].gameObject.SetActive(b_Enabled);
            }
		}
        #endregion
    }

    //--> Use to save Object state
    public string ReturnSaveData () {
        #region
        string value = r_TrueFalse(gameObject.activeSelf);

		if(gameObject.GetComponent<Renderer>())
			value = r_TrueFalse(gameObject.GetComponent<Renderer>().enabled);


        for (var i = 0; i < methodsListSaveExtend.Count; i++)
        {
            value += "_" + callMethods.Call_A_Method_Only_String(methodsListSaveExtend, "");
        }

		return value;
        #endregion
    }

    private string r_TrueFalse(bool s_Ref){
        #region
        if (s_Ref)return "T";
		else return "F";
        #endregion
    }

    private string r_TrueFalseVoiceAlreadyPlayed(bool s_Ref){
        #region
        if (s_Ref)return "T";
		else return "F";
        #endregion
    }

    public bool Bool_returnIfObjectIsActivated(){
        #region
        if(GetComponent<Renderer>())
            return gameObject.GetComponent<Renderer>().enabled;
        else
            return gameObject.activeInHierarchy;
        #endregion
    }

    public void ActivateObject(){
        #region
        gameObject.SetActive(true);
        #endregion
    }
    public void DeactivateObject()
    {
        #region
        gameObject.SetActive(false);
        #endregion
    }

    public bool B_AP_ActivateObject()
    {
        #region
        gameObject.SetActive(true);
        return true;
        #endregion
    }
    public bool B_AP_DeactivateObject()
    {
        #region
        gameObject.SetActive(false);
        return true;
        #endregion
    }

    public void ActivateRenderer()
    {
        #region
        InitRenderer(gameObject, true);
        #endregion
    }
    public void DeactivateRenderer()
    {
        #region
        InitRenderer(gameObject, false);
        #endregion
    }

    public bool SwitchObjectEnabledState()
    {
        #region
        gameObject.SetActive(!Bool_returnIfObjectIsActivated());
        return true;
        #endregion
    }

    public IEnumerator CallAllTheMethodsOneByOne(List<EditorMethodsList_Pc.MethodsList> listOfMethods, int value, bool TorF)
    {
        #region
        for (var i = 0; i < listOfMethods.Count; i++)
        {
            yield return new WaitUntil(() => callMethods.Call_One_Bool_Method(listOfMethods, i) == true);
        }


        yield return new WaitForEndOfFrame();

        if(value == 0){ // InitRenderer
            InitRenderer(gameObject, TorF);
        }
        else if(value == 1){// Set active
            gameObject.SetActive(TorF);
        }

        yield return null;
        #endregion
    }
}
