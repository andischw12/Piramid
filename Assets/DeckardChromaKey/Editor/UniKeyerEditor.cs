using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using System.IO;
using System;

namespace DeckardRender
{
    [CustomEditor(typeof(DeckardChromaKey))]
    public class UniKeyerEditor : Editor
    {
        private Texture banner;
        private GUILayoutOption[] style;
        public float fDistance;
        [SerializeField]
        public Material[] mats;



        // Use this for initialization

        void OnEnable()
        {
            banner = Resources.Load("KeyerHeader") as Texture;
        }


        public override void OnInspectorGUI()
        {

            EditorGUI.BeginChangeCheck();

            DeckardChromaKey VRP = (DeckardChromaKey)target;
            if (PlayerSettings.colorSpace == ColorSpace.Gamma)
            {
                GUILayout.BeginVertical("box");
                GUILayout.Label("WARNING! For best visual appearance");
                GUILayout.Label("switch Your project to Linear Color Space");
                if (GUILayout.Button("OK! Switch!"))
                {
                    PlayerSettings.colorSpace = ColorSpace.Linear;
                }
                GUILayout.EndVertical();
            }


            //mats = VRP.materials;
            //ScriptableObject scriptableObj = this;
            //SerializedObject serialObj = new SerializedObject(scriptableObj);
            //SerializedProperty serialProp = serialObj.FindProperty("mats");

            //EditorGUILayout.PropertyField(serialProp, true);
            //serialObj.ApplyModifiedProperties();





            GUILayout.Box(banner, GUILayout.ExpandWidth(true));
            EditorGUILayout.PrefixLabel("Keying Profile");
            VRP.chromaMat = (Material)EditorGUILayout.ObjectField(VRP.chromaMat, typeof(Material), allowSceneObjects: true);
            if (GUILayout.Button("Create New Profile"))
            {
                int random_nr = UnityEngine.Random.Range(10000, 99999);
                AssetDatabase.CopyAsset("Assets/DeckardChromaKey/Resources/Template/ChromaLiveTemplate.mat", "Assets/DeckardChromaKey/Resources/Profiles/ChromaKeyProfile" + random_nr + ".mat");
                VRP.chromaMat = (Material)AssetDatabase.LoadAssetAtPath("Assets/DeckardChromaKey/Resources/Profiles/ChromaKeyProfile" + random_nr + ".mat", typeof(Material));
            }





            EditorGUILayout.LabelField("Key Setup", GUI.skin.horizontalSlider);
            if (VRP.chromaMat != null)
            {
                VRP.sourceType = (DeckardChromaKey.SourceType)EditorGUILayout.EnumPopup(new GUIContent("Source Type", "Source type to choose"), VRP.sourceType);
                if (VRP.sourceType == DeckardChromaKey.SourceType.Webcam)
                {
                    VRP.deviceNumber = EditorGUILayout.IntField("Webcam Index", VRP.deviceNumber);

                    VRP.portraitMode = EditorGUILayout.Toggle("Portrait Mode", VRP.portraitMode);
                    VRP.SetPortraitMode(VRP.portraitMode);


                    if (!VRP.webcamOpened)
                    {
                        if (GUILayout.Button("Open Webcam"))
                        {
                            VRP.OpenWebcam();
                        }
                    }

                    if (VRP.webcamOpened)
                    {
                        if (GUILayout.Button("Close Webcam"))
                        {
                            VRP.CloseWebcam();
                        }
                    }

                    if (VRP.webcamOpened)
                    {
                        if (GUILayout.Button("Capture Garbage Matte"))
                        {
                            VRP.CaptureImage(VRP.webcamTexture);
                        }
                    }
                    VRP.useHistogram = EditorGUILayout.Toggle("Show Histogram", VRP.useHistogram);
                    if (VRP.useHistogram)
                    {
                        GUILayout.Box(VRP.histogram, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(false), GUILayout.Height(64));
                    }
                    GUILayout.Box(VRP.webcamTexture, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(false), GUILayout.Height(140));
                    GUILayout.Box(VRP.rt, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(false), GUILayout.Height(140));

                    // VRP.generateNormalMap = EditorGUILayout.Toggle("Generate Normal/Depth", VRP.generateNormalMap);

                    if (VRP.generateNormalMap)
                    {
                        GUILayout.Box(VRP.normalMap, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(false), GUILayout.Height(140));
                    }


                }

                else if (VRP.sourceType == DeckardChromaKey.SourceType.Video)
                {
                    VRP.inputVideo = (UnityEngine.Video.VideoPlayer)EditorGUILayout.ObjectField(VRP.inputVideo, typeof(UnityEngine.Video.VideoPlayer), allowSceneObjects: true);
                    VRP.animationFrame = EditorGUILayout.IntSlider(VRP.animationFrame, 0, VRP.maxAnimation);
                    VRP.portraitMode = EditorGUILayout.Toggle("Portrait Mode", VRP.portraitMode);
                    VRP.SetPortraitMode(VRP.portraitMode);
                    VRP.useHistogram = EditorGUILayout.Toggle("Show Histogram", VRP.useHistogram);
                    if (VRP.inputVideo != null)
                        if (VRP.useHistogram)
                        {
                            GUILayout.Box(VRP.histogram, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(false), GUILayout.Height(64));
                        }
                    GUILayout.Box(VRP.inputVideo.texture, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(false), GUILayout.Height(140));
                    GUILayout.Box(VRP.rt, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(false), GUILayout.Height(140));
                    

                    //    VRP.generateNormalMap = EditorGUILayout.Toggle("Generate Normal/Depth", VRP.generateNormalMap);
                    if (VRP.generateNormalMap)
                    {
                        GUILayout.Box(VRP.normalMap, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(false), GUILayout.Height(140));
                    }
                }

                else
                {
                    VRP.rtTextureInput = (Texture)EditorGUILayout.ObjectField(VRP.rtTextureInput, typeof(Texture), allowSceneObjects: true);

                    if (GUILayout.Button("Capture Garbage Matte"))
                    {
                        VRP.CaptureImage(VRP.rtTextureInput);
                    }

                    VRP.portraitMode = EditorGUILayout.Toggle("Portrait Mode", VRP.portraitMode);
                    VRP.SetPortraitMode(VRP.portraitMode);
                    VRP.useHistogram = EditorGUILayout.Toggle("Show Histogram", VRP.useHistogram);
                    if (VRP.useHistogram)
                    {
                        GUILayout.Box(VRP.histogram, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(false), GUILayout.Height(64));
                    }
                    GUILayout.Box(VRP.rtTextureInput, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(false), GUILayout.Height(140));
                    GUILayout.Box(VRP.rt, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(false), GUILayout.Height(140));

                    //    VRP.generateNormalMap = EditorGUILayout.Toggle("Generate Normal/Depth", VRP.generateNormalMap);
                    if (VRP.generateNormalMap)
                    {
                        GUILayout.Box(VRP.normalMap, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(false), GUILayout.Height(140));
                    }
                }





                EditorGUILayout.LabelField("Key Setup", GUI.skin.horizontalSlider);
                VRP.roll1 = EditorGUILayout.Foldout(VRP.roll1, new GUIContent("Key Settings", "Setting keying properties"), true);
                if (VRP.roll1)
                {
                    VRP.chromakeyMethod = (DeckardChromaKey.ChromaKeyMethod)EditorGUILayout.EnumPopup(new GUIContent("Chroma key Method", "Choose between color sample or image sample"), VRP.chromakeyMethod);
                    //  VRP.threshold = EditorGUILayout.Slider("Treshold", VRP.threshold, -0.5f, 1f);
                    if (VRP.chromakeyMethod == DeckardChromaKey.ChromaKeyMethod.GarbageKey)
                    {
                        EditorGUILayout.PrefixLabel("Garbage Key Source");
                        VRP.backgorundTex = (Texture2D)EditorGUILayout.ObjectField(VRP.backgorundTex, typeof(Texture2D), allowSceneObjects: true);
                    }
                    EditorGUILayout.PrefixLabel("Custom Mask");
                    VRP.customMask = (Texture2D)EditorGUILayout.ObjectField(VRP.customMask, typeof(Texture2D), allowSceneObjects: true);
                }
                EditorGUILayout.LabelField("Additional Passes", GUI.skin.horizontalSlider);
                VRP.roll5 = EditorGUILayout.Foldout(VRP.roll5, new GUIContent("Additional Passes", "Custom shader passes"), true);

                if (VRP.roll5)
                {
                    for (int i = 0; i < VRP.filterPasses.Length; i++)
                    {
                        VRP.filterPasses[i] = (Material)EditorGUILayout.ObjectField(VRP.filterPasses[i], typeof(Material), allowSceneObjects: true);
                    }

                    if (GUILayout.Button("Add Material filter"))
                    {
                        Array.Resize(ref VRP.filterPasses, VRP.filterPasses.Length + 1);
                    }
                    if (VRP.filterPasses.Length > 0)
                    {
                        if (GUILayout.Button("Remove Filter"))
                        {
                            Array.Resize(ref VRP.filterPasses, VRP.filterPasses.Length - 1);
                        }
                    }
                }


                EditorGUILayout.LabelField("Key Setup", GUI.skin.horizontalSlider);
                //VRP.outputType = (DeckardChromaKey.OutputType)EditorGUILayout.EnumPopup(new GUIContent("Output Type", "Output type to choose"), VRP.outputType);

                //if (VRP.outputType == DeckardChromaKey.OutputType.DynamicRenderTexture)
                //{
                //    VRP.roll2 = EditorGUILayout.Foldout(VRP.roll2, new GUIContent("Material Assignements", "Setting keying properties"), true);
                //    if (VRP.roll2)
                //    {
                //        for (int i = 0; i < VRP.materials.Length; i++)
                //        {
                //            VRP.materials[i] = (Material)EditorGUILayout.ObjectField(VRP.materials[i], typeof(Material), allowSceneObjects: true);



                //            for (int j = 0; j < VRP.properties.Length; j++)
                //            {

                //                VRP.properties[j] = EditorGUILayout.TextField(VRP.properties[j]);
                //            }
                //        }

                //    }
                //}


                EditorGUILayout.LabelField("Image Tracking", GUI.skin.horizontalSlider);

                if (VRP.outputType == DeckardChromaKey.OutputType.DynamicRenderTexture)
                {
                    VRP.roll3 = EditorGUILayout.Foldout(VRP.roll3, new GUIContent("Image Tracking", "Setting tracking properties"), true);
                    if (VRP.roll3)
                    {
                        VRP.useTracking = EditorGUILayout.Toggle("Use Image Tracking", VRP.useTracking);
                        if (VRP.useTracking)
                        {
                            //VRP.trackingResolution = (DeckardChromaKey.TrackingResolution)EditorGUILayout.EnumPopup(new GUIContent("Tracking Resolution", "Tracker Resolution"), VRP.trackingResolution);
                            VRP.trackerResolutiuon = EditorGUILayout.IntField("Tracker Resolution", VRP.trackerResolutiuon);
                            VRP.portraitMode = EditorGUILayout.Toggle("Camera is Vertical", VRP.portraitMode);
                            VRP.findFace = EditorGUILayout.Toggle("Find Face", VRP.findFace);
                            VRP.trackingTreshold = EditorGUILayout.Slider("Tracking Sensitivity", VRP.trackingTreshold, 0f, 1f);
                            VRP.headTrackingTrehold = EditorGUILayout.Slider("Head Sensitivity", VRP.headTrackingTrehold, 0f, 1f);
                            VRP.findShoulders = EditorGUILayout.Toggle("Find Shoulders", VRP.findShoulders);
                            VRP.shoulderTrackingTreshold = EditorGUILayout.Slider("Shoulders Sensitivity", VRP.shoulderTrackingTreshold, 0f, 1f);
                        }

                    }
                }
            }


            if (EditorGUI.EndChangeCheck())
            {

                Undo.RecordObject(VRP, "Undo Keyer");
                //   VRP.UpdateValues();

            }




            if (GUI.changed)
            {
                EditorUtility.SetDirty(VRP);
#if UNITY_5_4_OR_NEWER
                if (!Application.isPlaying)
                    EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
#endif



            }

        }
    }
}


