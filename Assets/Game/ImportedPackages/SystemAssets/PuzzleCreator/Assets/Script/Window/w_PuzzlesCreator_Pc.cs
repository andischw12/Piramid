// Description : w_PuzzlesCreator_Pc.cs :  Allow to create puzzles and access some puzzles parameters
#if (UNITY_EDITOR)
using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Linq;
using UnityEngine.SceneManagement;

public class w_PuzzlesCreator_Pc : EditorWindow
{
	private Vector2 				scrollPosAll;
    private Vector2                 scrollPosSection;
    SerializedObject                serializedObject2;
    SerializedObject                serializedObject3;
    SerializedProperty              helpBoxEditor;
    SerializedProperty              currentTypeSelected;
    SerializedProperty              listOfPuzzles;
    SerializedProperty              clueSystem;
    SerializedProperty              diaryList;
    SerializedProperty              diaryListVoice;
    SerializedProperty              b_ActivatedTheFirstTime;
    SerializedProperty              StarterKit;
    SerializedProperty              SaveAsPlayerPrefs;

    public datasWindowReadyToUse_Pc _windowReadyToUseDatas;

    public bool                     b_ProjectManagerAssetExist = true;

    public String[] arrTypeOfPrefab = new string[] {    "01 Puzzles","02 ObjIsActivated"};


    public String[] arPuzzleName = new string[] {    "01-Sliding",
                                                     "02-Digicode",
                                                     "03-Lever",
                                                     "04-Cylinder",
                                                     "05-Pipe",
                                                     "06-Gear",
                                                     "07-Logic"};





    [MenuItem("Tools/Puzzles/Puzzles Creator (w_PuzzlesCreator)")]
	public static void ShowWindow()
	{
		//Show existing window instance. If one doesn't exist, make one.
        EditorWindow.GetWindow(typeof(w_PuzzlesCreator_Pc));
	}

	private Texture2D MakeTex(int width, int height, Color col) {						// use to change the GUIStyle
		Color[] pix = new Color[width * height];
		for (int i = 0; i < pix.Length; ++i) {
			pix[i] = col;
		}
		Texture2D result = new Texture2D(width, height);
		result.SetPixels(pix);
		result.Apply();
		return result;
	}

	private Texture2D 			Tex_01;
	private Texture2D 			Tex_02;
	private Texture2D 			Tex_03;
	private Texture2D 			Tex_04;
	private Texture2D 			Tex_05;

	public string[] 			listItemType = new string[]{};

	public List<string> 		_test = new List<string>(); 
	public int 					page = 0;
	public int 					numberOfIndexInAPage = 50;
	public int 					seachSpecificID = 0;

	public Color 				_cGreen = new Color(1f,.8f,.4f,1);
	public Color 				_cGray = new Color(.9f,.9f,.9f,1);


	public Texture2D            eye;
    public Texture2D            currentItemDisplay;
    public int                  intcurrentItemDisplay = 0;
    public bool                 b_UpdateProcessDone = false;
    public bool                 b_AllowUpdateScene = false;

    public windowMethods_Pc _windowMethods;

    void OnEnable()
    {
        #region
        _windowMethods = new windowMethods_Pc();
       // manipulateTextList = new EditorManipulateTextList();

        _MakeTexture();

        string objectPath = "Assets/PuzzleCreator/Assets/Datas/wPuzzleCreator.asset";
        _windowReadyToUseDatas = AssetDatabase.LoadAssetAtPath(objectPath, typeof(UnityEngine.Object)) as datasWindowReadyToUse_Pc;
        if (_windowReadyToUseDatas)
        {
            serializedObject2 = new UnityEditor.SerializedObject(_windowReadyToUseDatas);
            helpBoxEditor = serializedObject2.FindProperty("helpBoxEditor");
            currentTypeSelected = serializedObject2.FindProperty("currentTypeSelected");

            listOfPuzzles = serializedObject2.FindProperty("listOfPuzzles");
            clueSystem = serializedObject2.FindProperty("clueSystem");

            b_ActivatedTheFirstTime = serializedObject2.FindProperty("b_ActivatedTheFirstTime");
            StarterKit = serializedObject2.FindProperty("StarterKit");
            SaveAsPlayerPrefs = serializedObject2.FindProperty("SaveAsPlayerPrefs");
        }
        else
        {
            b_ProjectManagerAssetExist = false;
        }

        string objectEye = "Assets/PuzzleCreator/Assets/Textures/Edit/Eye.png";
        eye = AssetDatabase.LoadAssetAtPath(objectEye, typeof(UnityEngine.Object)) as Texture2D;
        #endregion
    }

