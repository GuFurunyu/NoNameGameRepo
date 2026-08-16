using UnityEngine;
using UnityEngine.Video;

[DefaultExecutionOrder((int)ScriptsExecutionOrder.ExecutionOrder.catMove)]
public class CatMove : MonoBehaviour
{
    Constants CONS;
    Variables VARS;
    UniversalFunctionsLibrary UFL;
    ScriptsExecutionController SEC;

    GameObject gameManager;

    //horSpeed

    //verSpeed

    //jumpPreInput
    bool isJumpPreInputed;
    float jumpPreInputStartTime;

    //jumpPostInput
    float jumpPostInputStartTime;

    //wallJump
    //float wallJumpPreInputStartTime;
    float wallJumpPostInputStartTime;
    bool isPostWallJumpToRight;

    //dash
    //Vector3 VARS.curDashVector;
    float dashStartTime;

    //dashPreInput
    bool isDashPreInputed;
    //float dashPreInputStartTime;

    //acce
    float curAcceBonus = 1;
    float curGravityAcceBonus;
    float curClimbingAcceBonus;    

    float tempFloat;
    Vector3 tempVector;

    #region ConstantsUsed
    float justEnterNewFaceTime;

    Transform catTransform;

    GameObject catIniPositionPoint;

    float catMoveFixedDeltaTime;

    //horSpeed
    float horAcce;
    float horReverseAcce;
    float horMaxSpeed;
    float horStopThres;
    float horWallJumpBonusSpeed;
    float horToCeilingMaxSpeed;

    //verSpeed
    float verIniSpeed;
    float verAcce;
    float gravityAcce;
    float climbSpeed;
    float verMaxSpeed;
    float verFallMaxSpeed;

    //jumpPreInput
    float jumpPreInputThres;

    //jumpPostInput
    float jumpPostInputTres;

    //wallJump
    float wallJumpPreInputThres;
    float wallJumpPostInputThres;

    //dash
    float dashIniSpeed;
    float dashTime;

    //dashPreInput
    //float dashPreInputThres;

    //acceBonus
    float acceBonus;

    float horMovingAfterToCeilingTime;

    float attachWallEnergyDecreaseSpeed;
    float climbEnergyDecreaseSpeed;
    float attachCeilingEnergyDecreaseSpeed;
    float inAcceEnergyDecreaseSpeed;

    float jumpEnergyCost;
    float dashEnergyCost;
    #endregion

    #region VariablesUsed
    Vector3 curRight;
    Vector3 curUp;

    TileData curDownTileData;
    TileData curUpTileData;
    TileData curLeftTileData;
    TileData curRightTileData;

    TileData curLiquidTileData;
    TileData curGasTileData;
    TileData curMistTileData;

    float buoyancyDistanceFixFloat;

    //float curEnergy;

    float curRoomGravity;
    #endregion

    #region BoolVariablesUsed
    bool isOnGround;
    bool isToCeiling;
    bool isLeftBlocked;
    bool isRightBlocked;
    bool isInLiquid;
    bool isInGas;
    bool isInMist;
    #endregion

    void Start()
    {
        gameManager = GameObject.Find("GameManager");

        CONS = gameManager.GetComponent<Constants>();
        VARS = gameManager.GetComponent<Variables>();
        UFL = gameManager.GetComponent<UniversalFunctionsLibrary>();
        SEC = gameManager.GetComponent<ScriptsExecutionController>();

        #region ImportConstants
        justEnterNewFaceTime = CONS.justEnterNewFaceTime;
        catTransform = CONS.catTransform;
        catIniPositionPoint = CONS.catIniPositionPoint;
        catMoveFixedDeltaTime = CONS.catMoveFixedDeltaTime;
        horAcce = CONS.horAcce;
        horReverseAcce = CONS.horReverseAcce;
        horMaxSpeed = CONS.horMaxSpeed;
        horStopThres = CONS.horStopThres;
        horWallJumpBonusSpeed = CONS.horWallJumpBonusSpeed;
        horToCeilingMaxSpeed = CONS.horToCeilingMaxSpeed;
        verIniSpeed = CONS.verIniSpeed;
        verAcce = CONS.verAcce;
        gravityAcce = CONS.gravityAcce;
        climbSpeed = CONS.climbSpeed;
        verMaxSpeed = CONS.verMaxSpeed;
        verFallMaxSpeed = CONS.verFallMaxSpeed;
        jumpPreInputThres = CONS.jumpPreInputThres;
        jumpPostInputTres = CONS.jumpPostInputTres;
        wallJumpPreInputThres = CONS.wallJumpPreInputThres;
        wallJumpPostInputThres = CONS.wallJumpPostInputThres;
        dashIniSpeed = CONS.dashIniSpeed;
        dashTime = CONS.dashTime;
        acceBonus = CONS.acceBonus;
        horMovingAfterToCeilingTime = CONS.horMovingAfterToCeilingTime;
        attachWallEnergyDecreaseSpeed = CONS.attachWallEnergyDecreaseSpeed;
        climbEnergyDecreaseSpeed = CONS.climbEnergyDecreaseSpeed;
        attachCeilingEnergyDecreaseSpeed = CONS.attachCeilingEnergyDecreaseSpeed;
        inAcceEnergyDecreaseSpeed = CONS.inAcceEnergyDecreaseSpeed;
        jumpEnergyCost = CONS.jumpEnergyCost;
        dashEnergyCost = CONS.dashEnergyCost;
        #endregion

        #region ImportReferenceVariables
        #endregion

        VARS.horCurSpeed = 0;
        VARS.verCurSpeed = 0;
    }

