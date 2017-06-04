using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(MeshCombiner))]
public class MeshCombinerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        MeshCombiner mc = (MeshCombiner)target;
        if(GUILayout.Button("Combine Mesh"))
        {
            mc.CombineMeshes();
        }
    }
}
