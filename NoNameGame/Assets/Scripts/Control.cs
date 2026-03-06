using UnityEngine;

[DefaultExecutionOrder((int)ScriptsExecutionOrder.ExecutionOrder.control)]
public class Control : MonoBehaviour
{
    Constants CONS;
    Variables VARS;
    UniversalFunctionsLibrary UFL;
    ScriptsExecutionController SEC;

    GameObject gameManager;

    #region ConstantsUsed
    #endregion

    #region VariablesUsed
    KeyCode leftKeyCode;
    KeyCode rightKeyCode;
    KeyCode upKeyCode;
    KeyCode downKeyCode;
    KeyCode jumpKeyCode;
    KeyCode dashKeyCode;
    KeyCode confirmKeyCode;
    KeyCode backKeyCode;

    bool isKeyCodeChanged;
    #endregion

    void Start()
    {
        gameManager = GameObject.Find("GameManager");

        CONS = gameManager.GetComponent<Constants>();
        VARS = gameManager.GetComponent<Variables>();
        UFL = gameManager.GetComponent<UniversalFunctionsLibrary>();
        SEC = gameManager.GetComponent<ScriptsExecutionController>();
    }

    void Update()
    {
        //Debug.Log("enterControl");

        //if (Input.GetKey(KeyCode.Space))
        //{
        //    Debug.Log("inputtingSpace");
        //}

        isKeyCodeChanged = VARS.isKeyCodeChanged;

        if (isKeyCodeChanged)
        {
            leftKeyCode = VARS.leftKeyCode;
            rightKeyCode = VARS.rightKeyCode;
            upKeyCode = VARS.upKeyCode;
            downKeyCode = VARS.downKeyCode;
            jumpKeyCode = VARS.jumpKeyCode;
            dashKeyCode = VARS.dashKeyCode;
            confirmKeyCode = VARS.confirmKeyCode;
            backKeyCode = VARS.backKeyCode;

            VARS.isKeyCodeChanged = false;
        }

        VARS.isInputtingUpKey = Input.GetKey(upKeyCode);
        VARS.isInputtingDownKey = Input.GetKey(downKeyCode);
        if (!VARS.isOptionPanelActivated)
        {
            VARS.isInputtingLeftKey = Input.GetKey(leftKeyCode);
            VARS.isInputtingRightKey = Input.GetKey(rightKeyCode);
            VARS.isInputtingJumpKey = Input.GetKey(jumpKeyCode);
            VARS.isInputtingDashKey = Input.GetKey(dashKeyCode);
        }
        VARS.isInputtingConfirmKey = Input.GetKey(confirmKeyCode);
        VARS.isInputtingBackKey = Input.GetKey(backKeyCode);

        VARS.isUpKeyDown = Input.GetKeyDown(upKeyCode);
        VARS.isDownKeyDown = Input.GetKeyDown(downKeyCode);
        if (!VARS.isOptionPanelActivated)
        {
            VARS.isLeftKeyDown = Input.GetKeyDown(leftKeyCode);
            VARS.isRightKeyDown = Input.GetKeyDown(rightKeyCode);
            VARS.isJumpKeyDown = Input.GetKeyDown(jumpKeyCode);
            VARS.isDashKeyDown = Input.GetKeyDown(dashKeyCode);
        }
        VARS.isConfirmKeyDown = Input.GetKeyDown(confirmKeyCode);
        VARS.isBackKeyDown = Input.GetKeyDown(backKeyCode);

        ////debug
        //if (catTransform.position.y < 0)
        //{
        //    catTransform.position = new Vector3(catTransform.position.x, 1, catTransform.position.z);
        //}

        //if (VARS.isInNewRoomAllResetOver)
        //{
        //    #region CurRoom
        //    //camera
        //    //camTransform.position = RM.roomCenters[RM.curRoomIndex] - RM.roomStableForwards[RM.curRoomIndex] * 8;

        //    //if (leftRotationVector == Vector3.zero)
        //    //    leftRotationVector = RM.roomStableForwards[RM.curRoomIndex] * -1;
        //    //if (rightRotationVector == Vector3.zero)
        //    //    rightRotationVector = RM.roomStableForwards[RM.curRoomIndex] * 1;
        //    #endregion
        //}
    }
}