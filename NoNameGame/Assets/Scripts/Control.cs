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
    //KeyCode leftKeyCode;
    //KeyCode rightKeyCode;
    //KeyCode upKeyCode;
    //KeyCode downKeyCode;
    //KeyCode jumpKeyCode;
    //KeyCode dashKeyCode;
    //KeyCode confirmKeyCode;
    //KeyCode backKeyCode;

    //bool isKeyCodeChanged;
    #endregion

    void Start()
    {
        gameManager = GameObject.Find("GameManager");

        CONS = gameManager.GetComponent<Constants>();
        VARS = gameManager.GetComponent<Variables>();
        UFL = gameManager.GetComponent<UniversalFunctionsLibrary>();
        SEC = gameManager.GetComponent<ScriptsExecutionController>();

        #region ImportConstants
        #endregion

        #region ImportReferenceVariables
        #endregion
    }

    void Update()
    {
        #region ImportValueVariables
        #endregion

        //Debug.Log("enterControl");

        //if (Input.GetKey(KeyCode.Space))
        //{
        //    Debug.Log("inputtingSpace");
        //}

        int a;

        //if (isKeyCodeChanged)
        //{
        //    leftKeyCode = VARS.leftKeyCode;
        //    rightKeyCode = VARS.rightKeyCode;
        //    upKeyCode = VARS.upKeyCode;
        //    downKeyCode = VARS.downKeyCode;
        //    jumpKeyCode = VARS.jumpKeyCode;
        //    dashKeyCode = VARS.dashKeyCode;
        //    confirmKeyCode = VARS.confirmKeyCode;
        //    backKeyCode = VARS.backKeyCode;

        //    VARS.IsKeyCodeChanged = false;
        //}

        if (!VARS.IsRotating &&
            !VARS.IsTwisting &&
            !VARS.IsMiniMapRotating &&
            VARS.IsInNewRoomAllResetOver)
        {
            VARS.IsInputtingUpKey = Input.GetKey(VARS.upKeyCode);
            VARS.IsInputtingDownKey = Input.GetKey(VARS.downKeyCode);
            if (!VARS.IsOptionPanelActivated)
            {
                VARS.IsInputtingLeftKey = Input.GetKey(VARS.leftKeyCode);
                VARS.IsInputtingRightKey = Input.GetKey(VARS.rightKeyCode);
                VARS.IsInputtingJumpKey = Input.GetKey(VARS.jumpKeyCode);
                VARS.IsInputtingDashKey = Input.GetKey(VARS.dashKeyCode);
            }
            VARS.IsInputtingConfirmKey = Input.GetKey(VARS.confirmKeyCode);
            VARS.IsInputtingBackKey = Input.GetKey(VARS.backKeyCode);

            VARS.IsUpKeyDown = Input.GetKeyDown(VARS.upKeyCode);
            VARS.IsDownKeyDown = Input.GetKeyDown(VARS.downKeyCode);
            if (!VARS.IsOptionPanelActivated)
            {
                VARS.IsLeftKeyDown = Input.GetKeyDown(VARS.leftKeyCode);
                VARS.IsRightKeyDown = Input.GetKeyDown(VARS.rightKeyCode);
                VARS.IsJumpKeyDown = Input.GetKeyDown(VARS.jumpKeyCode);
                VARS.IsDashKeyDown = Input.GetKeyDown(VARS.dashKeyCode);
            }
            VARS.IsConfirmKeyDown = Input.GetKeyDown(VARS.confirmKeyCode);
            VARS.IsBackKeyDown = Input.GetKeyDown(VARS.backKeyCode);

            VARS.IsUpKeyUp = Input.GetKeyUp(VARS.upKeyCode);
            VARS.IsDownKeyUp = Input.GetKeyUp(VARS.downKeyCode);
            if (!VARS.IsOptionPanelActivated)
            {
                VARS.IsLeftKeyUp = Input.GetKeyUp(VARS.leftKeyCode);
                VARS.IsRightKeyUp = Input.GetKeyUp(VARS.rightKeyCode);
                VARS.IsJumpKeyUp = Input.GetKeyUp(VARS.jumpKeyCode);
                VARS.IsDashKeyUp = Input.GetKeyUp(VARS.dashKeyCode);
            }
            VARS.IsConfirmKeyUp = Input.GetKeyUp(VARS.upKeyCode);
            VARS.IsBackKeyUp = Input.GetKeyUp(VARS.downKeyCode);
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