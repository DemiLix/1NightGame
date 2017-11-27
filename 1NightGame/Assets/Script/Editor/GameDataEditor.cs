using Lix.NightGame.Enemy;
using System;
using System.IO;
using UnityEditor;
using UnityEngine;

public class GameDataEditor : EditorWindow
{

    public WaveData gameData;


    [MenuItem("Window/Game Data Editor")]
    static void Init()
    {
        EditorWindow.GetWindow(typeof(GameDataEditor)).Show();
    }

    void OnGUI()
    {
        if (gameData != null)
        {
            SerializedObject serializedObject = new SerializedObject(this);
            SerializedProperty serializedProperty = serializedObject.FindProperty("gameData");
            EditorGUILayout.PropertyField(serializedProperty, true);

            serializedObject.ApplyModifiedProperties();

            if (GUILayout.Button("Save data"))
            {
                SaveGameData();
            }
        }

        if (GUILayout.Button("Load data"))
        {
            LoadGameData();
        }
    }

    private void LoadGameData()
    {
        string filePath = Application.dataPath + Consts.WAVES_JSON_PATHNAME;
        if (File.Exists(filePath))
        {
            string dataAsJson = File.ReadAllText(filePath);
            gameData = JsonUtility.FromJson<WaveData>(dataAsJson);
        }
        else
        {
            gameData = new WaveData();
        }
    }

    private void SaveGameData()
    {
        bool IsSuccessSave = true;
        try
        {
            string dataAsJson = JsonUtility.ToJson(gameData);

            string filePath = Application.dataPath + Consts.WAVES_JSON_PATHNAME;
            File.WriteAllText(filePath, dataAsJson);
        }catch(Exception exc)
        {
            IsSuccessSave = false;
            Debug.LogException(exc,this);
        }
        if (IsSuccessSave)
        {
            Debug.Log("Save data is success");
        }
    }
}