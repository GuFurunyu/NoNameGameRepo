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

    #region ConstantsUsed
    Transform camTransform;

    float minimapRotationMovingSpeed;
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
        minimapRotationMovingSpeed = CONS.minimapRotationMovingSpeed;
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
            #region Minimap
            //intoMinimap
            if (!VARS.IsInMinimap)
            {
                if (!VARS.IsZoomedOut &&
                    //VARS.IsIniRotation &&
                    !VARS.IsOptionPanelActivated)
                {
                    if (!VARS.IsInCenter)
                    {
                        //if (VARS.IsInputtingUpKey)
                        //{
                        //    if (VARS.IsJumpKeyDown)
                        //    {
                        if (VARS.IsConfirmKeyDown)
                        {
                            UFL.IntoMinimap();

                            VARS.IsMinimapRotationCameraPointIndexNotInitialized = true;

                            VARS.IsInMinimap = true;
                        }
                        //    }
                        //}
                    }
                }
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
                    if (VARS.IsConfirmKeyDown)
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
            #endregion
        }
    }
}
