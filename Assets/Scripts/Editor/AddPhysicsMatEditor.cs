using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AddPhysicsMatToChildren))]
public class AddPhysicsMatEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("Add physics materials"))
        {
            AddPhysicsMatToChildren matScript = (AddPhysicsMatToChildren)target;
            matScript.AddPhysicsMats();
        }
    }
}
