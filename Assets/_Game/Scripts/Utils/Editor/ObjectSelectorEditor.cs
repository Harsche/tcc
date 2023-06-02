using UnityEditor;
using UnityEngine;

public class ObjectSelectorEditor : EditorWindow
{
    private float minimumZPosition = 0f; // Minimum Z position
    private string searchString = "Object"; // Specified string to search in object names

    [MenuItem("Window/Object Selector")]
    public static void ShowWindow()
    {
        GetWindow<ObjectSelectorEditor>("Object Selector");
    }

    private void OnGUI()
    {
        GUILayout.Label("Select Objects by Z Position and Name", EditorStyles.boldLabel);

        minimumZPosition = EditorGUILayout.FloatField("Minimum Z Position", minimumZPosition);
        searchString = EditorGUILayout.TextField("Search String", searchString);

        if (GUILayout.Button("Select"))
        {
            SelectObjects();
        }
    }

    private void SelectObjects()
    {
        GameObject[] allObjects = FindObjectsOfType<GameObject>(); // Get all game objects in the scene

        Selection.objects = new Object[0]; // Clear the current selection

        foreach (GameObject obj in allObjects)
        {
            if (obj.transform.position.z >= minimumZPosition && obj.name.Contains(searchString))
            {
                Selection.objects = AddObjectToSelection(Selection.objects, obj);
                Debug.Log("Selected: " + obj.name);
            }
        }
    }

    private Object[] AddObjectToSelection(Object[] currentSelection, Object obj)
    {
        Object[] newSelection = new Object[currentSelection.Length + 1];
        for (int i = 0; i < currentSelection.Length; i++)
        {
            newSelection[i] = currentSelection[i];
        }
        newSelection[newSelection.Length - 1] = obj;
        return newSelection;
    }
}