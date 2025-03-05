using UnityEditor;
using UnityEngine;
using Assets.scripts.MiniMapScripts.AStar;

public class window :EditorWindow {

    public Node node;
    private GameObject selectedobject;
    [MenuItem("Custom/SampleWindow")]

    private static void Initialize()
    {
        EditorWindow.GetWindow(typeof(window));
    }

    private void OnGUI()
    {
        GUILayout.Label("Base Settings", EditorStyles.boldLabel);
        node =(Node)EditorGUILayout.ObjectField(node,typeof(Node),true);
        if (GUILayout.Button("instantiate"))
        {
            Build();
        }
        GUILayout.Label("SelectedItem", EditorStyles.boldLabel);
        selectedobject = Selection.activeGameObject;
        EditorGUILayout.ObjectField(selectedobject, typeof(Node), true);   
    }

    private void Build()
    {
        Instantiate(node, Vector3.zero, Quaternion.identity);
    }
}
