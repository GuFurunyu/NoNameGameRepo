using UnityEngine;
using UnityEngine.Video;

[DefaultExecutionOrder((int)ScriptsExecutionOrder.ExecutionOrder.catRotate)]
public class CatRotate : MonoBehaviour
{
    Constants CONS;
    Variables VARS;
    UniversalFunctionsLibrary UFL;
    ScriptsExecutionController SEC;

    GameObject gameManager;

    //rotationNum
    public float rotationNumRestoreStartTime;

    //rotationProcess
    public Vector3 startEulerangles;
    public Vector3 targetEulerangles;
    public float accumulatedDegree;
    public float rotationStepAccumulatedDegree;
    public float targetDegree = 90;
    public bool isLeftRotated;

    //iniRotation
    public float outIniRotationDegree;
    public bool isIniRotated;

    float tempFloat;
    Vector3 tempVector;

    #region ConstantsUsed
    float gridBreadth;
    int roomCoordBreadth;

    Transform camTransform;

    float rotateInLiquidHorKeyUpThreshold;

    GameObject cat;
    Transform catTransform;

    Vector3 leftRotationVector;
    Vector3 rightRotationVector;

    int rotationMaxNum;

    float rotationNumRestoreThres;

    float rotationEndThres;

    float rotationSpeed;

    float rotationStep;

    float returnIniRotationTime;

    float rotationEnergyCost;

    Material fadedColor;
    Material normalColor;
    #endregion

    #region VariablesUsed
    //Vector3 planeForward;
    //Vector3 iniUp;
    //Vector3 iniRight;
    Vector3 curRoomStableForward;
    Vector3 curRoomStableUp;
    Vector3 curRoomStableRight;
    Vector3 curUp;
    Vector3 curRight;

    Vector3 camIniEulerangles;

    //float horCurSpeed;
    //float verCurSpeed;

    int rotationRestNum;

    //float curEnergy;
    #endregion

    #region BoolVariablesUsed
    bool isOnGround;
    bool isInLiquid;

    bool isCatStill;
    #endregion

    void Start()
    {
        gameManager = GameObject.Find("GameManager");

        CONS = gameManager.GetComponent<Constants>();
        VARS = gameManager.GetComponent<Variables>();
        UFL = gameManager.GetComponent<UniversalFunctionsLibrary>();
        SEC = gameManager.GetComponent<ScriptsExecutionController>();

        #region ImportConstants
        gridBreadth = CONS.gridBreadth;
        roomCoordBreadth = CONS.roomCoordBreadth;
        camTransform = CONS.camTransform;
        rotateInLiquidHorKeyUpThreshold = CONS.rotateInLiquidHorKeyUpThreshold;
        cat = CONS.cat;
        catTransform = CONS.catTransform;
        leftRotationVector = CONS.leftRotationVector;
        rightRotationVector = CONS.rightRotationVector;
        rotationMaxNum = CONS.rotationMaxNum;
        rotationNumRestoreThres = CONS.rotationNumRestoreThres;
        rotationEndThres = CONS.rotationEndThres;
        rotationSpeed = CONS.rotationSpeed;
        rotationStep = CONS.rotationStep;
        returnIniRotationTime = CONS.returnIniRotationTime;
        rotationEnergyCost = CONS.rotationEnergyCost;
        fadedColor = CONS.fadedColor;
        normalColor = CONS.normalColor;
        #endregion

        #region ImportReferenceVariables
        #endregion

        camTransform = CONS.camTransform;
        leftRotationVector = CONS.leftRotationVector;
        rightRotationVector = CONS.rightRotationVector;
        rotationMaxNum = CONS.rotationMaxNum;
        rotationNumRestoreThres = CONS.rotationNumRestoreThres;
        rotationEndThres = CONS.rotationEndThres;
        rotationSpeed = CONS.rotationSpeed;
        rotationStep = CONS.rotationStep;
        returnIniRotationTime = CONS.returnIniRotationTime;
        rotationEnergyCost = CONS.rotationEnergyCost;
    }

