using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;


[DefaultExecutionOrder((int)ScriptsExecutionOrder.ExecutionOrder.dataManager)]
public class DataManager : MonoBehaviour
{
    Constants CONS;
    Variables VARS;
    UniversalFunctionsLibrary UFL;
    ScriptsExecutionController SEC;

    GameObject gameManager;

    public class ProgressData
    {
        //guide
        public bool hasFinishedKeysGuide;
        public bool hasJumped;
        public bool hasBeenIntoMinimap;
        public bool hasClimbed;
        public bool hasTwisted;
        public bool hasBackCentered;
    }

    ProgressData curProgressData = new ProgressData();

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
        //public bool isCarryingAKey;
        //public int curCarriedKeyIndex;
        //public int curCarriedKeyIniRoomIndex;
        //public Vector3 curCarriedKeyIniLocalPosition;
        public List<int> deactivatedMinimapKeyIndexes = new List<int>();
        public List<int> deactivatedMinimapLockIndexes = new List<int>();

        //fragments
        public bool[] isRedFragmentsEmbeded = new bool[8];
        public bool[] isYellowFragmentsEmbeded = new bool[8];
        public bool[] isBlueFragmentsEmbeded = new bool[8];
        public bool[] isOrangeFragmentsEmbeded = new bool[8];
        public bool[] isGreenFragmentsEmbeded = new bool[8];
        public bool[] isPurpleFragmentsEmbeded = new bool[8];
        public Vector3[] redEmbededFragmentPositions = new Vector3[8];
        public Vector3[] yellowEmbededFragmentPositions = new Vector3[8];
        public Vector3[] blueEmbededFragmentPositions = new Vector3[8];
        public Vector3[] orangeEmbededFragmentPositions = new Vector3[8];
        public Vector3[] greenEmbededFragmentPositions = new Vector3[8];
        public Vector3[] purpleEmbededFragmentPositions = new Vector3[8];

        //isCenterFulfilled
        public bool[] isCenterFulfilled = new bool[6];

        //maxEnergyBonus
        public float maxEnergyBonus;

        //curLatestCenterSavePointPosition
        public Vector3 curLatestCenterSavePointPosition;

