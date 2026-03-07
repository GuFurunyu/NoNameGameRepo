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

        isKeyCodeChanged = VARS.IsKeyCodeChanged;

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

            VARS.IsKeyCodeChanged = false;
        }

        if (!VARS.IsRotating &&
            !VARS.IsTwisting &&
            !VARS.IsMiniMapRotating &&
            VARS.IsInNewRoomAllResetOver)
        {
            VARS.IsInputtingUpKey = Input.GetKey(upKeyCode);
            VARS.IsInputtingDownKey = Input.GetKey(downKeyCode);
            if (!VARS.IsOptionPanelActivated)
            {
                VARS.IsInputtingLeftKey = Input.GetKey(leftKeyCode);
                VARS.IsInputtingRightKey = Input.GetKey(rightKeyCode);
                VARS.IsInputtingJumpKey = Input.GetKey(jumpKeyCode);
                VARS.IsInputtingDashKey = Input.GetKey(dashKeyCode);
            }
            VARS.IsInputtingConfirmKey = Input.GetKey(confirmKeyCode);
            VARS.IsInputtingBackKey = Input.GetKey(backKeyCode);

            VARS.IsUpKeyDown = Input.GetKeyDown(upKeyCode);
            VARS.IsDownKeyDown = Input.GetKeyDown(downKeyCode);
            if (!VARS.IsOptionPanelActivated)
            {
                VARS.IsLeftKeyDown = Input.GetKeyDown(leftKeyCode);
                VARS.IsRightKeyDown = Input.GetKeyDown(rightKeyCode);
                VARS.IsJumpKeyDown = Input.GetKeyDown(jumpKeyCode);
                VARS.IsDashKeyDown = Input.GetKeyDown(dashKeyCode);
            }
            VARS.IsConfirmKeyDown = Input.GetKeyDown(confirmKeyCode);
            VARS.IsBackKeyDown = Input.GetKeyDown(backKeyCode);

            VARS.IsUpKeyUp = Input.GetKeyUp(upKeyCode);
            VARS.IsDownKeyUp = Input.GetKeyUp(downKeyCode);
            if (!VARS.IsOptionPanelActivated)
            {
                VARS.IsLeftKeyUp = Input.GetKeyUp(leftKeyCode);
                VARS.IsRightKeyUp = Input.GetKeyUp(rightKeyCode);
                VARS.IsJumpKeyUp = Input.GetKeyUp(jumpKeyCode);
                VARS.IsDashKeyUp = Input.GetKeyUp(dashKeyCode);
            }
            VARS.IsConfirmKeyUp = Input.GetKeyUp(upKeyCode);
            VARS.IsBackKeyUp = Input.GetKeyUp(downKeyCode);
        }
        else
        {
            VARS.IsInputtingUpKey = false;
            VARS.IsInputtingDownKey = false;
            VARS.IsInputtingLeftKey = false;
            VARS.IsInputtingRightKey = false;
            VARS.IsInputtingJumpKey = false;
            VARS.IsInputtingDashKey = false;
            VARS.IsInputtingConfirmKey = false;
            VARS.IsInputtingBackKey = false;

            VARS.IsUpKeyDown = false;
            VARS.IsDownKeyDown = false;
            VARS.IsLeftKeyDown = false;
            VARS.IsRightKeyDown = false;
            VARS.IsJumpKeyDown = false;
            VARS.IsDashKeyDown = false;
            VARS.IsConfirmKeyDown = false;
            VARS.IsBackKeyDown = false;

            VARS.IsUpKeyUp = false;
            VARS.IsDownKeyUp = false;
            VARS.IsLeftKeyUp = false;
            VARS.IsRightKeyUp = false;
            VARS.IsJumpKeyUp = false;
            VARS.IsDashKeyUp = false;
            VARS.IsConfirmKeyUp = false;
            VARS.IsBackKeyUp = false;
        }
    }
}