    void Update()
    {
        #region ImportValueVariables
        curRoomStableForward = VARS.curRoomStableForward;
        curRoomStableUp = VARS.curRoomStableUp;
        curRoomStableRight = VARS.curRoomStableRight;
        curUp = VARS.curUp;
        curRight = VARS.curRight;
        camIniEulerangles = VARS.camIniEulerangles;
        rotationRestNum = VARS.rotationRestNum;
        #endregion

        #region ImportBoolVariables
        isOnGround = VARS.IsOnGround;
        isInLiquid = VARS.IsInLiquid;
        isCatStill = VARS.IsCatStill;
        #endregion

        #region Rotate
        if (!VARS.IsInNewRoomCatRotateResetOver)
        {
            //setIsRotateEnabled
            VARS.IsRotateEnabled = VARS.IsRoomFragmentCollected[VARS.curRoomIndex];

            //ifNotByDeath
            if (VARS.outIniRotationStartTime != 0.1f)
                VARS.outIniRotationStartTime = 0;

            VARS.IsInNewRoomCatRotateResetOver = true;
        }

        if (VARS.IsCatRotateMainPartExecutable)
        {
            if (!VARS.IsRotating)
            {
                //ifIsIniRotation
                if (/*curRight == iniRight*/
                    /*camTransform.eulerAngles == camIniEulerangles*/
                    /*(camTransform.eulerAngles.x + 360) % 360 == (camIniEulerangles.x + 360) % 360 &&
                    (camTransform.eulerAngles.y + 360) % 360 == (camIniEulerangles.y + 360) % 360 &&
                    (camTransform.eulerAngles.z + 360) % 360 == (camIniEulerangles.z + 360) % 360*/
                    curRight == curRoomStableRight)
                {
                    VARS.IsIniRotation = true;
                }
                else
                {
                    //Debug.Log("enter");
                    //print(camTransform.eulerAngles);
                    //print(camIniEulerangles);

                    VARS.IsIniRotation = false;

                    outIniRotationDegree = Vector3.SignedAngle(curRoomStableRight, curRight, curRoomStableForward);

                    if (!VARS.IsRotating &&
                        !VARS.IsTwisting)
                    {
                        if (VARS.outIniRotationStartTime == 0)
                        {
                            VARS.outIniRotationStartTime = Time.time;
                        }
                    }
                }

                //returnIniRotation
                if (!VARS.IsIniRotation &&
                    Time.time - VARS.outIniRotationStartTime >= returnIniRotationTime /*&&*/
                    /*!isCatStill*/
                    /*Vector3.Distance(catTransform.position, VARS.roomCenters[VARS.curRoomIndex]) < (roomCoordBreadth / 2) * gridBreadth*/)
                {
                    //startEulerangles = camTransform.eulerAngles;
                    targetEulerangles = camIniEulerangles;

                    targetDegree = Mathf.Abs(outIniRotationDegree);

                    VARS.IsRotating = true;
                    VARS.IsOnGround = false;
                    VARS.IsCatEnergyResetExecutable = false;

                    isIniRotated = true;

                    if (outIniRotationDegree > 0)
                    {
                        isLeftRotated = true;
                    }
                    else if (outIniRotationDegree < 0)
                    {
                        isLeftRotated = false;
                    }
                }

                //rotationControl
                if (VARS.IsRotateEnabled &&
                    rotationRestNum > 0 &&
                    !VARS.IsInCenter &&
                    !VARS.isCenterFulfilled[VARS.curRoomIndex / 9] &&
                    !VARS.IsInLiquid /*&& !VARS.IsInGas && !VARS.IsInMist*/)
                {
                    if (VARS.curEnergy > rotationEnergyCost/*true*/)
                    {
                        if (VARS.IsInputtingDownKey/* &&
                            ((!VARS.IsInLiquid && (VARS.IsInputtingLeftKey || VARS.IsInputtingRightKey)) ||
                            (VARS.IsInLiquid && (
                            VARS.lastLeftKeyUpTime - VARS.lastLeftKeyDownTime < rotateInLiquidHorKeyUpThreshold || 
                            VARS.lastRightKeyUpTime - VARS.lastRightKeyDownTime < rotateInLiquidHorKeyUpThreshold)))*/)
                        {
                            ////ifInLiquidRequiringHorKeyToBeUpInThreshold
                            if (/*(!VARS.IsInLiquid && VARS.IsInputtingLeftKey) ||
                                (VARS.IsInLiquid && VARS.IsLeftKeyUp && Time.time - VARS.lastLeftKeyDownTime < rotateInLiquidHorKeyUpThreshold)*/
                                VARS.IsInputtingLeftKey)
                            {
                                //startEulerangles = camTransform.eulerAngles;
                                targetDegree = 90;
                                targetEulerangles = camTransform.eulerAngles + leftRotationVector * targetDegree;

                                VARS.IsRotating = true;
                                VARS.IsOnGround = false;
                                VARS.IsCatEnergyResetExecutable = false;

                                isLeftRotated = true;

                                isIniRotated = false;

                                VARS.IsIniRotation = false;

                                rotationRestNum--;

                                //curEnergy -= rotationEnergyCost;
                                //UFL.AddCurTargetEnergy(-rotationEnergyCost);
                                VARS.curTargetEnergy += -rotationEnergyCost;
                            }
                            ////ifInLiquidRequiringHorKeyToBeUpInThreshold
                            else if (/*(!VARS.IsInLiquid && VARS.IsInputtingRightKey) ||
                                (VARS.IsInLiquid && VARS.IsRightKeyUp && Time.time - VARS.lastRightKeyDownTime < rotateInLiquidHorKeyUpThreshold)*/
                                VARS.IsInputtingRightKey)
                            {
                                //startEulerangles = camTransform.eulerAngles;
                                targetDegree = 90;
                                targetEulerangles = camTransform.eulerAngles + rightRotationVector * targetDegree;

                                VARS.IsRotating = true;
                                VARS.IsOnGround = false;
                                VARS.IsCatEnergyResetExecutable = false;

                                isLeftRotated = false;

                                isIniRotated = false;

                                VARS.IsIniRotation = false;

                                rotationRestNum--;

                                //curEnergy -= rotationEnergyCost;
                                //UFL.AddCurTargetEnergy(-rotationEnergyCost);
                                VARS.curTargetEnergy += -rotationEnergyCost;
                            }
                        }
                    }
                }
            }
            //rotationProcess
            else if (VARS.IsRotating)
            {
                if (accumulatedDegree - targetDegree < rotationEndThres &&
                    targetDegree != 0)
                {
                    //camTransform.eulerAngles += (targetEulerangles - startEulerangles) * rotationSpeed * Time.deltaTime;

                    accumulatedDegree += targetDegree * rotationSpeed * Time.deltaTime;
                    //Debug.Log(accumulatedDegree);
                    //rotationStepAccumulatedDegree += targetDegree * rotationSpeed * Time.deltaTime;

                    //if (rotationStepAccumulatedDegree >= rotationStep)
                    //{
                    if (isLeftRotated)
                    {
                        //camTransform.Rotate(0, 0, -rotationStep);
                        //camTransform.Rotate(0, 0, -rotationSpeed * targetDegree * Time.deltaTime);
                        UFL.CameraRotate(-rotationSpeed * targetDegree * Time.deltaTime);
                        //camTransform.Rotate(leftRotationVector * rotationSpeed * targetDegree * Time.deltaTime);
                    }
                    else
                    {
                        //camTransform.Rotate(0, 0, rotationStep);
                        //camTransform.Rotate(0, 0, rotationSpeed * targetDegree * Time.deltaTime);
                        UFL.CameraRotate(rotationSpeed * targetDegree * Time.deltaTime);
                        //camTransform.Rotate(rightRotationVector * rotationSpeed * targetDegree * Time.deltaTime);
                    }

                    //rotationStepAccumulatedDegree -= rotationStep;
                    //}
                }
                else
                {
                    //camTransform.eulerAngles = targetEulerangles;
                    UFL.SetCameraEulerangles(targetEulerangles);

                    accumulatedDegree = 0;
                    //rotationStepAccumulatedDegree = 0;

                    VARS.IsRotating = false;

                    VARS.outIniRotationStartTime = 0;

                    if (!isIniRotated)
                    {
                        if (isLeftRotated)
                        {
                            tempVector = curRight;
                            curRight = -curUp;
                            curUp = tempVector;

                            tempFloat = VARS.horCurSpeed;
                            //horCurSpeed = -verCurSpeed;
                            //UFL.SetHorCurSpeed(-VARS.verCurSpeed);
                            VARS.horCurSpeed = -VARS.verCurSpeed;
                            //verCurSpeed = tempFloat;
                            //UFL.SetVerCurSpeed(tempFloat);
                            VARS.verCurSpeed = tempFloat;

                            //catTransform.Rotate(0, 0, -90);
                        }
                        else
                        {
                            tempVector = curRight;
                            curRight = curUp;
                            curUp = -tempVector;

                            tempFloat = VARS.horCurSpeed;
                            //horCurSpeed = verCurSpeed;
                            //UFL.SetHorCurSpeed(VARS.verCurSpeed);
                            VARS.horCurSpeed = VARS.verCurSpeed;
                            //verCurSpeed = -tempFloat;
                            //UFL.SetVerCurSpeed(-tempFloat);
                            VARS.verCurSpeed = -tempFloat;

                            //catTransform.Rotate(0, 0, 90);
                        }
                    }
                    else
                    {
                        curRight = curRoomStableRight;
                        curUp = curRoomStableUp;
                    }
                }
            }
        }

        //colorTransition
        if (rotationRestNum == 0)
        {
            //cat.GetComponent<MeshRenderer>().material = fadedColor;
            VARS.IsInFadedColor = true;
        }
        else
        {
            //cat.GetComponent<MeshRenderer>().material = normalColor;
            VARS.IsInFadedColor = false;
        }
        #endregion

        #region OnGroundOrInLiquidReset
        if (!VARS.IsRotating &&
            !VARS.IsTwisting)
        {
            if (isOnGround ||
                isInLiquid)
            {
                //rotationRestNumRestore
                if (VARS.IsIniRotation)
                {
                    if (rotationNumRestoreStartTime == 0)
                    {
                        rotationNumRestoreStartTime = Time.time;
                    }

                    if (rotationRestNum < rotationMaxNum)
                    {
                        if (Time.time - rotationNumRestoreStartTime > rotationNumRestoreThres)
                        {
                            rotationRestNum = rotationMaxNum;

                            rotationNumRestoreStartTime = 0;
                        }
                    }
                }
            }
        }
        #endregion

        //VARS.horCurSpeed = horCurSpeed;
        //VARS.verCurSpeed = verCurSpeed;

        //VARS.curEnergy = curEnergy;

        VARS.curUp = curUp;
        VARS.curRight = curRight;

        VARS.rotationRestNum = rotationRestNum;
    }
}
