using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(RoomScript)), CanEditMultipleObjects]
public class RoomScript_Editor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        RoomScript roomScript = (RoomScript)this.target;

        if (roomScript.isBossRoom) // if bool is true, show other fields
        { 
            roomScript.bossObject = EditorGUILayout.ObjectField("Boss Object",
                                                            roomScript.bossObject,
                                                            typeof(GameObject), true) as GameObject;
        }
    }
}
#endif