        //isMinimapActivated
        public bool isMinimapActivated;
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
        public KeyCode minimapKeyCode;
        public KeyCode backKeyCode;
    }

    KeyCodesData curKeyCodesData = new KeyCodesData();

    string tempPath;
    string tempJsonString;

    int tempInt;
    Vector3 tempVector;
    Transform tempTransform;

    #region ConstantsUsed
    float gridBreadth;
    int roomCoordBreadth;
    int minimapRoomCoordBreadth;

    GameObject[] faces = new GameObject[6];
    Vector3[] faceStableForwards = new Vector3[6];

    GameObject[] roomPlanes = new GameObject[54];

    List<GameObject> redFragments = new List<GameObject>();
    List<GameObject> yellowFragments = new List<GameObject>();
    List<GameObject> blueFragments = new List<GameObject>();
    List<GameObject> orangeFragments = new List<GameObject>();
    List<GameObject> greenFragments = new List<GameObject>();
    List<GameObject> purpleFragments = new List<GameObject>();

    //GameObject[] holeBlocks = new GameObject[6];

    GameObject storedActivatedSavePointBlock;

    GameObject[] twistingCenters = new GameObject[6];

    List<GameObject> keys = new List<GameObject>();
    List<GameObject> locks = new List<GameObject>();

    GameObject[] minimapFaces = new GameObject[6];
    GameObject[] minimapRoomPlanes = new GameObject[54];
    GameObject[] minimapTwistingCenters = new GameObject[6];

    List<GameObject> minimapRedFragments = new List<GameObject>();
    List<GameObject> minimapYellowFragments = new List<GameObject>();
    List<GameObject> minimapBlueFragments = new List<GameObject>();
    List<GameObject> minimapOrangeFragments = new List<GameObject>();
    List<GameObject> minimapGreenFragments = new List<GameObject>();
    List<GameObject> minimapPurpleFragments = new List<GameObject>();

    List<GameObject> minimapKeys = new List<GameObject>();
    List<GameObject> minimapLocks = new List<GameObject>();

    Material minimapCollectibleCollectedColor;

    Material connectedGateColor;

    Transform catTransform;

    GameObject catIniPositionPoint;
    #endregion

    #region VariablesUsed
    Vector3[] roomCenters = new Vector3[54];
    Vector3[] roomStableForwards = new Vector3[54];
    Vector3[] roomStableUps = new Vector3[54];
    Vector3[] roomStableRights = new Vector3[54];

    //bool[] isRedFragmentsEmbeded = new bool[8];
    //bool[] isYellowFragmentsEmbeded = new bool[8];
    //bool[] isBlueFragmentsEmbeded = new bool[8];
    //bool[] isOrangeFragmentsEmbeded = new bool[8];
    //bool[] isGreenFragmentsEmbeded = new bool[8];
    //bool[] isPurpleFragmentsEmbeded = new bool[8];

    //bool[] isCenterFulfilled = new bool[6];
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
        redFragments = CONS.redFragments;
        yellowFragments = CONS.yellowFragments;
        blueFragments = CONS.blueFragments;
        orangeFragments = CONS.orangeFragments;
        greenFragments = CONS.greenFragments;
        purpleFragments = CONS.purpleFragments;
        storedActivatedSavePointBlock = CONS.storedActivatedSavePointBlock;
        twistingCenters = CONS.twistingCenters;
        keys = CONS.keys;
        locks = CONS.locks;
        minimapFaces = CONS.minimapFaces;
        minimapRoomPlanes = CONS.minimapRoomPlanes;
        minimapTwistingCenters = CONS.minimapTwistingCenters;
        minimapRedFragments = CONS.minimapRedFragments;
        minimapYellowFragments = CONS.minimapYellowFragments;
        minimapBlueFragments = CONS.minimapBlueFragments;
        minimapOrangeFragments = CONS.minimapOrangeFragments;
        minimapGreenFragments = CONS.minimapGreenFragments;
        minimapPurpleFragments = CONS.minimapPurpleFragments;
        minimapKeys = CONS.minimapKeys;
        minimapLocks = CONS.minimapLocks;
        minimapCollectibleCollectedColor = CONS.minimapCollectibleCollectedColor;
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

        WriteProgressData(true);
        WriteWorldData(true);
        WriteCatWorldData(true);
        WriteKeyCodesData(true);

        tempPath = Path.Combine(Application.persistentDataPath, /*"Datas",*/ "Version0.3.5.txt");

        if (File.Exists(tempPath))
        {
            ReadProgressData();

            ReadWorldData();

            //WriteCatWorldData();

            ReadCatWorldData();

            ReadKeyCodesData();
        }
        else
        {
            File.Create(tempPath).Close();
        }
    }

    void Update()
    {
        #region ImportValueVariables
        #endregion

        if (VARS.IsToWriteProgressData)
        {
            WriteProgressData();

            VARS.IsToWriteProgressData = false;
        }

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

        if (VARS.IsToStartNewGame)
        {
            Debug.Log("startNewGame");

            SetNewGameData();

            VARS.IsToStartNewGame = false;

            //#if UNITY_EDITOR
            //UnityEditor.EditorApplication.isPlaying = false;
            //#endif

            //Application.Quit();
        }
    }

    #region ProgressData
    void ReadProgressData(bool isInitial = false)
    {
        if (!isInitial)
            tempPath = Path.Combine(Application.persistentDataPath, /*"Datas",*/ "ProgressData.txt");
        else
            tempPath = Path.Combine(Application.persistentDataPath, /*"Datas",*/ "InitialProgressData.txt");

        if (File.Exists(tempPath))
        {
            tempJsonString = File.ReadAllText(tempPath);
            curProgressData = JsonUtility.FromJson<ProgressData>(tempJsonString);

            VARS.HasFinishedKeysGuide = curProgressData.hasFinishedKeysGuide;
            VARS.HasJumped = curProgressData.hasJumped;
            VARS.HasBeenIntoMinimap = curProgressData.hasBeenIntoMinimap;
            VARS.HasClimbed = curProgressData.hasClimbed;
            VARS.HasTwisted = curProgressData.hasTwisted;
            VARS.HasBackCentered = curProgressData.hasBackCentered;
        }

    }

    void WriteProgressData(bool isInitial = false)
    {
        if (!isInitial)
            tempPath = Path.Combine(Application.persistentDataPath, /*"Datas",*/ "ProgressData.txt");
        else
            tempPath = Path.Combine(Application.persistentDataPath, /*"Datas",*/ "InitialProgressData.txt");

        curProgressData.hasFinishedKeysGuide = VARS.HasFinishedKeysGuide;
        curProgressData.hasJumped = VARS.HasJumped;
        curProgressData.hasBeenIntoMinimap = VARS.HasBeenIntoMinimap;
        curProgressData.hasClimbed = VARS.HasClimbed;
        curProgressData.hasTwisted = VARS.HasTwisted;
        curProgressData.hasBackCentered = VARS.HasBackCentered;

        tempJsonString = JsonUtility.ToJson(curProgressData);

        File.WriteAllText(tempPath, tempJsonString);
    }
    #endregion

    #region WorldData
    void ReadWorldData(bool isInitial = false)
    {
        if (!isInitial)
            tempPath = Path.Combine(Application.persistentDataPath, /*"Datas",*/ "WorldData.txt");
        else
            tempPath = Path.Combine(Application.persistentDataPath, /*"Datas",*/ "InitialWorldData.txt");

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

    void WriteWorldData(bool isInitial = false)
    {
        if (!isInitial)
            tempPath = Path.Combine(Application.persistentDataPath, /*"Datas",*/ "WorldData.txt");
        else
            tempPath = Path.Combine(Application.persistentDataPath, /*"Datas",*/ "InitialWorldData.txt");

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
    void ReadCatWorldData(bool isInitial = false)
    {
        if (!isInitial)
            tempPath = Path.Combine(Application.persistentDataPath, /*"Datas",*/ "CatWorldData.txt");
        else
            tempPath = Path.Combine(Application.persistentDataPath, /*"Datas",*/ "InitialCatWorldData.txt");

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
            //VARS.IsCarryingAKey = curCatWorldData.isCarryingAKey;
            //VARS.curCarriedKey = keys[curCatWorldData.curCarriedKeyIndex];
            //VARS.curCarriedKeyIniRoomIndex = curCatWorldData.curCarriedKeyIniRoomIndex;
            //VARS.curCarriedKeyIniParent = roomPlanes[curCatWorldData.curCarriedKeyIniRoomIndex].transform.GetChild(0).gameObject;
            //VARS.curCarriedKeyIniLocalPosition = curCatWorldData.curCarriedKeyIniLocalPosition;
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
                    //minimapKeys[i].SetActive(false);
                    minimapKeys[i].GetComponent<MeshRenderer>().material = minimapCollectibleCollectedColor;
                }
            }
            for (int i = 0; i < minimapLocks.Count; i++)
            {
                if (curCatWorldData.deactivatedMinimapLockIndexes.Contains(i))
                {
                    //minimapLocks[i].GetComponent<MeshRenderer>().material = connectedGateColor;
                    minimapLocks[i].SetActive(false);
                }
            }

            VARS.IsToActivateCurSavePoint = true;

            //fragments
            VARS.isRedFragmentsEmbeded = curCatWorldData.isRedFragmentsEmbeded;
            VARS.isYellowFragmentsEmbeded = curCatWorldData.isYellowFragmentsEmbeded;
            VARS.isBlueFragmentsEmbeded = curCatWorldData.isBlueFragmentsEmbeded;
            VARS.isOrangeFragmentsEmbeded = curCatWorldData.isOrangeFragmentsEmbeded;
            VARS.isGreenFragmentsEmbeded = curCatWorldData.isGreenFragmentsEmbeded;
            VARS.isPurpleFragmentsEmbeded = curCatWorldData.isPurpleFragmentsEmbeded;
            for (int i = 0; i < redFragments.Count; i++)
            {
                if (VARS.isRedFragmentsEmbeded[redFragments[i].GetComponent<TileData>().fragmentIndex - 1])
                {
                    redFragments[i].transform.position = curCatWorldData.redEmbededFragmentPositions[redFragments[i].GetComponent<TileData>().fragmentIndex - 1];
                    for (int j = 0; j < 9; j++)
                    {
                        redFragments[i].transform.GetChild(j).gameObject.SetActive(j > 2);
                    }
                    redFragments[i].transform.SetParent(roomPlanes[49].transform.GetChild(0), true);
                }
            }
            for (int i = 0; i < yellowFragments.Count; i++)
            {
                if (VARS.isYellowFragmentsEmbeded[yellowFragments[i].GetComponent<TileData>().fragmentIndex - 1])
                {
                    yellowFragments[i].transform.position = curCatWorldData.yellowEmbededFragmentPositions[yellowFragments[i].GetComponent<TileData>().fragmentIndex - 1];
                    for (int j = 0; j < 9; j++)
                    {
                        yellowFragments[i].transform.GetChild(j).gameObject.SetActive(j > 2);
                    }
                    yellowFragments[i].transform.SetParent(roomPlanes[4].transform.GetChild(0), true);
                }
            }
            for (int i = 0; i < blueFragments.Count; i++)
            {
                if (VARS.isBlueFragmentsEmbeded[blueFragments[i].GetComponent<TileData>().fragmentIndex - 1])
                {
                    blueFragments[i].transform.position = curCatWorldData.blueEmbededFragmentPositions[blueFragments[i].GetComponent<TileData>().fragmentIndex - 1];
                    for (int j = 0; j < 9; j++)
                    {
                        blueFragments[i].transform.GetChild(j).gameObject.SetActive(j > 2);
                    }
                    blueFragments[i].transform.SetParent(roomPlanes[31].transform.GetChild(0), true);
                }
            }
            for (int i = 0; i < orangeFragments.Count; i++)
            {
                if (VARS.isOrangeFragmentsEmbeded[orangeFragments[i].GetComponent<TileData>().fragmentIndex - 1])
                {
                    orangeFragments[i].transform.position = curCatWorldData.orangeEmbededFragmentPositions[orangeFragments[i].GetComponent<TileData>().fragmentIndex - 1];
                    for (int j = 0; j < 9; j++)
                    {
                        orangeFragments[i].transform.GetChild(j).gameObject.SetActive(j > 2);
                    }
                    orangeFragments[i].transform.SetParent(roomPlanes[22].transform.GetChild(0), true);
                }
            }
            for (int i = 0; i < greenFragments.Count; i++)
            {
                if (VARS.isGreenFragmentsEmbeded[greenFragments[i].GetComponent<TileData>().fragmentIndex - 1])
                {
                    greenFragments[i].transform.position = curCatWorldData.greenEmbededFragmentPositions[greenFragments[i].GetComponent<TileData>().fragmentIndex - 1];
                    for (int j = 0; j < 9; j++)
                    {
                        greenFragments[i].transform.GetChild(j).gameObject.SetActive(j > 2);
                    }
                    greenFragments[i].transform.SetParent(roomPlanes[40].transform.GetChild(0), true);
                }
            }
            for (int i = 0; i < purpleFragments.Count; i++)
            {
                if (VARS.isPurpleFragmentsEmbeded[purpleFragments[i].GetComponent<TileData>().fragmentIndex - 1])
                {
                    purpleFragments[i].transform.position = curCatWorldData.purpleEmbededFragmentPositions[purpleFragments[i].GetComponent<TileData>().fragmentIndex - 1];
                    for (int j = 0; j < 9; j++)
                    {
                        purpleFragments[i].transform.GetChild(j).gameObject.SetActive(j > 2);
                    }
                    purpleFragments[i].transform.SetParent(roomPlanes[13].transform.GetChild(0), true);
                }
            }
            for (int i = 0; i < minimapRedFragments.Count; i++)
            {
                if (VARS.isRedFragmentsEmbeded[i])
                {
                    minimapRedFragments[i].GetComponent<MeshRenderer>().material = minimapCollectibleCollectedColor;
                }
            }
            for (int i = 0; i < minimapYellowFragments.Count; i++)
            {
                if (VARS.isYellowFragmentsEmbeded[i])
                {
                    minimapYellowFragments[i].GetComponent<MeshRenderer>().material = minimapCollectibleCollectedColor;
                }
            }
            for (int i = 0; i < minimapBlueFragments.Count; i++)
            {
                if (VARS.isBlueFragmentsEmbeded[i])
                {
                    minimapBlueFragments[i].GetComponent<MeshRenderer>().material = minimapCollectibleCollectedColor;
                }
            }
            for (int i = 0; i < minimapOrangeFragments.Count; i++)
            {
                if (VARS.isOrangeFragmentsEmbeded[i])
                {
                    minimapOrangeFragments[i].GetComponent<MeshRenderer>().material = minimapCollectibleCollectedColor;
                }
            }
            for (int i = 0; i < minimapGreenFragments.Count; i++)
            {
                if (VARS.isGreenFragmentsEmbeded[i])
                {
                    minimapGreenFragments[i].GetComponent<MeshRenderer>().material = minimapCollectibleCollectedColor;
                }
            }
            for (int i = 0; i < minimapPurpleFragments.Count; i++)
            {
                if (VARS.isPurpleFragmentsEmbeded[i])
                {
                    minimapPurpleFragments[i].GetComponent<MeshRenderer>().material = minimapCollectibleCollectedColor;
                }
            }

            //isCenterFulfilled
            VARS.isCenterFulfilled = curCatWorldData.isCenterFulfilled;
            //for (int i = 0; i < 6; i++)
            //{
            //    if (isCenterFulfilled[i])
            //    {
            //        tempInt = i * 9 + 4;
            //        holeBlocks[i].transform.position = roomCenters[tempInt] - roomStableForwards[tempInt] * 0.9f;
            //    }
            //}
            
            //maxEneryBonus
            VARS.maxEnergyBonus = curCatWorldData.maxEnergyBonus;

            //curLatestCenterSavePointPosition
            VARS.curLatestCenterSavePointPosition = curCatWorldData.curLatestCenterSavePointPosition;

            //isMinimapActivated
            VARS.IsMinimapActivated = curCatWorldData.isMinimapActivated;
        }
    }

    void WriteCatWorldData(bool isInitial = false)
    {
        if (!isInitial)
            tempPath = Path.Combine(Application.persistentDataPath, /*"Datas",*/ "CatWorldData.txt");
        else
            tempPath = Path.Combine(Application.persistentDataPath, /*"Datas",*/ "InitialCatWorldData.txt");

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
        //curCatWorldData.isCarryingAKey = VARS.IsCarryingAKey;
        //curCatWorldData.curCarriedKeyIniRoomIndex = VARS.curCarriedKeyIniRoomIndex;
        //curCatWorldData.curCarriedKeyIniLocalPosition = VARS.curCarriedKeyIniLocalPosition;
        //for (int i = 0; i < keys.Count; i++)
        //{
        //    if (keys[i] == VARS.curCarriedKey)
        //    {
        //        curCatWorldData.curCarriedKeyIndex = i;
        //        break;
        //    }
        //}

        //fragments
        curCatWorldData.isRedFragmentsEmbeded = VARS.isRedFragmentsEmbeded;
        curCatWorldData.isYellowFragmentsEmbeded = VARS.isYellowFragmentsEmbeded;
        curCatWorldData.isBlueFragmentsEmbeded = VARS.isBlueFragmentsEmbeded;
        curCatWorldData.isOrangeFragmentsEmbeded = VARS.isOrangeFragmentsEmbeded;
        curCatWorldData.isGreenFragmentsEmbeded = VARS.isGreenFragmentsEmbeded;
        curCatWorldData.isPurpleFragmentsEmbeded = VARS.isPurpleFragmentsEmbeded;
        for (int i = 0; i < redFragments.Count; i++)
        {
            tempInt = redFragments[i].GetComponent<TileData>().fragmentIndex - 1;
            if (VARS.isRedFragmentsEmbeded[tempInt])
            {
                curCatWorldData.redEmbededFragmentPositions[tempInt] = redFragments[i].transform.position;
            }
        }
        for (int i = 0; i < yellowFragments.Count; i++)
        {
            tempInt = yellowFragments[i].GetComponent<TileData>().fragmentIndex - 1;
            if (VARS.isYellowFragmentsEmbeded[tempInt])
            {
                curCatWorldData.yellowEmbededFragmentPositions[tempInt] = yellowFragments[i].transform.position;
            }
        }
        for (int i = 0; i < blueFragments.Count; i++)
        {
            tempInt = blueFragments[i].GetComponent<TileData>().fragmentIndex - 1;
            if (VARS.isBlueFragmentsEmbeded[tempInt])
            {
                curCatWorldData.blueEmbededFragmentPositions[tempInt] = blueFragments[i].transform.position;
            }
        }
        for (int i = 0; i < orangeFragments.Count; i++)
        {
            tempInt = orangeFragments[i].GetComponent<TileData>().fragmentIndex - 1;
            if (VARS.isOrangeFragmentsEmbeded[tempInt])
            {
                curCatWorldData.orangeEmbededFragmentPositions[tempInt] = orangeFragments[i].transform.position;
            }
        }
        for (int i = 0; i < greenFragments.Count; i++)
        {
            tempInt = greenFragments[i].GetComponent<TileData>().fragmentIndex - 1;
            if (VARS.isGreenFragmentsEmbeded[tempInt])
            {
                curCatWorldData.greenEmbededFragmentPositions[tempInt] = greenFragments[i].transform.position;
            }
        }
        for (int i = 0; i < purpleFragments.Count; i++)
        {
            tempInt = purpleFragments[i].GetComponent<TileData>().fragmentIndex - 1;
            if (VARS.isPurpleFragmentsEmbeded[tempInt])
            {
                curCatWorldData.purpleEmbededFragmentPositions[tempInt] = purpleFragments[i].transform.position;
            }
        }

        //isCenterFulfilled
        curCatWorldData.isCenterFulfilled = VARS.isCenterFulfilled;

        //maxEnergyBonus
        curCatWorldData.maxEnergyBonus = VARS.maxEnergyBonus;

        //curLatestCenterSavePointPosition
        curCatWorldData.curLatestCenterSavePointPosition = VARS.curLatestCenterSavePointPosition;

        //minimapKeysAndLocks
        curCatWorldData.deactivatedMinimapKeyIndexes = VARS.deactivatedMinimapKeyIndexes;
        curCatWorldData.deactivatedMinimapLockIndexes = VARS.deactivatedMinimapLockIndexes;

        //isMinimapActivated
        curCatWorldData.isMinimapActivated = VARS.IsMinimapActivated;

        tempJsonString = JsonUtility.ToJson(curCatWorldData);

        File.WriteAllText(tempPath, tempJsonString);
    }
    #endregion

    #region KeyCodesData
    void ReadKeyCodesData(bool isInitial = false)
    {
        if (!isInitial)
            tempPath = Path.Combine(Application.persistentDataPath, /*"Datas",*/ "KeyCodesData.txt");
        else
            tempPath = Path.Combine(Application.persistentDataPath, /*"Datas",*/ "InitialKeyCodesData.txt");

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
            VARS.minimapKeyCode = curKeyCodesData.minimapKeyCode;
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
            VARS.curKeyCodes.Add(VARS.minimapKeyCode);
            //VARS.curKeyCodes.Add(VARS.backKeyCode);
        }
        else
        {
            VARS.IsToWriteKeyCodesData = true;
        }
    }

    void WriteKeyCodesData(bool isInitial = false)
    {
        if (!isInitial)
            tempPath = Path.Combine(Application.persistentDataPath, /*"Datas",*/ "KeyCodesData.txt");
        else
            tempPath = Path.Combine(Application.persistentDataPath, /*"Datas",*/ "InitialKeyCodesData.txt");

        curKeyCodesData.upKeyCode = VARS.upKeyCode;
        curKeyCodesData.downKeyCode = VARS.downKeyCode;
        curKeyCodesData.leftKeyCode = VARS.leftKeyCode;
        curKeyCodesData.rightKeyCode = VARS.rightKeyCode;
        curKeyCodesData.jumpKeyCode = VARS.jumpKeyCode;
        curKeyCodesData.acceKeyCode = VARS.acceKeyCode;
        curKeyCodesData.grabKeyCode = VARS.grabKeyCode;
        //curKeyCodesData.dashKeyCode = VARS.dashKeyCode;
        curKeyCodesData.minimapKeyCode = VARS.minimapKeyCode;
        //curKeyCodesData.backKeyCode = VARS.backKeyCode;

        tempJsonString = JsonUtility.ToJson(curKeyCodesData);

        File.WriteAllText(tempPath, tempJsonString);
    }
    #endregion

    #region StartNewGame
    void SetNewGameData()
    {
        //SetNewGameProgressData();
        //SetNewGameWorldData();
        //SetNewGameCatWorldData();
        //SetNewGameKeyCodesData();

        //ReadProgressData();
        //ReadWorldData();
        //ReadCatWorldData();
        //ReadKeyCodesData();

        ReadProgressData(true);
        ReadWorldData(true);
        ReadCatWorldData(true);
        ReadKeyCodesData(true);

        WriteProgressData();
        WriteWorldData();
        WriteCatWorldData();
        WriteKeyCodesData();

        Debug.Log("AllDataReset");
    }

    void SetNewGameProgressData()
    {
        tempPath = Path.Combine(Application.persistentDataPath, /*"Datas",*/ "ProgressData.txt");

        curProgressData.hasFinishedKeysGuide = false;
        curProgressData.hasBeenIntoMinimap = false;
        curProgressData.hasClimbed = false;
        curProgressData.hasTwisted = false;
        curProgressData.hasBackCentered = false;

        tempJsonString = JsonUtility.ToJson(curProgressData);

        File.WriteAllText(tempPath, tempJsonString);
    }

    void SetNewGameWorldData()
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

    void SetNewGameCatWorldData()
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
        //curCatWorldData.isCarryingAKey = VARS.IsCarryingAKey;
        //curCatWorldData.curCarriedKeyIniRoomIndex = VARS.curCarriedKeyIniRoomIndex;
        //curCatWorldData.curCarriedKeyIniLocalPosition = VARS.curCarriedKeyIniLocalPosition;
        //for (int i = 0; i < keys.Count; i++)
        //{
        //    if (keys[i] == VARS.curCarriedKey)
        //    {
        //        curCatWorldData.curCarriedKeyIndex = i;
        //        break;
        //    }
        //}

        //fragments
        curCatWorldData.isRedFragmentsEmbeded = VARS.isRedFragmentsEmbeded;
        curCatWorldData.isYellowFragmentsEmbeded = VARS.isYellowFragmentsEmbeded;
        curCatWorldData.isBlueFragmentsEmbeded = VARS.isBlueFragmentsEmbeded;
        curCatWorldData.isOrangeFragmentsEmbeded = VARS.isOrangeFragmentsEmbeded;
        curCatWorldData.isGreenFragmentsEmbeded = VARS.isGreenFragmentsEmbeded;
        curCatWorldData.isPurpleFragmentsEmbeded = VARS.isPurpleFragmentsEmbeded;
        for (int i = 0; i < redFragments.Count; i++)
        {
            tempInt = redFragments[i].GetComponent<TileData>().fragmentIndex - 1;
            if (VARS.isRedFragmentsEmbeded[tempInt])
            {
                curCatWorldData.redEmbededFragmentPositions[tempInt] = redFragments[i].transform.position;
            }
        }
        for (int i = 0; i < yellowFragments.Count; i++)
        {
            tempInt = yellowFragments[i].GetComponent<TileData>().fragmentIndex - 1;
            if (VARS.isYellowFragmentsEmbeded[tempInt])
            {
                curCatWorldData.yellowEmbededFragmentPositions[tempInt] = yellowFragments[i].transform.position;
            }
        }
        for (int i = 0; i < blueFragments.Count; i++)
        {
            tempInt = blueFragments[i].GetComponent<TileData>().fragmentIndex - 1;
            if (VARS.isBlueFragmentsEmbeded[tempInt])
            {
                curCatWorldData.blueEmbededFragmentPositions[tempInt] = blueFragments[i].transform.position;
            }
        }
        for (int i = 0; i < orangeFragments.Count; i++)
        {
            tempInt = orangeFragments[i].GetComponent<TileData>().fragmentIndex - 1;
            if (VARS.isOrangeFragmentsEmbeded[tempInt])
            {
                curCatWorldData.orangeEmbededFragmentPositions[tempInt] = orangeFragments[i].transform.position;
            }
        }
        for (int i = 0; i < greenFragments.Count; i++)
        {
            tempInt = greenFragments[i].GetComponent<TileData>().fragmentIndex - 1;
            if (VARS.isGreenFragmentsEmbeded[tempInt])
            {
                curCatWorldData.greenEmbededFragmentPositions[tempInt] = greenFragments[i].transform.position;
            }
        }
        for (int i = 0; i < purpleFragments.Count; i++)
        {
            tempInt = purpleFragments[i].GetComponent<TileData>().fragmentIndex - 1;
            if (VARS.isPurpleFragmentsEmbeded[tempInt])
            {
                curCatWorldData.purpleEmbededFragmentPositions[tempInt] = purpleFragments[i].transform.position;
            }
        }

        //isCenterFulfilled
        curCatWorldData.isCenterFulfilled = VARS.isCenterFulfilled;

        //curLatestCenterSavePointPosition
        curCatWorldData.curLatestCenterSavePointPosition = VARS.curLatestCenterSavePointPosition;

        //minimapKeysAndLocks
        curCatWorldData.deactivatedMinimapKeyIndexes = VARS.deactivatedMinimapKeyIndexes;
        curCatWorldData.deactivatedMinimapLockIndexes = VARS.deactivatedMinimapLockIndexes;

        //isMinimapActivated
        curCatWorldData.isMinimapActivated = VARS.IsMinimapActivated;

        tempJsonString = JsonUtility.ToJson(curCatWorldData);

        File.WriteAllText(tempPath, tempJsonString);
    }

    void SetNewGameKeyCodesData()
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
        curKeyCodesData.minimapKeyCode = VARS.minimapKeyCode;
        //curKeyCodesData.backKeyCode = VARS.backKeyCode;

        tempJsonString = JsonUtility.ToJson(curKeyCodesData);

        File.WriteAllText(tempPath, tempJsonString);
    }
    #endregion
}