    void OnGUI()
    {
        #region
        //--> Scrollview
        scrollPosAll = EditorGUILayout.BeginScrollView(scrollPosAll);
        //--> Window description
        //GUI.backgroundColor = _cGreen;
        CheckTex();
        GUIStyle style_Yellow_01 = new GUIStyle(GUI.skin.box); style_Yellow_01.normal.background = Tex_01;
        GUIStyle style_Blue = new GUIStyle(GUI.skin.box); style_Blue.normal.background = Tex_03;
        GUIStyle style_Purple = new GUIStyle(GUI.skin.box); style_Purple.normal.background = Tex_04;
        GUIStyle style_Orange = new GUIStyle(GUI.skin.box); style_Orange.normal.background = Tex_05;
        GUIStyle style_Yellow_Strong = new GUIStyle(GUI.skin.box); style_Yellow_Strong.normal.background = Tex_02;

        //		
        EditorGUILayout.BeginVertical(style_Purple);
        EditorGUILayout.HelpBox("Window Tab : Create Objects :", MessageType.Info);
        EditorGUILayout.EndVertical();

        // --> Display data
        EditorGUILayout.BeginHorizontal();
        _windowReadyToUseDatas = EditorGUILayout.ObjectField(_windowReadyToUseDatas, typeof(UnityEngine.Object), true) as datasWindowReadyToUse_Pc;
        EditorGUILayout.EndHorizontal();

        if (_windowReadyToUseDatas != null)
        {
            serializedObject2.Update();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("See HelpBoxes :", GUILayout.Width(85));
            EditorGUILayout.PropertyField(helpBoxEditor, new GUIContent(""), GUILayout.Width(30));
            EditorGUILayout.EndHorizontal();
            _helpBox(16);

            displayALLTypeOfPrefab(style_Purple, style_Yellow_01);

            displayOptions(style_Orange, style_Yellow_01);

           

            EditorGUILayout.LabelField("");
            //EditorGUILayout.LabelField("");
            EditorGUILayout.BeginVertical(style_Yellow_01);
            AP_GlobalOptions();
            EditorGUILayout.EndVertical();
            EditorGUILayout.LabelField("");

            EditorGUILayout.BeginVertical(style_Orange);
            AP_LayerOptions();
            EditorGUILayout.EndVertical();
            EditorGUILayout.LabelField("");


            EditorGUILayout.BeginVertical(style_Yellow_01);
            AP_starterKit();
            EditorGUILayout.EndVertical();
            EditorGUILayout.LabelField("");

            serializedObject2.ApplyModifiedProperties();
        }


       

        EditorGUILayout.EndScrollView();
        #endregion
    }

	void OnInspectorUpdate()
	{
		Repaint();
	}
		
	//--> If texture2D == null recreate the texture (use for color in the custom editor)
	private void CheckTex (){
		if (Tex_01 == null || Tex_02 == null || Tex_03 == null || Tex_04 == null || Tex_05 == null) {
			_MakeTexture ();
		}
	}

    private void _MakeTexture()
    {
        #region
        Tex_01 = MakeTex(2, 2, new Color(1, .8f, 0.2F, .4f));
        Tex_02 = MakeTex(2, 2, new Color(1, .8f, 0.2F, .4f));
        Tex_03 = MakeTex(2, 2, new Color(.3F, .9f, 1, .5f));
        Tex_04 = MakeTex(2, 2, new Color(.4f, 1f, .9F, 1f));
        Tex_05 = MakeTex(2, 2, new Color(1, .5f, 0.3F, .4f));
        #endregion
    }

