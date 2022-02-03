using UnityEngine;
using UnityEditor;




    public class DeckardChromakeyMenu : MonoBehaviour
    {
        //	private GameObject VRPano;
        [MenuItem("GameObject/Deckard/Deckard Chromakey HDRP", false, 10)]
        static void CreateDeckardCameraObject(MenuCommand menuCommand)
        {


            GameObject VRPano = PrefabUtility.InstantiatePrefab(Resources.Load("Prefabs/ChromaKeyPlaneHDRP10")) as GameObject;
            VRPano.name = "Deckard Chromakey";
           // PrefabUtility.DisconnectPrefabInstance(VRPano);


            GameObjectUtility.SetParentAndAlign(VRPano, menuCommand.context as GameObject);


          //  Undo.RegisterCreatedObjectUndo(VRPano, "Create " + VRPano.name);
            Selection.activeObject = VRPano;
        }
    }

