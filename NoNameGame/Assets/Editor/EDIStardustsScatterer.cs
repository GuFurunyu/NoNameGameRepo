//ByCopilot
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class EDIStardustsScatterer : EditorWindow
{
    private GameObject prefab;
    private Vector3 center = Vector3.zero;
    private float cubeSize = 10f;
    [Range(0f, 0.5f)]
    private float randomFactor = 0.2f;

    [MenuItem("Tools/StardustsScatterer")]
    public static void ShowWindow()
    {
        GetWindow<EDIStardustsScatterer>("StardustsScatterer");
    }

    void OnGUI()
    {
        EditorGUILayout.LabelField("散布设置", EditorStyles.boldLabel);
        prefab = (GameObject)EditorGUILayout.ObjectField("预制体", prefab, typeof(GameObject), false);
        center = EditorGUILayout.Vector3Field("正方体中心", center);
        cubeSize = EditorGUILayout.FloatField("正方体边长", cubeSize);
        randomFactor = EditorGUILayout.Slider("随机扰动比例", randomFactor, 0f, 0.5f);

        EditorGUILayout.Space();

        if (GUILayout.Button("一键散布预制体实例"))
        {
            ScatterPrefabs();
        }
    }

    void ScatterPrefabs()
    {
        if (prefab == null)
        {
            Debug.LogWarning("请先指定需要散布的预制体！");
            return;
        }

        // 查找场景中所有该预制体的实例
        GameObject[] allObjects = FindObjectsOfType<GameObject>();
        List<Transform> targets = new List<Transform>();
        foreach (var go in allObjects)
        {
            if (PrefabUtility.GetCorrespondingObjectFromSource(go) == prefab)
            {
                targets.Add(go.transform);
            }
        }

        int count = targets.Count;
        if (count == 0)
        {
            Debug.LogWarning("场景中没有找到该预制体的实例。");
            return;
        }

        Undo.RecordObjects(targets.ToArray(), "Scatter Stardusts");

        // 均匀分布+扰动（Halton序列）
        for (int i = 0; i < count; i++)
        {
            Vector3 pos = new Vector3(
                center.x + (Halton(i + 1, 2) - 0.5f) * cubeSize,
                center.y + (Halton(i + 1, 3) - 0.5f) * cubeSize,
                center.z + (Halton(i + 1, 5) - 0.5f) * cubeSize
            );
            pos += new Vector3(
                Random.Range(-randomFactor, randomFactor) * cubeSize,
                Random.Range(-randomFactor, randomFactor) * cubeSize,
                Random.Range(-randomFactor, randomFactor) * cubeSize
            );
            targets[i].position = pos;
        }
        Debug.Log($"已将 {count} 个实例散布在正方体空间内。");
    }

    // Halton序列，用于均匀分布
    float Halton(int index, int b)
    {
        float f = 1f, r = 0f;
        while (index > 0)
        {
            f /= b;
            r += f * (index % b);
            index = Mathf.FloorToInt(index / b);
        }
        return r;
    }
}
