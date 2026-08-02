//PartlyByCopilot
//NotFullyUnderstood
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class EDIBlocksArranger : EditorWindow
{
    public GameObject curPlaneEmpty;
    public Texture2D blocksArrangementTexture;

    // 颜色到方块的映射列表
    public List<ColorMapping> colorMappings = new List<ColorMapping>();

    // 保存的映射列表
    [System.Serializable]
    public class MappingList
    {
        public string name;
        public List<ColorMapping> mappings = new List<ColorMapping>();
    }
    private List<MappingList> savedMappingLists = new List<MappingList>();

    // 重命名相关
    private string renameBuffer = "";
    private int renamingIndex = -1;

    // 滚动位置
    private Vector2 scrollPosition;

    private string[] faceDirectionStrings = new string[6]
        {"Front","Back","Left","Right","Top","Bottom" };

    private Vector3[] faceStableForwards = new Vector3[6]
        {Vector3.forward,Vector3.back,Vector3.right,Vector3.left,Vector3.down,Vector3.up };
    private Vector3[] faceStableUps = new Vector3[6]
        {Vector3.right,Vector3.right,Vector3.up,Vector3.up,Vector3.forward,Vector3.forward };
    private Vector3[] faceStableRights = new Vector3[6]
        {Vector3.down,Vector3.up,Vector3.back,Vector3.forward,Vector3.right,Vector3.left };

    private Vector3 curRoomStableForward;
    private Vector3 curRoomStableUp;
    private Vector3 curRoomStableRight;

    [System.Serializable]
    public class ColorMapping
    {
        public Color color;
        public GameObject block;
        public float depth;

        public ColorMapping()
        {
            color = Color.white;
            block = null;
            depth = 0f;
        }
    }

    // 用于序列化List
    [System.Serializable]
    private class SerializationWrapper<T>
    {
        public List<T> list;
        public SerializationWrapper(List<T> l) { list = l; }
    }

    // EditorPrefs Key
    private const string MappingListsPrefsKey = "EDIBlocksArranger_MappingLists";

    [MenuItem("Tools/BlocksArranger")]
    static void Init()
    {
        EditorWindow.GetWindow<EDIBlocksArranger>("BlocksArranger");
    }

    private void OnEnable()
    {
        LoadMappingLists();
    }

    private void OnDisable()
    {
        SaveMappingLists();
    }

    private void SaveMappingLists()
    {
        // 只序列化可序列化的部分
        string json = JsonUtility.ToJson(new SerializationWrapper<MappingList>(savedMappingLists));
        EditorPrefs.SetString(MappingListsPrefsKey, json);
    }

    private void LoadMappingLists()
    {
        if (EditorPrefs.HasKey(MappingListsPrefsKey))
        {
            string json = EditorPrefs.GetString(MappingListsPrefsKey);
            var wrapper = JsonUtility.FromJson<SerializationWrapper<MappingList>>(json);
            if (wrapper != null && wrapper.list != null)
                savedMappingLists = wrapper.list;
            else
                savedMappingLists = new List<MappingList>();
        }
        else
        {
            savedMappingLists = new List<MappingList>();
        }
    }

    void AutoSetRoomStableDirections()
    {
        if (curPlaneEmpty == null || curPlaneEmpty.transform.parent == null || curPlaneEmpty.transform.parent.parent == null)
            return;

        string parent2Name = curPlaneEmpty.transform.parent.parent.name;
        for (int i = 0; i < faceDirectionStrings.Length; i++)
        {
            if (parent2Name.Contains(faceDirectionStrings[i]))
            {
                curRoomStableForward = faceStableForwards[i];
                curRoomStableUp = faceStableUps[i];
                curRoomStableRight = faceStableRights[i];
                break;
            }
        }
    }

    void OnGUI()
    {
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

        EditorGUILayout.LabelField("Color Mappings", EditorStyles.boldLabel);
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

        // SaveMapping按钮
        if (GUILayout.Button("Save Mapping"))
        {
            int nextNum = savedMappingLists.Count + 1;
            savedMappingLists.Add(new MappingList
            {
                name = $"MappingList{nextNum}",
                mappings = new List<ColorMapping>(colorMappings)
            });
            SaveMappingLists();
        }

        // 显示保存的映射列表
        for (int i = 0; i < savedMappingLists.Count; i++)
        {
            EditorGUILayout.BeginHorizontal("box");
            if (renamingIndex == i)
            {
                renameBuffer = EditorGUILayout.TextField(renameBuffer, GUILayout.Width(120));
                if (GUILayout.Button("OK", GUILayout.Width(35)))
                {
                    savedMappingLists[i].name = renameBuffer;
                    renamingIndex = -1;
                    SaveMappingLists();
                }
                if (GUILayout.Button("Cancel", GUILayout.Width(50)))
                {
                    renamingIndex = -1;
                }
            }
            else
            {
                EditorGUILayout.LabelField(savedMappingLists[i].name, GUILayout.Width(120));
                if (GUILayout.Button("Rename", GUILayout.Width(55)))
                {
                    renamingIndex = i;
                    renameBuffer = savedMappingLists[i].name;
                }
            }
            if (GUILayout.Button("Load", GUILayout.Width(45)))
            {
                colorMappings = new List<ColorMapping>(savedMappingLists[i].mappings);
            }
            if (GUILayout.Button("Delete", GUILayout.Width(55)))
            {
                savedMappingLists.RemoveAt(i);
                SaveMappingLists();
                if (renamingIndex == i) renamingIndex = -1;
                i--;
                continue;
            }
            EditorGUILayout.EndHorizontal();
        }

        // AddNewMapping按钮
        if (GUILayout.Button("Add New Mapping"))
        {
            colorMappings.Add(new ColorMapping());
        }

        // 清空所有映射的按钮
        if (GUILayout.Button("Clear All Mappings"))
        {
            colorMappings.Clear();
        }

        EditorGUILayout.Space();

        // 显示所有颜色映射
        for (int i = 0; i < colorMappings.Count; i++)
        {
            EditorGUILayout.BeginVertical("box");

            // 映射标题和删除按钮
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField($"Mapping {i + 1}", EditorStyles.boldLabel);
            if (GUILayout.Button("Remove", GUILayout.Width(60)))
            {
                colorMappings.RemoveAt(i);
                i--;
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.EndVertical();
                continue;
            }
            EditorGUILayout.EndHorizontal();

            // 颜色选择
            colorMappings[i].color = EditorGUILayout.ColorField("Color", colorMappings[i].color);

            // 方块选择
            colorMappings[i].block = (GameObject)EditorGUILayout.ObjectField("Block", colorMappings[i].block, typeof(GameObject), false);

            // 深度设置
            colorMappings[i].depth = EditorGUILayout.FloatField("Depth", colorMappings[i].depth);

            EditorGUILayout.EndVertical();
            EditorGUILayout.Space();
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Scene Settings", EditorStyles.boldLabel);
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

        curPlaneEmpty = (GameObject)EditorGUILayout.ObjectField("Cur Plane Empty", curPlaneEmpty, typeof(GameObject), true);
        blocksArrangementTexture = (Texture2D)EditorGUILayout.ObjectField("Blocks Arrangement Texture", blocksArrangementTexture, typeof(Texture2D), false);

        AutoSetRoomStableDirections();

        EditorGUILayout.Space();

        // 生成和清除按钮
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Generate Blocks"))
        {
            GenerateBlocks();
        }

        if (GUILayout.Button("Clear Generated Blocks"))
        {
            Clear();
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.EndScrollView();
    }

    void GenerateBlocks()
    {
        // 验证必要参数
        if (blocksArrangementTexture == null)
        {
            Debug.LogError("请选择 Blocks Arrangement Texture！");
            return;
        }

        if (curPlaneEmpty == null)
        {
            Debug.LogError("请选择 Cur Plane Empty！");
            return;
        }

        if (curPlaneEmpty.transform.childCount > 0)
        {
            Debug.LogError("curPlaneEmpty 必须没有子物体才能生成方块，请先清空其子物体！");
            return;
        }

        // 验证映射
        List<ColorMapping> validMappings = new List<ColorMapping>();
        for (int i = 0; i < colorMappings.Count; i++)
        {
            var mapping = colorMappings[i];
            if (mapping.block == null)
            {
                Debug.LogWarning($"Mapping {i + 1} 没有指定方块，已跳过");
                continue;
            }
            validMappings.Add(mapping);
        }

        if (validMappings.Count == 0)
        {
            Debug.LogError("没有有效的颜色映射！请至少添加一个带有方块的映射。");
            return;
        }

        int width = blocksArrangementTexture.width;
        int height = blocksArrangementTexture.height;
        Vector2 center = new Vector2((width - 1) / 2f, (height - 1) / 2f);

        // 构建颜色到映射的字典
        Dictionary<Color, ColorMapping> colorToMapping = new Dictionary<Color, ColorMapping>();
        foreach (var mapping in validMappings)
        {
            if (!colorToMapping.ContainsKey(mapping.color))
            {
                colorToMapping.Add(mapping.color, mapping);
            }
            else
            {
                Debug.LogWarning($"颜色 {mapping.color} 有多个映射，将使用第一个匹配的映射");
            }
        }

        Undo.RegisterFullObjectHierarchyUndo(curPlaneEmpty, "Generate Blocks");

        int generatedCount = 0;

        // 生成方块
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Color pixelColor = blocksArrangementTexture.GetPixel(x, y);

                ColorMapping mapping;
                if (!colorToMapping.TryGetValue(pixelColor, out mapping))
                    continue;

                float offsetX = x - center.x;
                float offsetY = y - center.y;
                Vector3 localPos = offsetX * curRoomStableRight + offsetY * curRoomStableUp + mapping.depth * curRoomStableForward;

                GameObject obj = (GameObject)PrefabUtility.InstantiatePrefab(mapping.block, curPlaneEmpty.transform);
                obj.transform.localPosition = localPos;
                obj.transform.localRotation = Quaternion.identity;
                generatedCount++;
            }
        }

        EditorUtility.SetDirty(curPlaneEmpty);
        Debug.Log($"方块生成完成！共生成了 {generatedCount} 个方块。");
    }

    void Clear()
    {
        if (curPlaneEmpty == null)
        {
            Debug.LogError("curPlaneEmpty 未赋值！");
            return;
        }

        Undo.RegisterFullObjectHierarchyUndo(curPlaneEmpty, "Clear Blocks");

        // 逆序删除所有子物体
        for (int i = curPlaneEmpty.transform.childCount - 1; i >= 0; i--)
        {
            GameObject child = curPlaneEmpty.transform.GetChild(i).gameObject;
            Undo.DestroyObjectImmediate(child);
        }

        EditorUtility.SetDirty(curPlaneEmpty);
        Debug.Log("curPlaneEmpty 的所有子物体已清空。");
    }
}
