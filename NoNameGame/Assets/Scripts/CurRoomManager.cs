using UnityEngine;

[DefaultExecutionOrder((int)ScriptsExecutionOrder.ExecutionOrder.curRoomManager)]
public class CurRoomManager : MonoBehaviour
{
    Constants CONS;
    Variables VARS;
    ScriptsExecutionController SEC;

    GameObject gameManager;

    //directions

    #region ConstantsUsed

    #endregion

    #region VariablesUsed
    int curRoomIndex;
    #endregion

    void Start()
    {
        gameManager = GameObject.Find("GameManager");

        CONS = gameManager.GetComponent<Constants>();
        VARS = gameManager.GetComponent<Variables>();
        SEC = gameManager.GetComponent<ScriptsExecutionController>();
    }

    void Update()
    {
        curRoomIndex = VARS.curRoomIndex;

        //InitializeDirections();

        //CurRoomInitialize();

        if (VARS.isInNewRoom)
        {
            CurRoomInitialize();

            ////directions
            //GetPlaneDirections();
            //VARS.iniUp = VARS.roomStableUps[VARS.curRoomIndex];
            //VARS.iniRight = VARS.roomStableRights[VARS.curRoomIndex];

            //leftRotationVector = VARS.roomStableForwards[VARS.curRoomIndex] * -1;
            //rightRotationVector = VARS.roomStableForwards[VARS.curRoomIndex] * 1;

            //camIniEulerangles = VARS.roomStableForwards[VARS.curRoomIndex] * Vector3.SignedAngle(Vector3.right, iniRight, VARS.roomStableForwards[VARS.curRoomIndex]);

            VARS.isInNewRoomCurRoomManagerResetOver = true;
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
