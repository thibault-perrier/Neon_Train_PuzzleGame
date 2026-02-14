using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Track))]
public class TrackEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUILayout.Space(10);

        Track track = (Track)target;
        if (GUILayout.Button("Toggle Button Track"))
        {
            // track.ToggleButtonTrack();
        }
    }
}
