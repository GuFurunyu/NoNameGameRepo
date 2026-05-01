using System.Collections.Generic;
using System.IO;
using System.Reflection;
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
        //rooms
        public Vector3[] roomPlanePositions = new Vector3[54];
        public Vector3[] roomPlaneEulerangles = new Vector3[54];
        public Vector3[] roomCenters = new Vector3[54];
        public Vector3[] roomStableForwards = new Vector3[54];
        public Vector3[] roomStableUps = new Vector3[54];
        public Vector3[] roomStableRights = new Vector3[54];

        //minimapRooms
        public Vector3[] minimapRoomPlanePositions = new Vector3[54];
        public Vector3[] minimapRoomPlaneEulerangles = new Vector3[54];
    }

    WorldData curWorldData = new WorldData();

    public class CatWorldData
    {
        //curRoom
        public int curRoomIndex;

        //curPosition
        public Vector3 curCatIniPosition;

        //savePoints
        public int curActivatedSavePointIndex;
        public Vector3 curActivatedSavePointPosition;
        public int curActivatedSavePointRoomIndex;

        //exploredRooms
        public bool[] isRoomExplored = new bool[54];

        //keysAndLocks
        public List<int> deactivatedKeyIndexes = new List<int>();
        public List<int> deactivatedLockIndexes = new List<int>();
        public bool isCarryingAKey;
        public int curCarriedKeyIndex;
        public int curCarriedKeyIniRoomIndex;
        public Vector3 curCarriedKeyIniLocalPosition;
        public List<int> deactivatedMinimapKeyIndexes = new List<int>();
        public List<int> deactivatedMinimapLockIndexes = new List<int>();
    }

    CatWorldData curCatWorldData = new CatWorldData();

    public class KeyCodesData
    {
        public KeyCode upKeyCode;
        public KeyCode downKeyCode;
        public KeyCode leftKeyCode;
        public KeyCode rightKeyCode;
        public KeyCode jumpKeyCode;
        public KeyCode grabKeyCode;
        //public KeyCode dashKeyCode;
        public KeyCode acceKeyCode;
        public KeyCode confirmKeyCode;
        public KeyCode backKeyCode;
    }

    KeyCodesData curKeyCodesData = new KeyCodesData();

    string tempPath;
    string tempJsonString;

    Vector3 tempVector;
    Transform tempTransform;

    #region ConstantsUsed
    float gridBreadth;
    int roomCoordBreadth;
    int minimapRoomCoordBreadth;

    GameObject[] faces = new GameObject[6];
    Vector3[] faceStableForwards = new Vector3[6];

    GameObject[] roomPlanes = new GameObject[54];

    GameObject storedActivatedSavePointBlock;

    GameObject[] twistingCenters = new GameObject[6];

    List<GameObject> keys = new List<GameObject>();
    List<GameObject> locks = new List<GameObject>();

    GameObject[] minimapFaces = new GameObject[6];
    GameObject[] minimapRoomPlanes = new GameObject[54];
    GameObject[] minimapTwistingCenters = new GameObject[6];

    List<GameObject> minimapKeys = new List<GameObject>();
    List<GameObject> minimapLocks = new List<GameObject>();

    Material connectedGateColor;

    Transform catTransform;

    GameObject catIniPositionPoint;
    #endregion

    #region VariablesUsed
    Vector3[] roomCenters = new Vector3[54];
    Vector3[] roomStableForwards = new Vector3[54];
    Vector3[] roomStableUps = new Vector3[54];
    Vector3[] roomStableRights = new Vector3[54];
    #endregion


    void Start()
    {
        gameManager = GameObject.Find("GameManager");

        CONS = gameManager.GetComponent<Constants>();
        VARS = gameManager.GetComponent<Variables>();
        UFL = gameManager.GetComponent<UniversalFunctionsLibrary>();
        SEC = gameManager.GetComponent<ScriptsExecutionController>();

        #region ImportConstants
        gridBreadth = CONS.gridBreadth;
        roomCoordBreadth = CONS.roomCoordBreadth;
        minimapRoomCoordBreadth = CONS.minimapRoomCoordBreadth;
        faces = CONS.faces;
        faceStableForwards = CONS.faceStableForwards;
        roomPlanes = CONS.roomPlanes;
        storedActivatedSavePointBlock = CONS.storedActivatedSavePointBlock;
        twistingCenters = CONS.twistingCenters;
        keys = CONS.keys;
        locks = CONS.locks;
        minimapFaces = CONS.minimapFaces;
        minimapRoomPlanes = CONS.minimapRoomPlanes;
        minimapTwistingCenters = CONS.minimapTwistingCenters;
        minimapKeys = CONS.minimapKeys;
        minimapLocks = CONS.minimapLocks;
        connectedGateColor = CONS.connectedGateColor;
        catTransform = CONS.catTransform;
        catIniPositionPoint = CONS.catIniPositionPoint;
        #endregion

        #region ImportReferenceVariables
        roomCenters = VARS.roomCenters;
        roomStableForwards = VARS.roomStableForwards;
        roomStableUps = VARS.roomStableUps;
        roomStableRights = VARS.roomStableRights;
        #endregion

        ReadWorldData();

        //WriteCatWorldData();

        ReadCatWorldData();

        ReadKeyCodesData();
    }

    void Update()
    {
        #region ImportValueVariables
        #endregion

        if (VARS.IsToWriteWorldData)
        {
            WriteWorldData();

            VARS.IsToWriteWorldData = false;
        }

        if (VARS.IsToWriteCatWorldData)
        {
            WriteCatWorldData();

            VARS.IsToWriteCatWorldData = false;
        }

        if (VARS.IsToWriteKeyCodesData)
        {
            WriteKeyCodesData();

            VARS.IsToWriteKeyCodesData = false;
        }

        VARS.IsWritingAllData = false;
    }

    #region WorldData
    void ReadWorldData()
    {
        tempPath = Path.Combine(Application.persistentDataPath, /*"Datas",*/ "WorldData.txt");

        if (File.Exists(tempPath))
        {
            tempJsonString = File.ReadAllText(tempPath);
            curWorldData = JsonUtility.FromJson<WorldData>(tempJsonString);

            for (int i = 0; i < 54; i++)
            {
                //rooms
                tempTransform = roomPlanes[i].transform;

                tempTransform.position = UFL.Vector3RoundToInt(curWorldData.roomPlanePositions[i]);
                tempTransform.eulerAngles = UFL.Vector3RoundToInt(curWorldData.roomPlaneEulerangles[i]);
                roomCenters[i] = curWorldData.roomCenters[i];
                roomStableForwards[i] = curWorldData.roomStableForwards[i];
                roomStableUps[i] = curWorldData.roomStableUps[i];
                roomStableRights[i] = curWorldData.roomStableRights[i];

                //roomPlanesChildToTheFaces
                for (int j = 0; j < 6; j++)
                {
                    //tempVector = tempTransform.position - twistingCenters[j].transform.position;

                    //ifIsInTheFaceChildToIt
                    if (/*Mathf.Abs(Vector3.Dot(tempVector, faceStableForwards[j])) <= (roomCoordBreadth / 2 + 2) * gridBreadth*/
                        UFL.IsPlaneInTheFace(i, j + 1))
                    {
                        tempTransform.SetParent(faces[j].transform, true);

                        break;
                    }
                }

                //minimapRooms
                tempTransform = minimapRoomPlanes[i].transform;

                tempTransform.position = UFL.Vector3RoundToInt(curWorldData.minimapRoomPlanePositions[i]);
                tempTransform.eulerAngles = UFL.Vector3RoundToInt(curWorldData.minimapRoomPlaneEulerangles[i]);

                //minimapRoomPlanesChildToTheFaces
                for (int j = 0; j < 6; j++)
                {
                    //tempVector = tempTransform.position - minimapTwistingCenters[j].transform.position;

                    //ifIsInTheFaceChildToIt
                    if (/*Mathf.Abs(Vector3.Dot(tempVector, faceStableForwards[j])) <= (minimapRoomCoordBreadth / 2 + 2) * gridBreadth*/
                        UFL.IsMinimapPlaneInTheFace(i, j + 1))
                    {
                        tempTransform.SetParent(minimapFaces[j].transform, true);

                        break;
                    }
                }
            }
        }
    }

    void WriteWorldData()
    {
        tempPath = Path.Combine(Application.persistentDataPath, /*"Datas",*/ "WorldData.txt");

        for (int i = 0; i < 54; i++)
        {
            //rooms
            tempTransform = roomPlanes[i].transform;

            curWorldData.roomPlanePositions[i] = UFL.Vector3RoundToInt(tempTransform.position);
            curWorldData.roomPlaneEulerangles[i] = UFL.Vector3RoundToInt(tempTransform.eulerAngles);
            curWorldData.roomCenters[i] = roomCenters[i];
            curWorldData.roomStableForwards[i] = roomStableForwards[i];
            curWorldData.roomStableUps[i] = roomStableUps[i];
            curWorldData.roomStableRights[i] = roomStableRights[i];

            //minimapRooms
            tempTransform = minimapRoomPlanes[i].transform;

            curWorldData.minimapRoomPlanePositions[i] = UFL.Vector3RoundToInt(tempTransform.position);
            curWorldData.minimapRoomPlaneEulerangles[i] = UFL.Vector3RoundToInt(tempTransform.eulerAngles);
        }

        tempJsonString = JsonUtility.ToJson(curWorldData);

        File.WriteAllText(tempPath, tempJsonString);
    }
    #endregion

    #region CatWorldData
    void ReadCatWorldData()
    {
        tempPath = Path.Combine(Application.persistentDataPath, /*"Datas",*/ "CatWorldData.txt");

        if (File.Exists(tempPath))
        {
            tempJsonString = File.ReadAllText(tempPath);
            curCatWorldData = JsonUtility.FromJson<CatWorldData>(tempJsonString);

            //curRoom
            VARS.curRoomIndex = curCatWorldData.curRoomIndex;

            //curPosition
            catIniPositionPoint.transform.position = curCatWorldData.curCatIniPosition;
            catTransform.position = curCatWorldData.curCatIniPosition;

            //savePoint
            VARS.curActivatedSavePointIndex = curCatWorldData.curActivatedSavePointIndex;
            VARS.curActivatedSavePointPosition = curCatWorldData.curActivatedSavePointPosition;
            VARS.curActivatedSavePointRoomIndex = curCatWorldData.curActivatedSavePointRoomIndex;

            //isRoomExplored
            VARS.IsRoomExplored = curCatWorldData.isRoomExplored;

            //keysAndLocks
            VARS.deactivatedKeyIndexes = curCatWorldData.deactivatedKeyIndexes;
            VARS.deactivatedLockIndexes = curCatWorldData.deactivatedLockIndexes;
            VARS.IsCarryingAKey = curCatWorldData.isCarryingAKey;
            VARS.curCarriedKey = keys[curCatWorldData.curCarriedKeyIndex];
            VARS.curCarriedKeyIniRoomIndex = curCatWorldData.curCarriedKeyIniRoomIndex;
            VARS.curCarriedKeyIniParent = roomPlanes[curCatWorldData.curCarriedKeyIniRoomIndex].transform.GetChild(0).gameObject;
            VARS.curCarriedKeyIniLocalPosition = curCatWorldData.curCarriedKeyIniLocalPosition;
            for (int i = 0; i < keys.Count; i++)
            {
                if (curCatWorldData.deactivatedKeyIndexes.Contains(i))
                {
                    keys[i].SetActive(false);
                }
            }
            for (int i = 0; i < locks.Count; i++)
            {
                if (curCatWorldData.deactivatedLockIndexes.Contains(i))
                {
                    locks[i].SetActive(false);
                }
            }

            //minimapKeysAndLocks
            VARS.deactivatedMinimapKeyIndexes = curCatWorldData.deactivatedMinimapKeyIndexes;
            VARS.deactivatedMinimapLockIndexes = curCatWorldData.deactivatedMinimapLockIndexes;
            for (int i = 0; i < minimapKeys.Count; i++)
            {
                if (curCatWorldData.deactivatedMinimapKeyIndexes.Contains(i))
                {
                    minimapKeys[i].SetActive(false);
                }
            }
            for (int i = 0; i < minimapLocks.Count; i++)
            {
                if (curCatWorldData.deactivatedMinimapLockIndexes.Contains(i))
                {
                    minimapLocks[i].GetComponent<MeshRenderer>().material = connectedGateColor;
                }
            }

            VARS.IsToActivateCurSavePoint = true;
        }
    }

    void WriteCatWorldData()
    {
        tempPath = Path.Combine(Application.persistentDataPath, /*"Datas",*/ "CatWorldData.txt");

        //curRoom
        curCatWorldData.curRoomIndex = VARS.curRoomIndex;

        //curPosition
        curCatWorldData.curCatIniPosition = catIniPositionPoint.transform.position;

        //savePoint
        curCatWorldData.curActivatedSavePointIndex = VARS.curActivatedSavePointIndex;
        curCatWorldData.curActivatedSavePointPosition = VARS.curActivatedSavePointPosition;
        storedActivatedSavePointBlock.transform.position = curCatWorldData.curActivatedSavePointPosition;
        curCatWorldData.curActivatedSavePointRoomIndex = VARS.curActivatedSavePointRoomIndex;

        //isRoomExplored
        curCatWorldData.isRoomExplored = VARS.IsRoomExplored;

        //keysAndLocks
        curCatWorldData.deactivatedKeyIndexes = VARS.deactivatedKeyIndexes;
        curCatWorldData.deactivatedLockIndexes = VARS.deactivatedLockIndexes;
        curCatWorldData.isCarryingAKey = VARS.IsCarryingAKey;
        curCatWorldData.curCarriedKeyIniRoomIndex = VARS.curCarriedKeyIniRoomIndex;
        curCatWorldData.curCarriedKeyIniLocalPosition = VARS.curCarriedKeyIniLocalPosition;
        for (int i = 0; i < keys.Count; i++)
        {
            if (keys[i] == VARS.curCarriedKey)
            {
                curCatWorldData.curCarriedKeyIndex = i;
                break;
            }
        }

        //minimapKeysAndLocks
        curCatWorldData.deactivatedMinimapKeyIndexes = VARS.deactivatedMinimapKeyIndexes;
        curCatWorldData.deactivatedMinimapLockIndexes = VARS.deactivatedMinimapLockIndexes;

        tempJsonString = JsonUtility.ToJson(curCatWorldData);

        File.WriteAllText(tempPath, tempJsonString);
    }
    #endregion

    #region KeyCodesData
    void ReadKeyCodesData()
    {
        tempPath = Path.Combine(Application.persistentDataPath, /*"Datas",*/ "KeyCodesData.txt");

        if (File.Exists(tempPath))
        {
            tempJsonString = File.ReadAllText(tempPath);
            curKeyCodesData = JsonUtility.FromJson<KeyCodesData>(tempJsonString);

            VARS.upKeyCode = curKeyCodesData.upKeyCode;
            VARS.downKeyCode = curKeyCodesData.downKeyCode;
            VARS.leftKeyCode = curKeyCodesData.leftKeyCode;
            VARS.rightKeyCode = curKeyCodesData.rightKeyCode;
            VARS.jumpKeyCode = curKeyCodesData.jumpKeyCode;
            VARS.acceKeyCode = curKeyCodesData.acceKeyCode;
            VARS.grabKeyCode = curKeyCodesData.grabKeyCode;
            //VARS.dashKeyCode = curKeyCodesData.dashKeyCode;
            //VARS.confirmKeyCode = curKeyCodesData.confirmKeyCode;
            //VARS.backKeyCode = curKeyCodesData.backKeyCode;

            VARS.curKeyCodes.Clear();

            VARS.curKeyCodes.Add(VARS.upKeyCode);
            VARS.curKeyCodes.Add(VARS.downKeyCode);
            VARS.curKeyCodes.Add(VARS.leftKeyCode);
            VARS.curKeyCodes.Add(VARS.rightKeyCode);
            VARS.curKeyCodes.Add(VARS.jumpKeyCode);
            VARS.curKeyCodes.Add(VARS.acceKeyCode);
            VARS.curKeyCodes.Add(VARS.grabKeyCode);
            //VARS.curKeyCodes.Add(VARS.dashKeyCode);
            //VARS.curKeyCodes.Add(VARS.confirmKeyCode);
            //VARS.curKeyCodes.Add(VARS.backKeyCode);
        }
        else
        {
            VARS.IsToWriteKeyCodesData = true;
        }
    }

    void WriteKeyCodesData()
    {
        tempPath = Path.Combine(Application.persistentDataPath, /*"Datas",*/ "KeyCodesData.txt");

        curKeyCodesData.upKeyCode = VARS.upKeyCode;
        curKeyCodesData.downKeyCode = VARS.downKeyCode;
        curKeyCodesData.leftKeyCode = VARS.leftKeyCode;
        curKeyCodesData.rightKeyCode = VARS.rightKeyCode;
        curKeyCodesData.jumpKeyCode = VARS.jumpKeyCode;
        curKeyCodesData.acceKeyCode = VARS.acceKeyCode;
        curKeyCodesData.grabKeyCode = VARS.grabKeyCode;
        //curKeyCodesData.dashKeyCode = VARS.dashKeyCode;
        //curKeyCodesData.confirmKeyCode = VARS.confirmKeyCode;
        //curKeyCodesData.backKeyCode = VARS.backKeyCode;

        tempJsonString = JsonUtility.ToJson(curKeyCodesData);

        File.WriteAllText(tempPath, tempJsonString);
    }
    #endregion
}
