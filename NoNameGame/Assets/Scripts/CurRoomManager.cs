using UnityEngine;

[DefaultExecutionOrder((int)ScriptsExecutionOrder.ExecutionOrder.curRoomManager)]
public class CurRoomManager : MonoBehaviour
{
    Constants CONS;
    Variables VARS;
    UniversalFunctionsLibrary UFL;
    ScriptsExecutionController SEC;

    GameObject gameManager;

    Transform tempTransform;
    GameObject tempGameObject;

    //storedFaceIndex
    int storedFaceIndex = -1;

    #region ConstantsUsed
    GameObject[] faces = new GameObject[6];
    #endregion

    #region VariablesUsed
    int curRoomIndex;
    #endregion

    void Start()
    {
        gameManager = GameObject.Find("GameManager");

        CONS = gameManager.GetComponent<Constants>();
        VARS = gameManager.GetComponent<Variables>();
        UFL = gameManager.GetComponent<UniversalFunctionsLibrary>();
        SEC = gameManager.GetComponent<ScriptsExecutionController>();

        #region ImportConstants
        faces = CONS.faces;
        #endregion

        #region ImportReferenceVariables
        #endregion
    }

    void Update()
    {
        #region ImportValueVariables
        curRoomIndex = VARS.curRoomIndex;
        #endregion

        //InitializeDirections();

        //CurRoomInitialize();

        if (!VARS.IsInNewRoomCurRoomManagerResetOver)
        {
            CurRoomInitialize();

            if (storedFaceIndex != VARS.curFaceIndex)
            {
                VARS.horCurSpeed = 0;
                VARS.verCurSpeed = 0;

                VARS.justEnterNewFaceStartTime = Time.time;

                VARS.IsJustEnterNewFace = true;
            }
            storedFaceIndex = VARS.curFaceIndex;

            //ifCurRoomNotExploredMarkItExplored
            if (!VARS.IsRoomExplored[curRoomIndex])
            {
                VARS.IsRoomExplored[curRoomIndex] = true;

                VARS.IsToWriteCatWorldData = true;
            }

            ////directions
            //GetPlaneDirections();
            //VARS.iniUp = VARS.roomStableUps[VARS.curRoomIndex];
            //VARS.iniRight = VARS.roomStableRights[VARS.curRoomIndex];

            //leftRotationVector = VARS.roomStableForwards[VARS.curRoomIndex] * -1;
            //rightRotationVector = VARS.roomStableForwards[VARS.curRoomIndex] * 1;

            //camIniEulerangles = VARS.roomStableForwards[VARS.curRoomIndex] * Vector3.SignedAngle(Vector3.right, iniRight, VARS.roomStableForwards[VARS.curRoomIndex]);

            VARS.IsInNewRoomCurRoomManagerResetOver = true;
        }
    }
    //void GetPlaneDirections()
    //{
    //    VARS.planeForward = VARS.roomStableForwards[VARS.curRoomIndex];
    //    VARS.planeUp = VARS.roomStableUps[VARS.curRoomIndex];
    //    VARS.planeRight = VARS.roomStableRights[VARS.curRoomIndex];
    //}

    //void InitializeDirections()
    //{
    //    //if (VARS.planeForward == Vector3.zero ||
    //    //    VARS.planeUp == Vector3.zero ||
    //    //    VARS.planeRight == Vector3.zero)
    //    //{
    //    //    GetPlaneDirections();
    //    //}

    //    //if (VARS.iniUp == Vector3.zero)
    //    //    VARS.iniUp = VARS.roomStableUps[VARS.curRoomIndex];
    //    //if (VARS.iniRight == Vector3.zero)
    //    //    VARS.iniRight = VARS.roomStableRights[VARS.curRoomIndex];

    //    //if (VARS.curUp == Vector3.zero)
    //    //    VARS.curUp = VARS.iniUp;
    //    //if (VARS.curRight == Vector3.zero)
    //    //    VARS.curRight = VARS.iniRight;
    //}

    void CurRoomInitialize()
	{
        tempTransform = CONS.roomPlanes[VARS.curRoomIndex].transform;
        tempGameObject = tempTransform.parent.gameObject;

		for (int i = 0; i < 6; i++)
        {
            if (tempGameObject == faces[i])
            {
                VARS.curFaceIndex = i + 1;

                break;
            }
        }

		VARS.curPlaneEmpty = tempTransform.GetChild(0).gameObject;

		VARS.curRoomCenter = VARS.roomCenters[curRoomIndex];
        VARS.curRoomStableForward = VARS.roomStableForwards[curRoomIndex];
        VARS.curRoomStableUp = VARS.roomStableUps[curRoomIndex];
        VARS.curRoomStableRight = VARS.roomStableRights[curRoomIndex];
        VARS.curRoomGravity = CONS.roomGravities[curRoomIndex];

        if (VARS.curUp == Vector3.zero)
            VARS.curUp = VARS.curRoomStableUp;
        if (VARS.curRight == Vector3.zero)
            VARS.curRight = VARS.curRoomStableRight;
    }
}
