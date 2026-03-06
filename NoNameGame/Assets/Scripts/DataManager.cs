using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;


[DefaultExecutionOrder((int)ScriptsExecutionOrder.ExecutionOrder.dataManager)]
public class DataManager : MonoBehaviour
{
    Constants CONS;
    Variables VARS;
    UniversalFunctionsLibrary UFL;
    ScriptsExecutionController SEC;

    GameObject gameManager;

    public class WorldData
    {
        public Vector3[] roomPlanePositions = new Vector3[54];
        public Vector3[] roomPlaneEulerangles = new Vector3[54];
    }

    WorldData curWorldData = new WorldData();

    public class CatWorldData
    {
        //savePoint
        public int curActivatedSavePointIndex;
        public Vector3 curActivatedSavePointPosition;

        //exploredRooms
        public bool[] isRoomExplored = new bool[54];
    }

    CatWorldData curCatWorldData = new CatWorldData();

    public class KeyCodesData
    {
        public KeyCode upKeyCode;
        public KeyCode downKeyCode;
        public KeyCode leftKeyCode;
        public KeyCode rightKeyCode;
        public KeyCode jumpKeyCode;
        public KeyCode dashKeyCode;
    }

    KeyCodesData curKeyCodesData = new KeyCodesData();

    string tempPath;
    string tempJsonString;

    Vector3 tempVector;
    Transform tempTransform;

    #region ConstantsUsed
    float gridBreadth;
    int roomCoordBreadth;

    GameObject[] faces = new GameObject[6];
    Vector3[] faceStableForwards = new Vector3[6];

    GameObject[] roomPlanes = new GameObject[54];

    GameObject[] twistingCenters = new GameObject[6];
    #endregion

    #region VariablesUsed

    #endregion


    void Start()
    {
        gameManager = GameObject.Find("GameManager");

        CONS = gameManager.GetComponent<Constants>();
        VARS = gameManager.GetComponent<Variables>();
        UFL = gameManager.GetComponent<UniversalFunctionsLibrary>();
        SEC = gameManager.GetComponent<ScriptsExecutionController>();

        gridBreadth = CONS.gridBreadth;
        roomCoordBreadth = CONS.roomCoordBreadth;
        faces = CONS.faces;
        faceStableForwards = CONS.faceStableForwards;
        roomPlanes = CONS.roomPlanes;
        twistingCenters = CONS.twistingCenters;

        ReadWorldData();

        ReadCatWorldData();

        ReadKeyCodesData();
    }

    void Update()
    {
        if (VARS.isToWriteWorldData)
        {
            WriteWorldData();

            VARS.isToWriteWorldData = false;
        }

        if (VARS.isToWriteCatWorldData)
        {
            WriteCatWorldData();

            VARS.isToWriteCatWorldData = false;
        }

        if (VARS.isToWriteKeyCodesData)
        {
            WriteKeyCodesData();

            VARS.isToWriteKeyCodesData = false;
        }
    }

    #region WorldData
    void ReadWorldData()
    {
        tempPath = Path.Combine(Application.persistentDataPath, "Datas", "WorldData.txt");

        if (File.Exists(tempPath))
        {
            tempJsonString = File.ReadAllText(tempPath);
            curWorldData = JsonUtility.FromJson<WorldData>(tempJsonString);

            for (int i = 0; i < 54; i++)
            {
                tempTransform = roomPlanes[i].transform;

                //Debug.Log("position: " + curWorldData.roomPlanePositions[i]);
                //Debug.Log("eulerangles: " + curWorldData.roomPlaneEulerangles[i]);

                //if (i == 5)
                //{
                //    Debug.Log("position1: " + tempTransform.position);
                //    Debug.Log("eulerangles1: " + tempTransform.eulerAngles);
                //    Debug.Log("curActivatedSavePointPosition1: " + CONS.savePoints[VARS.curActivatedSavePointIndex].transform.position);
                //}

                tempTransform.position = curWorldData.roomPlanePositions[i];
                tempTransform.eulerAngles = curWorldData.roomPlaneEulerangles[i];

                //if(i==5)
                //{
                //    Debug.Log("position2: " + tempTransform.position);
                //    Debug.Log("eulerangles2: " + tempTransform.eulerAngles);
                //    Debug.Log("curActivatedSavePointPosition2: " + CONS.savePoints[VARS.curActivatedSavePointIndex].transform.position);
                //}

                //childToTheFaces
                for (int j = 0; j < 6; j++)
                {
                    tempVector = tempTransform.position - twistingCenters[j].transform.position;

                    //ifIsInTheFaceChildToIt
                    if (Mathf.Abs(Vector3.Dot(tempVector, faceStableForwards[j])) <= (roomCoordBreadth / 2 + 2) * gridBreadth)
                    {
                        tempTransform.SetParent(faces[j].transform, true);

                        break;
                    }
                }
            }
        }
    }

