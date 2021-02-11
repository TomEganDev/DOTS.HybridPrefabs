using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(HybridPrefab))]
public class HybridPrefabEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        
        if (GUILayout.Button("Gather instantiate callbacks"))
        {
            var prefab = (HybridPrefab)target;
            prefab.GatherInstantiateCallbacks();
            EditorUtility.SetDirty(prefab);
        }
    }
}