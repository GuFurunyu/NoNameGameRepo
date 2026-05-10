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
    float intoMinimapDownJumpKeyDownThreshold;
    float backCenterDoubleUpKeyDownThreshold;
    #endregion

    #region VariablesUsed
    //KeyCode leftKeyCode;
    //KeyCode rightKeyCode;
    //KeyCode upKeyCode;
    //KeyCode downKeyCode;
    //KeyCode jumpKeyCode;
    //KeyCode dashKeyCode;
    //KeyCode minimapKeyCode;
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
        intoMinimapDownJumpKeyDownThreshold = CONS.intoMinimapDownJumpKeyDownThreshold;
        backCenterDoubleUpKeyDownThreshold = CONS.backCenterDoubleUpKeyDownThreshold;
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

        //if (isKeyCodeChanged)
        //{
        //    leftKeyCode = VARS.leftKeyCode;
        //    rightKeyCode = VARS.rightKeyCode;
        //    upKeyCode = VARS.upKeyCode;
        //    downKeyCode = VARS.downKeyCode;
        //    jumpKeyCode = VARS.jumpKeyCode;
        //    dashKeyCode = VARS.dashKeyCode;
        //    minimapKeyCode = VARS.minimapKeyCode;
        //    backKeyCode = VARS.backKeyCode;

        //    VARS.IsKeyCodeChanged = false;
        //}

        if (!VARS.IsRotating &&
            !VARS.IsTwisting &&
            !VARS.IsMinimapRotating &&
            VARS.IsInNewRoomAllResetOver)
        {
            VARS.IsInputtingUpKey = Input.GetKey(VARS.upKeyCode);
            VARS.IsInputtingDownKey = Input.GetKey(VARS.downKeyCode);
            if (!VARS.IsOptionPanelActivated)
            {
                VARS.IsInputtingLeftKey = Input.GetKey(VARS.leftKeyCode);
                VARS.IsInputtingRightKey = Input.GetKey(VARS.rightKeyCode);
                VARS.IsInputtingJumpKey = Input.GetKey(VARS.jumpKeyCode);
                //VARS.IsInputtingDashKey = Input.GetKey(VARS.dashKeyCode);
                VARS.IsInputtingAcceKey = Input.GetKey(VARS.acceKeyCode);
                VARS.IsInputtingGrabKey = Input.GetKey(VARS.grabKeyCode);
            }
            VARS.IsInputtingMinimapKey = Input.GetKey(VARS.minimapKeyCode);
            VARS.IsInputtingBackKey = Input.GetKey(VARS.backKeyCode);

                VARS.IsUpKeyDown = Input.GetKeyDown(VARS.upKeyCode);
            VARS.IsDownKeyDown = Input.GetKeyDown(VARS.downKeyCode);
            if (!VARS.IsOptionPanelActivated)
            {
                VARS.IsLeftKeyDown = Input.GetKeyDown(VARS.leftKeyCode);
                VARS.IsRightKeyDown = Input.GetKeyDown(VARS.rightKeyCode);
                VARS.IsJumpKeyDown = Input.GetKeyDown(VARS.jumpKeyCode);
                //VARS.IsDashKeyDown = Input.GetKeyDown(VARS.dashKeyCode);
                VARS.IsAcceKeyDown = Input.GetKeyDown(VARS.acceKeyCode);
                VARS.IsGrabKeyDown = Input.GetKeyDown(VARS.grabKeyCode);
            }
            VARS.IsMinimapKeyDown = Input.GetKeyDown(VARS.minimapKeyCode);
            VARS.IsBackKeyDown = Input.GetKeyDown(VARS.backKeyCode);

            VARS.IsUpKeyUp = Input.GetKeyUp(VARS.upKeyCode);
            VARS.IsDownKeyUp = Input.GetKeyUp(VARS.downKeyCode);
            if (!VARS.IsOptionPanelActivated)
            {
                VARS.IsLeftKeyUp = Input.GetKeyUp(VARS.leftKeyCode);
                VARS.IsRightKeyUp = Input.GetKeyUp(VARS.rightKeyCode);
                VARS.IsJumpKeyUp = Input.GetKeyUp(VARS.jumpKeyCode);
                //VARS.IsDashKeyUp = Input.GetKeyUp(VARS.dashKeyCode);
                VARS.IsAcceKeyUp = Input.GetKeyUp(VARS.acceKeyCode);
                VARS.IsGrabKeyUp = Input.GetKeyUp(VARS.grabKeyCode);
            }
            VARS.IsMinimapKeyUp = Input.GetKeyUp(VARS.upKeyCode);
            VARS.IsBackKeyUp = Input.GetKeyUp(VARS.downKeyCode);

            //trigger
            if (VARS.IsUpKeyDown)
            {
                //backCenterTrigger
                if (Time.time - VARS.lastUpKeyDownTime < backCenterDoubleUpKeyDownThreshold &&
                    !VARS.IsOptionPanelActivated &&
                    !VARS.IsInMinimap)
                {
                    VARS.IsBackCenterTriggered = true;
                }

                VARS.lastUpKeyDownTime = Time.time;
            }
            if (VARS.IsDownKeyDown)
            {
                VARS.lastDownKeyDownTime = Time.time;
            }
            if (VARS.IsJumpKeyDown)
            {
                //intoMinimapTrigger
                if (Time.time - VARS.lastDownKeyDownTime < intoMinimapDownJumpKeyDownThreshold)
                {
                    VARS.IsIntoMinimapTriggered = true;
                }

                ////justOutOfMinimap
                //if (VARS.IsJustOutOfMinimap)
                //{
                //    VARS.IsJumpKeyDown = false;

                //    VARS.IsJustOutOfMinimap = false;
                //}
            }

            //specificKeys
            VARS.IsSpaceDown = Input.GetKeyDown(KeyCode.Space);
        }
        else
        {
            VARS.IsInputtingUpKey = false;
            VARS.IsInputtingDownKey = false;
            VARS.IsInputtingLeftKey = false;
            VARS.IsInputtingRightKey = false;
            VARS.IsInputtingJumpKey = false;
            VARS.IsInputtingDashKey = false;
            VARS.IsInputtingGrabKey = false;
            VARS.IsInputtingMinimapKey = false;
            VARS.IsInputtingBackKey = false;

            VARS.IsUpKeyDown = false;
            VARS.IsDownKeyDown = false;
            VARS.IsLeftKeyDown = false;
            VARS.IsRightKeyDown = false;
            VARS.IsJumpKeyDown = false;
            VARS.IsDashKeyDown = false;
            VARS.IsGrabKeyDown = false;
            VARS.IsMinimapKeyDown = false;
            VARS.IsBackKeyDown = false;

            VARS.IsUpKeyUp = false;
            VARS.IsDownKeyUp = false;
            VARS.IsLeftKeyUp = false;
            VARS.IsRightKeyUp = false;
            VARS.IsJumpKeyUp = false;
            VARS.IsDashKeyUp = false;
            VARS.IsGrabKeyUp = false;
            VARS.IsMinimapKeyUp = false;
            VARS.IsBackKeyUp = false;

            VARS.IsSpaceDown = false;
        }
    }
}