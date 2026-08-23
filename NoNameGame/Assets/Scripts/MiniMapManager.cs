using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder((int)ScriptsExecutionOrder.ExecutionOrder.minimapManager)]
public class MinimapManager : MonoBehaviour
{
    Constants CONS;
    Variables VARS;
    UniversalFunctionsLibrary UFL;
    ScriptsExecutionController SEC;

    GameObject gameManager;

    int curMinimapRotatingDirIndex;

    Vector3 curMinimapRotationCameraMovingVector;

    Vector3 curMinimapRotationAxis;

    Vector3 camMinimapRotationStartEulerAngles;
    Quaternion curMinimapRotationTargetQuaternion;
    Vector3 camMinimapRotationTargetEulerAngles;

    float accumulatedMinimapRotationDegree;

    float tempH;
    float tempS;
    float tempV;

    #region ConstantsUsed
    Transform camTransform;

    float minimapRotationMovingSpeed;

    List<GameObject> minimapRedFragments = new List<GameObject>();
    List<GameObject> minimapYellowFragments = new List<GameObject>();
    List<GameObject> minimapBlueFragments = new List<GameObject>();
    List<GameObject> minimapOrangeFragments = new List<GameObject>();
    List<GameObject> minimapGreenFragments = new List<GameObject>();
    List<GameObject> minimapPurpleFragments = new List<GameObject>();

    GameObject starDustsEmpty;
    #endregion

    #region VariablesUsed
    List<GameObject> curSpawnedBlocks = new List<GameObject>();

    bool[] isRedFragmentsEmbeded = new bool[8];
    bool[] isYellowFragmentsEmbeded = new bool[8];
    bool[] isBlueFragmentsEmbeded = new bool[8];
    bool[] isOrangeFragmentsEmbeded = new bool[8];
    bool[] isGreenFragmentsEmbeded = new bool[8];
    bool[] isPurpleFragmentsEmbeded = new bool[8];
    #endregion

    void Start()
    {
        gameManager = GameObject.Find("GameManager");

        CONS = gameManager.GetComponent<Constants>();
        VARS = gameManager.GetComponent<Variables>();
        UFL = gameManager.GetComponent<UniversalFunctionsLibrary>();
        SEC = gameManager.GetComponent<ScriptsExecutionController>();

        #region ImportConstants
        camTransform = CONS.camTransform;
        minimapRotationMovingSpeed = CONS.minimapRotationMovingSpeed;
        minimapRedFragments = CONS.minimapRedFragments;
        minimapYellowFragments = CONS.minimapYellowFragments;
        minimapBlueFragments = CONS.minimapBlueFragments;
        minimapOrangeFragments = CONS.minimapOrangeFragments;
        minimapGreenFragments = CONS.minimapGreenFragments;
        minimapPurpleFragments = CONS.minimapPurpleFragments;
        starDustsEmpty = CONS.starDustsEmpty;
        #endregion

        #region ImportReferenceVariables
        curSpawnedBlocks = VARS.curSpawnedBlocks;
        isRedFragmentsEmbeded = VARS.isRedFragmentsEmbeded;
        isYellowFragmentsEmbeded = VARS.isYellowFragmentsEmbeded;
        isBlueFragmentsEmbeded = VARS.isBlueFragmentsEmbeded;
        isOrangeFragmentsEmbeded = VARS.isOrangeFragmentsEmbeded;
        isGreenFragmentsEmbeded = VARS.isGreenFragmentsEmbeded;
        isPurpleFragmentsEmbeded = VARS.isPurpleFragmentsEmbeded;
        #endregion
    }

