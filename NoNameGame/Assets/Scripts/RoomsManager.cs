using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;

[DefaultExecutionOrder((int)ScriptsExecutionOrder.ExecutionOrder.roomsManager)]
public class RoomsManager : MonoBehaviour
{
    Constants CONS;
    Variables VARS;
    UniversalFunctionsLibrary UFL;
    ScriptsExecutionController SEC;

    GameObject gameManager;

    bool isTwistingPresetOver;

    GameObject curTwistingCenter;
    Vector3 curTwistingCenterPosition;

    Vector3 curFaceStableForward;
    Vector3 curFaceStableUp;
    Vector3 curFaceStableRight;

    List<GameObject> curRelatedRoomPlanes = new List<GameObject>();
    List<int> curRelatedRoomPlaneIndexes = new List<int>();

    float twistingAccumulatedDegree;
    Vector3 twistingTargetEulerangles;

    int curMiniMapRotatingDirIndex;

    Vector3 curMiniMapRotationAxis;

    Vector3 camMiniMapRotationStartEulerAngles;
    Quaternion curMiniMapRotationTargetQuaternion;
    Vector3 camMiniMapRotationTargetEulerAngles;

    float accumulatedMiniMapRotationDegree;

    int tempInt;
    float tempFloat1;
    float tempFloat2;
    float tempFloat3;
    float tempFloat4;
    Vector3 tempVector;
    GameObject tempGameObject;

    #region ConstantsUsed
    float gridBreadth;
    int roomCoordBreadth;

    float inRoomMaxForwardDistance;

    GameObject[] faces = new GameObject[6];
    Vector3[] faceStableForwards = new Vector3[6];
    Vector3[] faceStableUps = new Vector3[6];
    Vector3[] faceStableRights = new Vector3[6];

    GameObject[] roomPlanes = new GameObject[54];

    GameObject[] twistingCenters = new GameObject[6];

    float twistSpeed;

    float miniMapRotationMovingSpeed;

    Transform camTransform;

    float camMiniMapDistanceToCubeCore;

    Transform catTransform;
    #endregion

    #region VariablesUsed
    GameObject curPlaneEmpty;

    Vector3[] roomCenters = new Vector3[54];
    Vector3[] roomStableForwards = new Vector3[54];
    Vector3[] roomStableUps = new Vector3[54];
    Vector3[] roomStableRights = new Vector3[54];
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
        inRoomMaxForwardDistance = CONS.inRoomMaxForwardDistance;
        faces = CONS.faces;
        faceStableForwards = CONS.faceStableForwards;
        faceStableUps = CONS.faceStableUps;
        faceStableRights = CONS.faceStableRights;
        roomPlanes = CONS.roomPlanes;
        twistingCenters = CONS.twistingCenters;
        twistSpeed = CONS.twistSpeed;
        miniMapRotationMovingSpeed = CONS.miniMapRotationMovingSpeed;
        camTransform = CONS.camTransform;
        camMiniMapDistanceToCubeCore = CONS.camMiniMapDistanceToCubeCore;
        catTransform = CONS.catTransform;

