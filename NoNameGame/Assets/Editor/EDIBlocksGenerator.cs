//ByCopilot
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class EDIBlocksGenerator : EditorWindow
{
    public int maxBlockTypes = 5;
    public int blockTypeCount = 2;

    public List<GameObject> blocks = new List<GameObject>();
    public List<float> blockDepths = new List<float>();

    public GameObject curPlaneEmpty;

    public Vector3 curRoomStableForward;
    public Vector3 curRoomStableUp;
    public Vector3 curRoomStableRight;

    public Texture2D blocksArrangementTexture;

    [MenuItem("Tools/BlocksGenerator")]
    static void Init()
    {
        EditorWindow.GetWindow<EDIBlocksGenerator>("BlocksGenerator");
    }

    void OnGUI()
    {
        maxBlockTypes = EditorGUILayout.IntField("Block Type Max", maxBlockTypes);
        blockTypeCount = EditorGUILayout.IntSlider("Block Type Count", blockTypeCount, 2, maxBlockTypes);

        // 保证列表长度
        while (blocks.Count < blockTypeCount) blocks.Add(null);
        while (blocks.Count > blockTypeCount) blocks.RemoveAt(blocks.Count - 1);
        while (blockDepths.Count < blockTypeCount) blockDepths.Add(0f);
        while (blockDepths.Count > blockTypeCount) blockDepths.RemoveAt(blockDepths.Count - 1);

        for (int i = 0; i < blockTypeCount; i++)
        {
            blocks[i] = (GameObject)EditorGUILayout.ObjectField($"Block {i + 1}", blocks[i], typeof(GameObject), false);
            blockDepths[i] = EditorGUILayout.FloatField($"Block {i + 1} Depth", blockDepths[i]);
        }

        curPlaneEmpty = (GameObject)EditorGUILayout.ObjectField("Cur Plane Empty", curPlaneEmpty, typeof(GameObject), true);

        curRoomStableForward = EditorGUILayout.Vector3Field("Room Stable Forward", curRoomStableForward);
        curRoomStableUp = EditorGUILayout.Vector3Field("Room Stable Up", curRoomStableUp);
        curRoomStableRight = EditorGUILayout.Vector3Field("Room Stable Right", curRoomStableRight);

        blocksArrangementTexture = (Texture2D)EditorGUILayout.ObjectField("Blocks Arrangement Texture", blocksArrangementTexture, typeof(Texture2D), false);

        EditorGUILayout.Space();

        if (GUILayout.Button("Generate"))
        {
            GenerateBlocks();
        }
    }
    void GenerateBlocks()
    {
        if (blocksArrangementTexture == null || curPlaneEmpty == null)
        {
            Debug.LogError("请确保所有必要的变量都已赋值！");
            return;
        }
        if (curPlaneEmpty.transform.childCount > 0)
        {
            Debug.LogError("curPlaneEmpty 必须没有子物体才能生成方块，请先清空其子物体！");
            return;
        }
        for (int i = 0; i < blockTypeCount; i++)
        {
            if (blocks[i] == null)
            {
                Debug.LogError($"Block {i + 1} 未赋值！");
                return;
            }
        }

        int width = blocksArrangementTexture.width;
        int height = blocksArrangementTexture.height;
        Vector2 center = new Vector2((width - 1) / 2f, (height - 1) / 2f);

        // 1. 收集所有明度种类
        SortedSet<float> uniqueBrightness = new SortedSet<float>();
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float brightness = blocksArrangementTexture.GetPixel(x, y).grayscale;
                // 纯白色不生成方块
                if (brightness >= 0.999f)
                    continue;
                uniqueBrightness.Add(brightness);
            }
        }

        // 2. 检查明度种类数量
        if (uniqueBrightness.Count < blockTypeCount)
        {
            Debug.LogError($"明度种类数量({uniqueBrightness.Count})少于方块种类({blockTypeCount})，请检查贴图或减少方块种类数。");
            return;
        }

        // 3. 构建明度到block索引的映射
        Dictionary<float, int> brightnessToBlockIdx = new Dictionary<float, int>();
        int idx = 0;
        foreach (float b in uniqueBrightness)
        {
            if (idx < blockTypeCount)
                brightnessToBlockIdx[b] = idx++;
            else
                brightnessToBlockIdx[b] = -1; // 多余的明度映射为空
        }

        Undo.RegisterFullObjectHierarchyUndo(curPlaneEmpty, "Generate Blocks");

        // 4. 生成方块
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float brightness = blocksArrangementTexture.GetPixel(x, y).grayscale;
                if (brightness >= 0.999f)
                    continue;

                int blockIdx;
                if (!brightnessToBlockIdx.TryGetValue(brightness, out blockIdx) || blockIdx == -1)
                    continue; // 不生成方块

                float offsetX = x - center.x;
                float offsetY = y - center.y;
                Vector3 localPos = offsetX * curRoomStableRight + offsetY * curRoomStableUp + blockDepths[blockIdx] * curRoomStableForward;

                GameObject obj = (GameObject)PrefabUtility.InstantiatePrefab(blocks[blockIdx], curPlaneEmpty.transform);
                obj.transform.localPosition = localPos;
                obj.transform.localRotation = Quaternion.identity;
            }
        }

        EditorUtility.SetDirty(curPlaneEmpty);
        Debug.Log("Blocks generated based on texture.");
    }
}
