using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MapGen))]
public class MapGenEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        MapGen generator = (MapGen)target;
        if (GUILayout.Button("Generate Map"))
        {
            generator.GenerateMap();
        }
        if (GUILayout.Button("Clear Map"))
        {
            generator.ClearMap();
        }

    }
}
