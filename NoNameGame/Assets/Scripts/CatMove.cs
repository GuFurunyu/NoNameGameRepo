using UnityEngine;

[DefaultExecutionOrder((int)ScriptsExecutionOrder.ExecutionOrder.catMove)]
public class CatMove : MonoBehaviour
{
    Constants CONS;
    Variables VARS;
    UniversalFunctionsLibrary UFL;
    ScriptsExecutionController SEC;

    GameObject gameManager;

    //horSpeed
    public float horCurAcce;
    public float horCurReverseAcce;
    public float horCurMaxSpeed;
    public float horCurWallJumpBonusSpeed;

    //verSpeed
    public float verCurIniSpeed;
    public float verCurAcce;
    public float curGravityAcce;
    public float curClimbSpeed;
    public float verCurMaxSpeed;

    //jumpPreInput
    public bool isJumpPreInputed;
    public float jumpPreInputStartTime;

    //jumpPostInput
    public float jumpPostInputStartTime;

    //wallJump
    public float wallJumpPreInputStartTime;
    public float wallJumpPostInputStartTime;
    public bool isPostWallJumpToRight;

    //dash
    public Vector3 dashVector;
    public float dashStartTime;

    #region ConstantsUsed
    Transform catTransform;

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

    float attachWallEnergyDecreaseSpeed;
    float climbEnergyDecreaseSpeed;
    float toCeilingEnergyDecreaseSpeed;
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

    //float horCurSpeed;
    //float verCurSpeed;

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
        catTransform = CONS.catTransform;
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
        jumpPreInputThres = CONS.jumpPreInputThres;
        jumpPostInputTres = CONS.jumpPostInputTres;
        wallJumpPreInputThres = CONS.wallJumpPreInputThres;
        wallJumpPostInputThres = CONS.wallJumpPostInputThres;
        dashIniSpeed = CONS.dashIniSpeed;
        dashTime = CONS.dashTime;
        attachWallEnergyDecreaseSpeed = CONS.attachWallEnergyDecreaseSpeed;
        climbEnergyDecreaseSpeed = CONS.climbEnergyDecreaseSpeed;
        toCeilingEnergyDecreaseSpeed = CONS.toCeilingEnergyDecreaseSpeed;
        jumpEnergyCost = CONS.jumpEnergyCost;
        dashEnergyCost = CONS.dashEnergyCost;
        #endregion

        #region ImportReferenceVariables
        #endregion

