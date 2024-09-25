using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Lock))]
public class LockEditor : Editor
{
    public override void OnInspectorGUI() {
        DrawDefaultInspector();
        
        Lock targetLock = target as Lock;
        
        GUILayout.BeginHorizontal("box");
        
        if(GUILayout.Button("<"))
            targetLock.DecreaseIndexOnDial();
        
        if(GUILayout.Button(">"))
            targetLock.IncreaseIndexOnDial();
        
        GUILayout.EndHorizontal();
    }
}
