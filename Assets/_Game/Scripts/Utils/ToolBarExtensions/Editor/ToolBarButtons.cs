using UnityEditor;
using UnityEngine;
using UnityToolbarExtender;

[InitializeOnLoad]
public class ToolBarButtons
{
    static ToolBarButtons()
    {
        ToolbarExtender.LeftToolbarGUI.Add(OnToolbarGUILeft);
    }

    private static void OnToolbarGUILeft()
    {
        GUILayout.FlexibleSpace();

        if (GUILayout.Button(new GUIContent("DeleteSave", "Deletes Save Data")))
        {
            DeleteSave();
        }
    }

    private static void DeleteSave()
    {
        PlayerPrefs.DeleteAll();
    }
}