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
            !VARS.IsTwisting;

        //catMove
        VARS.IsCatMoveMainPartExecutable =
            VARS.IsInNewRoomAllResetOver &&
            !VARS.IsRotating &&
            !VARS.IsTwisting &&
            !VARS.IsInMiniMap &&
            !VARS.IsOptionPanelActivated;

        //catRotate
        VARS.IsCatRotateMainPartExecutable =
            VARS.IsInNewRoomAllResetOver &&
            !VARS.IsTwisting &&
            !VARS.IsInMiniMap;

        //catEnergy
        VARS.IsCatEnergyResetExecutable =
            !VARS.IsRotating &&
            !VARS.IsTwisting &&
            !VARS.IsInAcce &&
            !VARS.IsInHotState &&
            !VARS.IsInColdState &&
            !VARS.IsInElectricState &&
            !VARS.IsInToxicState &&
            (VARS.IsOnGround ||
            VARS.IsInLiquid);

        //catTrigger
        VARS.IsCatTriggerMainPartExecutable =
            !VARS.IsRotating &&
            !VARS.IsTwisting;

        //blocksManager
        VARS.IsBlocksManagerBlocksMoveExecutable =
            VARS.IsInNewRoomAllResetOver;

        //optionsManager
        VARS.IsOptionsManagerActivationExecutable =
            !VARS.IsRotating &&
            !VARS.IsTwisting &&
            !VARS.IsZoomedOut &&
            !VARS.IsInMiniMap;
    }
}
