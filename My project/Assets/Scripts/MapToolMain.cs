using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MapToolMain : EditorWindow
{

    string myString = "Hello World";
    bool groupEnabled;
    bool myBool = true;
    float myFloat = 1.23f;
    Vector3 CursorPosition = new Vector3(0, 0, 0);
    public GameObject TempTile;
    public GameObject MapTile;

    [MenuItem("Window/MapTool")]

    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(MapToolMain));
    }

    private void OnEnable()
    {
        SceneView.duringSceneGui += OnSceneGUI;
    }


    private void OnDisable()
    {
        SceneView.duringSceneGui += OnSceneGUI;
    }



    void OnGUI()
    {
        GUILayout.Label("Base Settings", EditorStyles.boldLabel);
        TempTile = (GameObject)EditorGUILayout.ObjectField("오브젝트 필드 11", TempTile, typeof(GameObject), true);
        MapTile = (GameObject)EditorGUILayout.ObjectField("오브젝트 필드", MapTile, typeof(GameObject), true);
        myString = EditorGUILayout.TextField("Text Field", myString);


        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("버튼 생성", GUILayout.Width(120), GUILayout.Height(30)))
        {

        }
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();

        groupEnabled = EditorGUILayout.BeginToggleGroup("Optional Settings", groupEnabled);
        myBool = EditorGUILayout.Toggle("Toggle", myBool);
        myFloat = EditorGUILayout.Slider("Slider", myFloat, -3, 3);

        EditorGUILayout.EndToggleGroup();
    }

    void OnSceneGUI(SceneView sceneView)
    {
        if (Selection.activeGameObject != null)
        {
            if(Selection.activeGameObject.name == "Plane")
            {
                Selection.activeGameObject = TempTile;
            }
        }
        if (Event.current.type != EventType.MouseDown || Event.current.button != 0) return;

        var mousePosition = Event.current.mousePosition * EditorGUIUtility.pixelsPerPoint;
        mousePosition.y = Camera.current.pixelHeight - mousePosition.y;

        var Ray = Camera.current.ScreenPointToRay(mousePosition);

        if (Physics.Raycast(Ray, out RaycastHit hit))
        {
            Vector3Int TempVector = new Vector3Int((int)hit.point.x, (int)hit.point.y, (int)hit.point.z);
            GameObject Temp = (GameObject)PrefabUtility.InstantiatePrefab(MapTile);
            Temp.transform.position = TempVector;
        }

        Selection.activeGameObject = TempTile;
    }

}
