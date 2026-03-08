using UnityEngine;

[DefaultExecutionOrder((int)ScriptsExecutionOrder.ExecutionOrder.cameraManager)]
public class CameraManager : MonoBehaviour
{
    Constants CONS;
    Variables VARS;
    UniversalFunctionsLibrary UFL;
    ScriptsExecutionController SEC;

    GameObject gameManager;

    GameObject[] initialSightMasks = new GameObject[4];

    float accumulatedInitialSightMasksDistance;

    float accumulatedZoomSize;

    float zoomInAutoTriggerStartTime;

    int tempInt;
    float tempFloat;

    #region ConstantsUsed
    float gridBreadth;
    int roomCoordBreadth;

    GameObject[] roomPlanes = new GameObject[54];

    Transform camTransform;

    GameObject initialSightMasksEmpty;
    float initialSightMasksSpeed;
    float initialSightMasksMaxDistance;

    float zoomSpeed;

    float zoomInAutoTriggerTime;

    float camNormalSize;
    float camZoomedOutMaxSize;

    Transform catTransform;
    #endregion

    #region VariablesUsed
    //Vector3 planeForward;
    //Vector3 planeUp;
    //Vector3 planeRight;
    //Vector3 iniRight;
    #endregion

    void Start()
    {
        gameManager = GameObject.Find("GameManager");

        CONS = gameManager.GetComponent<Constants>();
        VARS = gameManager.GetComponent<Variables>();
        UFL = gameManager.GetComponent<UniversalFunctionsLibrary>();
        SEC = gameManager.GetComponent<ScriptsExecutionController>();

        gridBreadth = CONS.gridBreadth;
        roomCoordBreadth = CONS.roomCoordBreadth;
        roomPlanes = CONS.roomPlanes;
        camTransform = CONS.camTransform;
        initialSightMasksEmpty = CONS.initialSightMasksEmpty;
        initialSightMasksSpeed = CONS.initialSightMasksSpeed;
        initialSightMasksMaxDistance = CONS.initialSightMasksMaxDistance;
        zoomSpeed = CONS.zoomSpeed;
        zoomInAutoTriggerStartTime = CONS.zoomInAutoTriggerTime;
        camNormalSize = CONS.camNormalSize;
        camZoomedOutMaxSize = CONS.camZoomedOutMaxSize;
        catTransform = CONS.catTransform;

        //getInitialSightMasks
        for (int i = 0; i < 4; i++)
        {
            initialSightMasks[i] = initialSightMasksEmpty.transform.GetChild(i).gameObject;
        }
    }