    private void displayALLTypeOfPrefab(GUIStyle style_Purple, GUIStyle style_Yellow_01)
    {
        #region
        _helpBox(0);
        EditorGUILayout.BeginVertical(style_Purple);

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Select Category : ", GUILayout.Width(110));
        EditorGUI.BeginChangeCheck();

        currentTypeSelected.intValue = EditorGUILayout.Popup(currentTypeSelected.intValue, arrTypeOfPrefab);

        if (EditorGUI.EndChangeCheck())
        {
            currentTypeSelected.intValue = currentTypeSelected.intValue;
            currentItemDisplay = null;
        }
        EditorGUILayout.EndHorizontal();


        EditorGUILayout.EndVertical();
        #endregion
    }

    private void puzzleList(GUIStyle style_Purple, GUIStyle style_Yellow_01)
    {
        #region
        EditorGUILayout.BeginVertical(style_Purple);

        _helpBox(19);

        EditorGUILayout.HelpBox("if an Object is currently selected in the Hierarchy the puzzle will be created at this position.", MessageType.Info);

        // arPuzzleName

        for (var i = 0; i < arPuzzleName.Length; i++)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(arPuzzleName[i], GUILayout.Width(110));
            if (GUILayout.Button("Create", GUILayout.Width(50)))
            {
                Vector3 instantiatePosition = Vector3.zero;
                if (Selection.activeTransform != null)
                    instantiatePosition = Selection.activeTransform.position;
                GameObject newPuzzle = Instantiate((GameObject)listOfPuzzles.GetArrayElementAtIndex(i).objectReferenceValue, instantiatePosition, Quaternion.identity, null);
                Undo.RegisterCreatedObjectUndo(newPuzzle, newPuzzle.name);

                Selection.activeGameObject = newPuzzle;
            }
            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.LabelField("");

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Clue system", GUILayout.Width(110));
        if (GUILayout.Button("Create", GUILayout.Width(50)))
        {
            Vector3 instantiatePosition = Vector3.zero;
            if (Selection.activeTransform != null && Selection.activeTransform.GetComponent<conditionsToAccessThePuzzle_Pc>())
            {
                instantiatePosition = Selection.activeTransform.position;
                GameObject newClueSystem = Instantiate((GameObject)clueSystem.objectReferenceValue, instantiatePosition, Quaternion.identity, Selection.activeTransform);
                Undo.RegisterCreatedObjectUndo(newClueSystem, newClueSystem.name);
                newClueSystem.name = "ClueBox";
                Selection.activeGameObject = newClueSystem;
            }
            else
            {
                if (EditorUtility.DisplayDialog("INFO : Action none available"
                , "You need to select a puzzle in the Hierarchy."
                , "Continue"))
                {

                }
            }


        }
        EditorGUILayout.EndHorizontal();


        EditorGUILayout.EndVertical();
        #endregion
    }

    private void displayOptions(GUIStyle style_Orange, GUIStyle style_Yellow_01)
    {
        #region
        switch (currentTypeSelected.intValue)
        {
            case 1:
                F_SaveIfObjectActivated(style_Orange);
                break;
            case 0:
                puzzleList(style_Orange, style_Yellow_01);
                break;
        }
        #endregion
    }

    private void F_SaveIfObjectActivated(GUIStyle style_Orange)
    {
        #region
        EditorGUILayout.BeginVertical(style_Orange);

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Object Activated the first Time :", GUILayout.Width(180));
        EditorGUILayout.PropertyField(b_ActivatedTheFirstTime, new GUIContent(""), GUILayout.Width(30));
        EditorGUILayout.EndHorizontal();



        if (GUILayout.Button("Apply"))
        {
            if (Selection.activeTransform)
            {
                //-> Add Save System components
                if (!Selection.activeTransform.GetComponent<SaveData_Pc>())
                {
                    Undo.AddComponent(Selection.activeGameObject, typeof(isObjectActivated_Pc));
                    Selection.activeTransform.GetComponent<isObjectActivated_Pc>().firstTimeEnabledObject = b_ActivatedTheFirstTime.boolValue;
                }
                if (!Selection.activeTransform.GetComponent<SaveData_Pc>())
                { Undo.AddComponent(Selection.activeGameObject, typeof(SaveData_Pc)); }


                //-> find all the MonoBehaviour in a gameObject and select the scriptisObjectActvated.cs
                MonoBehaviour[] comp3 = Selection.activeTransform.GetComponents<MonoBehaviour>();

                for (var i = 0; i < comp3.Length; i++)
                {
                    if (comp3[i].GetType().ToString() == "isObjectActivated_Pc")
                    {
                        Selection.activeTransform.GetComponent<SaveData_Pc>().isObjectActivatedIndex = i;
                        Selection.activeTransform.GetComponent<SaveData_Pc>().b_isObjectActivated = true;

                        Debug.Log("Method " + i + " : " + comp3[i].GetType().ToString());
                        break;
                    }
                }
            }
            else
            {
                if (EditorUtility.DisplayDialog("INFO : Action none available"
                , "You need to select an object in the Hierarchy."
                , "Continue"))
                {

                }
            }
        }

        EditorGUILayout.EndVertical();
        #endregion
    }