		roomCenters = VARS.roomCenters;
		roomStableForwards = VARS.roomStableForwards;
		roomStableUps = VARS.roomStableUps;
		roomStableRights = VARS.roomStableRights;
	}

    void Update()
    {
		curPlaneEmpty = VARS.curPlaneEmpty;

		#region IfIsInNewRoom
		if (UFL.IsInRoom(VARS.curRoomIndex, catTransform.position))
        {
        }
        else
        {
            for (int i = 0; i < roomCenters.Length; i++)
            {
                if (UFL.IsInRoom(i, catTransform.position))
                {
                    VARS.curRoomIndex = i;
                    break;
                }
            }

            VARS.IsIntoNewRoom = true;
        }

        if (VARS.IsIntoNewRoom)
        {
            IntoNewRoom();
        }
        #endregion

        #region InNewRoomReset
        if (!VARS.IsInNewRoomAllResetOver)
        {
            if (VARS.IsInNewRoomCurRoomManagerResetOver &&
                VARS.IsInNewRoomCameraManagerResetOver &&
                VARS.IsInNewRoomCatRotateResetOver &&
                VARS.IsInNewRoomBlocksManagerResetOver)
            {
                //hideOtherPlanes
                if (!VARS.IsZoomedOut)
                    UFL.HideOtherPlanes();

                VARS.IsInNewRoom = false;

                VARS.IsInNewRoomAllResetOver = true;
            }
        }
        #endregion

        if (VARS.IsInNewRoomAllResetOver)
        {
            #region Twist
            //control
            if (!VARS.IsTwisting)
            {
                if (VARS.IsInCenter)
                {
                    if (VARS.IsInputtingDownKey)
                    {
                        if (VARS.IsLeftKeyDown ||
                            VARS.IsRightKeyDown)
                        {
                            //getTwistingFaceIndex
                            tempGameObject = curPlaneEmpty.transform.parent.parent.gameObject;
                            for (int i = 0; i < 6; i++)
                            {
                                if (tempGameObject == faces[i])
                                {
                                    VARS.twistingFaceIndex = i + 1;
                                    break;
                                }
                            }

                            //determineTwistingDirection
                            if (VARS.IsLeftKeyDown)
                            {
                                VARS.IsClockwiseTwisting = true;
                            }
                            else if (VARS.IsRightKeyDown)
                            {
                                VARS.IsClockwiseTwisting = false;
                            }

                            VARS.IsTwisting = true;
                        }
                    }
                }
            }

            //process
            else
            {
                if (!isTwistingPresetOver)
                {
                    tempInt = VARS.twistingFaceIndex;

                    curTwistingCenter = twistingCenters[tempInt - 1];
                    curTwistingCenterPosition = curTwistingCenter.transform.position;

                    curFaceStableForward = faceStableForwards[tempInt - 1];
                    curFaceStableUp = faceStableUps[tempInt - 1];
                    curFaceStableRight = faceStableRights[tempInt - 1];

                    //getRelatedRoomPlanes
                    for (int i = 0; i < roomPlanes.Length; i++)
                    {
                        tempVector = roomCenters[i] - curTwistingCenterPosition;

                        //getRoomPlanesInTheFace
                        if (/*Mathf.Abs(Vector3.Dot(tempVector, curFaceStableForward)) <= (roomCoordBreadth / 2 + 2) * gridBreadth*/
                            UFL.IsPlaneInTheFace(i, tempInt))
                        {
                            curRelatedRoomPlanes.Add(roomPlanes[i]);
                            curRelatedRoomPlaneIndexes.Add(i);
                        }

                        //getRoomPlanesSurroundingTheFace
                        if (/*Mathf.Abs(Vector3.SignedAngle(tempVector, curFaceStableUp, curFaceStableForward)) <= 6 &&
                            Mathf.Abs(Vector3.SignedAngle(tempVector, curFaceStableRight, curFaceStableForward)) <= 6*/
                            UFL.IsPlaneSurroundingTheFace(i, tempInt))
                        {
                            curRelatedRoomPlanes.Add(roomPlanes[i]);
                            curRelatedRoomPlaneIndexes.Add(i);
                        }
                    }

                    //roomPlanesTempChildToCurTwistingCenter
                    for (int i = 0; i < curRelatedRoomPlanes.Count; i++)
                    {
                        curRelatedRoomPlanes[i].transform.SetParent(curTwistingCenter.transform, true);
                    }

                    //catAndCamTempChildToCurTwistingCenter
                    //camTransform.SetParent(curTwistingCenter.transform, true);
                    catTransform.SetParent(curTwistingCenter.transform, true);

                    //setTargetEulerangles
                    if (VARS.IsClockwiseTwisting)
                    {
                        twistingTargetEulerangles = curTwistingCenter.transform.eulerAngles + new Vector3(0, 0, 90);
                    }
                    else
                    {
                        twistingTargetEulerangles = curTwistingCenter.transform.eulerAngles + new Vector3(0, 0, -90);
                    }

                    isTwistingPresetOver = true;
                }

                //twist
                if (twistingAccumulatedDegree < 90)
                {
                    twistingAccumulatedDegree += twistSpeed * Time.deltaTime;
                    if (VARS.IsClockwiseTwisting)
                    {
                        curTwistingCenter.transform.Rotate(curFaceStableForward * twistSpeed * Time.deltaTime);
                    }
                    else
                    {
                        curTwistingCenter.transform.Rotate(-curFaceStableForward * twistSpeed * Time.deltaTime);
                    }
                }
                else
                {
                    //setPositionsAndEulerangles(~?)
                    curTwistingCenter.transform.eulerAngles = twistingTargetEulerangles;

                    //resetRoomPlanesParents
                    ResetCurRelatedPlanes();
                    curRelatedRoomPlanes.Clear();
                    curRelatedRoomPlaneIndexes.Clear();

                    //freeCatAndCam
                    //camTransform.SetParent(null);
                    catTransform.SetParent(null);

                    //resetCatEulerangles
                    catTransform.eulerAngles = Vector3.zero;

                    //setMiniMapRoomPlanes
                    UFL.SetMiniMapRoomPlanesByRoomPlanes();

                    isTwistingPresetOver = false;

                    twistingAccumulatedDegree = 0;

                    VARS.IsIntoNewRoom = true;

                    VARS.IsToWriteWorldData = true;

                    VARS.IsToDetermineCurActivatedSavePointPosition = true;

                    VARS.IsTwisting = false;
                }
            }
            #endregion

            #region MiniMap
            //intoMiniMap
            if (!VARS.IsInMiniMap)
            {
                //HR
                if (!VARS.IsZoomedOut
                    && VARS.IsIniRotation)
                {
                    if (!VARS.IsInCenter)
                    {
                        //if (VARS.IsInputtingUpKey)
                        //{
                        //    if (VARS.IsJumpKeyDown)
                        //    {
                        if (VARS.IsConfirmKeyDown)
                        {
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
                        //camMiniMapRotationStartEulerAngles = camTransform.eulerAngles;
                        //curMiniMapRotationTargetQuaternion = camTransform.rotation * Quaternion.AngleAxis(-90, Vector3.right);
                        //camMiniMapRotationTargetEulerAngles = curMiniMapRotationTargetQuaternion.eulerAngles;
                        ////Debug.Log(camMiniMapRotationTargetEulerAngles);
                        //accumulatedMiniMapRotationDegree = 0;
                    }
                    else if (VARS.IsDownKeyDown)
                    {
                        VARS.IsMiniMapRotating = true;
                        curMiniMapRotatingDirIndex = 2;
                        UFL.GetCurToMiniMapRotationCameraPoint(2);
                        //camMiniMapRotationStartEulerAngles = camTransform.eulerAngles;
                        //curMiniMapRotationTargetQuaternion = camTransform.rotation * Quaternion.AngleAxis(90, Vector3.right);
                        //camMiniMapRotationTargetEulerAngles = curMiniMapRotationTargetQuaternion.eulerAngles;
                        ////Debug.Log(camMiniMapRotationTargetEulerAngles);
                        //accumulatedMiniMapRotationDegree = 0;
                    }
                    else if (VARS.IsLeftKeyDown)
                    {
                        VARS.IsMiniMapRotating = true;
                        curMiniMapRotatingDirIndex = 3;
                        UFL.GetCurToMiniMapRotationCameraPoint(3);
                        //camMiniMapRotationStartEulerAngles = camTransform.eulerAngles;
                        //curMiniMapRotationTargetQuaternion = camTransform.rotation * Quaternion.AngleAxis(-90, Vector3.up);
                        //camMiniMapRotationTargetEulerAngles = curMiniMapRotationTargetQuaternion.eulerAngles;
                        ////Debug.Log(camMiniMapRotationTargetEulerAngles);
                        //accumulatedMiniMapRotationDegree = 0;
                    }
                    else if (VARS.IsRightKeyDown)
                    {
                        VARS.IsMiniMapRotating = true;
                        curMiniMapRotatingDirIndex = 4;
                        UFL.GetCurToMiniMapRotationCameraPoint(4);
                        //camMiniMapRotationStartEulerAngles = camTransform.eulerAngles;
                        //curMiniMapRotationTargetQuaternion = camTransform.rotation * Quaternion.AngleAxis(90, Vector3.up);
                        //camMiniMapRotationTargetEulerAngles = curMiniMapRotationTargetQuaternion.eulerAngles;
                        ////Debug.Log(camMiniMapRotationTargetEulerAngles);
                        //accumulatedMiniMapRotationDegree = 0;
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
                        Vector3.Distance(camTransform.position,VARS.curToMiniMapRotationCameraPoint.transform.position) > 3)
                    {
                        UFL.MiniMapCameraRotate(curMiniMapRotatingDirIndex, miniMapRotationMovingSpeed * Time.deltaTime);

                        //accumulatedMiniMapRotationDegree += miniMapRotationMovingSpeed * Time.deltaTime;
                        //accumulatedMiniMapRotationDegree = Vector3.Angle(camTransform.eulerAngles, camMiniMapRotationStartEulerAngles);
                    }
                    else
                    {
                        UFL.SetCameraPosition(VARS.curToMiniMapRotationCameraPoint.transform.position);

                        //tempVector = camTransform.eulerAngles;

                        ////Debug.Log("1: " + camTransform.eulerAngles);
                        //camTransform.LookAt(Vector3.zero);
                        ////Debug.Log("2: " + camTransform.eulerAngles);

                        //UFL.SetCameraEulerangles(new Vector3(camTransform.eulerAngles.x, camTransform.eulerAngles.y, tempVector.z));
                        ////Debug.Log("3: " + camTransform.eulerAngles);

                        //// 保存LookAt前的z轴旋转
                        //float z = camTransform.eulerAngles.z;
                        ////// LookAt会重置rotation
                        ////camTransform.LookAt(Vector3.zero);
                        ////// 用四元数叠加z轴旋转
                        ////camTransform.rotation = Quaternion.Euler(camTransform.eulerAngles.x, camTransform.eulerAngles.y, z);

                        //Quaternion lookAtRotation = Quaternion.LookRotation(Vector3.zero - camTransform.position, Vector3.up);
                        //Quaternion zRotation = Quaternion.AngleAxis(z, Vector3.forward);
                        //camTransform.rotation = lookAtRotation * zRotation;

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

    //public bool IsInRoom(int roomIndex, Vector3 position)
    //{
    //    tempVector = position - roomCenters[roomIndex];

    //    //ifIsInThePlane
    //    if (Mathf.Abs(Vector3.Dot(tempVector, roomStableForwards[roomIndex])) <= inRoomMaxForwardDistance)
    //    {
    //        //ifIsInsideTheBoundary
    //        if (Mathf.Abs(Vector3.Dot(tempVector, roomStableUps[roomIndex])) <= (roomCoordBreadth / 2 + 1) * gridBreadth &&
    //            Mathf.Abs(Vector3.Dot(tempVector, roomStableRights[roomIndex])) <= (roomCoordBreadth / 2 + 1)* gridBreadth)
    //        {
    //            return true;
    //        }
    //    }

    //    return false;
    //}

    void IntoNewRoom()
    {
        VARS.IsIntoNewRoom = false;

        VARS.IsInNewRoom = true;

        VARS.IsInNewRoomCurRoomManagerResetOver = false;
        VARS.IsInNewRoomCameraManagerResetOver = false;
        VARS.IsInNewRoomCatRotateResetOver = false;
        VARS.IsInNewRoomBlocksManagerResetOver = false;
        VARS.IsInNewRoomAllResetOver = false;
    }

    void ResetCurRelatedPlanes()
    {
        for(int i = 0; i < curRelatedRoomPlanes.Count; i++)
        {
            tempFloat1 = curRelatedRoomPlanes[i].transform.position.x;
            tempFloat2 = curRelatedRoomPlanes[i].transform.position.y;
            tempFloat3 = curRelatedRoomPlanes[i].transform.position.z;
            tempFloat4 = Mathf.Max(Mathf.Abs(tempFloat1), Mathf.Abs(tempFloat2), Mathf.Abs(tempFloat3));

            //frontalFace
            if (tempFloat4 == Mathf.Abs(tempFloat3))
            {
                //frontFace
                if (tempFloat3 < 0)
                {
                    ResetCurRelatedPlane(curRelatedRoomPlaneIndexes[i], 1);
                }
                //backFace
                else
                {
                    ResetCurRelatedPlane(curRelatedRoomPlaneIndexes[i], 2);
                }
            }
            //profileFace
            else if (tempFloat4 == Mathf.Abs(tempFloat1))
            {
                //leftFace
                if(tempFloat1 < 0)
                {
                    ResetCurRelatedPlane(curRelatedRoomPlaneIndexes[i], 3);
                }
                //rightFace
                else
                {
                    ResetCurRelatedPlane(curRelatedRoomPlaneIndexes[i], 4);
                }
            }
            //horizontalFace
            else
            {
                //topFace
                if (tempFloat2 < 0)
                {
                    ResetCurRelatedPlane(curRelatedRoomPlaneIndexes[i], 5);
                }
                //bottomFace
                else
                {
                    ResetCurRelatedPlane(curRelatedRoomPlaneIndexes[i], 6);
                }
            }
        }
    }

    void ResetCurRelatedPlane(int roomIndex, int faceIndex)
    {
        roomPlanes[roomIndex].transform.SetParent(faces[faceIndex - 1].transform, true);

        roomPlanes[roomIndex].transform.position = new Vector3
            (Mathf.Round(tempFloat1), Mathf.Round(tempFloat2), Mathf.Round(tempFloat3));

        roomCenters[roomIndex] = roomPlanes[roomIndex].transform.position;

        roomStableForwards[roomIndex] = faceStableForwards[faceIndex - 1];
        roomStableUps[roomIndex] = faceStableUps[faceIndex - 1];
        roomStableRights[roomIndex] = faceStableRights[faceIndex - 1];
    }
}