    void Update()
    {
        #region ImportValueVariables
        #endregion

        if (VARS.IsMinimapMainPartExecutable)
        {
            //intoMinimap
            if (!VARS.IsInMinimap)
            {
                //VARS.IsJustOutOfMinimap = false;

                //if (!VARS.IsZoomedOut &&
                //    //VARS.IsIniRotation &&
                //    !VARS.IsOptionPanelActivated)
                //{
                //    if (!VARS.IsInCenter)
                //    {
                //if (VARS.IsInputtingUpKey)
                //{
                //    if (VARS.IsJumpKeyDown)
                //    {
                if (VARS.IsMinimapKeyDown)
                //if (VARS.IsIntoMinimapTriggered)
                {
                    UFL.IntoMinimap();

                    VARS.IsMinimapRotationCameraPointIndexNotInitialized = true;

                    VARS.IsInMinimap = true;

                    VARS.IsIntoMinimapTriggered = false;
                }
                //    }
                //}
                //    }
                //}
            }

            //inMinimap
            else
            {
                if (!VARS.IsMinimapRotating)
                {
                    //minimapRotationControl
                    if (VARS.IsUpKeyDown)
                    {
                        VARS.IsMinimapRotating = true;
                        curMinimapRotatingDirIndex = 1;
                        UFL.GetCurToMinimapRotationCameraPoint(1);
                    }
                    else if (VARS.IsDownKeyDown)
                    {
                        VARS.IsMinimapRotating = true;
                        curMinimapRotatingDirIndex = 2;
                        UFL.GetCurToMinimapRotationCameraPoint(2);
                    }
                    else if (VARS.IsLeftKeyDown)
                    {
                        VARS.IsMinimapRotating = true;
                        curMinimapRotatingDirIndex = 3;
                        UFL.GetCurToMinimapRotationCameraPoint(3);
                    }
                    else if (VARS.IsRightKeyDown)
                    {
                        VARS.IsMinimapRotating = true;
                        curMinimapRotatingDirIndex = 4;
                        UFL.GetCurToMinimapRotationCameraPoint(4);
                    }
                    if (VARS.IsMinimapRotating)
                    {
                        curMinimapRotationCameraMovingVector =
                            VARS.curToMinimapRotationCameraPoint.transform.position - VARS.curMinimapRotationCameraPoint.transform.position;
                    }

                    //outOfMinimap
                    if (VARS.IsMinimapKeyDown ||
                        VARS.IsBackKeyDown)
                    //if (VARS.IsJumpKeyDown)
                    {
                        UFL.OutOfMinimap();

                        VARS.IsInMinimap = false;
                    }
                }
                //minimapRotationProcess
                else
                {
                    //Debug.Log("distance: " + Vector3.Distance(camTransform.position, VARS.curToMinimapRotationCameraPoint.transform.position));

                    if (/*accumulatedMinimapRotationDegree < 90*/
                        Vector3.Distance(camTransform.position, VARS.curToMinimapRotationCameraPoint.transform.position) > 3 &&
                        Vector3.Dot(VARS.curToMinimapRotationCameraPoint.transform.position - camTransform.position, curMinimapRotationCameraMovingVector) > 0)
                    {
                        UFL.MinimapCameraRotate(curMinimapRotatingDirIndex, minimapRotationMovingSpeed * Time.deltaTime);

                        //accumulatedMinimapRotationDegree += minimapRotationMovingSpeed * Time.deltaTime;
                        //accumulatedMinimapRotationDegree = Vector3.Angle(camTransform.eulerAngles, camMinimapRotationStartEulerAngles);
                    }
                    else
                    {
                        UFL.SetCameraPosition(VARS.curToMinimapRotationCameraPoint.transform.position);

                        camTransform.LookAt(Vector3.zero, camTransform.up);

                        VARS.curMinimapRotationCameraPointIndex = VARS.curToMinimapRotationCameraPointIndex;
                        //VARS.curMinimapRotationCameraPoint = VARS.curToMinimapRotationCameraPoint;

                        //UFL.SetCameraEulerangles(camMinimapRotationTargetEulerAngles);

                        VARS.IsMinimapRotating = false;
                    }
                }
            }
        }

        //inMinimapFragmentChangingColorEffect
        if (VARS.IsInMinimap)
        {
            for (int i = 0; i < 8; i++)
            {
                if (!isRedFragmentsEmbeded[i])
                {
                    Color.RGBToHSV(minimapRedFragments[i].GetComponent<MeshRenderer>().material.color, out tempH, out tempS, out tempV);
                    tempS += 1 * (1.2f - tempS) * (1 + VARS.curOneColorFragmentCollectedNumbers[5] * 0.25f) * Time.deltaTime;
                    if (tempS >= 1) tempS = 0.01f;
                    minimapRedFragments[i].GetComponent<MeshRenderer>().material.color = Color.HSVToRGB(tempH, tempS, tempV);
                }
                if (!isBlueFragmentsEmbeded[i])
                {
                    Color.RGBToHSV(minimapBlueFragments[i].GetComponent<MeshRenderer>().material.color, out tempH, out tempS, out tempV);
                    tempS += 1 * (1.2f - tempS) * (1 + VARS.curOneColorFragmentCollectedNumbers[3] * 0.25f) * Time.deltaTime;
                    if (tempS >= 1) tempS = 0.01f;
                    minimapBlueFragments[i].GetComponent<MeshRenderer>().material.color = Color.HSVToRGB(tempH, tempS, tempV);
                }
                if (!isYellowFragmentsEmbeded[i])
                {
                    Color.RGBToHSV(minimapYellowFragments[i].GetComponent<MeshRenderer>().material.color, out tempH, out tempS, out tempV);
                    tempS += 1 * (1.2f - tempS) * (1 + VARS.curOneColorFragmentCollectedNumbers[0] * 0.25f) * Time.deltaTime;
                    if (tempS >= 1) tempS = 0.01f;
                    minimapYellowFragments[i].GetComponent<MeshRenderer>().material.color = Color.HSVToRGB(tempH, tempS, tempV);
                }
                if (!isOrangeFragmentsEmbeded[i])
                {
                    Color.RGBToHSV(minimapOrangeFragments[i].GetComponent<MeshRenderer>().material.color, out tempH, out tempS, out tempV);
                    tempS += 1 * (1.2f - tempS) * (1 + VARS.curOneColorFragmentCollectedNumbers[2] * 0.25f) * Time.deltaTime;
                    if (tempS >= 1) tempS = 0.01f;
                    minimapOrangeFragments[i].GetComponent<MeshRenderer>().material.color = Color.HSVToRGB(tempH, tempS, tempV);
                }
                if (!isGreenFragmentsEmbeded[i])
                {
                    Color.RGBToHSV(minimapGreenFragments[i].GetComponent<MeshRenderer>().material.color, out tempH, out tempS, out tempV);
                    tempS += 1 * (1.2f - tempS) * (1 + VARS.curOneColorFragmentCollectedNumbers[4] * 0.25f) * Time.deltaTime;
                    if (tempS >= 1) tempS = 0.01f;
                    minimapGreenFragments[i].GetComponent<MeshRenderer>().material.color = Color.HSVToRGB(tempH, tempS, tempV);
                }
                if (!isPurpleFragmentsEmbeded[i])
                {
                    Color.RGBToHSV(minimapPurpleFragments[i].GetComponent<MeshRenderer>().material.color, out tempH, out tempS, out tempV);
                    tempS += 1 * (1.2f - tempS) * (1 + VARS.curOneColorFragmentCollectedNumbers[1] * 0.25f) * Time.deltaTime;
                    if (tempS >= 1) tempS = 0.01f;
                    minimapPurpleFragments[i].GetComponent<MeshRenderer>().material.color = Color.HSVToRGB(tempH, tempS, tempV);
                }
            }
        }

        //starDusts
        if (VARS.IsInMainBoard ||
            VARS.IsOptionPanelActivated ||
            VARS.IsInKeysGuide)
        {
            starDustsEmpty.SetActive(false);
        }
        else
        {
            starDustsEmpty.SetActive(true);
        }
    }
}
