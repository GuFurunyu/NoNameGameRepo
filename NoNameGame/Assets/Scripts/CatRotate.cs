using UnityEngine;

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
    Transform camTransform;

    Vector3 leftRotationVector;
    Vector3 rightRotationVector;

    int rotationMaxNum;

    float rotationNumRestoreThres;

    float rotationEndThres;

    float rotationSpeed;

    float rotationStep;

    float returnIniRotationTime;

    float rotationEnergyCost;
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

    bool isOnGround;
    bool isInLiquid;

    //float horCurSpeed;
    //float verCurSpeed;

    bool isCatStill;

    int rotationRestNum;

    //float curEnergy;
    #endregion

    void Start()
    {
        gameManager = GameObject.Find("GameManager");

        CONS = gameManager.GetComponent<Constants>();
        VARS = gameManager.GetComponent<Variables>();
        UFL = gameManager.GetComponent<UniversalFunctionsLibrary>();
        SEC = gameManager.GetComponent<ScriptsExecutionController>();

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
        //planeForward = VARS.planeForward;
        //iniUp = VARS.iniUp;
        //iniRight = VARS.iniRight;
        curRoomStableForward = VARS.curRoomStableForward;
        curRoomStableUp = VARS.curRoomStableUp;
        curRoomStableRight = VARS.curRoomStableRight;
        curUp = VARS.curUp;
        curRight = VARS.curRight;
        camIniEulerangles = VARS.camIniEulerangles;
        isOnGround = VARS.IsOnGround;
        isInLiquid = VARS.IsInLiquid;
        //horCurSpeed = VARS.horCurSpeed;
        //verCurSpeed = VARS.verCurSpeed;
        isCatStill = VARS.IsCatStill;
        rotationRestNum = VARS.rotationRestNum;
        //curEnergy = VARS.curEnergy;

        #region Rotate
        if (VARS.IsInNewRoom)
        {
            //ifNotByDeath
            if (VARS.outIniRotationStartTime != 0.1f)
                VARS.outIniRotationStartTime = 0;

            VARS.IsInNewRoomCatRotateResetOver = true;
        }

        if (!VARS.IsRotating &&
            !VARS.IsTwisting &&
            !VARS.IsInMiniMap &&
            VARS.IsInNewRoomAllResetOver)
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
                Time.time - VARS.outIniRotationStartTime >= returnIniRotationTime &&
                !isCatStill)
            {
                //startEulerangles = camTransform.eulerAngles;
                targetEulerangles = camIniEulerangles;

                targetDegree = Mathf.Abs(outIniRotationDegree);

                VARS.IsRotating = true;

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
            if (rotationRestNum > 0 &&
                !VARS.IsInCenter)
            {
                if (VARS.curEnergy > rotationEnergyCost)
                {
                    if (VARS.IsInputtingDownKey)
                    {
                        if (VARS.IsInputtingLeftKey)
                        {
                            //startEulerangles = camTransform.eulerAngles;
                            targetDegree = 90;
                            targetEulerangles = camTransform.eulerAngles + leftRotationVector * targetDegree;

                            VARS.IsRotating = true;

                            isLeftRotated = true;

                            isIniRotated = false;

                            VARS.IsIniRotation = false;

                            rotationRestNum--;

                            //curEnergy -= rotationEnergyCost;
                            UFL.AddCurEnergy(-rotationEnergyCost);
                        }
                        else if (VARS.IsInputtingRightKey)
                        {
                            //startEulerangles = camTransform.eulerAngles;
                            targetDegree = 90;
                            targetEulerangles = camTransform.eulerAngles + rightRotationVector * targetDegree;

                            VARS.IsRotating = true;

                            isLeftRotated = false;

                            isIniRotated = false;

                            VARS.IsIniRotation = false;

                            rotationRestNum--;

                            //curEnergy -= rotationEnergyCost;
                            UFL.AddCurEnergy(-rotationEnergyCost);
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
                        UFL.SetHorCurSpeed(-VARS.verCurSpeed);
                        //verCurSpeed = tempFloat;
                        UFL.SetVerCurSpeed(tempFloat);

                        //catTransform.Rotate(0, 0, -90);
                    }
                    else
                    {
                        tempVector = curRight;
                        curRight = curUp;
                        curUp = -tempVector;

                        tempFloat = VARS.horCurSpeed;
                        //horCurSpeed = verCurSpeed;
                        UFL.SetHorCurSpeed(VARS.verCurSpeed);
                        //verCurSpeed = -tempFloat;
                        UFL.SetVerCurSpeed(-tempFloat);

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