    void WriteWorldData()
    {
        tempPath = Path.Combine(Application.persistentDataPath, "Datas", "WorldData.txt");

        for (int i = 0; i < 54; i++)
        {
            tempTransform = roomPlanes[i].transform;

            curWorldData.roomPlanePositions[i] = tempTransform.position;
            curWorldData.roomPlaneEulerangles[i] = tempTransform.eulerAngles;
        }

        tempJsonString = JsonUtility.ToJson(curWorldData);

        File.WriteAllText(tempPath, tempJsonString);
    }
    #endregion

    #region CatWorldData
    void ReadCatWorldData()
    {
        tempPath = Path.Combine(Application.persistentDataPath, "Datas", "CatWorldData.txt");

        if (File.Exists(tempPath))
        {
            tempJsonString = File.ReadAllText(tempPath);
            curCatWorldData = JsonUtility.FromJson<CatWorldData>(tempJsonString);

            //savePoint
            VARS.curActivatedSavePointIndex = curCatWorldData.curActivatedSavePointIndex;
            VARS.curActivatedSavePointPosition = curCatWorldData.curActivatedSavePointPosition;

            //isRoomExplored
            VARS.isRoomExplored = curCatWorldData.isRoomExplored;

            VARS.isToActivateCurSavePoint = true;
        }
    }

    void WriteCatWorldData()
    {
        tempPath = Path.Combine(Application.persistentDataPath, "Datas", "CatWorldData.txt");

        //savePoint
        curCatWorldData.curActivatedSavePointIndex = VARS.curActivatedSavePointIndex;
        curCatWorldData.curActivatedSavePointPosition = VARS.curActivatedSavePointPosition;

        //isRoomExplored
        curCatWorldData.isRoomExplored = VARS.isRoomExplored;

        tempJsonString = JsonUtility.ToJson(curCatWorldData);

        File.WriteAllText(tempPath, tempJsonString);
    }
    #endregion

    #region KeyCodesData
    void ReadKeyCodesData()
    {
        tempPath = Path.Combine(Application.persistentDataPath, "Datas", "KeyCodesData.txt");

        if (File.Exists(tempPath))
        {
            tempJsonString = File.ReadAllText(tempPath);
            curKeyCodesData = JsonUtility.FromJson<KeyCodesData>(tempJsonString);

            VARS.upKeyCode = curKeyCodesData.upKeyCode;
            VARS.downKeyCode = curKeyCodesData.downKeyCode;
            VARS.leftKeyCode = curKeyCodesData.leftKeyCode;
            VARS.rightKeyCode = curKeyCodesData.rightKeyCode;
            VARS.jumpKeyCode = curKeyCodesData.jumpKeyCode;
            VARS.dashKeyCode = curKeyCodesData.dashKeyCode;

            VARS.curKeyCodes.Clear();

            VARS.curKeyCodes.Add(VARS.upKeyCode);
            VARS.curKeyCodes.Add(VARS.downKeyCode);
            VARS.curKeyCodes.Add(VARS.leftKeyCode);
            VARS.curKeyCodes.Add(VARS.rightKeyCode);
            VARS.curKeyCodes.Add(VARS.jumpKeyCode);
            VARS.curKeyCodes.Add(VARS.dashKeyCode);
        }
        //else
        //{
        //    Debug.Log("没有找到存档文件");
        //}
    }

    void WriteKeyCodesData()
    {
        tempPath = Path.Combine(Application.persistentDataPath, "Datas", "KeyCodesData.txt");

        curKeyCodesData.upKeyCode = VARS.upKeyCode;
        curKeyCodesData.downKeyCode = VARS.downKeyCode;
        curKeyCodesData.leftKeyCode = VARS.leftKeyCode;
        curKeyCodesData.rightKeyCode = VARS.rightKeyCode;
        curKeyCodesData.jumpKeyCode = VARS.jumpKeyCode;
        curKeyCodesData.dashKeyCode = VARS.dashKeyCode;

        tempJsonString = JsonUtility.ToJson(curKeyCodesData);

        File.WriteAllText(tempPath, tempJsonString);
    }
    #endregion
}
