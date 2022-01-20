using UnityEngine;
using UnityEditor;
using BlazeAISpace;

[CanEditMultipleObjects]
[CustomEditor(typeof(BlazeAI))]
public class BlazeAIEditor : Editor
{
    
    string[] tabs = {"General", "States", "Distractions", "Hits", "Death", "Profile"};
    int tabSelected = -1;

    // variables
    SerializedProperty groundLayers,
    pathRecalculationRate,
    pathFindingProxy,
    proxyOffset,
    enableGravity,
    useRootMotion,
    avoidFacingObstacles,
    obstacleRayDistance,
    obstacleRayOffset,
    obstacleLayers,
    tagsToAvoid,
    waypoints,
    vision,

    normalState,
    alertState,
    attackState,

    distractions,
    hits,
    death,
    blazeProfile,
    profileSync;


    void OnEnable()
    {
        tabSelected = 0;

        // general
        groundLayers = serializedObject.FindProperty("groundLayers");
        pathRecalculationRate = serializedObject.FindProperty("pathRecalculationRate");
        pathFindingProxy = serializedObject.FindProperty("pathFindingProxy");
        proxyOffset = serializedObject.FindProperty("proxyOffset");
        enableGravity = serializedObject.FindProperty("enableGravity");
        useRootMotion = serializedObject.FindProperty("useRootMotion");
        avoidFacingObstacles = serializedObject.FindProperty("avoidFacingObstacles");
        obstacleRayDistance = serializedObject.FindProperty("obstacleRayDistance");
        obstacleRayOffset = serializedObject.FindProperty("obstacleRayOffset");
        obstacleLayers = serializedObject.FindProperty("obstacleLayers");
        tagsToAvoid = serializedObject.FindProperty("tagsToAvoid");
        waypoints = serializedObject.FindProperty("waypoints");
        vision = serializedObject.FindProperty("vision");

        // states
        normalState = serializedObject.FindProperty("normalState");
        alertState = serializedObject.FindProperty("alertState");
        attackState = serializedObject.FindProperty("attackState");

        // distractions
        distractions = serializedObject.FindProperty("distractions");

        // hits
        hits = serializedObject.FindProperty("hits");

        // death
        death = serializedObject.FindProperty("death");

        // profile
        blazeProfile = serializedObject.FindProperty("blazeProfile");
        profileSync = serializedObject.FindProperty("profileSync");
    }

    public override void OnInspectorGUI () {

        var oldColor = GUI.backgroundColor;
        GUI.backgroundColor = new Color(0.55f, 0.55f, 0.55f, 1f);
        
        EditorGUILayout.BeginVertical();
        tabSelected = GUILayout.Toolbar(tabSelected, tabs, ToolbarStyle());
        EditorGUILayout.EndVertical();

        GUI.backgroundColor = oldColor;
        BlazeAI script = (BlazeAI)target;

        EditorGUILayout.Space(25);
        EditorGUILayout.LabelField("Hover on any property below for more details", EditorStyles.helpBox);

        EditorGUILayout.Space(1);
        if (script.profileSync && script.blazeProfile) EditorGUILayout.HelpBox("Profile Sync enabled. Some changes can only be made from the profile.", MessageType.Warning);

        switch (tabSelected)
        {
            case 0:
                GeneralTab(script);
                break;
            case 1:
                StatesTab();
                break;
            case 2:
                DistractionsTab();
                break;
            case 3:
                HitsTab();
                break;
            case 4:
                DeathTab();
                break;
            case 5:
                ProfileTab();
                break;
        }

        serializedObject.ApplyModifiedProperties();
        
        if (script.profileSync) script.LoadProfile(script.blazeProfile);
        script.lastProfile = script.blazeProfile;
    }

    // styles the header button
    GUIStyle HeaderBtnStyle()
    {
        var headerBtnStyle = new GUIStyle(GUI.skin.button);
        headerBtnStyle.normal.textColor = Color.black;
        headerBtnStyle.hover.textColor = new Color(1f, 0.6537189f, 0.04245278f, 1);
        headerBtnStyle.fontSize = 15;
        headerBtnStyle.fixedHeight = 25;

        return headerBtnStyle;
    }
    
    // style the toolbar
    GUIStyle ToolbarStyle()
    {
        var style = new GUIStyle(GUI.skin.button);
        style.fixedHeight = 45;

        return style;
    }

    // build npc button functionality and style
    void BuildNPC()
    {
        var btnStyle = new GUIStyle(GUI.skin.button);
        btnStyle.normal.textColor = Color.white;
        btnStyle.active.textColor = new Color(1f, 0.6537189f, 0.04245278f, 1);
        btnStyle.fontSize = 15;
        btnStyle.fixedHeight = 30;

        BlazeAI script = (BlazeAI) target;

        if (GUILayout.Button("Build Agent", btnStyle)) {
            if(script.CheckNPCBuild()){
                if(EditorUtility.DisplayDialog("Rebuild structure?","Blaze AI has detected that you have already built the agent. Are you sure you want to rebuild? This will break things!", "Build", "Do Not Build")){
                    script.BuildNPC();
                    EditorUtility.DisplayDialog("Agent Built!","Please read the messages printed in the console for the next steps", "OK");
                }
            }else{
                script.BuildNPC();
                EditorUtility.DisplayDialog("Agent Built!","Please read the messages printed in the console for the next steps", "OK");
            }
        }
    }

    // render the general tab properties
    void GeneralTab(BlazeAI script)
    {
        EditorGUILayout.PropertyField(groundLayers);
        EditorGUILayout.PropertyField(pathRecalculationRate);
        EditorGUILayout.PropertyField(pathFindingProxy);
        EditorGUILayout.PropertyField(proxyOffset);
        EditorGUILayout.PropertyField(enableGravity);
        EditorGUILayout.PropertyField(useRootMotion);
        EditorGUILayout.PropertyField(avoidFacingObstacles);

        if (script.avoidFacingObstacles) {
            EditorGUILayout.PropertyField(obstacleRayDistance);
            EditorGUILayout.PropertyField(obstacleRayOffset);
            EditorGUILayout.PropertyField(obstacleLayers);
        }

        EditorGUILayout.PropertyField(tagsToAvoid);
        EditorGUILayout.PropertyField(waypoints);
        EditorGUILayout.PropertyField(vision);

        EditorGUILayout.Space(25);
        BuildNPC();
    }

    // render the states classes
    void StatesTab()
    {
        EditorGUILayout.PropertyField(normalState);
        EditorGUILayout.PropertyField(alertState);
        EditorGUILayout.PropertyField(attackState);

        EditorGUILayout.Space(15);
    }

    // render the distractions tab class
    void DistractionsTab()
    {
        EditorGUILayout.PropertyField(distractions);
        EditorGUILayout.Space(15);
    }

    // render the hits tab class
    void HitsTab()
    {
        EditorGUILayout.PropertyField(hits);
        EditorGUILayout.Space(15);
    }

    // render the death tab class
    void DeathTab()
    {
        EditorGUILayout.PropertyField(death);
        EditorGUILayout.Space(15);
    }

    // render the profile tab
    void ProfileTab()
    {
        EditorGUILayout.PropertyField(blazeProfile);
        EditorGUILayout.PropertyField(profileSync);

        EditorGUILayout.Space(15);
    }

}
