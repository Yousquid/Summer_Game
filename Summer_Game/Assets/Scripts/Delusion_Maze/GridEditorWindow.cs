using UnityEngine;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class GridEditorWindow : EditorWindow
{
    private int defaultWidth = 10;
    private int defaultHeight = 10;
    private int cellSize = 20;

    private List<LevelData> levels = new List<LevelData>();
    private int currentLevelIndex = 0;

    private LevelData currentLevel;

    [MenuItem("Tools/Grid Level Editor")]
    public static void OpenWindow()
    {
        GetWindow<GridEditorWindow>("Grid Editor");
    }

    private void OnEnable()
    {
        if (levels.Count == 0)
        {
            CreateNewLevel(defaultWidth, defaultHeight);
        }
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("�ؿ�����", EditorStyles.boldLabel);

        // Ĭ�ϴ�С�����
        defaultWidth = EditorGUILayout.IntField("Ĭ�Ͽ��", defaultWidth);
        defaultHeight = EditorGUILayout.IntField("Ĭ�ϸ߶�", defaultHeight);

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("�½��ؿ�", GUILayout.Height(25)))
        {
            CreateNewLevel(defaultWidth, defaultHeight);
        }

        if (GUILayout.Button("���浱ǰ�ؿ�", GUILayout.Height(25)))
        {
            SaveCurrentLevel();
        }

        if (GUILayout.Button("�л��ؿ�", GUILayout.Height(25)))
        {
            if (levels.Count > 0)
            {
                currentLevelIndex = (currentLevelIndex + 1) % levels.Count;
                currentLevel = levels[currentLevelIndex];
            }
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();

        if (currentLevel == null) return;

        // ��ʾ��ǰ�ؿ���Ϣ
        EditorGUILayout.LabelField($"��ǰ�ؿ���{currentLevelIndex + 1}/{levels.Count}");
        EditorGUILayout.LabelField($"�ߴ磺{currentLevel.width} �� {currentLevel.height}");

        EditorGUILayout.Space();

        // �������
        for (int y = 0; y < currentLevel.height; y++)
        {
            EditorGUILayout.BeginHorizontal();
            for (int x = 0; x < currentLevel.width; x++)
            {
                int value = currentLevel.GetCell(x, y);

                Color oldColor = GUI.backgroundColor;
                GUI.backgroundColor = GetColorForValue(value);

                if (GUILayout.Button(value.ToString(), GUILayout.Width(cellSize), GUILayout.Height(cellSize)))
                {
                    int nextValue = (value + 1) % 5; 
                    currentLevel.SetCell(x, y, nextValue);
                }

                GUI.backgroundColor = oldColor;
            }
            EditorGUILayout.EndHorizontal();
        }
    }

    private void CreateNewLevel(int w, int h)
    {
        if (w <= 0 || h <= 0)
        {
            Debug.LogError("�ؿ��ߴ������� 0��");
            return;
        }

        LevelData newLevel = new LevelData(w, h);
        levels.Add(newLevel);
        currentLevel = newLevel;
        currentLevelIndex = levels.Count - 1;
    }

    private void SaveCurrentLevel()
    {
        string path = EditorUtility.SaveFilePanel("����ؿ�Ϊ JSON", Application.dataPath, $"LevelData_{levels.Count}", "json");
        if (!string.IsNullOrEmpty(path))
        {
            string json = JsonUtility.ToJson(currentLevel, true);
            System.IO.File.WriteAllText(path, json);
            AssetDatabase.Refresh();
            Debug.Log("�ؿ��ѱ��浽: " + path);
        }
    }

    private Color GetColorForValue(int value)
    {
        switch (value)
        {
            case 0: return Color.white;   // �յ�
            case 1: return Color.black;   // ǽ
            case 2: return Color.green;   // ���
            case 3: return Color.red;     // �յ�
            case 4: return Color.yellow;  // ����
            default: return Color.gray;
        }
    }
}