    void Update()
    {
        //planeForward = VARS.planeForward;
        //planeUp = VARS.planeUp;
        //planeRight = VARS.planeRight;
        //iniRight = VARS.iniRight;

        if (VARS.IsInNewRoom)
        {
            //position
            camTransform.position = VARS.roomCenters[VARS.curRoomIndex] - VARS.curRoomStableForward * 7;

            //tempFloat = camTransform.eulerAngles.z;
            //camTransform.LookAt(CONS.roomPlanes[VARS.curRoomIndex].transform.position);
            //camIniEulerangles = camTransform.eulerAngles;
            //camTransform.eulerAngles = new Vector3(camTransform.eulerAngles.x, camTransform.eulerAngles.y, tempFloat);
            //camIniEulerangles += Vector3.forward * Vector3.SignedAngle(planeRight, iniRight, planeForward);

            //eulerangles
            tempFloat = camTransform.eulerAngles.z;
            camTransform.LookAt(CONS.roomPlanes[VARS.curRoomIndex].transform.position);
            VARS.camIniEulerangles = camTransform.eulerAngles;
            VARS.camIniEulerangles += Vector3.forward * Vector3.SignedAngle(camTransform.right, VARS.curRoomStableRight, VARS.curRoomStableForward);
            camTransform.eulerAngles = new Vector3(camTransform.eulerAngles.x, camTransform.eulerAngles.y, tempFloat);

            VARS.curUp = new Vector3
                (Mathf.RoundToInt(camTransform.up.x), Mathf.RoundToInt(camTransform.up.y), Mathf.RoundToInt(camTransform.up.z));
            VARS.curRight = new Vector3
                (Mathf.RoundToInt(camTransform.right.x), Mathf.RoundToInt(camTransform.right.y), Mathf.RoundToInt(camTransform.right.z));

            VARS.IsInNewRoomCameraManagerResetOver = true;
        }

        if (VARS.IsInNewRoomAllResetOver)
        {
            #region InitializeSight
            if (VARS.IsToInitializeSight)
            {
                for (int i = 0; i < 4; i++)
                {
                    initialSightMasks[i].SetActive(true);
                    initialSightMasks[i].transform.position = catTransform.position - VARS.curRoomStableForward * 4;
                    initialSightMasks[i].transform.localEulerAngles = Vector3.zero;
                }

                tempFloat = roomCoordBreadth * gridBreadth;

                initialSightMasks[0].transform.position += VARS.curRoomStableUp * tempFloat;
                initialSightMasks[1].transform.position -= VARS.curRoomStableUp * tempFloat;
                initialSightMasks[2].transform.position -= VARS.curRoomStableRight * tempFloat;
                initialSightMasks[3].transform.position += VARS.curRoomStableRight * tempFloat;

                accumulatedInitialSightMasksDistance = 0;

                VARS.IsInitializingSight = true;

                VARS.IsToInitializeSight = false;
            }
            if (VARS.IsInitializingSight)
            {
                if (accumulatedInitialSightMasksDistance < initialSightMasksMaxDistance)
                {
                    tempFloat = initialSightMasksSpeed * Time.deltaTime;

                    initialSightMasks[0].transform.position += VARS.curRoomStableUp * tempFloat;
                    initialSightMasks[1].transform.position -= VARS.curRoomStableUp * tempFloat;
                    initialSightMasks[2].transform.position -= VARS.curRoomStableRight * tempFloat;
                    initialSightMasks[3].transform.position += VARS.curRoomStableRight * tempFloat;

                    accumulatedInitialSightMasksDistance += tempFloat;
                }
                else
                {
                    for (int i = 0; i < 4; i++)
                    {
                        initialSightMasks[i].SetActive(false);
                    }

                    VARS.IsInitializingSight = false;
                }
            }
            #endregion

            #region Zoom
            #region ZoomOut
            if (!VARS.IsRotating &&
                !VARS.IsTwisting)
            {
                if (!VARS.IsZoomedOut)
                {
                    //control
                    if (!VARS.IsZoomingOut)
                    {
                        if (VARS.IsInCenter)
                        {
                            if (/*VARS.IsUpKeyDown*/
                                VARS.IsConfirmKeyDown)
                            {
                                VARS.IsToZoomOut = true;
                            }
                        }
                    }

                    //process
                    if (VARS.IsToZoomOut)
                    {
                        accumulatedZoomSize = 0;

                        VARS.IsZoomingOut = true;

                        VARS.IsToZoomOut = false;
                    }
                    if (VARS.IsZoomingOut)
                    {
                        if (accumulatedZoomSize < camZoomedOutMaxSize - camNormalSize)
                        {
                            tempFloat = zoomSpeed * Time.deltaTime;

                            UFL.AddCameraSize(tempFloat);

                            accumulatedZoomSize += tempFloat;
                        }
                        else
                        {
                            UFL.SetCameraSize(camZoomedOutMaxSize);

                            tempInt = VARS.curFaceIndex;

                            //showPlanesOfExploredRoomsInTheFace
                            for (int i = 0; i < 54; i++)
                            {
                                if (UFL.IsPlaneInTheFace(i, tempInt))
                                {
                                    if (UFL.IsRoomExplored(i))
                                    {
                                        roomPlanes[i].SetActive(true);
                                    }
                                }
                            }

                            VARS.IsZoomingOut = false;

                            VARS.IsZoomedOut = true;
                        }
                    }
                }
                #endregion
                #region ZoomIn
                else
                {
                    if (!VARS.IsZoomingIn)
                    {
                        //control
                        if (VARS.IsInCenter)
                        {
                            zoomInAutoTriggerStartTime = 0;

                            if (/*VARS.IsDownKeyUp*/
                                VARS.IsConfirmKeyDown)
                            {
                                VARS.IsToZoomIn = true;
                            }
                        }
                        //autoTrigger
                        else
                        {
                            if(zoomInAutoTriggerStartTime==0)
                                zoomInAutoTriggerStartTime = Time.time;

                            if (Time.time - zoomInAutoTriggerStartTime > zoomInAutoTriggerTime)
                            {
                                Debug.Log("enter");

                                VARS.IsToZoomIn = true;
                            }
                        }
                    }

                    //process
                    if (VARS.IsToZoomIn)
                    {
                        //hideOtherPlanes
                        UFL.HideOtherPlanes();

                        accumulatedZoomSize = 0;

                        VARS.IsZoomingIn = true;

                        VARS.IsToZoomIn = false;
                    }
                    if (VARS.IsZoomingIn)
                    {
                        if (accumulatedZoomSize < camZoomedOutMaxSize - camNormalSize)
                        {
                            tempFloat = zoomSpeed * Time.deltaTime;

                            UFL.AddCameraSize(-tempFloat);

                            accumulatedZoomSize += tempFloat;
                        }
                        else
                        {
                            UFL.SetCameraSize(camNormalSize);

                            VARS.IsZoomingIn = false;

                            VARS.IsZoomedOut = false;
                        }
                    }
                }
            }
            #endregion
            #endregion

            #region MiniMap
            //if (VARS.IsInMiniMap)
            //{
            //    UFL.SetCameraEulerangles(new Vector3(camTransform.eulerAngles.x, camTransform.eulerAngles.y, VARS.camEuleranglesBeforeIntoMiniMap.z));
            //}
            #endregion
        }
    }
}