    //private void FixedUpdate()
    void Update()
    {
        #region ImportValueVariables
        curRight = VARS.curRight;
        curUp = VARS.curUp;
        curDownTileData = VARS.curDownTileData;
        curUpTileData = VARS.curUpTileData;
        curLeftTileData = VARS.curLeftTileData;
        curRightTileData = VARS.curRightTileData;
        curLiquidTileData = VARS.curLiquidTileData;
        curGasTileData = VARS.curGasTileData;
        curMistTileData = VARS.curMistTileData;
        buoyancyDistanceFixFloat = VARS.buoyancyDistanceFixFloat;
        curRoomGravity = VARS.curRoomGravity;
        #endregion

        #region ImportBoolVariables
        isOnGround = VARS.IsOnGround;
        isToCeiling = VARS.IsToCeiling;
        isLeftBlocked = VARS.IsLeftBlocked;
        isRightBlocked = VARS.IsRightBlocked;
        isInLiquid = VARS.IsInLiquid;
        isInGas = VARS.IsInGas;
        isInMist = VARS.IsInMist;
        #endregion

        ////debug
        //VARS.horCurSpeed = 0;
        //VARS.verCurSpeed = 0;

        //if (Time.time - VARS.catMoveLastUpdatedTime < catMoveFixedDeltaTime)
        //{
        //    return;
        //}
        //else
        //{
        //    VARS.catMoveLastUpdatedTime = Time.time;
        //}

        //justEnterNewFaceFix
        if (VARS.IsJustEnterNewFace)
        {
            if ((VARS.IsInUpEdgeGate && (VARS.IsLeftKeyDown || VARS.IsRightKeyDown || VARS.IsDownKeyDown)) ||
                (VARS.IsInDownEdgeGate && VARS.IsJumpKeyDown) ||
                Time.time - VARS.justEnterNewFaceStartTime > justEnterNewFaceTime)
            {
                VARS.IsJustEnterNewFace = false;

                VARS.IsCatMoveMainPartExecutable =
                    //Time.deltaTime < 0.0167f &&//~?
                    VARS.IsInNewRoomAllResetOver &&
                    !VARS.IsRotating &&
                    !VARS.IsTwisting &&
                    !VARS.IsInMinimap &&
                    !VARS.IsOptionPanelActivated &&
                    !VARS.IsExiting &&
                    !VARS.IsEdgeGateTriggered &&
                    !(VARS.IsJustEnterNewFace &&
                    (VARS.IsInUpEdgeGate ||
                    VARS.IsInDownEdgeGate));
            }
            //else
            //{
            //    return;
            //}
        }

        #region Move
        if (VARS.IsCatMoveMainPartExecutable)
        {
            #region LeftAndRight
            //setCurParameters
            if (!isInLiquid && !isInGas && !isInMist)
            {
                if (isOnGround)
                {
                    VARS.horCurAcce = horAcce * curDownTileData.friction * curAcceBonus;
                    VARS.horCurReverseAcce = horReverseAcce * curDownTileData.friction * curAcceBonus;
                    VARS.horCurMaxSpeed = (horMaxSpeed - curDownTileData.tackiness) * curAcceBonus;
                }
                else
                {
                    VARS.horCurAcce = horAcce * curAcceBonus;
                    VARS.horCurReverseAcce = horReverseAcce * curAcceBonus;
                    VARS.horCurMaxSpeed = horMaxSpeed * curAcceBonus;
                }
            }
            else
            {
                if (isInLiquid)
                {
                    VARS.horCurAcce = (horAcce / curLiquidTileData.fluidDrag) * curAcceBonus;
                    VARS.horCurReverseAcce = (horReverseAcce / curLiquidTileData.fluidDrag) * curAcceBonus;
                    VARS.horCurMaxSpeed = (horMaxSpeed / curLiquidTileData.fluidDrag) * curAcceBonus;
                }
                else if (isInGas)
                {
                    VARS.horCurAcce = (horAcce / curGasTileData.fluidDrag) * curAcceBonus;
                    VARS.horCurReverseAcce = (horReverseAcce / curGasTileData.fluidDrag) * curAcceBonus;
                    VARS.horCurMaxSpeed = (horMaxSpeed / curGasTileData.fluidDrag) * curAcceBonus;
                }
                else if (isInMist)
                {
                    VARS.horCurAcce = (horAcce / curMistTileData.fluidDrag) * curAcceBonus;
                    VARS.horCurReverseAcce = (horReverseAcce / curMistTileData.fluidDrag) * curAcceBonus;
                    VARS.horCurMaxSpeed = (horMaxSpeed / curMistTileData.fluidDrag) * curAcceBonus;
                }
            }

            //if (isToCeiling)
            //{
            //    horCurMaxSpeed = horToCeilingMaxSpeed;
            //}

            //horInput
            if (VARS.IsInputtingLeftKey ||
                VARS.IsInputtingRightKey)
            {
                //moveLeft            
                if (VARS.IsInputtingLeftKey)
                {
                    //forDash
                    //lastHorDirectionInput = leftKeyCode;
                    VARS.curFacingDirectionIndex = 1;
                    VARS.curDashingDirectionIndex = 1;

                    if (VARS.horCurSpeed >= -VARS.horCurMaxSpeed &&
                        !VARS.IsLeftBlocked)
                    {
                        VARS.horCurSpeed += -VARS.horCurAcce * Time.deltaTime;
                    }

                    VARS.IsHorInputting = true;
                }
                //moveRight
                else if (VARS.IsInputtingRightKey)
                {
                    //lastHorDirectionInput = rightKeyCode;
                    VARS.curFacingDirectionIndex = 2;
                    VARS.curDashingDirectionIndex= 2;

                    if (VARS.horCurSpeed <= VARS.horCurMaxSpeed &&
                        !VARS.IsRightBlocked)
                    {
                        VARS.horCurSpeed += VARS.horCurAcce * Time.deltaTime;
                    }

                    VARS.IsHorInputting = true;
                }
            }
            else
            {
                VARS.curFacingDirectionIndex = 0;
            }

            //isLeftBlocked
            if (isLeftBlocked &&
                VARS.curFacingDirectionIndex == 1)
            {
                //attachWall
                //if (!isOnGround)
                //{
                if (VARS.IsAttachWallEnabled)
                {
                    if (VARS.IsInputtingLeftKey/* &&
                        VARS.IsInputtingGrabKey*/)
                    {
                        //lastHorDirectionInput = rightKeyCode;
                        //VARS.curFacingDirectionIndex = 1;

                        if (/*VARS.curEnergy > 0*/true)
                        {
                            //verCurSpeed = 0;
                            //UFL.SetVerCurSpeed(0);
                            VARS.verCurSpeed = 0;

                            VARS.IsAttachWall = true;

                            VARS.curAttachedWallTile = VARS.curLeftTile;
                        }
                        else
                        {
                            VARS.IsAttachWall = false;
                        }
                    }
                    else
                    {
                        VARS.IsAttachWall = false;
                    }
                }
                //}
            }
            //ifRightBlocked
            else if (isRightBlocked &&
                VARS.curFacingDirectionIndex == 2)
            {
                //attachWall
                //if (!isOnGround)
                //{
                if (VARS.IsAttachWallEnabled)
                {
                    if (VARS.IsInputtingRightKey/* &&
                        VARS.IsInputtingGrabKey*/)
                    {
                        //lastHorDirectionInput = rightKeyCode;
                        //VARS.curFacingDirectionIndex = 2;

                        if (/*VARS.curEnergy > 0*/true)
                        {
                            //verCurSpeed = 0;
                            //UFL.SetVerCurSpeed(0);
                            VARS.verCurSpeed = 0;

                            VARS.IsAttachWall = true;

                            VARS.curAttachedWallTile = VARS.curRightTile;
                        }
                        else
                        {
                            VARS.IsAttachWall = false;
                        }
                    }
                    else
                    {
                        VARS.IsAttachWall = false;
                    }
                }
                //}
            }
            //awayFormWall
            else
            {
                VARS.IsAttachWall = false;
            }

            ////awayFromWall
            //if (!isLeftBlocked &&
            //    !isRightBlocked)
            //{
            //    VARS.IsAttachWall = false;
            //}

            //attachWallEnergyDecrease
            if (VARS.IsAttachWall)
            {
                //curEnergy -= attachWallEnergyDecreaseSpeed * Time.deltaTime;
                //UFL.AddCurTargetEnergy(-attachWallEnergyDecreaseSpeed * Time.deltaTime);
                VARS.curTargetEnergy += -attachWallEnergyDecreaseSpeed * Time.deltaTime;
            }

            //stop
            if (!VARS.IsHorInputting)
            {
                if (VARS.horCurSpeed < -horStopThres)
                {
                    if (isOnGround)
                    {
                        VARS.horCurSpeed += VARS.horCurReverseAcce * Time.deltaTime;
                    }
                    else
                    {
                        VARS.horCurSpeed += VARS.horCurReverseAcce / 2 * Time.deltaTime;
                    }
                }
                else if (VARS.horCurSpeed > horStopThres)
                {
                    if (isOnGround)
                    {
                        VARS.horCurSpeed += -VARS.horCurReverseAcce * Time.deltaTime;
                    }
                    else
                    {
                        VARS.horCurSpeed += -VARS.horCurReverseAcce / 2 * Time.deltaTime;
                    }
                }
                else
                {
                    VARS.horCurSpeed = 0;
                }
            }

            //movingInAttachingCeiling
            if (VARS.IsAttachCeiling &&
                VARS.horCurSpeed != 0)
            {
                VARS.IsMovingInAttachingCeiling = true;
            }
            else
            {
                VARS.IsMovingInAttachingCeiling = false;
            }

            //horSpeedSum
            if (VARS.horCurSpeed != 0)
            {
                //inDashingHorCurSpeedKeepsConstant
                if (VARS.IsDashing)
                {
                    VARS.horCurSpeed = VARS.curDashHorSpeed;
                }

                UFL.AddCatPosition(curRight * VARS.horCurSpeed * Time.deltaTime);
            }
            #endregion

            #region UpAndDown
            //setCurParameters
            if (!isInLiquid && !isInGas && !isInMist)
            {
                if (isOnGround)
                {
                    VARS.verCurIniSpeed = verIniSpeed;
                    VARS.curGravityAcce = gravityAcce * curGravityAcceBonus;
                    VARS.verCurMaxSpeed = verMaxSpeed - curDownTileData.tackiness;
                }
                //HR: ~fluid
                else
                {
                    VARS.verCurIniSpeed = verIniSpeed;
                    VARS.curGravityAcce = gravityAcce * curGravityAcceBonus;
                    VARS.verCurMaxSpeed = verMaxSpeed;
                }
            }
            else
            {
                if (isInLiquid)
                {
                    VARS.verCurIniSpeed = verIniSpeed / curLiquidTileData.fluidDrag;
                    if (VARS.IsInputtingUpKey)
                    {
                        VARS.curGravityAcce = (gravityAcce - gravityAcce * curLiquidTileData.mass * (1 - buoyancyDistanceFixFloat) /** 3*/ /** 2*/ /** 1.5f*/ * 2) * curGravityAcceBonus;
                    }
                    else if (VARS.IsInputtingDownKey)
                    {
                        VARS.curGravityAcce = (gravityAcce - gravityAcce * curLiquidTileData.mass * (1 - buoyancyDistanceFixFloat) /** 3*/ /** 2*/ /** 1.5f*/ * 0.9f) * curGravityAcceBonus;
                    }
                    else
                    {
                        VARS.curGravityAcce = (gravityAcce - gravityAcce * curLiquidTileData.mass * (1 - buoyancyDistanceFixFloat) /** 3*/ /** 2*/ * 1.5f) * curGravityAcceBonus;
                    }
                    VARS.verCurMaxSpeed = verMaxSpeed / curLiquidTileData.fluidDrag;

                    VARS.IsJustInLiquid = true;
                }
                else if (isInGas)
                {
                    VARS.verCurIniSpeed = verIniSpeed / curGasTileData.fluidDrag;
                    //curGravityAcce = (gravityAcce - gravityAcce * curGasTileData.mass * 0.5f /*/ 2*/) * curGravityAcceBonus;
                    if (VARS.IsInputtingUpKey)
                    {
                        VARS.curGravityAcce = (gravityAcce - gravityAcce * curGasTileData.mass * 0.75f) * curGravityAcceBonus;
                    }
                    else if (VARS.IsInputtingDownKey)
                    {
                        VARS.curGravityAcce = (gravityAcce - gravityAcce * curGasTileData.mass * 0.5f) * curGravityAcceBonus;
                    }
                    else
                    {
                        VARS.curGravityAcce = (gravityAcce - gravityAcce * curGasTileData.mass * 0.25f) * curGravityAcceBonus;
                    }
                    VARS.verCurMaxSpeed = verMaxSpeed / curGasTileData.fluidDrag;
                }
                else if (isInMist)
                {
                    VARS.verCurIniSpeed = verIniSpeed / curMistTileData.fluidDrag;
                    //curGravityAcce = (gravityAcce - gravityAcce * curMistTileData.mass * 0.5f /*/ 2*/) * curGravityAcceBonus;
                    if (VARS.IsInputtingUpKey)
                    {
                        VARS.curGravityAcce = (gravityAcce - gravityAcce * curMistTileData.mass * 0.75f) * curGravityAcceBonus;
                    }
                    else if (VARS.IsInputtingDownKey)
                    {
                        VARS.curGravityAcce = (gravityAcce - gravityAcce * curMistTileData.mass * 0.5f) * curGravityAcceBonus;
                    }
                    else
                    {
                        VARS.curGravityAcce = (gravityAcce - gravityAcce * curMistTileData.mass * 0.25f) * curGravityAcceBonus;
                    }
                    VARS.verCurMaxSpeed = verMaxSpeed / curMistTileData.fluidDrag;
                }
            }

            if (!isInLiquid)
            {
                VARS.IsJustNotInLiquid = true;
            }

            if (isLeftBlocked)
            {
                if (VARS.IsInputtingLeftKey)
                {
                    VARS.curClimbSpeed = climbSpeed * curLeftTileData.friction * curClimbingAcceBonus;
                }
            }
            if (isRightBlocked)
            {
                if (VARS.IsInputtingRightKey)
                {
                    VARS.curClimbSpeed = climbSpeed * curRightTileData.friction * curClimbingAcceBonus;
                }
            }

            //outOfLiquidDeacce
            if (!isInLiquid &&
                VARS.IsJustInLiquid &&
                VARS.verCurSpeed > 0 &&
                !VARS.IsHighJumping)
            {
                VARS.verCurSpeed /= 2;

                VARS.IsJustInLiquid = false;
            }

            //intoLiquidDeacce
            if (isInLiquid &&
                VARS.IsJustNotInLiquid &&
                VARS.verCurSpeed < 0)
            {
                //Debug.Log("intoLiquidDeacce");

                VARS.verCurSpeed /= 2;

                VARS.IsJustNotInLiquid = false;
            }

            //outOfHighJumping
            if (VARS.IsJumpKeyDown ||
                !VARS.IsInputtingJumpKey)
            {
                VARS.IsHighJumping = false;
            }

            //just"||isInLiquid"?
            if (isOnGround ||
                isInLiquid)
            {
                if (isInLiquid)
                {
                    //gravity
                    if (!VARS.IsAttachWall &&
                        !VARS.IsCatMovedByRailBlock &&
                        !VARS.IsHorMovingAfterToCeiling)
                    {
                        //verCurSpeed -= curGravityAcce * curRoomGravity * Time.deltaTime;
                        //UFL.AddVerCurSpeed(-curGravityAcce * curRoomGravity * Time.deltaTime);
                        VARS.verCurSpeed += -VARS.curGravityAcce * curRoomGravity * Time.deltaTime;
                    }

                    //highJump
                    if (VARS.IsInputtingJumpKey)
                    {
                        if (VARS.IsHighJumping)
                        {
                            if (VARS.verCurSpeed > 0 &&
                                VARS.verCurSpeed <= /*verCurMaxSpeed*/ VARS.curHighJumpingMaxSpeed)
                            {
                                //verCurSpeed += verAcce * Time.deltaTime;
                                //UFL.AddVerCurSpeed(verAcce * Time.deltaTime);
                                VARS.verCurSpeed += verAcce * Time.deltaTime;
                            }
                            else
                            {
                                VARS.IsHighJumping = false;
                            }
                        }
                    }
                    else
                    {
                        VARS.IsHighJumping = false;
                    }
                }

                //verCurSpeed = 0;

                //jump
                if (VARS.IsJumpKeyDown ||
                    (isJumpPreInputed &&
                    Time.time - jumpPreInputStartTime <= jumpPreInputThres))
                {
                    //Debug.Log("jump");

                    Jump();
                }

                isJumpPreInputed = false;

                jumpPostInputStartTime = Time.time;
            }
            else
            {
                //gravity
                if (!VARS.IsAttachWall &&
                    !VARS.IsCatMovedByRailBlock &&
                    !VARS.IsHorMovingAfterToCeiling)
                {
                    //verCurSpeed -= curGravityAcce * curRoomGravity * Time.deltaTime;
                    //UFL.AddVerCurSpeed(-curGravityAcce * curRoomGravity * Time.deltaTime);
                    VARS.verCurSpeed += -VARS.curGravityAcce * curRoomGravity * Time.deltaTime;

                    //fallQuicklier
                    //if (VARS.verCurSpeed < 0/* && VARS.IsIniRotation*/)
                    //{
                    //    VARS.verCurSpeed += -curGravityAcce * curRoomGravity * 0.5f /*0.2f*/ * Time.deltaTime;
                    //}
                }

                //highJump
                if (VARS.IsInputtingJumpKey)
                {
                    if (VARS.IsHighJumping)
                    {
                        if (VARS.verCurSpeed > 0 &&
                            VARS.verCurSpeed <= /*verCurMaxSpeed*/ VARS.curHighJumpingMaxSpeed)
                        {
                            //verCurSpeed += verAcce * Time.deltaTime;
                            //UFL.AddVerCurSpeed(verAcce * Time.deltaTime);
                            VARS.verCurSpeed += verAcce * Time.deltaTime;
                        }
                        else
                        {
                            VARS.IsHighJumping = false;
                        }
                    }
                }
                else
                {
                    VARS.IsHighJumping = false;
                }

                //specialJumps1
                if (VARS.IsJumpKeyDown)
                {
                    isJumpPreInputed = true;

                    jumpPreInputStartTime = Time.time;

                    //wallJumpPreInputStartTime = Time.time;

                    if (Time.time - jumpPostInputStartTime <= jumpPostInputTres)
                    {
                        Jump();

                        jumpPostInputStartTime = 0;
                    }

                    if (Time.time - wallJumpPostInputStartTime <= wallJumpPostInputThres &&
                        wallJumpPostInputStartTime != 0)
                    {
                        if (isPostWallJumpToRight)
                        {
                            VARS.horCurSpeed = horMaxSpeed + horWallJumpBonusSpeed - curLeftTileData.tackiness;

                            Jump();
                        }
                        else
                        {
                            VARS.horCurSpeed = -horMaxSpeed - horWallJumpBonusSpeed + curRightTileData.tackiness;

                            Jump();
                        }
                    }
                }

                //specialJumps2
                if (!VARS.IsInputtingUpKey)
                {
                    if (VARS.IsJumpKeyDown /*||
                        Time.time - wallJumpPreInputStartTime <= wallJumpPreInputThres*/)
                    {
                        //if (Time.time - wallJumpPreInputStartTime <= wallJumpPreInputThres)
                        //{
                        //    Debug.Log("enter");
                        //}

                        //wallJump
                        if (isLeftBlocked)
                        {
                            VARS.horCurSpeed = horMaxSpeed + horWallJumpBonusSpeed - curLeftTileData.tackiness;

                            Jump();
                        }
                        else if (isRightBlocked)
                        {
                            VARS.horCurSpeed = -horMaxSpeed - horWallJumpBonusSpeed + curRightTileData.tackiness;

                            Jump();
                        }
                    }
                    if (isLeftBlocked)
                    {
                        isPostWallJumpToRight = true;

                        wallJumpPostInputStartTime = Time.time;
                    }
                    else if (isRightBlocked)
                    {
                        isPostWallJumpToRight = false;

                        wallJumpPostInputStartTime = Time.time;
                    }
                }

                //climb
                if (VARS.IsClimbEnabled)
                {
                    if (VARS.IsAttachWall)
                    {
                        //up
                        if (VARS.IsInputtingUpKey)
                        {
                            //verCurSpeed = curClimbSpeed;
                            //UFL.SetVerCurSpeed(curClimbSpeed);
                            VARS.verCurSpeed = VARS.curClimbSpeed;

                            //curEnergy -= climbEnergyDecreaseSpeed * Time.deltaTime;
                            //UFL.AddCurTargetEnergy(-climbEnergyDecreaseSpeed * Time.deltaTime);
                            VARS.curTargetEnergy += -climbEnergyDecreaseSpeed * Time.deltaTime;

                            ////climbJump
                            //if (VARS.IsJumpKeyDown)
                            //{
                            //    Jump();

                            //    isAttachWall = false;
                            //}

                            VARS.IsClimbing = true;
                        }
                        //down
                        else if (VARS.IsInputtingDownKey)
                        {
                            VARS.verCurSpeed = -VARS.curClimbSpeed;
                            VARS.curTargetEnergy += -climbEnergyDecreaseSpeed * 0.75f * Time.deltaTime;

                            VARS.IsClimbing = true;
                        }
                        else
                        {
                            VARS.IsClimbing = false;
                        }
                    }
                    else
                    {
                        VARS.IsClimbing = false;
                    }
                }
            }

            //ifToCeiling
            if (isToCeiling)
            {
                //horMovingAfterToCeiling
                if (VARS.verCurSpeed > 0 || VARS.IsInputtingJumpKey)
                {
                    if (VARS.IsHorMovingAfterToCeilingActivated)
                    {
                        if (Mathf.Abs(VARS.horMovingAfterToCeilingStartTime) < 1e-6f)
                        {
                            //Debug.Log("horMovingAfterToCeiling");

                            VARS.horMovingAfterToCeilingStartTime = Time.time;

                            VARS.IsHorMovingAfterToCeiling = true;

                            VARS.IsHorMovingAfterToCeilingActivated = false;
                        }
                    }
                }

                if (VARS.verCurSpeed > 0)
                {
                    //verCurSpeed = 0;
                    //UFL.SetVerCurSpeed(0);
                    VARS.verCurSpeed = 0;
                }

                //attachCeiling
                if (VARS.IsAttachCeilingEnabled)
                {
                    if (VARS.IsInputtingUpKey)
                    {
                        if (/*VARS.curEnergy > 0*/true)
                        {
                            //verCurSpeed = 0;
                            //UFL.SetVerCurSpeed(0);
                            VARS.verCurSpeed = 0;

                            //curEnergy -= attachCeilingEnergyDecreaseSpeed * Time.deltaTime;
                            //UFL.AddCurTargetEnergy(-attachCeilingEnergyDecreaseSpeed * Time.deltaTime);
                            VARS.curTargetEnergy += -attachCeilingEnergyDecreaseSpeed * Time.deltaTime;

                            VARS.IsAttachCeiling = true;

                            VARS.curAttachedCeilingTile = VARS.curUpTile;
                        }
                        else
                        {
                            VARS.IsAttachCeiling = false;
                        }
                    }
                    else
                    {
                        VARS.IsAttachCeiling = false;
                    }
                }
            }
            else
            {
                VARS.IsAttachCeiling = false;
            }

            //horMovingAfterToCeiling
            if (!VARS.IsHorMovingAfterToCeilingActivated && VARS.IsOnGround)
            {
                //Debug.Log("horMovingAfterToCeilingReactivated");

                VARS.IsHorMovingAfterToCeilingActivated = true;

                VARS.horMovingAfterToCeilingStartTime = 0;

                VARS.IsHorMovingAfterToCeiling = false;
            }
            if (VARS.IsHorMovingAfterToCeiling)
            {
                if (Time.time - VARS.horMovingAfterToCeilingStartTime > horMovingAfterToCeilingTime)
                {
                    VARS.horMovingAfterToCeilingStartTime = 0;

                    VARS.IsHorMovingAfterToCeiling = false;
                }
            }

            if (VARS.verCurSpeed != 0)
            {
                //fallMaxSpeed
                if (VARS.verCurSpeed < -verFallMaxSpeed)
                {
                    VARS.verCurSpeed = -verFallMaxSpeed;
                }

                //inDashingVerCurSpeedKeepsZero
                if (VARS.IsDashing)
                {
                    VARS.verCurSpeed = 0;
                }

                ////debug
                //VARS.verCurSpeed = 0;

                //verSpeedSum
                //catTransform.position += curUp * VARS.verCurSpeed * Time.deltaTime;
                UFL.AddCatPosition(curUp * VARS.verCurSpeed * Time.deltaTime);

                //energyDecrease
                if (VARS.verCurSpeed > 0)
                {
                    //curEnergy -= verCurSpeed * jumpEnergyDecreaseSpeedFixParameter * Time.deltaTime;

                    //if (curEnergy <= 0)
                    //{
                    //    verCurSpeed = 0;
                    //}
                }
            }
            #endregion

            #region Dash
            if (VARS.IsDashEnabled)
            {
                if (/*dashStartTime == 0*/
                    !VARS.IsDashing)
                {
                    if (VARS.curEnergy > dashEnergyCost &&
                        (VARS.IsDashKeyDown || isDashPreInputed))
                    {
                        //dashPreInput
                        if (!VARS.IsDashKeyDown && isDashPreInputed)
                        {
                            Debug.Log("dashPreInput");
                        }
                        isDashPreInputed = false;

                        //if (/*!VARS.IsDashing &&*/
                        //    /*((VARS.IsOnGround && VARS.IsInputtingDashKey) ||
                        //    (!VARS.IsOnGround && VARS.IsDashKeyDown))*/
                        //    /*VARS.IsInputtingDashKey*/
                        //    VARS.IsDashKeyDown)
                        //{
                        //dir
                        if (VARS.IsInputtingLeftKey)
                        {
                            VARS.curDashVector = -curRight;
                        }
                        else if (VARS.IsInputtingRightKey)
                        {
                            VARS.curDashVector = curRight;
                        }
                        else
                        {
                            if (/*lastHorDirectionInput == leftKeyCode*/
                                VARS.curDashingDirectionIndex == 1)
                            {
                                VARS.curDashVector = -curRight;
                            }
                            else if (/*lastHorDirectionInput == rightKeyCode*/
                                VARS.curDashingDirectionIndex == 2 ||
                                VARS.curDashingDirectionIndex == 0)
                            {
                                VARS.curDashVector = curRight;
                            }
                        }

                        ////blockedReverse
                        //if (VARS.curDashVector == -curRight)
                        //{
                        //    if (isLeftBlocked)
                        //    {
                        //        VARS.curDashVector = curRight;
                        //    }
                        //}
                        //else if (VARS.curDashVector == curRight)
                        //{
                        //    if (isRightBlocked)
                        //    {
                        //        VARS.curDashVector = -curRight;
                        //    }
                        //}

                        VARS.curDashHorSpeed = Vector3.Dot(VARS.curDashVector, curRight) * dashIniSpeed;
                        VARS.horCurSpeed = VARS.curDashHorSpeed;

                        //dashMaxSpeed
                        if (VARS.horCurSpeed > dashIniSpeed)
                        {
                            VARS.horCurSpeed = dashIniSpeed;
                        }
                        else if (VARS.horCurSpeed < -dashIniSpeed)
                        {
                            VARS.horCurSpeed = -dashIniSpeed;
                        }

                        VARS.verCurSpeed = 0;

                        dashStartTime = Time.time;

                        VARS.curAccumulatedDashDistance = 0;

                        VARS.curTargetEnergy += -dashEnergyCost;

                        VARS.IsDashing = true;
                        //}
                    }
                }
                else
                {
                    //dashPreInput
                    if (VARS.IsDashKeyDown)
                    {
                        //dashPreInputStartTime = Time.time;

                        isDashPreInputed = true;
                    }

                    VARS.curAccumulatedDashDistance += Mathf.Abs(VARS.curDashHorSpeed) * Time.deltaTime;

                    if (Time.time - dashStartTime > dashTime ||
                        VARS.curAccumulatedDashDistance >= Mathf.Abs(VARS.curDashHorSpeed) * dashTime - 0.1f)
                    {
                        //horCurSpeed -= Vector3.Dot(VARS.curDashVector, curRight) * dashIniSpeed * 0.6f;
                        //verCurSpeed -= Vector3.Dot(VARS.curDashVector, curUp) * dashIniSpeed * 0.6f;

                        VARS.horCurSpeed = 0;
                        VARS.verCurSpeed = 0;

                        dashStartTime = 0;

                        VARS.IsDashing = false;
                    }
                }

                //if (!VARS.IsDashing && VARS.IsInputtingDashKey)
                //{
                //    VARS.curTargetEnergy += -dashEnergyCost;
                //}
            }
            #endregion

            #region Acce
            if (VARS.IsAcceEnabled)
            {
                ////acceControl
                //if (VARS.IsInputtingAcceKey)
                //{
                //    VARS.IsInAcce = true;
                //}
                //else
                //{
                //    VARS.IsInAcce = false;
                //}

                //curAcceBonus
                if (VARS.IsInAcce)
                {
                    curAcceBonus = acceBonus;
                    if (VARS.IsInputtingDownKey)
                    {
                        curGravityAcceBonus = acceBonus /** 2*/ * 1.5f;
                    }
                    curClimbingAcceBonus = acceBonus;

                    //UFL.AddCurTargetEnergy(-inAcceEnergyDecreaseSpeed * Time.deltaTime);
                    VARS.curTargetEnergy += -inAcceEnergyDecreaseSpeed * Time.deltaTime;
                }
                else
                {
                    curAcceBonus = 1;
                    curGravityAcceBonus = 1;
                    curClimbingAcceBonus = 1;
                }
            }
            else
            {
                curAcceBonus = 1;
                curGravityAcceBonus = 1;
                curClimbingAcceBonus = 1;
            }
            #endregion

            #region BackCenterAndBetweenCentersTransport
            if (VARS.IsBackCenterTriggered)
            {
                //betweenCentersTransport
                if (VARS.IsInCenter &&
                    VARS.curAccessedCenterSavePointPositions.Count > 1)
                {
                    tempVector = VARS.curAccessedCenterSavePointPositions[VARS.curBackToAccessedCenterSavePointPositionIndex];
                    VARS.curBackToAccessedCenterSavePointPositionIndex = (VARS.curBackToAccessedCenterSavePointPositionIndex + 1) % VARS.curAccessedCenterSavePointPositions.Count;

                    if (tempVector == VARS.curLatestCenterSavePointPosition)
                    {
                        tempVector = VARS.curAccessedCenterSavePointPositions[VARS.curBackToAccessedCenterSavePointPositionIndex];
                        VARS.curBackToAccessedCenterSavePointPositionIndex = (VARS.curBackToAccessedCenterSavePointPositionIndex + 1) % VARS.curAccessedCenterSavePointPositions.Count;
                    }

                    if (tempVector != VARS.curLatestCenterSavePointPosition)
                    {
                        VARS.lastActivatedSavePointTime = -1;
                    }

                    VARS.curLatestCenterSavePointPosition = tempVector;
                }

                //backCenter
                if (Vector3.Magnitude(VARS.curLatestCenterSavePointPosition) > 1)
                {
                    if (VARS.IsCarryingFragments)
                    {
                        VARS.IsToNotLoseCarriedFragments = true;
                    }

                    Debug.Log("backCenterDie");

                    catIniPositionPoint.transform.position = VARS.curLatestCenterSavePointPosition;
                    VARS.IsToDie = true;
                }

                //VARS.IsBackCenterTriggered = false;
            }
            #endregion

            #region IfIsStill
            if (Mathf.Abs(VARS.horCurSpeed) < 1e-6f &&
                Mathf.Abs(VARS.verCurSpeed) < 1e-6f)
            {
                VARS.IsCatStill = true;
            }
            else
            {
                VARS.IsCatStill = false;
            }
            #endregion

            #region IfIsInputting
            if (!VARS.IsInputtingLeftKey && !VARS.IsInputtingRightKey)
            {
                VARS.IsHorInputting = false;
            }
            if (!VARS.IsInputtingJumpKey)
            {
                //isVerInputting = false;
            }
            #endregion
        }
        #endregion

        //VARS.horCurSpeed = horCurSpeed;
        //VARS.verCurSpeed = verCurSpeed;

        //VARS.curEnergy = curEnergy;
    }
    void Jump()
    {
        if (VARS.curEnergy > jumpEnergyCost &&
            !VARS.IsHighJumping)
        {
            UFL.AddCatPosition(VARS.curUp * 0.01f);

            ////avoidTheCaseOfUnableToJumpOnPlatform
            //if (VARS.curDownTileData.isPlatform)
            //{
            //    Debug.Log("enter");                

            //    tempVector = catTransform.position - VARS.curDownTile.transform.position;
            //    tempFloat = Vector3.Dot(tempVector, VARS.curUp);

            //    Debug.Log(tempFloat);

            //    while (tempFloat < 1)
            //    {
            //        tempVector = catTransform.position - VARS.curDownTile.transform.position;
            //        tempFloat = Vector3.Dot(tempVector, VARS.curUp);

            //        Debug.Log("while " + tempFloat);

            //        UFL.AddCatPosition(VARS.curUp * 0.02f);
            //    }
            //}

            //Debug.Log("jump");

            //verCurSpeed = verCurIniSpeed;
            //UFL.SetVerCurSpeed(verCurIniSpeed);
            VARS.verCurSpeed = VARS.verCurIniSpeed;

            VARS.curHighJumpingMaxSpeed = VARS.verCurMaxSpeed;

            if (VARS.IsInLiquid)
            {
                VARS.curHighJumpingMaxSpeed += VARS.curLiquidTileData.mass;
            }

            VARS.IsHighJumping = true;

            VARS.IsCatEnergyResetExecutable = false;

            VARS.IsContracting = true;

            //curEnergy -= jumpEnergyCost;
            //UFL.AddCurTargetEnergy(-jumpEnergyCost);
            VARS.curTargetEnergy += -jumpEnergyCost;

            //VARS.IsJustJumped = true;
        }
    }
}
