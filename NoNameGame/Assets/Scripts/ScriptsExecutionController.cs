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

        //cameraManager
        VARS.IsCameraManagerMainPartExecutable =
            !VARS.IsInMainBoard &&
            VARS.IsInNewRoomAllResetOver;

        //catCollision
        VARS.IsCatCollisionMainPartExecutable =
            //Time.deltaTime < 0.0167f &&//~?
            !VARS.IsInMainBoard &&
            !VARS.IsPaused &&
            VARS.IsInNewRoomAllResetOver &&
            !VARS.IsRotating &&
            !VARS.IsTwisting &&
            !VARS.IsExiting;

        //catMove
        VARS.IsCatMoveMainPartExecutable =
            //Time.deltaTime < 0.0167f &&//~?
            !VARS.IsInMainBoard &&
            !VARS.IsPaused &&
            VARS.IsInNewRoomAllResetOver &&
            !VARS.IsRotating &&
            !VARS.IsTwisting &&
            !VARS.IsInMinimap &&
            !VARS.IsOptionPanelActivated &&
            !VARS.IsEdgeGateTriggered &&
            !(VARS.IsJustEnterNewFace &&
            (VARS.IsInUpEdgeGate ||
            VARS.IsInDownEdgeGate)) &&
            !VARS.IsJustReborn &&
            !VARS.IsEmbeddingFragments &&
            !VARS.IsCenterFulfilled &&
            !VARS.IsAbsorbingAnEnergyFragment &&
            !VARS.IsExiting;

        //catRotate
        VARS.IsCatRotateMainPartExecutable =
            !VARS.IsInMainBoard &&
            !VARS.IsPaused &&
            VARS.IsInNewRoomAllResetOver &&
            !VARS.IsTwisting &&
            !VARS.IsInMinimap &&
            !VARS.IsExiting;

        //catEnergy
        VARS.IsCatEnergyResetExecutable =
            !VARS.IsInMainBoard &&
            !VARS.IsPaused &&
            VARS.HasJumped &&
            VARS.IsInNewRoomAllResetOver &&
            !VARS.IsRotating &&
            !VARS.IsTwisting &&
            !VARS.IsJustIntoVoid &&
            !VARS.IsInputtingDashKey &&
            !VARS.IsDashing &&
            !VARS.IsInAcce &&
            !VARS.IsHighJumping &&
            !VARS.IsInHotState &&
            !VARS.IsInColdState &&
            !VARS.IsInElectricState &&
            !VARS.IsInToxicState &&
            !VARS.IsDiving &&
            (VARS.IsOnGround ||
            VARS.IsInLiquid ||
            VARS.IsJustReborn) &&
            !VARS.IsExiting;

        //catTrigger
        VARS.IsCatTriggerMainPartExecutable =
            !VARS.IsInMainBoard &&
            !VARS.IsPaused &&
            VARS.IsInNewRoomAllResetOver &&
            !VARS.IsRotating &&
            !VARS.IsTwisting &&
            !VARS.IsExiting;

        //blocksManager
        VARS.IsBlocksManagerMainPartExecutable =
            !VARS.IsInMainBoard &&
            !VARS.IsPaused &&
            VARS.IsInNewRoomAllResetOver &&
            VARS.IsIniRotation &&
            !VARS.IsRotating &&
            !VARS.IsTwisting &&
            !VARS.IsExiting;

        //minimapManager
        VARS.IsMinimapMainPartExecutable =
            !VARS.IsInMainBoard &&
            VARS.IsMinimapActivated &&
            VARS.IsInNewRoomAllResetOver &&
            !VARS.IsZoomedOut &&
            !VARS.IsOptionPanelActivated /*&&
            !VARS.IsInGuide*//*&&
            !VARS.IsInCenter*/;

        //optionsManager
        VARS.IsOptionsManagerActivationExecutable =
            !VARS.IsInMainBoard &&
            VARS.IsInNewRoomAllResetOver &&
            !VARS.IsRotating &&
            !VARS.IsTwisting &&
            !VARS.IsZoomedOut &&
            !VARS.IsInMinimap &&
            !VARS.IsInGuide;

        //guideManager
        VARS.IsGuideManagerMainPartExecutable =
            !VARS.IsInMainBoard;
    }
}