        VARS.horCurSpeed = 0;
        VARS.verCurSpeed = 0;
    }

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

        #region Move
        if (!VARS.IsRotating && 
            !VARS.IsTwisting &&
            !VARS.IsInMiniMap &&
            VARS.IsInNewRoomAllResetOver)
        {
            #region LeftAndRight
            if (!isInLiquid && !isInGas && !isInMist)
            {
                if (isOnGround)
                {
                    horCurAcce = horAcce * curDownTileData.friction;
                    horCurReverseAcce = horReverseAcce * curDownTileData.friction;
                    horCurMaxSpeed = horMaxSpeed - curDownTileData.tackiness;
                }
                //HR: ~fluid
                else
                {
                    horCurAcce = horAcce;
                    horCurReverseAcce = horReverseAcce;
                    horCurMaxSpeed = horMaxSpeed;
                }
            }
            else
            {
                if (isInLiquid)
                {
                    horCurAcce = horAcce / curLiquidTileData.fluidDrag;
                    horCurReverseAcce = horReverseAcce / curLiquidTileData.fluidDrag;
                    horCurMaxSpeed = horMaxSpeed / curLiquidTileData.fluidDrag;
                }
                else if (isInGas)
                {
                    horCurAcce = horAcce / curGasTileData.fluidDrag;
                    horCurReverseAcce = horReverseAcce / curGasTileData.fluidDrag;
                    horCurMaxSpeed = horMaxSpeed / curGasTileData.fluidDrag;
                }
                else if (isInMist)
                {
                    horCurAcce = horAcce / curMistTileData.fluidDrag;
                    horCurReverseAcce = horReverseAcce / curMistTileData.fluidDrag;
                    horCurMaxSpeed = horMaxSpeed / curMistTileData.fluidDrag;
                }
            }

            if (isToCeiling)
            {
                horCurMaxSpeed = horToCeilingMaxSpeed;
            }

            //moveLeft
            if (VARS.IsInputtingLeftKey)
            {
                //forDash
                //lastHorDirectionInput = leftKeyCode;
                VARS.curFacingDirectionIndex = 1;

                if (VARS.horCurSpeed >= -horCurMaxSpeed)
                {
                    //drag
                    //horCurSpeed -= horCurAcce * Time.deltaTime;
                    UFL.AddHorCurSpeed(-horCurAcce * Time.deltaTime);
                }

                VARS.IsHorInputting = true;
            }
            //moveRight
            else if (VARS.IsInputtingRightKey)
            {
                //lastHorDirectionInput = rightKeyCode;
                VARS.curFacingDirectionIndex = 2;

                if (VARS.horCurSpeed <= horCurMaxSpeed)
                {
                    //drag
                    //horCurSpeed += horCurAcce * Time.deltaTime;
                    UFL.AddHorCurSpeed(horCurAcce * Time.deltaTime);
                }

                VARS.IsHorInputting = true;
            }

            //isLeftBlocked
            if (isLeftBlocked)
            {
                //if (horCurSpeed < 0)
                //{
                //    horCurSpeed = 0;
                //}

                //attachWall
                if (!isOnGround)
                {
                    if (VARS.IsInputtingLeftKey &&
                        VARS.IsInputtingGrabKey)
                    {
                        //lastHorDirectionInput = rightKeyCode;
                        VARS.curFacingDirectionIndex = 2;

                        if (VARS.curEnergy > 0)
                        {
                            //verCurSpeed = 0;
                            UFL.SetVerCurSpeed(0);

                            VARS.IsAttachWall = true;
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
            }
            //ifRightBlocked
            else if (isRightBlocked)
            {
                //if (horCurSpeed > 0)
                //{
                //    horCurSpeed = 0;
                //}

                //attachWall
                if (!isOnGround)
                {
                    if (VARS.IsInputtingRightKey &&
                        VARS.IsInputtingGrabKey)
                    {
                        //lastHorDirectionInput = rightKeyCode;
                        VARS.curFacingDirectionIndex = 2;

                        if (VARS.curEnergy > 0)
                        {
                            //verCurSpeed = 0;
                            UFL.SetVerCurSpeed(0);

                            VARS.IsAttachWall = true;
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
            }

            //awayFromWall
            if (!isLeftBlocked &&
                !isRightBlocked)
            {
                VARS.IsAttachWall = false;
            }

            //attachWallEnergyDecrease
            if (VARS.IsAttachWall)
            {
                //curEnergy -= attachWallEnergyDecreaseSpeed * Time.deltaTime;
                UFL.AddCurTargetEnergy(-attachWallEnergyDecreaseSpeed * Time.deltaTime);
            }

            //stop
            if (!VARS.IsHorInputting)
            {
                if (VARS.horCurSpeed < -horStopThres)
                {
                    if (isOnGround)
                    {
                        //horCurSpeed += horCurReverseAcce * Time.deltaTime;
                        UFL.AddHorCurSpeed(horCurReverseAcce * Time.deltaTime);
                    }
                    else
                    {
                        //horCurSpeed += horCurReverseAcce / 2 * Time.deltaTime;
                        UFL.AddHorCurSpeed(horCurReverseAcce / 2 * Time.deltaTime);
                    }
                }
                else if (VARS.horCurSpeed > horStopThres)
                {
                    if (isOnGround)
                    {
                        //horCurSpeed -= horCurReverseAcce * Time.deltaTime;
                        UFL.AddHorCurSpeed(-horCurReverseAcce * Time.deltaTime);
                    }
                    else
                    {
                        //horCurSpeed -= horCurReverseAcce / 2 * Time.deltaTime;
                        UFL.AddHorCurSpeed(-horCurReverseAcce / 2 * Time.deltaTime);
                    }
                }
                else
                {
                    //horCurSpeed = 0;
                    UFL.SetHorCurSpeed(0);
                }
            }

            //horSpeedSum
            if (VARS.horCurSpeed != 0)
            {
                catTransform.position += curRight * VARS.horCurSpeed * Time.deltaTime;
            }
            #endregion

            #region UpAndDown
            if (!isInLiquid && !isInGas && !isInMist)
            {
                if (isOnGround)
                {
                    verCurIniSpeed = verIniSpeed;
                    curGravityAcce = gravityAcce;
                    verCurMaxSpeed = verMaxSpeed - curDownTileData.tackiness;
                }
                //HR: ~fluid
                else
                {
                    verCurIniSpeed = verIniSpeed;
                    curGravityAcce = gravityAcce;
                    verCurMaxSpeed = verMaxSpeed;
                }
            }
            else
            {
                if (isInLiquid)
                {
                    verCurIniSpeed = verIniSpeed / curLiquidTileData.fluidDrag;
                    curGravityAcce = gravityAcce - gravityAcce * curLiquidTileData.mass * (1 - buoyancyDistanceFixFloat) * 3;
                    verCurMaxSpeed = verMaxSpeed / curLiquidTileData.fluidDrag;
                }
                else if (isInGas)
                {
                    verCurIniSpeed = verIniSpeed / curGasTileData.fluidDrag;
                    curGravityAcce = gravityAcce - gravityAcce * curGasTileData.mass / 2;
                    verCurMaxSpeed = verMaxSpeed / curGasTileData.fluidDrag;
                }
                else if (isInMist)
                {
                    verCurIniSpeed = verIniSpeed / curMistTileData.fluidDrag;
                    curGravityAcce = gravityAcce - gravityAcce * curMistTileData.mass / 2;
                    verCurMaxSpeed = verMaxSpeed / curMistTileData.fluidDrag;
                }
            }

            if (isLeftBlocked)
            {
                if (VARS.IsInputtingLeftKey)
                {
                    curClimbSpeed = climbSpeed * curLeftTileData.friction;
                }
            }
            if (isRightBlocked)
            {
                if (VARS.IsInputtingRightKey)
                {
                    curClimbSpeed = climbSpeed * curRightTileData.friction;
                }
            }

            //just"||isInLiquid"?
            if (isOnGround ||
                isInLiquid)
            {
                if (isInLiquid)
                {
                    //gravity
                    if (!VARS.IsAttachWall)
                    {
                        //verCurSpeed -= curGravityAcce * curRoomGravity * Time.deltaTime;
                        UFL.AddVerCurSpeed(-curGravityAcce * curRoomGravity * Time.deltaTime);
                    }
                }

                //verCurSpeed = 0;

                //jump
                if (VARS.IsJumpKeyDown ||
                    (isJumpPreInputed &&
                    Time.time - jumpPreInputStartTime <= jumpPreInputThres))
                {
                    Jump();
                }

                isJumpPreInputed = false;

                jumpPostInputStartTime = Time.time;
            }
            else
            {
                //gravity
                if (!VARS.IsAttachWall)
                {
                    //verCurSpeed -= curGravityAcce * curRoomGravity * Time.deltaTime;
                    UFL.AddVerCurSpeed(-curGravityAcce * curRoomGravity * Time.deltaTime);
                }

                //highJump
                if (VARS.IsInputtingJumpKey)
                {
                    if (VARS.IsHighJumping)
                    {
                        if (VARS.verCurSpeed > 0 &&
                            VARS.verCurSpeed <= verCurMaxSpeed)
                        {
                            //verCurSpeed += verAcce * Time.deltaTime;
                            UFL.AddVerCurSpeed(verAcce * Time.deltaTime);
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

                    wallJumpPreInputStartTime = Time.time;

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
                            //horCurSpeed = horMaxSpeed + horWallJumpBonusSpeed - curLeftTileData.tackiness;
                            UFL.SetHorCurSpeed(horMaxSpeed + horWallJumpBonusSpeed - curLeftTileData.tackiness);

                            Jump();
                        }
                        else
                        {
                            //horCurSpeed = -horMaxSpeed - horWallJumpBonusSpeed + curRightTileData.tackiness;
                            UFL.SetHorCurSpeed(-horMaxSpeed - horWallJumpBonusSpeed + curRightTileData.tackiness);

                            Jump();
                        }
                    }
                }

                //specialJumps2
                if (!VARS.IsInputtingUpKey)
                {
                    if (VARS.IsJumpKeyDown ||
                        Time.time - wallJumpPreInputStartTime <= wallJumpPreInputThres)
                    {
                        //wallJump
                        if (isLeftBlocked)
                        {
                            //horCurSpeed = horMaxSpeed + horWallJumpBonusSpeed - curLeftTileData.tackiness;
                            UFL.SetHorCurSpeed(horMaxSpeed + horWallJumpBonusSpeed - curLeftTileData.tackiness);

                            Jump();
                        }
                        else if (isRightBlocked)
                        {
                            //horCurSpeed = -horMaxSpeed - horWallJumpBonusSpeed + curRightTileData.tackiness;
                            UFL.SetHorCurSpeed(-horMaxSpeed - horWallJumpBonusSpeed + curRightTileData.tackiness);

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
                if (VARS.IsAttachWall)
                {
                    if (VARS.IsInputtingUpKey)
                    {
                        //verCurSpeed = curClimbSpeed;
                        UFL.SetVerCurSpeed(curClimbSpeed);

                        //curEnergy -= climbEnergyDecreaseSpeed * Time.deltaTime;
                        UFL.AddCurTargetEnergy(-climbEnergyDecreaseSpeed * Time.deltaTime);

                        ////climbJump
                        //if (VARS.IsJumpKeyDown)
                        //{
                        //    Jump();

                        //    isAttachWall = false;
                        //}
                    }
                }
            }

            //ifToCeiling
            if (isToCeiling)
            {
                if (VARS.verCurSpeed > 0)
                {
                    //verCurSpeed = 0;
                    UFL.SetVerCurSpeed(0);
                }

                //toCeiling
                if (VARS.IsInputtingUpKey)
                {
                    if (VARS.curEnergy > 0)
                    {
                        //verCurSpeed = 0;
                        UFL.SetVerCurSpeed(0);

                        //curEnergy -= toCeilingEnergyDecreaseSpeed * Time.deltaTime;
                        UFL.AddCurTargetEnergy(-toCeilingEnergyDecreaseSpeed * Time.deltaTime);
                    }
                }
            }

            if (VARS.verCurSpeed != 0)
            {
                //verSpeedSum
                catTransform.position += curUp * VARS.verCurSpeed * Time.deltaTime;

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
            if (dashStartTime == 0)
            {
                if (VARS.IsDashEnabled)
                {
                    if (VARS.curEnergy > dashEnergyCost)
                    {
                        if (VARS.IsDashKeyDown)
                        {
                            //dir
                            if (VARS.IsInputtingLeftKey)
                            {
                                dashVector = -curRight;
                            }
                            else if (VARS.IsInputtingRightKey)
                            {
                                dashVector = curRight;
                            }
                            else
                            {
                                if (/*lastHorDirectionInput == leftKeyCode*/
                                    VARS.curFacingDirectionIndex == 1)
                                {
                                    dashVector = -curRight;
                                }
                                else if (/*lastHorDirectionInput == rightKeyCode*/
                                    VARS.curFacingDirectionIndex == 2)
                                {
                                    dashVector = curRight;
                                }
                            }

                            //blockedReverse
                            if (dashVector == -curRight)
                            {
                                if (isLeftBlocked)
                                {
                                    dashVector = curRight;
                                }
                            }
                            else if (dashVector == curRight)
                            {
                                if (isRightBlocked)
                                {
                                    dashVector = -curRight;
                                }
                            }

                            //horCurSpeed += Vector3.Dot(dashVector, curRight) * dashIniSpeed;
                            UFL.AddHorCurSpeed(Vector3.Dot(dashVector, curRight) * dashIniSpeed);

                            //dashMaxSpeed
                            if (VARS.horCurSpeed > dashIniSpeed)
                            {
                                //horCurSpeed = dashIniSpeed;
                                UFL.SetHorCurSpeed(dashIniSpeed);
                            }
                            else if (VARS.horCurSpeed < -dashIniSpeed)
                            {
                                //horCurSpeed = -dashIniSpeed;
                                UFL.SetHorCurSpeed(-dashIniSpeed);
                            }

                            //verCurSpeed = 0;
                            UFL.SetVerCurSpeed(0);

                            dashStartTime = Time.time;

                            //curEnergy -= dashEnergyCost;
                            UFL.AddCurTargetEnergy(-dashEnergyCost);
                        }
                    }
                }
            }

            else
            {
                if (Time.time - dashStartTime > dashTime)
                {
                    //horCurSpeed -= Vector3.Dot(dashVector, curRight) * dashIniSpeed * 0.6f;
                    //verCurSpeed -= Vector3.Dot(dashVector, curUp) * dashIniSpeed * 0.6f;

                    //horCurSpeed = 0;
                    UFL.SetHorCurSpeed(0);
                    //verCurSpeed = 0;
                    UFL.SetVerCurSpeed(0);

                    dashStartTime = 0;
                }
            }
            #endregion

            #region IfIsStill
            if (VARS.horCurSpeed == 0 &&
                VARS.verCurSpeed == 0)
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
            //verCurSpeed = verCurIniSpeed;
            UFL.SetVerCurSpeed(verCurIniSpeed);

            VARS.IsHighJumping = true;

            VARS.IsContracting = true;

            //curEnergy -= jumpEnergyCost;
            UFL.AddCurTargetEnergy(-jumpEnergyCost);
        }
    }
}
