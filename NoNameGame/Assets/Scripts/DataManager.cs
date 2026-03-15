using System.IO;
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

        //miniMapRooms
        public Vector3[] miniMapRoomPlanePositions = new Vector3[54];
        public Vector3[] miniMapRoomPlaneEulerangles = new Vector3[54];
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
    int miniMapRoomCoordBreadth;

    GameObject[] faces = new GameObject[6];
    Vector3[] faceStableForwards = new Vector3[6];

    GameObject[] roomPlanes = new GameObject[54];

    GameObject[] twistingCenters = new GameObject[6];

    GameObject[] miniMapFaces = new GameObject[6];
    GameObject[] miniMapRoomPlanes = new GameObject[54];
    GameObject[] miniMapTwistingCenters = new GameObject[6];
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

        #region ImportConstants
        gridBreadth = CONS.gridBreadth;
        roomCoordBreadth = CONS.roomCoordBreadth;
        miniMapRoomCoordBreadth = CONS.miniMapRoomCoordBreadth;
        faces = CONS.faces;
        faceStableForwards = CONS.faceStableForwards;
        roomPlanes = CONS.roomPlanes;
        twistingCenters = CONS.twistingCenters;
        miniMapFaces = CONS.miniMapFaces;
        miniMapRoomPlanes = CONS.miniMapRoomPlanes;
        miniMapTwistingCenters = CONS.miniMapTwistingCenters;
        #endregion

        #region ImportReferenceVariables
        #endregion

        ReadWorldData();

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
                //rooms
                tempTransform = roomPlanes[i].transform;

                tempTransform.position = UFL.Vector3RoundToInt(curWorldData.roomPlanePositions[i]);
                tempTransform.eulerAngles = UFL.Vector3RoundToInt(curWorldData.roomPlaneEulerangles[i]);

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

                //miniMapRooms
                tempTransform = miniMapRoomPlanes[i].transform;

                tempTransform.position = UFL.Vector3RoundToInt(curWorldData.miniMapRoomPlanePositions[i]);
                tempTransform.eulerAngles = UFL.Vector3RoundToInt(curWorldData.miniMapRoomPlaneEulerangles[i]);

                //miniMapRoomPlanesChildToTheFaces
                for (int j = 0; j < 6; j++)
                {
                    //tempVector = tempTransform.position - miniMapTwistingCenters[j].transform.position;

                    //ifIsInTheFaceChildToIt
                    if (/*Mathf.Abs(Vector3.Dot(tempVector, faceStableForwards[j])) <= (miniMapRoomCoordBreadth / 2 + 2) * gridBreadth*/
                        UFL.IsMiniMapPlaneInTheFace(i, j + 1))
                    {
                        tempTransform.SetParent(miniMapFaces[j].transform, true);

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
            //rooms
            tempTransform = roomPlanes[i].transform;

            curWorldData.roomPlanePositions[i] = UFL.Vector3RoundToInt(tempTransform.position);
            curWorldData.roomPlaneEulerangles[i] = UFL.Vector3RoundToInt(tempTransform.eulerAngles);

            //miniMapRooms
            tempTransform = miniMapRoomPlanes[i].transform;

            curWorldData.miniMapRoomPlanePositions[i] = UFL.Vector3RoundToInt(tempTransform.position);
            curWorldData.miniMapRoomPlaneEulerangles[i] = UFL.Vector3RoundToInt(tempTransform.eulerAngles);
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
            VARS.IsRoomExplored = curCatWorldData.isRoomExplored;

            VARS.IsToActivateCurSavePoint = true;
        }
    }

    void WriteCatWorldData()
    {
        tempPath = Path.Combine(Application.persistentDataPath, "Datas", "CatWorldData.txt");

        //savePoint
        curCatWorldData.curActivatedSavePointIndex = VARS.curActivatedSavePointIndex;
        curCatWorldData.curActivatedSavePointPosition = VARS.curActivatedSavePointPosition;

        //isRoomExplored
        curCatWorldData.isRoomExplored = VARS.IsRoomExplored;

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
        tempPath = Path.Combine(Application.persistentDataPath, "Datas", "KeyCodesData.txt");

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