    private void AP_GlobalOptions()
    {
        #region
        EditorGUILayout.LabelField("Global Puzzles Parameters", EditorStyles.boldLabel);

    

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Use Keyboard + Mouse"))
        {
            switchToKeyBoardMousePuzzle(true, false);
            switchToKeyBoardMouseCharacter(true, false);
            canvasMobileCharacter(false);
        }
        if (GUILayout.Button("Use Gamepad"))
        {
            switchToKeyBoardMousePuzzle(true, true);
            switchToKeyBoardMouseCharacter(true, true);
            canvasMobileCharacter(false);
        }

        if (GUILayout.Button("Use Mobile"))
        {
            switchToKeyBoardMousePuzzle(false, false);
            switchToKeyBoardMouseCharacter(false, false);
            canvasMobileCharacter(true);
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.LabelField("");


        _helpBox(1);
        EditorGUILayout.BeginHorizontal();

        string sType = "Current Save Type: .Dat";
        if(SaveAsPlayerPrefs.boolValue)
            sType = "Current Save Type: playerPrefs";

        if (GUILayout.Button(sType))
        {
            if (sType == "Current Save Type: .Dat")
                SaveAsPlayerPrefs.boolValue = true;
            else
                SaveAsPlayerPrefs.boolValue = false;

        }



        EditorGUILayout.EndHorizontal();


        if (GUILayout.Button("Update Save System (current Scene)", GUILayout.Height(30)))
        {
            _windowMethods.saveLevelInfos();                        // Update save system
        }



      

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Show .Dat In Explorer"))
        {
            ShowDataInExplorer();
        }
        if (GUILayout.Button("Delete all save datas (All Scenes)"))
        {
            DeleteCurrentSceneDataSave();
        }
        EditorGUILayout.EndHorizontal();
        #endregion
    }

    private void AP_LayerOptions()
    {
        #region
        EditorGUILayout.LabelField("Layers Options", EditorStyles.boldLabel);

        #region --> Update + Play Game

        #endregion

        SerializedObject serializedObject4 = new UnityEditor.SerializedObject(_windowReadyToUseDatas);
        SerializedProperty currentLayerPuzzle = serializedObject4.FindProperty("currentLayerPuzzle");
        SerializedProperty currentLayerPuzzleFeedbackCam = serializedObject4.FindProperty("currentLayerPuzzleFeedbackCam");
        SerializedProperty currentLayerPuzzleRay = serializedObject4.FindProperty("currentLayerPuzzleRay");
        SerializedProperty currentLayerPuzzleDragAndDrop = serializedObject4.FindProperty("currentLayerPuzzleDragAndDrop");
        serializedObject4.Update();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Layer: Puzzle:", GUILayout.Width(180));
        EditorGUILayout.PropertyField(currentLayerPuzzle, new GUIContent(""), GUILayout.Width(30));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Layer: PuzzleFeedbackCam:", GUILayout.Width(180));
        EditorGUILayout.PropertyField(currentLayerPuzzleFeedbackCam, new GUIContent(""), GUILayout.Width(30));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Layer: PuzzleRay:", GUILayout.Width(180));
        EditorGUILayout.PropertyField(currentLayerPuzzleRay, new GUIContent(""), GUILayout.Width(30));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Layer: PuzzleDragAndDrop:", GUILayout.Width(180));
        EditorGUILayout.PropertyField(currentLayerPuzzleDragAndDrop, new GUIContent(""), GUILayout.Width(30));
        EditorGUILayout.EndHorizontal();



        serializedObject4.ApplyModifiedProperties();


       
        #endregion
    }

    void OnSceneGUI(GUIStyle style_Orange )
	{
	}

    public void _helpBox(int value)
    {
        #region
        if (helpBoxEditor.boolValue)
        {
            switch (value)
            {
                case 0:
                    EditorGUILayout.HelpBox("Select the category of object you want to create", MessageType.Info);
                    break;

                case 1:
                    EditorGUILayout.HelpBox("IMPORTANT: " +
                    	"\n1-Don't forget to add each of your scene in the ScenesInBuild" +
                    	"\n2-Each scene used in the build needs to have a unique name." +
                    "\n3-If you have change the scenes order in Build Settings: Scenes in Build -> Reset all the save datas using the button ''Delete all save datas (All Scene'')", MessageType.Warning);
                    break;



                case 16:
                    EditorGUILayout.HelpBox("IMPORTANT: After creating object don't forget to update the current scene " +
                                            "using the button ''Update Save System (current Scene)'' in section Global Puzzles Parameters.", MessageType.Warning);
                    break;

                case 19:
                    EditorGUILayout.HelpBox("Create a puzzle in the Hierachy by clicking one of the buttons below.", MessageType.Info);
                    break;
            }
        }
        #endregion
    }

    void Update()
    {
        #region
        if (_windowMethods.bool_UpdateProcessDone)
        {
            _windowMethods.bool_UpdateProcessDone = false;
            b_UpdateProcessDone = false;
            EditorApplication.isPlaying = true;
        }
        if (b_UpdateProcessDone)
        {
            b_UpdateProcessDone = false;
            EditorApplication.isPlaying = true;
        }
        #endregion
    }

    public class inputParams
    {
        #region
        public string name;
        public string descriptiveName;
        public string descriptiveNegativeName;
        public string negativeButton;
        public string positiveButton;
        public string altNegativeButton;
        public string altPositiveButton;

        public float gravity;
        public float dead;
        public float sensitivity;

        public bool snap = false;
        public bool invert = false;

        public int type;

        public int axis;
        public int joyNum;
        #endregion
    }

    private static void CreateInput(inputParams newInput)
    {
        #region
        SerializedObject serializedObject = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/InputManager.asset")[0]);

        serializedObject.Update();
        SerializedProperty m_Axes = serializedObject.FindProperty("m_Axes");

        m_Axes.InsertArrayElementAtIndex(m_Axes.arraySize - 1);

        SerializedProperty m_Name = m_Axes.GetArrayElementAtIndex(m_Axes.arraySize - 1).FindPropertyRelative("m_Name");
        SerializedProperty descriptiveName = m_Axes.GetArrayElementAtIndex(m_Axes.arraySize - 1).FindPropertyRelative("descriptiveName");
        SerializedProperty descriptiveNegativeName = m_Axes.GetArrayElementAtIndex(m_Axes.arraySize - 1).FindPropertyRelative("descriptiveNegativeName");
        SerializedProperty negativeButton = m_Axes.GetArrayElementAtIndex(m_Axes.arraySize - 1).FindPropertyRelative("negativeButton");
        SerializedProperty positiveButton = m_Axes.GetArrayElementAtIndex(m_Axes.arraySize - 1).FindPropertyRelative("positiveButton");
        SerializedProperty altNegativeButton = m_Axes.GetArrayElementAtIndex(m_Axes.arraySize - 1).FindPropertyRelative("altNegativeButton");
        SerializedProperty altPositiveButton = m_Axes.GetArrayElementAtIndex(m_Axes.arraySize - 1).FindPropertyRelative("altPositiveButton");
        SerializedProperty gravity = m_Axes.GetArrayElementAtIndex(m_Axes.arraySize - 1).FindPropertyRelative("gravity");
        SerializedProperty dead = m_Axes.GetArrayElementAtIndex(m_Axes.arraySize - 1).FindPropertyRelative("dead");
        SerializedProperty sensitivity = m_Axes.GetArrayElementAtIndex(m_Axes.arraySize - 1).FindPropertyRelative("sensitivity");
        SerializedProperty snap = m_Axes.GetArrayElementAtIndex(m_Axes.arraySize - 1).FindPropertyRelative("snap");
        SerializedProperty invert = m_Axes.GetArrayElementAtIndex(m_Axes.arraySize - 1).FindPropertyRelative("invert");
        SerializedProperty type = m_Axes.GetArrayElementAtIndex(m_Axes.arraySize - 1).FindPropertyRelative("type");
        SerializedProperty axis = m_Axes.GetArrayElementAtIndex(m_Axes.arraySize - 1).FindPropertyRelative("axis");
        SerializedProperty joyNum = m_Axes.GetArrayElementAtIndex(m_Axes.arraySize - 1).FindPropertyRelative("joyNum");

        m_Name.stringValue = newInput.name;
        descriptiveName.stringValue = newInput.descriptiveName;
        descriptiveNegativeName.stringValue = newInput.descriptiveNegativeName;
        negativeButton.stringValue = newInput.negativeButton;
        positiveButton.stringValue = newInput.positiveButton;
        altNegativeButton.stringValue = newInput.altNegativeButton;
        altPositiveButton.stringValue = newInput.altPositiveButton;
        gravity.floatValue = newInput.gravity;
        dead.floatValue = newInput.dead;
        sensitivity.floatValue = newInput.sensitivity;
        snap.boolValue = newInput.snap;
        invert.boolValue = newInput.invert;
        type.intValue = newInput.type;
        axis.intValue = newInput.axis;
        joyNum.intValue = newInput.joyNum;

        serializedObject.ApplyModifiedProperties();
        #endregion
    }

    public static bool checkIfAlreadyExist(string nameToCheck)
    {
        #region
        SerializedObject serializedObject = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/InputManager.asset")[0]);
        SerializedProperty m_Axes = serializedObject.FindProperty("m_Axes");
        serializedObject.Update();
        Debug.Log(m_Axes.arraySize);
        for (var i = 0; i < m_Axes.arraySize; i++)
        {
            if (m_Axes.GetArrayElementAtIndex(i).FindPropertyRelative("m_Name").stringValue == nameToCheck)
                return true;
        }

        return false;
        #endregion
    }

    public void switchToKeyBoardMousePuzzle(bool b_DesktopInputs, bool b_Joystick)
    {
        #region
        GameObject[] allObjects = SceneManager.GetActiveScene().GetRootGameObjects();

        //-> Find the reticule
        for (var i = 0; i < allObjects.Length; i++)
        {
            Transform[] Children = allObjects[i].GetComponentsInChildren<Transform>(true);
            for (var j = 0; j < Children.Length; j++)
            {
                if (Children[j].name == "GlobalPuzzleManager")
                {
                    SerializedObject serializedObject = new SerializedObject(Children[j].GetComponent<AP_GlobalPuzzleManager_Pc>());

                    serializedObject.Update();
                    SerializedProperty m_b_DesktopInputs = serializedObject.FindProperty("b_DesktopInputs");
                    SerializedProperty m_b_Joystick = serializedObject.FindProperty("b_Joystick");

                    m_b_DesktopInputs.boolValue = b_DesktopInputs;
                    m_b_Joystick.boolValue = b_Joystick;

                    serializedObject.ApplyModifiedProperties();

                    /*Undo.RegisterFullObjectHierarchyUndo(Children[j].gameObject, Children[j].name);
                    Children[j].GetComponent<AP_GlobalPuzzleManager>().b_DesktopInputs = b_DesktopInputs;
                    Children[j].GetComponent<AP_GlobalPuzzleManager>().b_Joystick = b_Joystick;*/
                    break;
                }
            }
        }
        #endregion
    }

    public void switchToKeyBoardMouseCharacter(bool b_DesktopInputs, bool b_Joystick)
    {
        #region
        GameObject[] allObjects = SceneManager.GetActiveScene().GetRootGameObjects();

        //-> Find the reticule
        for (var i = 0; i < allObjects.Length; i++)
        {
            Transform[] Children = allObjects[i].GetComponentsInChildren<Transform>(true);
            for (var j = 0; j < Children.Length; j++)
            {
                if (Children[j].name == "Character" && Children[j].GetComponent<Character_Pc>())
                {
                    SerializedObject serializedObject = new SerializedObject(Children[j].GetComponent<Character_Pc>());

                    serializedObject.Update();
                    SerializedProperty m_b_DesktopInputs = serializedObject.FindProperty("b_DesktopInputs");
                    SerializedProperty m_b_Joystick = serializedObject.FindProperty("b_Joystick");

                    m_b_DesktopInputs.boolValue = b_DesktopInputs;
                    m_b_Joystick.boolValue = b_Joystick;

                    serializedObject.ApplyModifiedProperties();
                    break;
                }
            }
        }
        #endregion
    }

    public void canvasMobileCharacter(bool b_CanvasMobile)
    {
        #region
        GameObject[] allObjects = SceneManager.GetActiveScene().GetRootGameObjects();
        for (var i = 0; i < allObjects.Length; i++)
        {
            Transform[] Children = allObjects[i].GetComponentsInChildren<Transform>(true);
            for (var j = 0; j < Children.Length; j++)
            {
                if (Children[j].name == "Canvas_Mobile")
                {
                    SerializedObject serializedObject = new SerializedObject(Children[j].gameObject);

                    serializedObject.Update();
                    SerializedProperty m_IsActive = serializedObject.FindProperty("m_IsActive");
                    m_IsActive.boolValue = b_CanvasMobile;
                    serializedObject.ApplyModifiedProperties();
                    break;
                }
            }
        }
        #endregion
    }

    public void ShowDataInExplorer()
    {
        #region
        string itemPath = Application.persistentDataPath;
        itemPath = itemPath.TrimEnd(new[] { '\\', '/' });
        System.Diagnostics.Process.Start(itemPath);
        #endregion
    }

    static void DeleteCurrentSceneDataSave()
    {
        #region
        if (EditorUtility.DisplayDialog("Delete all the save files",
                                        "Are you sure you delete all the .Dat and PlayerPrefs files ", "Yes", "No"))
        {
            string itemPath = Application.persistentDataPath;
            itemPath = itemPath.TrimEnd(new[] { '\\', '/' });


            for (var j = 0; j < 30; j++)
            {    // Check all the Slots
                for (var i = 0; i < 30; i++)
                {
                    //Delete PlayerPrefs
                    if (PlayerPrefs.HasKey(j + "_" + "Puzzles_" + (i).ToString()))
                        PlayerPrefs.DeleteKey(j + "_" + "Puzzles_" + (i).ToString());

                    //Delete .Dat
                    //Debug.Log(itemPath + "/" + j + "_" + "Puzzles_" + (i).ToString() + ".dat");
                    FileUtil.DeleteFileOrDirectory(itemPath + "/" + j + "_" + "Puzzles_" + (i).ToString() + ".dat");
                }
            }


        }
        #endregion
    }

    public void AP_starterKit()
    {
        EditorGUILayout.LabelField("Starter Kit",EditorStyles.boldLabel);
        if (GUILayout.Button("Add Starter kit to the current scene"))
        {
            GameObject newStarterKit = PrefabUtility.InstantiatePrefab((GameObject)StarterKit.objectReferenceValue as GameObject) as GameObject;
            Undo.RegisterCreatedObjectUndo(newStarterKit, newStarterKit.name);
                newStarterKit.name = "StarterKit";
                Selection.activeGameObject = newStarterKit;
                PrefabUtility.UnpackPrefabInstance(newStarterKit,PrefabUnpackMode.OutermostRoot,InteractionMode.UserAction);
        }

       

        if (GUILayout.Button("Create Inputs for Gamepad"))
        {
            #region 
            if (checkIfAlreadyExist("JoyHorizontalHead"))
                Debug.Log("Already Exist");
            else
            {
                CreateInput(new inputParams()
                {
                    name = "JoyHorizontalHead",
                    descriptiveName = "",
                    descriptiveNegativeName = "",
                    negativeButton = "",
                    positiveButton = "",
                    altNegativeButton = "",
                    gravity = 0,
                    dead = .2f,
                    sensitivity = 1.0f,
                    snap = false,
                    invert = false,
                    type = 2,
                    axis = 2,
                    joyNum = 0
                });

                CreateInput(new inputParams()
                {
                    name = "JoyVerticalHead",
                    descriptiveName = "",
                    descriptiveNegativeName = "",
                    negativeButton = "",
                    positiveButton = "",
                    altNegativeButton = "",
                    gravity = 0,
                    dead = .2f,
                    sensitivity = 1.0f,
                    snap = false,
                    invert = false,
                    type = 2,
                    axis = 3,
                    joyNum = 0
                });
            }
            #endregion
        }
        EditorGUILayout.LabelField("");



    }
}
#endif