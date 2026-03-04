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

    bool isOnGround;
    bool isToCeiling;
    bool isLeftBlocked;
    bool isRightBlocked;
    bool isInLiquid;
    bool isInGas;
    bool isInMist;

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

    void Start()
    {
        gameManager = GameObject.Find("GameManager");

        CONS = gameManager.GetComponent<Constants>();
        VARS = gameManager.GetComponent<Variables>();
        UFL = gameManager.GetComponent<UniversalFunctionsLibrary>();
        SEC = gameManager.GetComponent<ScriptsExecutionController>();

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
    }

    void Update()
    {
        curRight = VARS.curRight;
        curUp = VARS.curUp;
        isOnGround = VARS.isOnGround;
        isToCeiling = VARS.isToCeiling;
        isLeftBlocked = VARS.isLeftBlocked;
        isRightBlocked = VARS.isRightBlocked;
        isInLiquid = VARS.isInLiquid;
        isInGas = VARS.isInGas;
        isInMist = VARS.isInMist;
        curDownTileData = VARS.curDownTileData;
        curUpTileData = VARS.curUpTileData;
        curLeftTileData = VARS.curLeftTileData;
        curRightTileData = VARS.curRightTileData;
        curLiquidTileData = VARS.curLiquidTileData;
        curGasTileData = VARS.curGasTileData;
        curMistTileData = VARS.curMistTileData;
        buoyancyDistanceFixFloat = VARS.buoyancyDistanceFixFloat;
        //horCurSpeed = VARS.horCurSpeed;
        //verCurSpeed = VARS.verCurSpeed;
        //curEnergy = VARS.curEnergy;
        curRoomGravity = VARS.curRoomGravity;

        #region Move
        if (!VARS.isRotating && 
            !VARS.isTwisting)
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
            if (VARS.isInputtingLeftKey)
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

                VARS.isHorInputting = true;
            }
            //moveRight
            else if (VARS.isInputtingRightKey)
            {
                //lastHorDirectionInput = rightKeyCode;
                VARS.curFacingDirectionIndex = 2;

                if (VARS.horCurSpeed <= horCurMaxSpeed)
                {
                    //drag
                    //horCurSpeed += horCurAcce * Time.deltaTime;
                    UFL.AddHorCurSpeed(horCurAcce * Time.deltaTime);
                }

                VARS.isHorInputting = true;
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
                    if (VARS.isInputtingLeftKey)
                    {
                        //lastHorDirectionInput = rightKeyCode;
                        VARS.curFacingDirectionIndex = 2;

                        if (VARS.curEnergy > 0)
                        {
                            //verCurSpeed = 0;
                            UFL.SetVerCurSpeed(0);

                            VARS.isAttachWall = true;
                        }
                        else
                        {
                            VARS.isAttachWall = false;
                        }
                    }
                    else
                    {
                        VARS.isAttachWall = false;
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
                    if (VARS.isInputtingRightKey)
                    {
                        //lastHorDirectionInput = rightKeyCode;
                        VARS.curFacingDirectionIndex = 2;

                        if (VARS.curEnergy > 0)
                        {
                            //verCurSpeed = 0;
                            UFL.SetVerCurSpeed(0);

                            VARS.isAttachWall = true;
                        }
                        else
                        {
                            VARS.isAttachWall = false;
                        }
                    }
                    else
                    {
                        VARS.isAttachWall = false;
                    }
                }
            }

            //awayFromWall
            if (!isLeftBlocked &&
                !isRightBlocked)
            {
                VARS.isAttachWall = false;
            }

            //attachWallEnergyDecrease
            if (VARS.isAttachWall)
            {
                //curEnergy -= attachWallEnergyDecreaseSpeed * Time.deltaTime;
                UFL.AddCurEnergy(-attachWallEnergyDecreaseSpeed * Time.deltaTime);
            }

            //stop
            if (!VARS.isHorInputting)
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
                if (VARS.isInputtingLeftKey)
                {
                    curClimbSpeed = climbSpeed * curLeftTileData.friction;
                }
            }
            if (isRightBlocked)
            {
                if (VARS.isInputtingRightKey)
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
                    if (!VARS.isAttachWall)
                    {
                        //verCurSpeed -= curGravityAcce * curRoomGravity * Time.deltaTime;
                        UFL.AddVerCurSpeed(-curGravityAcce * curRoomGravity * Time.deltaTime);
                    }
                }

                //verCurSpeed = 0;

                //jump
                if (VARS.isJumpKeyDown ||
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
                if (!VARS.isAttachWall)
                {
                    //verCurSpeed -= curGravityAcce * curRoomGravity * Time.deltaTime;
                    UFL.AddVerCurSpeed(-curGravityAcce * curRoomGravity * Time.deltaTime);
                }

                //highJump
                if (VARS.isInputtingJumpKey)
                {
                    if (VARS.isHighJumping)
                    {
                        if (VARS.verCurSpeed > 0 &&
                            VARS.verCurSpeed <= verCurMaxSpeed)
                        {
                            //verCurSpeed += verAcce * Time.deltaTime;
                            UFL.AddVerCurSpeed(verAcce * Time.deltaTime);
                        }
                        else
                        {
                            VARS.isHighJumping = false;
                        }
                    }
                }
                else
                {
                    VARS.isHighJumping = false;
                }

                //specialJumps1
                if (VARS.isJumpKeyDown)
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
                if (!VARS.isInputtingUpKey)
                {
                    if (VARS.isJumpKeyDown ||
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
                if (VARS.isAttachWall)
                {
                    if (VARS.isInputtingUpKey)
                    {
                        //verCurSpeed = curClimbSpeed;
                        UFL.SetVerCurSpeed(curClimbSpeed);

                        //curEnergy -= climbEnergyDecreaseSpeed * Time.deltaTime;
                        UFL.AddCurEnergy(-climbEnergyDecreaseSpeed * Time.deltaTime);

                        ////climbJump
                        //if (VARS.isJumpKeyDown)
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
                if (VARS.isInputtingUpKey)
                {
                    if (VARS.curEnergy > 0)
                    {
                        //verCurSpeed = 0;
                        UFL.SetVerCurSpeed(0);

                        //curEnergy -= toCeilingEnergyDecreaseSpeed * Time.deltaTime;
                        UFL.AddCurEnergy(-toCeilingEnergyDecreaseSpeed * Time.deltaTime);
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
                if (VARS.curEnergy > dashEnergyCost)
                {
                    if (VARS.isDashKeyDown)
                    {
                        //dir
                        if (VARS.isInputtingLeftKey)
                        {
                            dashVector = -curRight;
                        }
                        else if (VARS.isInputtingRightKey)
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
                        UFL.AddCurEnergy(-dashEnergyCost);
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
                VARS.isCatStill = true;
            }
            else
            {
                VARS.isCatStill = false;
            }
            #endregion

            #region IfIsInputting
            if (!VARS.isInputtingLeftKey && !VARS.isInputtingRightKey)
            {
                VARS.isHorInputting = false;
            }
            if (!VARS.isInputtingJumpKey)
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
            !VARS.isHighJumping)
        {
            //verCurSpeed = verCurIniSpeed;
            UFL.SetVerCurSpeed(verCurIniSpeed);

            VARS.isHighJumping = true;

            VARS.isContracting = true;

            //curEnergy -= jumpEnergyCost;
            UFL.AddCurEnergy(-jumpEnergyCost);
        }
    }
}
