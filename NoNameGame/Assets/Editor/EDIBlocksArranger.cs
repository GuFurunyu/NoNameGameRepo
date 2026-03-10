//PartlyByCopilot
//NotFullyUnderstood
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class EDIBlocksArranger : EditorWindow
{
    public GameObject curPlaneEmpty;

    public int maxBlockTypes = 5;
    public int blockTypeCount = 2;

    public List<GameObject> blocks = new List<GameObject>();
    public List<float> blockDepths = new List<float>();

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

    public Texture2D blocksArrangementTexture;

    void AutoSetRoomStableDirections()
    {
        if (curPlaneEmpty == null || curPlaneEmpty.transform.parent == null || curPlaneEmpty.transform.parent.parent == null)
            return;

        string parent2Name = curPlaneEmpty.transform.parent.parent.name;
        for (int i = 0; i < faceDirectionStrings.Length; i++)
        {
            if (parent2Name.Contains(faceDirectionStrings[i]))
            {
                Debug.Log("enter");

                curRoomStableForward = faceStableForwards[i];
                curRoomStableUp = faceStableUps[i];
                curRoomStableRight = faceStableRights[i];
                break;
            }
        }
    }


    [MenuItem("Tools/BlocksArranger")]
    static void Init()
    {
        EditorWindow.GetWindow<EDIBlocksArranger>("BlocksArranger");
    }

    void OnGUI()
    {
        maxBlockTypes = EditorGUILayout.IntField("Block Type Max", maxBlockTypes);
        blockTypeCount = EditorGUILayout.IntSlider("Block Type Count", blockTypeCount, 2, maxBlockTypes);

        // 保证列表长度（~？）
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

        AutoSetRoomStableDirections();

        blocksArrangementTexture = (Texture2D)EditorGUILayout.ObjectField("Blocks Arrangement Texture", blocksArrangementTexture, typeof(Texture2D), false);

        EditorGUILayout.Space();

        if (GUILayout.Button("Generate"))
        {
            GenerateBlocks();
        }

        if (GUILayout.Button("Clear"))
        {
            Clear();
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

        // 2. 构建明度到block索引的映射（只映射前几个方块，多余的方块不参与）
        Dictionary<float, int> brightnessToBlockIdx = new Dictionary<float, int>();
        int idx = 0;
        foreach (float b in uniqueBrightness)
        {
            if (idx < blockTypeCount)
                brightnessToBlockIdx[b] = idx++;
            else
                break;
        }

        Undo.RegisterFullObjectHierarchyUndo(curPlaneEmpty, "Generate Blocks");

        // 3. 生成方块
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float brightness = blocksArrangementTexture.GetPixel(x, y).grayscale;
                if (brightness >= 0.999f)
                    continue;

                int blockIdx;
                if (!brightnessToBlockIdx.TryGetValue(brightness, out blockIdx))
                    continue; // 没有映射的明度不生成方块

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
