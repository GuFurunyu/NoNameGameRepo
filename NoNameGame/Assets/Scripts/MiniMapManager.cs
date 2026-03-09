using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder((int)ScriptsExecutionOrder.ExecutionOrder.miniMapManager)]
public class MiniMapManager : MonoBehaviour
{
    Constants CONS;
    Variables VARS;
    UniversalFunctionsLibrary UFL;
    ScriptsExecutionController SEC;

    GameObject gameManager;

    int curMiniMapRotatingDirIndex;

    Vector3 curMiniMapRotationAxis;

    Vector3 camMiniMapRotationStartEulerAngles;
    Quaternion curMiniMapRotationTargetQuaternion;
    Vector3 camMiniMapRotationTargetEulerAngles;

    float accumulatedMiniMapRotationDegree;

    #region ConstantsUsed
    Transform camTransform;

    float miniMapRotationMovingSpeed;
    #endregion

    #region VariablesUsed

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
        miniMapRotationMovingSpeed = CONS.miniMapRotationMovingSpeed;
        #endregion

        #region ImportReferenceVariables
        #endregion
    }

    void Update()
    {
        #region ImportValueVariables
        #endregion

        if (VARS.IsInNewRoomAllResetOver)
        {
            Debug.Log("enter1");

            #region MiniMap
            //intoMiniMap
            if (!VARS.IsInMiniMap)
            {
                Debug.Log("enter2");
                //HR
                if (!VARS.IsZoomedOut &&
                    VARS.IsIniRotation &&
                    !VARS.IsOptionPanelActivated)
                {
                    Debug.Log("enter3");
                    if (!VARS.IsInCenter)
                    {
                        Debug.Log("enter4");
                        //if (VARS.IsInputtingUpKey)
                        //{
                        //    if (VARS.IsJumpKeyDown)
                        //    {
                        if (VARS.IsConfirmKeyDown)
                        {
                            Debug.Log("enter5");
                            UFL.IntoMiniMap();

                            VARS.IsMiniMapRotationCameraPointIndexNotInitialized = true;

                            VARS.IsInMiniMap = true;
                        }
                        //    }
                        //}
                    }
                }
            }

            //inMiniMap
            else
            {
                if (!VARS.IsMiniMapRotating)
                {
                    //miniMapRotationControl
                    if (VARS.IsUpKeyDown)
                    {
                        VARS.IsMiniMapRotating = true;
                        curMiniMapRotatingDirIndex = 1;
                        UFL.GetCurToMiniMapRotationCameraPoint(1);
                    }
                    else if (VARS.IsDownKeyDown)
                    {
                        VARS.IsMiniMapRotating = true;
                        curMiniMapRotatingDirIndex = 2;
                        UFL.GetCurToMiniMapRotationCameraPoint(2);
                    }
                    else if (VARS.IsLeftKeyDown)
                    {
                        VARS.IsMiniMapRotating = true;
                        curMiniMapRotatingDirIndex = 3;
                        UFL.GetCurToMiniMapRotationCameraPoint(3);
                    }
                    else if (VARS.IsRightKeyDown)
                    {
                        VARS.IsMiniMapRotating = true;
                        curMiniMapRotatingDirIndex = 4;
                        UFL.GetCurToMiniMapRotationCameraPoint(4);
                    }

                    //outOfMiniMap
                    if (VARS.IsConfirmKeyDown)
                    {
                        UFL.OutOfMiniMap();

                        VARS.IsInMiniMap = false;
                    }
                }
                //miniMapRotationProcess
                else
                {
                    //Debug.Log("distance: " + Vector3.Distance(camTransform.position, VARS.curToMiniMapRotationCameraPoint.transform.position));

                    if (/*accumulatedMiniMapRotationDegree < 90*/
                        Vector3.Distance(camTransform.position, VARS.curToMiniMapRotationCameraPoint.transform.position) > 3)
                    {
                        UFL.MiniMapCameraRotate(curMiniMapRotatingDirIndex, miniMapRotationMovingSpeed * Time.deltaTime);

                        //accumulatedMiniMapRotationDegree += miniMapRotationMovingSpeed * Time.deltaTime;
                        //accumulatedMiniMapRotationDegree = Vector3.Angle(camTransform.eulerAngles, camMiniMapRotationStartEulerAngles);
                    }
                    else
                    {
                        UFL.SetCameraPosition(VARS.curToMiniMapRotationCameraPoint.transform.position);

                        camTransform.LookAt(Vector3.zero, camTransform.up);

                        VARS.curMiniMapRotationCameraPointIndex = VARS.curToMiniMapRotationCameraPointIndex;
                        //VARS.curMiniMapRotationCameraPoint = VARS.curToMiniMapRotationCameraPoint;

                        //UFL.SetCameraEulerangles(camMiniMapRotationTargetEulerAngles);

                        VARS.IsMiniMapRotating = false;
                    }
                }
            }
            #endregion
        }
    }
}
