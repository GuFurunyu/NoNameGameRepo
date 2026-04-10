//ByCopilot
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Reflection;

[InitializeOnLoad]
public static class EDIAutoLinkerForPrefabsToConstantsList
{
    // 配置：预制体名 => Constants中List字段名
    private static readonly Dictionary<string, string> prefabToListField = new Dictionary<string, string>
    {
        {"GateBlock", "gates" },
        {"EdgeGateBlock", "edgeGates" },
        {"EdgeGateTriggerBlock", "edgeGateTriggers" },
        {"SavePointBlock", "savePoints" },
        {"KeyBlock","keys" },
        {"LockedBlock", "locks" },
        {"MinimapKey","minimapKeys" },
        {"MinimapLock","minimapLocks" }
        // 可扩展
        // { "NewPrefabName", "targetListFieldName" },
    };

    static EDIAutoLinkerForPrefabsToConstantsList()
    {
        EditorApplication.hierarchyChanged += OnHierarchyChanged;
    }

    private static void OnHierarchyChanged()
    {
        // 查找GameManager和Constants
        GameObject gameManager = GameObject.Find("GameManager");
        if (gameManager == null) return;
        var constants = gameManager.GetComponent("Constants");
        if (constants == null) return;

        // 获取场景所有对象
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();

        foreach (var kvp in prefabToListField)
        {
            string prefabName = kvp.Key;
            string listFieldName = kvp.Value;

            // 获取Constants中的List<GameObject>
            FieldInfo listField = constants.GetType().GetField(listFieldName, BindingFlags.Public | BindingFlags.Instance);
            if (listField == null) continue;
            var list = listField.GetValue(constants) as IList<GameObject>;
            if (list == null) continue;

            // 1. 移除列表中已被删除或Missing的对象
            bool changed = false;
            for (int i = list.Count - 1; i >= 0; i--)
            {
                var go = list[i];
                if (go == null)
                {
                    Undo.RecordObject(constants, $"Remove missing GameObject from {listFieldName}");
                    list.RemoveAt(i);
                    changed = true;
                }
            }

            // 2. 添加场景中所有符合条件的对象
            foreach (var obj in allObjects)
            {
                if (obj.name.StartsWith(prefabName) && obj.scene.IsValid())
                {
                    if (!list.Contains(obj))
                    {
                        Undo.RecordObject(constants, $"Add {obj.name} to {listFieldName}");
                        list.Add(obj);
                        changed = true;
                    }
                }
            }

            if (changed)
            {
                EditorUtility.SetDirty(constants);
            }
        }
    }
}
