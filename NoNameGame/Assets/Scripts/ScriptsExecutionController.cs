using UnityEngine;

[DefaultExecutionOrder((int)ScriptsExecutionOrder.ExecutionOrder.scriptsExecutionController)]
public class ScriptsExecutionController : MonoBehaviour
{
    Constants CONS;
    Variables VARS;

    GameObject gameManager;

    private void Start()
    {
        gameManager = GameObject.Find("GameManager");

        CONS = gameManager.GetComponent<Constants>();
        VARS = gameManager.GetComponent<Variables>();

        #region ImportConstants

        #endregion

        #region ImportReferenceVariables

        #endregion
    }

    private void Update()
    {
        #region ImportValueVariables

        #endregion

        //catCollision
        VARS.IsCatCollisionMainPartExecutable =
            !VARS.IsRotating &&
            !VARS.IsTwisting &&
            !VARS.IsExiting;

        //catMove
        VARS.IsCatMoveMainPartExecutable =
            VARS.IsInNewRoomAllResetOver &&
            !VARS.IsRotating &&
            !VARS.IsTwisting &&
            !VARS.IsInMinimap &&
            !VARS.IsOptionPanelActivated &&
            !VARS.IsExiting &&
            !VARS.IsEdgeGateTriggered &&
            !(VARS.IsJustEnterNewFace &&
            !VARS.IsOnGround);

        //catRotate
        VARS.IsCatRotateMainPartExecutable =
            VARS.IsInNewRoomAllResetOver &&
            !VARS.IsTwisting &&
            !VARS.IsInMinimap &&
            !VARS.IsExiting;

        //catEnergy
        VARS.IsCatEnergyResetExecutable =
            VARS.IsInNewRoomAllResetOver &&
            !VARS.IsRotating &&
            !VARS.IsTwisting &&
            !VARS.IsInAcce &&
            !VARS.IsInHotState &&
            !VARS.IsInColdState &&
            !VARS.IsInElectricState &&
            !VARS.IsInToxicState &&
            (VARS.IsOnGround ||
            VARS.IsInLiquid) &&
            !VARS.IsExiting;

        //catTrigger
        VARS.IsCatTriggerMainPartExecutable =
            VARS.IsInNewRoomAllResetOver &&
            !VARS.IsRotating &&
            !VARS.IsTwisting &&
            !VARS.IsExiting;

        //blocksManager
        VARS.IsBlocksManagerMainPartExecutable =
            VARS.IsInNewRoomAllResetOver &&
            VARS.IsIniRotation &&
            !VARS.IsRotating &&
            !VARS.IsTwisting &&
            !VARS.IsExiting;

        //optionsManager
        VARS.IsOptionsManagerActivationExecutable =
            VARS.IsInNewRoomAllResetOver &&
            !VARS.IsRotating &&
            !VARS.IsTwisting &&
            !VARS.IsZoomedOut &&
            !VARS.IsInMinimap;
    }
}
